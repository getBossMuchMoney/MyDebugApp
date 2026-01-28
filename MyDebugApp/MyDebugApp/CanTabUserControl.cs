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


public struct MOD_STA
{
    public ushort u16_Vab;
    public ushort u16_Vbc;
    public ushort u16_Vca;
    public ushort u16_Ia;
    public ushort u16_Ib;
    public ushort u16_Ic;
    public ushort u16_Iai1;
    public ushort u16_Ibi1;
    public ushort u16_Ici1;
    public ushort u16_Iai2;
    public ushort u16_Ibi2;
    public ushort u16_Ici2;
    public ushort u16_Vbus;
    public ushort u16_Resv13;
    public ushort u16_Resv14;
    public ushort u16_Iout1;
    public ushort u16_Vout2;
    public ushort u16_Freq;
    public ushort u16_Idcout;
    public ushort u16_Vout;
    public ushort u16_Pin;
    public ushort u16_Pinact;
    public ushort u16_Pout;
    public short i16_Temp1;
    public short i16_Temp2;
    public short i16_Temp3;
    public short i16_Temp4;
    public ushort u16_Addr;
    public ushort u16_WorkMode;
    public ushort u16_State;

    public uint u32_ErrCode;
    public uint u32_Version;
    public uint u32_Resv;

}

[StructLayout(LayoutKind.Explicit)]
unsafe public struct U_MOD_STA
{
    [FieldOffset(0)]
    public MOD_STA REG;           // 结构体部分

    [FieldOffset(0)]
    public fixed ushort buff[36]; // 固定大小的 uint16_t 数组
}


[StructLayout(LayoutKind.Explicit)]
public struct CMD_WD
{
    [FieldOffset(0)]
    public ushort all;

    [FieldOffset(0)]
    private BitFields BIT;

    public ushort PowerOnOff
    {
        get => (byte)(all & 0x01U);
        set => all = (ushort)((all & ~0x01U) | (value & 0x01U));
    }

    public ushort Resv
    {
        get => (byte)((all >> 1) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 1)) | ((value & 0x01U) << 1));
    }

    public ushort ClearErr
    {
        get => (byte)((all >> 2) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 2)) | ((value & 0x01U) << 2));
    }

    public ushort Boot
    {
        get => (byte)((all >> 3) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 3)) | ((value & 0x01U) << 3));
    }

    public ushort PfcOL
    {
        get => (byte)((all >> 4) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 4)) | ((value & 0x01U) << 4));
    }

    public ushort DcOL
    {
        get => (byte)((all >> 5) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 5)) | ((value & 0x01U) << 5));
    }

    public ushort DataSave
    {
        get => (byte)((all >> 6) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 6)) | ((value & 0x01U) << 6));
    }

    // 辅助结构体（不实际使用，仅用于占位）
    [StructLayout(LayoutKind.Sequential)]
    private struct BitFields { }
}

public struct MOD_SET
{
    public ushort u16_MaxIout;
    public ushort u16_MaxVout;
    public ushort u16_MaxPout;
    public ushort u16_OutputMode;
    public ushort u16_SlopeIcc;
    public ushort u16_SlopeVcv;
    public ushort u16_SlopePcp;
    public CMD_WD u16_CmdWd;
}

[StructLayout(LayoutKind.Explicit)]
unsafe public struct U_MOD_SET
{
    [FieldOffset(0)]
    public MOD_SET REG;           // 结构体部分

    [FieldOffset(0)]
    public fixed ushort buff[8]; // 固定大小的 uint16_t 数组
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



[StructLayout(LayoutKind.Explicit)]
public struct CanAppId
{
    [FieldOffset(0)]
    public uint IdFrame;

    [FieldOffset(0)]
    private BitFields BIT;

    public byte AckSts
    {
        get => (byte)((IdFrame >> 4) & 0x07U);
        set => IdFrame = (IdFrame & ~(0x07U << 4)) | (((uint)value & 0x07U) << 4);
    }

    public byte ActFlag
    {
        get => (byte)((IdFrame >> 7) & 0x01U);
        set => IdFrame = (IdFrame & ~(0x01U << 7)) | (((uint)value & 0x01U) << 7);
    }

