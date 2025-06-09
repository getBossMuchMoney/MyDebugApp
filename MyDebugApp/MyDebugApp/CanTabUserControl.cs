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

unsafe public struct VCI_BOARD_INFO//使用不安全代码
{
    public UInt16 hw_Version;
    public UInt16 fw_Version;
    public UInt16 dr_Version;
    public UInt16 in_Version;
    public UInt16 irq_Num;
    public byte can_Num;

    public fixed byte str_Serial_Num[20];
    public fixed byte str_hw_Type[40];
    public fixed byte Reserved[8];
}

/////////////////////////////////////////////////////
//2.定义CAN信息帧的数据类型。
unsafe public struct VCI_CAN_OBJ  //使用不安全代码
{
    public uint ID;
    public uint TimeStamp;        //时间标识
    public byte TimeFlag;         //是否使用时间标识
    public byte SendType;         //发送标志。保留，未用
    public byte RemoteFlag;       //是否是远程帧
    public byte ExternFlag;       //是否是扩展帧
    public byte DataLen;          //数据长度
    public fixed byte Data[8];    //数据
    public fixed byte Reserved[3];//保留位

}

//3.定义初始化CAN的数据类型
public struct VCI_INIT_CONFIG
{
    public UInt32 AccCode;
    public UInt32 AccMask;
    public UInt32 Reserved;
    public byte Filter;   //0或1接收所有帧。2标准帧滤波，3是扩展帧滤波。
    public byte Timing0;  //波特率参数，具体配置，请查看二次开发库函数说明书。
    public byte Timing1;
    public byte Mode;     //模式，0表示正常模式，1表示只听模式,2自测模式
}

/*------------其他数据结构描述---------------------------------*/
//4.USB-CAN总线适配器板卡信息的数据类型1，该类型为VCI_FindUsbDevice函数的返回参数。
public struct VCI_BOARD_INFO1
{
    public UInt16 hw_Version;
    public UInt16 fw_Version;
    public UInt16 dr_Version;
    public UInt16 in_Version;
    public UInt16 irq_Num;
    public byte can_Num;
    public byte Reserved;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public byte[] str_Serial_Num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] str_hw_Type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] str_Usb_Serial;
}

[StructLayout(LayoutKind.Explicit)]
public struct CANID_UNION
{
    [FieldOffset(0)]
    public uint IdFrame;

    [FieldOffset(0)]
    private BitFields BIT;

    public byte AckSts
    {
        get => (byte)(IdFrame & 0x03U);
        set => IdFrame = (IdFrame & ~0x03U) | ((uint)value & 0x03U);
    }

    public byte FuncCode
    {
        get => (byte)((IdFrame >> 2) & 0x7FU);
        set => IdFrame = (IdFrame & ~(0x7FU << 2)) | (((uint)value & 0x7FU) << 2);
    }

    public byte DesId
    {
        get => (byte)((IdFrame >> 9) & 0x3FU);
        set => IdFrame = (IdFrame & ~(0x3FU << 9)) | (((uint)value & 0x3FU) << 9);
    }

    public byte SrcId
    {
        get => (byte)((IdFrame >> 15) & 0x3FU);
        set => IdFrame = (IdFrame & ~(0x3FU << 15)) | (((uint)value & 0x3FU) << 15);
    }

    public byte Index
    {
        get => (byte)((IdFrame >> 21) & 0xFFU);
        set => IdFrame = (IdFrame & ~(0xFFU << 21)) | (((uint)value & 0xFFU) << 21);
    }

    //public byte Resv
    //{
    //    get => (byte)((IdFrame >> 31) & 0x07U);
    //    set => IdFrame = (IdFrame & ~(0x07U << 31)) | (((uint)value & 0x07U) << 31);
    //}

    // 辅助结构体（不实际使用，仅用于占位）
    [StructLayout(LayoutKind.Sequential)]
    private struct BitFields { }
}



/*------------数据结构描述完成---------------------------------*/



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
        uint CanDevIndex, CanPassNum = 0;
        byte[] FileDataBuffer;
        byte[] FileCrc = new byte[2];
        UInt32 FileSize = 0;
        ModbusCrc CrcInter = new ModbusCrc();
        BlockingCollection<uint[]> RxQueue = new BlockingCollection<uint[]>(new ConcurrentQueue<uint[]>());
        BlockingCollection<string> ShowStrRxQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
        byte ChoseUpdateID = 0;
        CancellationTokenSource updateCts = new CancellationTokenSource();
        CancellationTokenSource showDataCts = new CancellationTokenSource();
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

            CanDevPassNumBox.SelectedIndex = 0;
            CanBandListBox.SelectedIndex = 1;
            ChoseDevListBox.SelectedIndex = 1;

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

        private void OpenCanDevButton_Click(object sender, EventArgs e)
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
                            canRcvThread = new Thread(CanData_Recieve);
                            canRcvThread.Start();
                        }
                    }

                }

            }
            else
            {
                OpenCanDevButton.Text = "打开分析仪";
                if (updateThread != null && updateThread.IsAlive)
                {
                    updateCts.Cancel();
                    updateThread.Join();
                }
                try
                {
                    canRcvThread.Abort();
                }
                catch (Exception) { }
                VCI_CloseDevice(VCI_USBCAN2, (uint)CanDevListBox.SelectedIndex);
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

        private unsafe void CanData_Recieve()
        {
            uint ret = 0, id = 0;
            CANID_UNION msgID = new CANID_UNION() { IdFrame = 0};
            while (true)
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
                            if ((msgID.DesId == 0x3F) && ((msgID.FuncCode & 0x0F) == 0x09))
                            {
                                uint[] msbuffer = new uint[CanRcvBuff[i].DataLen + 1];
                                msbuffer[0] = id;
                                for (uint k = 1; k < (CanRcvBuff[i].DataLen + 1); k++)
                                {
                                    msbuffer[k] = data[k - 1];
                                }
                                RxQueue.Add(msbuffer);
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
            CANID_UNION id = new CANID_UNION() { IdFrame = 0};
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
                StartUpdateButton.Enabled = false;
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
            CANID_UNION id = new CANID_UNION() { IdFrame = 0};
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
            CANID_UNION id = new CANID_UNION() { IdFrame = 0};
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
            CANID_UNION id = new CANID_UNION() { IdFrame = 0};
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
            CANID_UNION id = new CANID_UNION() {IdFrame = 0 };
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
            CANID_UNION id = new CANID_UNION() { IdFrame = 0};
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
                                RxQueue.TryTake(out frame, 1000);
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
                                RxQueue.TryTake(out frame, 1000);
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
                                    RxQueue.TryTake(out frame, 5);
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
                                    RxQueue.TryTake(out frame, 1000);
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
                                    RxQueue.TryTake(out frame, 5);
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
                                    RxQueue.TryTake(out frame, 1000);
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
                                RxQueue.TryTake(out frame, 1000);
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
