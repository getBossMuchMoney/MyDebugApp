using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;




namespace MyDebugApp
{
    public partial class CanTabUserControl : UserControl
    {
        VCI_BOARD_INFO[] boardInfos = new VCI_BOARD_INFO[50];
        VCI_CAN_OBJ[] CanRcvBuff = new VCI_CAN_OBJ[2500];

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_FindUsbDevice2(ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_UsbDeviceReset(UInt32 DevType, UInt32 DevIndex, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);


        static UInt32 VCI_USBCAN2 = 4;//USBCAN2
        static byte[,] DeviceTimming = new byte[3, 2] { { 0x03, 0x1C }, { 0x01, 0x1C }, { 0x00, 0x1C } };

        VCI_INIT_CONFIG vci_init = new VCI_INIT_CONFIG() { AccCode = 0x80000008, AccMask = 0xFFFFFFFF, Reserved = 0, Filter = 3, Timing0 = 0, Timing1 = 0, Mode = 0 };//扩展帧正常模式
        Thread updateThread;
        Thread canRcvThread;
        Thread canAppSendThread;
        uint CanDevIndex, CanPassNum = 0;
        byte[] FileDataBuffer;
        byte[] FileCrc = new byte[2];
        UInt32 FileSize = 0;
        ModbusCrc CrcInter = new ModbusCrc();
        BlockingCollection<uint[]> UpdateRxQueue = new BlockingCollection<uint[]>(new ConcurrentQueue<uint[]>());
        BlockingCollection<uint[]> ModAnswQueue = new BlockingCollection<uint[]>(new ConcurrentQueue<uint[]>());
        BlockingCollection<uint[]> AppTxQueue = new BlockingCollection<uint[]>(new ConcurrentQueue<uint[]>());
        BlockingCollection<string> ShowStrRxQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
        byte ChoseUpdateID = 0;
        CancellationTokenSource updateCts = new CancellationTokenSource();
        CancellationTokenSource CanDataRecieveCts = new CancellationTokenSource();
        CancellationTokenSource CanSendCts = new CancellationTokenSource();
        CancellationTokenSource DealModCts = new CancellationTokenSource();

        byte SetFlag = 0, setFunccode = 0;
        uint[] Setbuff = new uint[8];
        byte SendFunccode = 0;
        U_MOD_STA[] modstadata = new U_MOD_STA[12];
        U_MOD_SET[] modsetdata = new U_MOD_SET[12];
        uint ReadSettingFinish = 0;
        uint[] DevOfflineCheckCnt = new uint[12];
        uint[] SlaverConnectSta = new uint[12];
        ushort SystemSlaverNum = 12;

        public CanTabUserControl()
        {
            InitializeComponent();
            UInt32 CanDevNum = VCI_FindUsbDevice2(ref boardInfos[0]);
            for (int i = 0; i < CanDevNum; i++)
            {
                CanDevListBox.Items.Add(i);
                if (i == 0)
                {
                    CanDevListBox.SelectedIndex = 0;
                }
            }

            for (int i = 0; i < SystemSlaverNum; i++)
            {
                modsetdata[i].REG.u16_ModuleEn = 0xFFFF;
            }

            CanDevPassNumBox.SelectedIndex = 0;
            CanBandListBox.SelectedIndex = 0;
            ChoseDevListBox.SelectedIndex = 1;
            Thread dealModDataThread = new Thread(() => DealModAnsw(DealModCts.Token));
            dealModDataThread.Start();

        }

        private unsafe void DealModAnsw(CancellationToken token)
        {
            uint[] data = new uint[9];
            CanAppId id = new CanAppId();
            Array.Clear(DevOfflineCheckCnt, 0, DevOfflineCheckCnt.Length);
            Array.Clear(SlaverConnectSta, 0, SlaverConnectSta.Length);
            while (!token.IsCancellationRequested)
            {
                if (ModAnswQueue.TryTake(out data))
                {
                    id.IdFrame = data[0];
                    if (id.FuncCode < 0x09)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            modstadata[id.SrcId - 1].buff[id.FuncCode * 4 + i] = (ushort)((data[i * 2 + 1] << 8) | (data[i * 2 + 2]));
                        }

                    }
                    else
                    {
                        if (id.FuncCode == 0x20)
                        {

                        }
                        else if (id.FuncCode == 0x21)
                        {
                            if (id.ActFlag == 1)
                            {
                                for (int i = 0; i < data[2]; i++)
                                {
                                    modsetdata[id.SrcId - 1].buff[data[1] - 128 + i] = (ushort)((data[i * 2 + 3] << 8) | (data[i * 2 + 4]));
                                }

                                if (data[1] == 134)
                                {
                                    ReadSettingFinish = 1;
                                }
                            }

                        }

                    }

                    SlaverConnectSta[id.SrcId - 1] = 1;
                    DevOfflineCheckCnt[id.SrcId - 1] = 0;

                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        DevOfflineCheckCnt[i]++;
                        if (DevOfflineCheckCnt[i] > 100)
                        {
                            DevOfflineCheckCnt[i] = 0;
                            SlaverConnectSta[i] = 0;
                        }
                    }
                    Thread.Sleep(1);
                }
            }

        }
        private void ScanCanButton_Click(object sender, EventArgs e)
        {
            if (OpenCanDevButton.Text == "打开分析仪")
            {
                UInt32 CanDevNum = VCI_FindUsbDevice2(ref boardInfos[0]);
                CanDevListBox.Items.Clear();
                for (int i = 0; i < CanDevNum; i++)
                {
                    CanDevListBox.Items.Add(i);
                }
                if (CanDevNum > 0)
                {
                    CanDevListBox.SelectedIndex = 0;
                }

            }

        }

        private void ResetCanDevButton_Click(object sender, EventArgs e)
        {
            if (OpenCanDevButton.Text == "打开分析仪")
            {
                if (CanDevListBox.Items.Count > 0)
                {
                    VCI_UsbDeviceReset(VCI_USBCAN2, (uint)CanDevListBox.SelectedIndex, 0);
                }
            }

        }

        private unsafe void OpenCanDevButton_Click(object sender, EventArgs e)
        {
            uint ret = 0;
            if (OpenCanDevButton.Text == "打开分析仪")
            {
                ret = VCI_OpenDevice(VCI_USBCAN2, (uint)CanDevListBox.SelectedIndex, 0);
                if (ret == 1)
                {
                    vci_init.Timing0 = DeviceTimming[CanBandListBox.SelectedIndex, 0];
                    vci_init.Timing1 = DeviceTimming[CanBandListBox.SelectedIndex, 1];
                    ret = VCI_InitCAN(VCI_USBCAN2, (uint)CanDevListBox.SelectedIndex, (uint)CanDevPassNumBox.SelectedIndex, ref vci_init);
                    if (ret == 1)
                    {
                        ret = VCI_StartCAN(VCI_USBCAN2, (uint)CanDevListBox.SelectedIndex, (uint)CanDevPassNumBox.SelectedIndex);
                        if (ret == 1)
                        {
                            OpenCanDevButton.Text = "关闭分析仪";
                            CanDevIndex = (uint)CanDevListBox.SelectedIndex;
                            CanPassNum = (uint)CanDevPassNumBox.SelectedIndex;
                            CanDataRecieveCts = new CancellationTokenSource();
                            canRcvThread = new Thread(() => CanData_Recieve(CanDataRecieveCts.Token));
                            canRcvThread.Start();
                            CanSendCts = new CancellationTokenSource();
                            canAppSendThread = new Thread(() => CanSendTask(CanSendCts.Token));
                            canAppSendThread.Start();
                        }
                    }

                }

            }
            else
            {
                if (canAppSendThread != null && canAppSendThread.IsAlive)
                {
                    CanSendCts.Cancel();
                    while (AppTxQueue.TryTake(out uint[] _)) ;
                    canAppSendThread.Join();
                }

                if (canRcvThread != null && canRcvThread.IsAlive)
                {
                    CanDataRecieveCts.Cancel();
                    while (ShowStrRxQueue.TryTake(out string _)) ;
                    while (UpdateRxQueue.TryTake(out uint[] _)) ;
                    while (ModAnswQueue.TryTake(out uint[] _)) ;
                    canRcvThread.Join();
                }

                if (updateThread != null && updateThread.IsAlive)
                {
                    updateCts.Cancel();
                    updateThread.Join();
                }

                for (int i = 0; i < 12; i++)
                {
                    for (int a = 0; a < 8; a++)
                    {
                        modsetdata[i].buff[0] = 0;
                    }
                    for (int a = 0; a < 36; a++)
                    {
                        modstadata[i].buff[0] = 0;
                    }
                }

                SetFlag = 0;
                setFunccode = 0;
                SendFunccode = 0;
                VCI_CloseDevice(VCI_USBCAN2, (uint)CanDevListBox.SelectedIndex);
                OpenCanDevButton.Text = "打开分析仪";
            }

        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            try
            {
                if (bytes != null)
                {
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        returnStr += bytes[i].ToString("X2");
                        returnStr += " ";//两个16进制用空格隔开,方便看数据
                    }
                }
                return returnStr;
            }
            catch (Exception)
            {
                return returnStr;
            }
        }

        private unsafe void CanData_Recieve(CancellationToken token)
        {
            uint ret = 0, id = 0;
            CANID_UNION msgID = new CANID_UNION() { IdFrame = 0 };
            CanAppId msgID1 = new CanAppId() { IdFrame = 0 };
            while (!token.IsCancellationRequested)
            {
                ret = VCI_Receive(VCI_USBCAN2, CanDevIndex, CanPassNum, ref CanRcvBuff[0], 2500, 0);

                if (ret > 0)
                {
                    for (uint i = 0; i < ret; i++)
                    {
                        byte[] data = new byte[CanRcvBuff[i].DataLen];
                        for (uint size = 0; size < CanRcvBuff[i].DataLen; size++)
                        {
                            data[size] = CanRcvBuff[i].Data[size];
                        }
                        id = CanRcvBuff[i].ID;
                        if (updateThread != null && updateThread.IsAlive)
                        {
                            msgID.IdFrame = id;
                            if ((msgID.DesId == 0x3F) && (msgID.FuncCode != 0x09))
                            {
                                uint[] msbuffer = new uint[CanRcvBuff[i].DataLen + 1];
                                msbuffer[0] = id;
                                for (uint k = 1; k < (CanRcvBuff[i].DataLen + 1); k++)
                                {
                                    msbuffer[k] = data[k - 1];
                                }
                                UpdateRxQueue.Add(msbuffer);
                            }
                        }
                        else
                        {
                            msgID1.IdFrame = id;
                            if (msgID1.DesId == 0 && msgID1.SrcId > 0 && msgID1.SrcId < 13)
                            {
                                uint[] msbuffer = new uint[CanRcvBuff[i].DataLen + 1];
                                msbuffer[0] = id;
                                for (uint k = 1; k < (CanRcvBuff[i].DataLen + 1); k++)
                                {
                                    msbuffer[k] = data[k - 1];
                                }
                                ModAnswQueue.Add(msbuffer);
                            }

                        }


                        string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                        string str = '[' + timestamp + ']' + "收←◆" + "ID:" + id.ToString("X") + " Data:" + byteToHexStr(data) + Environment.NewLine;
                        ShowStrRxQueue.Add(str);
                    }

                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        private void BootButton_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[8];
            CANID_UNION id = new CANID_UNION() { IdFrame = 0 };
            id.SrcId = 0x3F;
            id.DesId = 0x00;
            id.FuncCode = 0x09;
            Array.Clear(data, 0, data.Length);

            if (OpenCanDevButton.Text == "关闭分析仪")
            {
                Can_Transmit(id.IdFrame, data);
            }


        }

        private void ClearRcvDataShowButton_Click(object sender, EventArgs e)
        {
            ShowRcvDataBox.Clear();
        }

        private void ChoseBinFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Bin文件 (*.bin)|*.bin";
                openFileDialog.Title = "请选择文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog.FileName;
                    byte[] fileBytes = File.ReadAllBytes(fileName);
                    List<byte> listData = new List<byte>();
                    listData.AddRange(fileBytes);
                    int needAdd = 16 - (fileBytes.Length % 16);
                    if (needAdd != 0)
                    {
                        for (ushort i = 0; i < needAdd; i++)
                        {
                            listData.Add(0xFF);
                        }
                    }
                    FileDataBuffer = listData.ToArray();
                    BinFilePathBox.Text = fileName;
                    FileSize = (UInt32)FileDataBuffer.Length;
                    FileCrc = CrcInter.Crc16(FileDataBuffer, FileSize);
                }
            }
        }

        private void StartUpdateButton_Click(object sender, EventArgs e)
        {
            if ((FileSize > 0) && (OpenCanDevButton.Text == "关闭分析仪"))
            {
                while (UpdateRxQueue.TryTake(out uint[] _)) ;
                while (AppTxQueue.TryTake(out uint[] _)) ;
                if (canAppSendThread != null && canAppSendThread.IsAlive)
                {
                    CanSendCts.Cancel();
                    while (AppTxQueue.TryTake(out uint[] _)) ;
                    canAppSendThread.Join();
                }
                SendFunccode = 0;
                StartUpdateButton.Enabled = false;
                updateCts = new CancellationTokenSource();
                updateThread = new Thread(() => Update_Process(updateCts.Token));
                ChoseUpdateID = (byte)ChoseDevListBox.SelectedIndex;
                updateThread.Start();
            }
        }

        private unsafe void Can_Transmit(uint devid, byte[] data)
        {
            VCI_CAN_OBJ vci_can_obj = new VCI_CAN_OBJ
            {
                ID = devid,
                TimeStamp = 0,
                TimeFlag = 0,
                SendType = 0,
                RemoteFlag = 0,
                ExternFlag = 1,
                DataLen = (byte)data.Length
            };

            for (int i = 0; i < data.Length; i++)
            {
                vci_can_obj.Data[i] = data[i];
            }

            for (int i = 0; i < 3; i++)
            {
                vci_can_obj.Reserved[i] = 0;
            }
            uint ret = VCI_Transmit(VCI_USBCAN2, CanDevIndex, CanPassNum, ref vci_can_obj, 1);

            if (ret == 1)
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                string str = '[' + timestamp + ']' + "发→◇" + "ID:" + devid.ToString("X") + " Data:" + byteToHexStr(data) + Environment.NewLine;
                ShowStrRxQueue.Add(str);

            }

        }

        private void Iap_Req()
        {
            CANID_UNION id = new CANID_UNION() { IdFrame = 0 };
            byte[] data = new byte[8];
            id.DesId = ChoseUpdateID;
            id.SrcId = 0x3F;
            id.FuncCode = 0x19;
            data[0] = (byte)(FileSize & 0xFF);
            data[1] = (byte)((FileSize >> 8) & 0xFF);
            data[2] = (byte)((FileSize >> 16) & 0xFF);
            data[3] = (byte)((FileSize >> 24) & 0xFF);
            data[4] = FileCrc[0];
            data[5] = FileCrc[1];
            data[6] = 0;
            data[7] = 0;

            Can_Transmit(id.IdFrame, data);

        }

        private void Iap_Erase()
        {
            CANID_UNION id = new CANID_UNION() { IdFrame = 0 };
            byte[] data = new byte[8];
            id.DesId = ChoseUpdateID;
            id.SrcId = 0x3F;
            id.FuncCode = 0x29;
            data[0] = (byte)(FileSize & 0xFF);
            data[1] = (byte)((FileSize >> 8) & 0xFF);
            data[2] = (byte)((FileSize >> 16) & 0xFF);
            data[3] = (byte)((FileSize >> 24) & 0xFF);
            data[4] = FileCrc[0];
            data[5] = FileCrc[1];
            data[6] = 0;
            data[7] = 0;

            Can_Transmit(id.IdFrame, data);
        }

        private void Iap_Data(byte[] bindata, byte index)
        {
            CANID_UNION id = new CANID_UNION() { IdFrame = 0 };
            byte[] data = new byte[8];
            id.DesId = ChoseUpdateID;
            id.SrcId = 0x3F;
            id.Index = index;
            id.FuncCode = 0x39;

            Buffer.BlockCopy(bindata, 0, data, 0, 8);

            Can_Transmit(id.IdFrame, data);

        }

        private void Iap_Write(byte[] crc)
        {
            CANID_UNION id = new CANID_UNION() { IdFrame = 0 };
            byte[] data = new byte[8];
            id.DesId = ChoseUpdateID;
            id.SrcId = 0x3F;
            id.FuncCode = 0x49;
            data[0] = crc[0];
            data[1] = crc[1];
            data[2] = 0;
            data[3] = 0;
            data[4] = 0;
            data[5] = 0;
            data[6] = 0;
            data[7] = 0;

            Can_Transmit(id.IdFrame, data);
        }

        private void Iap_Done()
        {
            CANID_UNION id = new CANID_UNION() { IdFrame = 0 };
            byte[] data = new byte[8];
            id.DesId = ChoseUpdateID;
            id.SrcId = 0x3F;
            id.FuncCode = 0x59;
            Array.Clear(data, 0, data.Length);

            Can_Transmit(id.IdFrame, data);


        }

        private void showDataTimer_Tick(object sender, EventArgs e)
        {
            string str = null;
            string showstr = null;

            if (updateThread != null && updateThread.IsAlive == false && OpenCanDevButton.Text == "关闭分析仪")
            {
                if ((canAppSendThread == null) || (canAppSendThread.IsAlive == false))
                {
                    CanSendCts = new CancellationTokenSource();
                    canAppSendThread = new Thread(() => CanSendTask(CanSendCts.Token));
                    canAppSendThread.Start();
                }
            }

            while (ShowStrRxQueue.TryTake(out str))
            {
                showstr += str;
            }
            // 更新 UI
            if (showstr != null)
            {
                if (ShowRcvDataBox.InvokeRequired)
                {
                    ShowRcvDataBox.Invoke(new Action(() =>
                    {
                        ShowRcvDataBox.AppendText(showstr);
                    }));
                }
                else
                {
                    ShowRcvDataBox.AppendText(showstr);
                }
                showstr = null;
            }

        }

        private void CanSendTask(CancellationToken token)
        {
            uint[] data = new uint[9];
            byte[] sendData = new byte[8];
            uint[] querydata = new uint[9];
            CanAppId id = new CanAppId() { IdFrame = 0 };
            id.DesId = 0;
            id.SlaverFlag = 1;

            ReadSetting();

            while (!token.IsCancellationRequested)
            {
                if (AppTxQueue.TryTake(out data))
                {
                    for (uint i = 0; i < 8; i++)
                    {
                        sendData[i] = (byte)data[i + 1];
                    }

                    Can_Transmit(data[0], sendData);
                    Thread.Sleep(50);
                }
                else
                {
                    if (CheckSlaverStaBox.Checked == true)
                    {
                        id.ActFlag = 0;
                        id.FuncCode = (byte)SendFunccode;
                        Array.Clear(querydata, 0, querydata.Length);
                        if (SetFlag == 1)
                        {
                            if (SendFunccode == setFunccode)
                            {
                                SetFlag = 0;
                                id.ActFlag = 1;
                                for (uint i = 1; i < 9; i++)
                                {
                                    querydata[i] = Setbuff[i - 1];
                                }
                            }
                        }

                        querydata[0] = id.IdFrame;
                        AppTxQueue.Add(querydata);
                        if (SendFunccode < 8)
                        {
                            SendFunccode++;
                        }
                        else
                        {
                            SendFunccode = 0;
                        }
                    }
                    else
                    {
                        SetFlag = 0;
                        Thread.Sleep(1);
                    }
                }

            }

        }


        private void ModStaUpdateTimer_Tick(object sender, EventArgs e)
        {
            float fvalue = 0;
            float syscurr = 0;
            ushort u16value = 0;
            short i16value = 0;
            uint u32value = 0;
            uint OnlineNum = 0;
            uint ErrNum = 0;

            int deviceid = ChoseDevListBox.SelectedIndex;
            if (deviceid == 0)
            {
                return;
            }

            if (SlaverConnectSta[0] == 1)
            {
                OnlineNum++;
                Slave1Box.Checked = true;
                if (modstadata[0].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver1ErrBox.Checked = true;
                }
                else
                {
                    Slaver1ErrBox.Checked = false;
                }
                i16value = (short)modstadata[0].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver1CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave1Box.Checked = false;
                Slaver1ErrBox.Checked = false;
                Slaver1CurrBox.Text = "0";
            }

            if (SlaverConnectSta[1] == 1)
            {
                OnlineNum++;
                Slave2Box.Checked = true;
                if (modstadata[1].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver2ErrBox.Checked = true;
                }
                else
                {
                    Slaver2ErrBox.Checked = false;
                }
                i16value = (short)modstadata[1].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver2CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave2Box.Checked = false;
                Slaver2ErrBox.Checked = false;
                Slaver2CurrBox.Text = "0";

            }

            if (SlaverConnectSta[2] == 1)
            {
                OnlineNum++;
                Slave3Box.Checked = true;
                if (modstadata[2].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver3ErrBox.Checked = true;

                }
                else
                {
                    Slaver3ErrBox.Checked = false;
                }
                i16value = (short)modstadata[2].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver3CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave3Box.Checked = false;
                Slaver3ErrBox.Checked = false;
                Slaver3CurrBox.Text = "0";

            }

            if (SlaverConnectSta[3] == 1)
            {
                OnlineNum++;
                Slave4Box.Checked = true;
                if (modstadata[3].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver4ErrBox.Checked = true;

                }
                else
                {
                    Slaver4ErrBox.Checked = false;
                }
                i16value = (short)modstadata[3].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver4CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave4Box.Checked = false;
                Slaver4ErrBox.Checked = false;
                Slaver4CurrBox.Text = "0";

            }

            if (SlaverConnectSta[4] == 1)
            {
                OnlineNum++;
                Slave5Box.Checked = true;
                if (modstadata[4].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver5ErrBox.Checked = true;

                }
                else
                {
                    Slaver5ErrBox.Checked = false;
                }
                i16value = (short)modstadata[4].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver5CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave5Box.Checked = false;
                Slaver5ErrBox.Checked = false;
                Slaver5CurrBox.Text = "0";

            }

            if (SlaverConnectSta[5] == 1)
            {
                OnlineNum++;
                Slave6Box.Checked = true;
                if (modstadata[5].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver6ErrBox.Checked = true;

                }
                else
                {
                    Slaver6ErrBox.Checked = false;
                }
                i16value = (short)modstadata[5].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver6CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave6Box.Checked = false;
                Slaver6ErrBox.Checked = false;
                Slaver6CurrBox.Text = "0";

            }

            if (SlaverConnectSta[6] == 1)
            {
                OnlineNum++;
                Slave7Box.Checked = true;
                if (modstadata[6].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver7ErrBox.Checked = true;
                }
                else
                {
                    Slaver7ErrBox.Checked = false;
                }
                i16value = (short)modstadata[6].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver7CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave7Box.Checked = false;
                Slaver7ErrBox.Checked = false;
                Slaver7CurrBox.Text = "0";

            }

            if (SlaverConnectSta[7] == 1)
            {
                OnlineNum++;
                Slave8Box.Checked = true;
                if (modstadata[7].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver8ErrBox.Checked = true;
                }
                else
                {
                    Slaver8ErrBox.Checked = false;
                }
                i16value = (short)modstadata[7].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver8CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave8Box.Checked = false;
                Slaver8ErrBox.Checked = false;
                Slaver8CurrBox.Text = "0";
            }

            if (SlaverConnectSta[8] == 1)
            {
                OnlineNum++;
                Slave9Box.Checked = true;
                if (modstadata[8].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver9ErrBox.Checked = true;

                }
                else
                {
                    Slaver9ErrBox.Checked = false;
                }
                i16value = (short)modstadata[8].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver9CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave9Box.Checked = false;
                Slaver9ErrBox.Checked = false;
                Slaver9CurrBox.Text = "0";
            }

            if (SlaverConnectSta[9] == 1)
            {
                OnlineNum++;
                Slave10Box.Checked = true;
                if (modstadata[9].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver10ErrBox.Checked = true;

                }
                else
                {
                    Slaver10ErrBox.Checked = false;
                }
                i16value = (short)modstadata[9].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver10CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave10Box.Checked = false;
                Slaver10ErrBox.Checked = false;
                Slaver10CurrBox.Text = "0";
            }

            if (SlaverConnectSta[10] == 1)
            {
                OnlineNum++;
                Slave11Box.Checked = true;
                if (modstadata[10].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver11ErrBox.Checked = true;

                }
                else
                {
                    Slaver11ErrBox.Checked = false;
                }
                i16value = (short)modstadata[10].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver11CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave11Box.Checked = false;
                Slaver11ErrBox.Checked = false;
                Slaver11CurrBox.Text = "0";
            }

            if (SlaverConnectSta[11] == 1)
            {
                OnlineNum++;
                Slave12Box.Checked = true;
                if (modstadata[11].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver12ErrBox.Checked = true;

                }
                else
                {
                    Slaver12ErrBox.Checked = false;
                }
                i16value = (short)modstadata[11].REG.u16_Idcout;
                fvalue = i16value * 1.0f;
                Slaver12CurrBox.Text = fvalue.ToString("F1");
                syscurr += fvalue;
            }
            else
            {
                Slave12Box.Checked = false;
                Slaver12ErrBox.Checked = false;
                Slaver12CurrBox.Text = "0";
            }

            SysCurrBox.Text = syscurr.ToString("F1");
            OnlinerNumBox.Text = OnlineNum.ToString();
            ErrNumBox.Text = ErrNum.ToString();

            if (ReadSettingFinish == 1)
            {
                ReadSettingFinish = 0;
                CMD_WD cmd = new CMD_WD() { all = 0 };
                fvalue = (float)(modsetdata[deviceid - 1].REG.i32_MaxIout * 0.1f);
                MaxCurrBox.Text = fvalue.ToString("F1");

                u16value = (ushort)(modsetdata[deviceid - 1].REG.i16_MaxVout * 0.1f);
                MaxVoltBox.Text = u16value.ToString();

                u16value = (ushort)(modsetdata[deviceid - 1].REG.i16_MaxPout * 0.1f);
                MaxPowerBox.Text = u16value.ToString();

                switch (modsetdata[deviceid - 1].REG.u16_OutputMode)
                {
                    case 0:
                        {
                            CCButton.Checked = true;
                        }
                        break;

                    case 1:
                        {
                            CVButton.Checked = true;

                        }
                        break;

                    case 2:
                        {
                            CPButton.Checked = true;

                        }
                        break;

                }

                u16value = (ushort)(modsetdata[deviceid - 1].REG.u16_SlopeIcc * 0.1);
                CurrStepBox.Text = u16value.ToString();

                u16value = (ushort)(modsetdata[deviceid - 1].REG.u16_SlopeVcv * 0.1);
                VoltStepBox.Text = u16value.ToString();

                u16value = (ushort)(modsetdata[deviceid - 1].REG.u16_SlopePcp * 0.1);
                PowerStepBox.Text = u16value.ToString();

                cmd.all = modsetdata[deviceid - 1].REG.u16_CmdWd.all;

                if (cmd.PfcOL == 1)
                {
                    DabOLEnBox.Checked = true;
                }
                else
                {
                    DabOLEnBox.Checked = false;
                }

                if (cmd.DcOL == 1)
                {
                    DCDcOlEnBox.Checked = true;
                }
                else
                {
                    DCDcOlEnBox.Checked = false;
                }

            }


            u16value = modstadata[deviceid - 1].REG.u16_Vab;
            fvalue = 0.1f * u16value;
            VabBox.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Vbc;
            fvalue = 0.1f * u16value;
            VbcBox.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Vca;
            fvalue = 0.1f * u16value;
            VacBox.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Freq;
            fvalue = 0.1f * u16value;
            FgBox.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Ia;
            fvalue = 0.1f * i16value;
            IaBox.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Ib;
            fvalue = 0.1f * i16value;
            IbBox.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Ic;
            fvalue = 0.1f * i16value;
            IcBox.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Iai1;
            fvalue = 0.1f * i16value;
            Ia1Box.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Iai2;
            fvalue = 0.1f * i16value;
            Ia2Box.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Ibi1;
            fvalue = 0.1f * i16value;
            Ib1Box.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Ibi2;
            fvalue = 0.1f * i16value;
            Ib2Box.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Ici1;
            fvalue = 0.1f * i16value;
            Ic1Box.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Ici2;
            fvalue = 0.1f * i16value;
            Ic2Box.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Vbus;
            fvalue = 0.1f * u16value;
            VbusBox.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Idabpri1;
            fvalue = 0.1f * u16value; ;
            Ipri1Box.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Idabpri2;
            fvalue = 0.1f * u16value; ;
            Ipri2Box.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Idabout1;
            fvalue = 1.0f * i16value;
            Iout1Box.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Idabout2;
            fvalue = 1.0f * i16value;
            Iout2Box.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Vout;
            fvalue = 0.01f * u16value;
            VoutBox.Text = fvalue.ToString("F2");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Idcout;
            fvalue = 1.0f * i16value;
            IoutBox.Text = fvalue.ToString("F1");

            switch (modstadata[deviceid - 1].REG.u16_WorkMode)
            {
                case 0:
                    {
                        WorkModeBox.Text = "上电模式";

                    }
                    break;

                case 1:
                    {
                        WorkModeBox.Text = "待机模式";

                    }
                    break;

                case 2:
                    {
                        WorkModeBox.Text = "软起模式";
                    }
                    break;

                case 3:
                    {
                        WorkModeBox.Text = "工作模式";

                    }
                    break;

                case 4:
                    {
                        WorkModeBox.Text = "工作模式";

                    }
                    break;

                case 5:
                    {
                        WorkModeBox.Text = "工作模式";

                    }
                    break;

                case 6:
                    {
                        WorkModeBox.Text = "故障模式";

                    }
                    break;

                default: { } break;
            }

            u16value = modstadata[deviceid - 1].REG.u16_Pin;
            fvalue = 0.1f * u16value;
            PinBox.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Pinact;
            fvalue = 0.1f * u16value;
            PinactBox.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Pout;
            fvalue = 0.1f * i16value;
            PoutBox.Text = fvalue.ToString("F1");

            i16value = modstadata[deviceid - 1].REG.i16_Temp1;
            fvalue = 0.1f * i16value;
            Temp1Box.Text = fvalue.ToString("F1");

            i16value = modstadata[deviceid - 1].REG.i16_Temp2;
            fvalue = 0.1f * i16value;
            Temp2Box.Text = fvalue.ToString("F1");

            i16value = modstadata[deviceid - 1].REG.i16_Temp3;
            fvalue = 0.1f * i16value;
            Temp3Box.Text = fvalue.ToString("F1");

            i16value = modstadata[deviceid - 1].REG.i16_Temp4;
            fvalue = 0.1f * i16value;
            Temp4Box.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Addr;
            AddrBox.Text = u16value.ToString();

            u16value = modstadata[deviceid - 1].REG.u16_State;
            StaCodeBox.Text = u16value.ToString();

            u32value = modstadata[deviceid - 1].REG.u32_ErrCode;
            ErrCodeBox.Text = u32value.ToString();

            u32value = modstadata[deviceid - 1].REG.u32_Version;

            VersionBox.Text = (u32value / 1000 / 1000).ToString() + "." + (u32value / 1000 % 10000).ToString() + "." + (u32value % 10000).ToString();

        }

        private void ReadRegFunc(uint offset, uint reg_num)
        {
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            id.SlaverFlag = 1;
            id.ActFlag = 1;
            id.DesId = 0;
            id.FuncCode = 0x21;
            data[0] = id.IdFrame;
            data[1] = offset;
            data[2] = reg_num;
            AppTxQueue.Add(data);
        }

        private void ReadSetting()
        {
            ReadRegFunc(128, 3);
            ReadRegFunc(131, 3);
            ReadRegFunc(134, 3);
        }

        private void ReadSettingButton_Click(object sender, EventArgs e)
        {
            ReadSetting();
        }

        private void ResetCmdBitUsedOnce()
        {
            for (int i = 0; i < SystemSlaverNum; i++)
            {
                ushort SetCmdMask = 0x00CE;
                modsetdata[i].REG.u16_CmdWd.all &= (ushort)~SetCmdMask;
            }

        }

        unsafe private void SetParaMap(byte FuncCode, byte Offset, string StSetValue,float Format, bool Data32Bit)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            SetFlag = 1;
            setFunccode = FuncCode;
            float fvalue = 0;
            ushort u16value = 0;
            uint u32value = 0;

            if (float.TryParse(StSetValue, out fvalue))
            {
                fvalue = fvalue / Format;
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            for (int i = 0; i < SystemSlaverNum; i++)
            {
                if (!Data32Bit)
                {
                    u16value = (ushort)fvalue;
                    modsetdata[i].buff[setFunccode * 4 + Offset] = u16value;
                }
                else
                {
                    u32value = (uint)fvalue;
                    modsetdata[i].buff[setFunccode * 4 + Offset] = (ushort)(u32value & 0xFFFF);
                    modsetdata[i].buff[setFunccode * 4 + Offset + 1] = (ushort)(u32value >> 16);
                }
            }

            int index = 0;
            for (int i = 0; i < SystemSlaverNum; i++)
            {
                if (SlaverConnectSta[i] == 1)
                {
                    index = i;
                    break;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Setbuff[i * 2] = (uint)(modsetdata[index].buff[setFunccode * 4 + i] >> 8);
                Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[setFunccode * 4 + i] & 0xFF);
            }

            //ResetCmdBitUsedOnce();

        }

        unsafe private void SetParaSpe(byte Offset, string StSetValue, float Format, bool Data32Bit,bool Brdct)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;
            uint u32value = 0;
            int index = 0;

            if (float.TryParse(StSetValue, out fvalue))
            {
                fvalue = fvalue / Format;
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            if (!Brdct)
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                index = ChoseDevListBox.SelectedIndex;
            }
            else
            {
                for (int i = 0; i < SystemSlaverNum; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }
            }
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = Offset;
            if (!Data32Bit)
            {
                data[2] = 1;
                u16value = (ushort)fvalue;
                if (!Brdct)
                {
                    modsetdata[index - 1].buff[Offset - 128] = u16value;
                }
                else
                {
                    for (int i = 0; i < SystemSlaverNum; i++)
                    {
                        modsetdata[i].buff[Offset - 128] = u16value;
                    }
                }
                data[3] = (uint)(u16value >> 8);
                data[4] = (uint)(u16value & 0xFF);
            }
            else
            {
                data[2] = 2;
                u32value = (uint)fvalue;
                if (!Brdct)
                {
                    modsetdata[index - 1].buff[Offset - 128] = (ushort)(u32value & 0xFFFF);
                    modsetdata[index - 1].buff[Offset - 128 + 1] = (ushort)(u32value >> 16);
                }
                else
                {
                    for (int i = 0; i < SystemSlaverNum; i++)
                    {
                        modsetdata[i].buff[Offset - 128] = (ushort)(u32value & 0xFFFF);
                        modsetdata[i].buff[Offset - 128 + 1] = (ushort)(u32value >> 16);
                    }
                }
                data[3] = (uint)((u32value >> 8) & 0xFF);
                data[4] = (uint)(u32value & 0xFF);
                data[5] = (uint)((u32value >> 24) & 0xFF);
                data[6] = (uint)((u32value >> 16) & 0xFF);
            }
            AppTxQueue.Add(data);

            //ResetCmdBitUsedOnce();
        }

        private unsafe void PowerOnButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };

            cmd.PowerOnOff = 1;
            if (DabOLEnBox.Checked)
            {
                cmd.PfcOL = 1;
            }

            if (BrdctSendBox.Checked)
            {
                SetParaMap(0x02,0, cmd.all.ToString(), 1,false);
            }
            else
            {
                SetParaSpe(136, cmd.all.ToString(), 1,false,false);
            }
            ResetCmdBitUsedOnce();
        }

        private unsafe void PowerOffButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };

            if (DabOLEnBox.Checked)
            {
                cmd.PfcOL = 1;
            }

            if (BrdctSendBox.Checked)
            {              
                SetParaMap(0x02, 0, cmd.all.ToString(), 1, false);

            }
            else
            {
                SetParaSpe(136, cmd.all.ToString(), 1, false, false);
            }
            ResetCmdBitUsedOnce();
        }

        private unsafe void SetMaxCurrButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(0, 0, MaxCurrBox.Text.ToString(), 0.1f, true);
            }
            else
            {
                SetParaSpe(128, MaxCurrBox.Text.ToString(), 0.1f, true, false);
            }
        }

        private unsafe void SetMaxVoltButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(0, 2, MaxVoltBox.Text.ToString(), 0.1f, false);
            }
            else
            {
                SetParaSpe(130, MaxVoltBox.Text.ToString(), 0.1f, false, false);
            }

        }

        private unsafe void SetMaxPowerButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(0, 3, MaxPowerBox.Text.ToString(), 0.1f, false);
            }
            else
            {
                SetParaSpe(131, MaxPowerBox.Text.ToString(), 0.1f, false, false);
            }
        }

        private unsafe void SetCurrStepButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(1, 1, CurrStepBox.Text.ToString(), 0.1f, false);
            }
            else
            {
                SetParaSpe(133, CurrStepBox.Text.ToString(), 0.1f, false, false);
            }

        }

        private unsafe void SetVoltStepButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(1, 2, VoltStepBox.Text.ToString(), 0.1f, false);
            }
            else
            {
                SetParaSpe(134, VoltStepBox.Text.ToString(), 0.1f, false, false);
            }
        }

        private unsafe void SetPowerStepButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(1, 3, PowerStepBox.Text.ToString(), 0.1f, false);
            }
            else
            {
                SetParaSpe(135, PowerStepBox.Text.ToString(), 0.1f, false, false);
            }
        }

        private unsafe void ClearErrButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };

            cmd.ClearErr = 1;

            if (DabOLEnBox.Checked)
            {
                cmd.PfcOL = 1;
            }

            if (BrdctSendBox.Checked)
            {
                SetParaMap(0x02, 0, cmd.all.ToString(), 1, false);
            }
            else
            {
                SetParaSpe(136, cmd.all.ToString(), 1, false, false);
            }
        }

        unsafe private void DabOLEnBox_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            int index = 0;
            if (BrdctSendBox.Checked == true)
            {                
                for (int i = 0; i < SystemSlaverNum; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }                

            }
            else
            {
                index = ChoseDevListBox.SelectedIndex;
                
            }
            if (modsetdata[index].REG.u16_CmdWd.PowerOnOff == 1)
            {
                if (DabOLEnBox.Checked)
                {
                    DabOLEnBox.Checked = false;
                }
                else
                {
                    DabOLEnBox.Checked = true;
                }
                MessageBox.Show("请点击关机后操作", "警告!");
                return;
            }
            else
            {
                PowerOffButton_Click(sender, e);
            }
        }

        unsafe private void CCButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(1, 0, "0", 1, false);
            }
            else
            {
                SetParaSpe(132, "0", 1, false, false);
            }
        }

        unsafe private void CVButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(1, 0, "1", 1, false);
            }
            else
            {
                SetParaSpe(132, "1", 1, false, false);
            }
        }

        unsafe private void CPButton_Click(object sender, EventArgs e)
        {
            if (BrdctSendBox.Checked)
            {
                SetParaMap(1, 0, "2", 1, false);
            }
            else
            {
                SetParaSpe(132, "2", 1, false, false);
            }
        }

        unsafe private void DCDcOlEnBox_Click(object sender, EventArgs e)
        {
            
        }


        private void Update_Process(CancellationToken token)
        {
            byte UpdateState = 0;
            CANID_UNION id = new CANID_UNION();
            uint[] frame = null;
            while (!token.IsCancellationRequested)
            {

                switch (UpdateState)
                {
                    case 0:
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Iap_Req();
                                frame = null;
                                UpdateRxQueue.TryTake(out frame, 1000);
                                if (frame != null && frame.Length == 9)
                                {
                                    UpdateState = 1;
                                    break;
                                }

                                if (i == 2)
                                {
                                    Invoke((Action)(() =>
                                    {

                                        MessageBox.Show("未找到目标设备", "错误!");
                                        StartUpdateButton.Enabled = true;
                                    }));
                                    return;
                                }
                            }

                        }
                        break;

                    case 1:
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Iap_Erase();
                                frame = null;
                                UpdateRxQueue.TryTake(out frame, 1000);
                                if (frame != null)
                                {
                                    id.IdFrame = frame[0];
                                    if (id.AckSts == 0)
                                    {
                                        UpdateState = 2;
                                        break;
                                    }
                                    else
                                    {
                                        if (i == 2)
                                        {
                                            Invoke((Action)(() =>
                                            {

                                                MessageBox.Show("擦除失败", "错误!");
                                                StartUpdateButton.Enabled = true;
                                            }));
                                            return;
                                        }

                                    }
                                }

                                if (i == 2)
                                {
                                    Invoke((Action)(() =>
                                    {

                                        MessageBox.Show("目标设备已失去连接", "错误!");
                                        StartUpdateButton.Enabled = true;
                                    }));
                                    return;
                                }
                            }

                        }
                        break;

                    case 2:
                        {
                            int Group = (int)(FileSize / 2048);
                            int remainSize = (int)(FileSize % 2048);
                            if (remainSize > 0)
                            {
                                Invoke((Action)(() =>
                                {
                                    UpdateProgressBar.Maximum = (int)(Group + 1);
                                }));
                            }
                            else
                            {
                                Invoke((Action)(() =>
                                {
                                    UpdateProgressBar.Maximum = (int)Group;
                                }));
                            }

                            int groupIndex = 0;
                            for (groupIndex = 0; groupIndex < Group; groupIndex++)
                            {
                                byte[] groupData = FileDataBuffer.Skip(groupIndex * 2048).Take(2048).ToArray();
                                for (int i = 0; i < 3; i++)
                                {
                                    for (int k = 0; k < 256; k++)
                                    {
                                        byte[] sendData = groupData.Skip(k * 8).Take(8).ToArray();
                                        Iap_Data(sendData, (byte)k);
                                    }
                                    frame = null;
                                    UpdateRxQueue.TryTake(out frame, 5);
                                    if (frame == null)
                                    {
                                        break;
                                    }
                                    if (i == 2)
                                    {
                                        Invoke((Action)(() =>
                                        {

                                            MessageBox.Show("数据发送错误", "错误!");
                                            StartUpdateButton.Enabled = true;
                                            UpdateProgressBar.Value = 0;
                                        }));
                                        return;
                                    }
                                }
                                byte[] crc = CrcInter.Crc16(groupData, 2048);
                                for (uint i = 0; i < 3; i++)
                                {
                                    Iap_Write(crc);
                                    frame = null;
                                    UpdateRxQueue.TryTake(out frame, 1000);
                                    if (frame != null)
                                    {
                                        id.IdFrame = frame[0];
                                        if (id.AckSts != 0)
                                        {
                                            Invoke((Action)(() =>
                                            {

                                                MessageBox.Show("烧录失败", "错误!");
                                                StartUpdateButton.Enabled = true;
                                                UpdateProgressBar.Value = 0;
                                            }));
                                            return;
                                        }
                                        else
                                        {
                                            Invoke((Action)(() =>
                                            {
                                                UpdateProgressBar.Value = groupIndex + 1;
                                            }));
                                            break;
                                        }

                                    }
                                    else
                                    {
                                        if (i == 2)
                                        {
                                            Invoke((Action)(() =>
                                            {

                                                MessageBox.Show("目标设备已失去连接", "错误!");
                                                StartUpdateButton.Enabled = true;
                                                UpdateProgressBar.Value = 0;
                                            }));
                                            return;
                                        }

                                    }
                                }

                            }

                            if (remainSize > 0)
                            {
                                byte[] groupData = FileDataBuffer.Skip(groupIndex * 2048).Take(remainSize).ToArray();
                                for (int i = 0; i < 3; i++)
                                {
                                    for (int k = 0; k < remainSize / 8; k++)
                                    {
                                        byte[] sendData = groupData.Skip(k * 8).Take(8).ToArray();
                                        Iap_Data(sendData, (byte)k);
                                    }
                                    frame = null;
                                    UpdateRxQueue.TryTake(out frame, 5);
                                    if (frame == null)
                                    {
                                        break;
                                    }
                                    if (i == 2)
                                    {
                                        Invoke((Action)(() =>
                                        {

                                            MessageBox.Show("数据发送错误", "错误!");
                                            StartUpdateButton.Enabled = true;
                                            UpdateProgressBar.Value = 0;
                                        }));
                                        return;
                                    }
                                }
                                byte[] crc = CrcInter.Crc16(groupData, (uint)remainSize);
                                for (uint i = 0; i < 3; i++)
                                {
                                    Iap_Write(crc);
                                    frame = null;
                                    UpdateRxQueue.TryTake(out frame, 1000);
                                    if (frame != null)
                                    {
                                        id.IdFrame = frame[0];
                                        if (id.AckSts != 0)
                                        {
                                            Invoke((Action)(() =>
                                            {

                                                MessageBox.Show("烧录失败", "错误!");
                                                StartUpdateButton.Enabled = true;
                                                UpdateProgressBar.Value = 0;
                                            }));
                                            return;
                                        }
                                        else
                                        {
                                            Invoke((Action)(() =>
                                            {
                                                UpdateProgressBar.Value = groupIndex + 1;
                                            }));
                                            break;
                                        }

                                    }
                                    else
                                    {
                                        if (i == 2)
                                        {
                                            Invoke((Action)(() =>
                                            {

                                                MessageBox.Show("目标设备已失去连接", "错误!");
                                                StartUpdateButton.Enabled = true;
                                                UpdateProgressBar.Value = 0;
                                            }));
                                            return;
                                        }

                                    }
                                }
                            }
                            UpdateState = 3;

                        }
                        break;

                    case 3:
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Iap_Done();
                                frame = null;
                                UpdateRxQueue.TryTake(out frame, 1000);
                                if (frame != null)
                                {
                                    id.IdFrame = frame[0];
                                    if (id.AckSts == 0)
                                    {
                                        Invoke((Action)(() =>
                                        {

                                            MessageBox.Show("固件烧录成功", "提示!");
                                            StartUpdateButton.Enabled = true;
                                            UpdateProgressBar.Value = 0;
                                        }));
                                        return;
                                    }
                                    else
                                    {
                                        Invoke((Action)(() =>
                                        {

                                            MessageBox.Show("校验失败", "错误!");
                                            StartUpdateButton.Enabled = true;
                                            UpdateProgressBar.Value = 0;
                                        }));
                                        return;
                                    }
                                }

                                if (i == 2)
                                {
                                    Invoke((Action)(() =>
                                    {

                                        MessageBox.Show("目标设备已失去连接", "错误!");
                                        StartUpdateButton.Enabled = true;
                                        UpdateProgressBar.Value = 0;
                                    }));
                                    return;
                                }


                            }

                        }
                        break;

                    default: break;

                }

                Thread.Sleep(1);
            }

        }
    }
}