    public byte FuncCode
    {
        get => (byte)((IdFrame >> 8) & 0xFFU);
        set => IdFrame = (IdFrame & ~(0xFFU << 8)) | (((uint)value & 0xFFU) << 8);
    }

    public byte DesId
    {
        get => (byte)((IdFrame >> 16) & 0x3FU);
        set => IdFrame = (IdFrame & ~(0x3FU << 16)) | (((uint)value & 0x3FU) << 16);
    }

    public byte SlaverFlag
    {
        get => (byte)((IdFrame >> 22) & 0x01U);
        set => IdFrame = (IdFrame & ~(0x01U << 22)) | (((uint)value & 0x01U) << 22);
    }

    public byte SrcId
    {
        get => (byte)((IdFrame >> 23) & 0x3FU);
        set => IdFrame = (IdFrame & ~(0x3FU << 23)) | (((uint)value & 0x3FU) << 23);
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
        U_MOD_STA[] modstadata = new U_MOD_STA[15];
        U_MOD_SET[] modsetdata = new U_MOD_SET[15];
        uint ReadSettingFinish = 0;
        uint[] DevOfflineCheckCnt = new uint[15];
        uint[] SlaverConnectSta = new uint[15];

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
            CanBandListBox.SelectedIndex = 0;
            ChoseDevListBox.SelectedIndex = 1;
            Thread dealModDataThread = new Thread(() => DealModAnsw(DealModCts.Token)); 
            dealModDataThread.Start();

        }
  
        private unsafe void DealModAnsw(CancellationToken token)
        {
            uint[] data = new uint[9];
            CanAppId id = new CanAppId();
            Array.Clear(DevOfflineCheckCnt,0, DevOfflineCheckCnt.Length);
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
                                    modsetdata[id.SrcId - 1].buff[data[1] + i] = (ushort)((data[i * 2 + 3] << 8) | (data[i * 2 + 4]));
                                }

                                if (data[1] == 6)
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
                    for (int i = 0; i < 15; i++)
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
                            if (msgID1.DesId == 0 && msgID1.SrcId > 0 && msgID1.SrcId <= 15)
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
                fvalue = i16value * 0.01f;
                Slaver1CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver2CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver3CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver4CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver5CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver6CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver7CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver8CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver9CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver10CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver11CurrBox.Text = fvalue.ToString("F2");
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
                fvalue = i16value * 0.01f;
                Slaver12CurrBox.Text = fvalue.ToString("F2");
                syscurr += fvalue;
            }
            else
            {
                Slave12Box.Checked = false;
                Slaver12ErrBox.Checked = false;
                Slaver12CurrBox.Text = "0";
            }


            if (SlaverConnectSta[12] == 1)
            {
                OnlineNum++;
                Slave13Box.Checked = true;
                if (modstadata[12].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver13ErrBox.Checked = true;

                }
                else
                {
                    Slaver13ErrBox.Checked = false;
                }
                i16value = (short)modstadata[12].REG.u16_Idcout;
                fvalue = i16value * 0.01f;
                Slaver13CurrBox.Text = fvalue.ToString("F2");
                syscurr += fvalue;
            }
            else
            {
                Slave13Box.Checked = false;
                Slaver13ErrBox.Checked = false;
                Slaver13CurrBox.Text = "0";
            }

            if (SlaverConnectSta[13] == 1)
            {
                OnlineNum++;
                Slave14Box.Checked = true;
                if (modstadata[13].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver14ErrBox.Checked = true;

                }
                else
                {
                    Slaver14ErrBox.Checked = false;
                }
                i16value = (short)modstadata[13].REG.u16_Idcout;
                fvalue = i16value * 0.01f;
                Slaver14CurrBox.Text = fvalue.ToString("F2");
                syscurr += fvalue;
            }
            else
            {
                Slave14Box.Checked = false;
                Slaver14ErrBox.Checked = false;
                Slaver14CurrBox.Text = "0";
            }

            if (SlaverConnectSta[14] == 1)
            {
                OnlineNum++;
                Slave15Box.Checked = true;
                if (modstadata[14].REG.u16_WorkMode == 6)
                {
                    ErrNum++;
                    Slaver15ErrBox.Checked = true;

                }
                else
                {
                    Slaver15ErrBox.Checked = false;
                }
                i16value = (short)modstadata[14].REG.u16_Idcout;
                fvalue = i16value * 0.01f;
                Slaver15CurrBox.Text = fvalue.ToString("F2");
                syscurr += fvalue;
            }
            else
            {
                Slave15Box.Checked = false;
                Slaver15ErrBox.Checked = false;
                Slaver15CurrBox.Text = "0";
            }



            SysCurrBox.Text = syscurr.ToString("F2");
            OnlinerNumBox.Text = OnlineNum.ToString();
            ErrNumBox.Text = ErrNum.ToString();

            if (ReadSettingFinish == 1)
            {
                ReadSettingFinish = 0;
                CMD_WD cmd = new CMD_WD() { all = 0 };
                fvalue = (float)(modsetdata[deviceid - 1].REG.u16_MaxIout * 0.01f);
                MaxCurrBox.Text = fvalue.ToString("F2");

                u16value = (ushort)(modsetdata[deviceid - 1].REG.u16_MaxVout * 0.1);
                MaxVoltBox.Text = u16value.ToString();

                u16value = (ushort)(modsetdata[deviceid - 1].REG.u16_MaxPout * 0.1);
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
                    PfcOlEnBox.Checked = true;
                }
                else
                {
                    PfcOlEnBox.Checked = false;
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

            fvalue = 0;
            PVbusBox.Text = fvalue.ToString("F1");

            fvalue = 0;
            NVbusBox.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Iout1;
            fvalue = 0.1f * i16value;
            Iout1Box.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Vout2;
            fvalue = 0.1f * u16value;
            Vout2Box.Text = fvalue.ToString("F1");

            u16value = modstadata[deviceid - 1].REG.u16_Vout;
            fvalue = 0.1f * u16value;
            VoutBox.Text = fvalue.ToString("F1");

            i16value = (short)modstadata[deviceid - 1].REG.u16_Idcout;
            fvalue = 0.01f * i16value;
            IoutBox.Text = fvalue.ToString("F2");

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
                        modsetdata[deviceid - 1].REG.u16_CmdWd.PowerOnOff = 0;

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
            VersionBox.Text = u32value.ToString();

        }

        private void ReadSetting()
        {
            uint[] data = new uint[9];
            uint[] data1 = new uint[9];
            uint[] data2 = new uint[9];
            CanAppId id = new CanAppId() { IdFrame = 0 };
            for (int i = 0; i < 9; i++)
            {
                data[i] = 0;
                data1[i] = 0;
                data2[i] = 0;
            }
            id.SlaverFlag = 1;
            id.ActFlag = 1;
            id.DesId = 0;
            id.FuncCode = 0x21;
            data[0] = id.IdFrame;
            data1[0] = id.IdFrame;
            data2[0] = id.IdFrame;

            data[1] = 0;
            data[2] = 3;
            AppTxQueue.Add(data);
            data1[1] = 3;
            data1[2] = 3;
            AppTxQueue.Add(data1);
            data2[1] = 6;
            data2[2] = 2;
            AppTxQueue.Add(data2);
        }

        private void ReadSettingButton_Click(object sender, EventArgs e)
        {
            ReadSetting();
        }

        private unsafe void PowerOnButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 1;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_CmdWd.PowerOnOff = 1;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[4 + i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[4 + i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 7;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.PowerOnOff = 1;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.all;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void PowerOffButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 1;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_CmdWd.PowerOnOff = 0;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[4 + i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[4 + i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 7;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.PowerOnOff = 0;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.all;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void PfcOlEnBox_CheckedChanged(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            if (BrdctSendBox.Checked == true)
            {

                SetFlag = 1;
                setFunccode = 1;

                if (PfcOlEnBox.Checked == true)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        modsetdata[i].REG.u16_CmdWd.PfcOL = 1;
                    }
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        modsetdata[i].REG.u16_CmdWd.PfcOL = 0;
                    }
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[4 + i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[4 + i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 7;
                data[2] = 1;
                if (PfcOlEnBox.Checked == true)
                {
                    modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.PfcOL = 1;
                }
                else
                {
                    modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.PfcOL = 0;
                }
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.all;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);
            }
        }

        private unsafe void CCButton_CheckedChanged(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 0;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_OutputMode = 0;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 3;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_OutputMode = 0;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_OutputMode;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void CVButton_CheckedChanged(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 0;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_OutputMode = 1;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 3;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_OutputMode = 1;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_OutputMode;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void CPButton_CheckedChanged(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 0;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_OutputMode = 2;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 3;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_OutputMode = 2;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_OutputMode;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void SetMaxCurrButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float value = 0;

            if (float.TryParse(MaxCurrBox.Text, out value))
            {
                value = value * 100;
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 0;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_MaxIout = (ushort)value;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 0;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_MaxIout = (ushort)value;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_MaxIout;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void SetMaxVoltButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float value = 0;

            if (float.TryParse(MaxVoltBox.Text, out value))
            {
                value = value * 10;
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 0;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_MaxVout = (ushort)value;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 1;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_MaxVout = (ushort)value;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_MaxVout;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void SetMaxPowerButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float value = 0;

            if (float.TryParse(MaxPowerBox.Text, out value))
            {
                value = value * 10;
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 0;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_MaxPout = (ushort)value;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 2;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_MaxPout = (ushort)value;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_MaxPout;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void SetCurrStepButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float value = 0;

            if (float.TryParse(CurrStepBox.Text, out value))
            {
                value = value * 10;
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 1;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_SlopeIcc = (ushort)value;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[4+ i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[4 + i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 4;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_SlopeIcc = (ushort)value;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_SlopeIcc;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void SetVoltStepButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float value = 0;

            if (float.TryParse(VoltStepBox.Text, out value))
            {
                value = value * 10;
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 1;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_SlopeVcv = (ushort)value;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[4 + i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[4 + i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 5;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_SlopeVcv = (ushort)value;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_SlopeVcv;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void SetPowerStepButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float value = 0;

            if (float.TryParse(PowerStepBox.Text, out value))
            {
                value = value * 10;
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 1;
                for (int i = 0; i < 12; i++)
                {
                    modsetdata[i].REG.u16_SlopePcp = (ushort)value;
                }

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[4 + i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[4 + i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 6;
                data[2] = 1;
                modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_SlopePcp = (ushort)value;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_SlopePcp;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void ClearErrButton_Click(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 1;

                int index = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SlaverConnectSta[i] == 1)
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[index].buff[4 + i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[index].buff[4 + i] & 0xFF);
                }
                Setbuff[7] |= 0x04;
            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 7;
                data[2] = 1;
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.all;
                cmd.ClearErr = 1;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

            }

        }

        private unsafe void DCDcOlEnBox_CheckedChanged(object sender, EventArgs e)
        {
            CMD_WD cmd = new CMD_WD() { all = 0 };
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (BrdctSendBox.Checked)
            {
                SetFlag = 1;
                setFunccode = 1;

                if (DCDcOlEnBox.Checked == true)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        modsetdata[i].REG.u16_CmdWd.DcOL = 1;
                    }
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        modsetdata[i].REG.u16_CmdWd.DcOL = 0;
                    }
                }


                for (int i = 0; i < 4; i++)
                {
                    Setbuff[i * 2] = (uint)(modsetdata[0].buff[4 + i] >> 8);
                    Setbuff[i * 2 + 1] = (uint)(modsetdata[0].buff[4 + i] & 0xFF);
                }

            }
            else
            {
                id.DesId = (byte)ChoseDevListBox.SelectedIndex;
                id.FuncCode = 0x20;
                id.SlaverFlag = 1;
                data[0] = id.IdFrame;
                data[1] = 7;
                data[2] = 1;
                if (DCDcOlEnBox.Checked == true)
                {
                    modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.DcOL = 1;
                }
                else
                {
                    modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.DcOL = 0;
                }
                cmd.all = modsetdata[ChoseDevListBox.SelectedIndex - 1].REG.u16_CmdWd.all;
                data[3] = (uint)(cmd.all >> 8);
                data[4] = (uint)(cmd.all & 0xFF);
                AppTxQueue.Add(data);

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
