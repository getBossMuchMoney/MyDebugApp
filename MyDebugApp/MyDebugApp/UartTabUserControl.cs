using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace MyDebugApp
{
    public partial class UartTabUserControl : UserControl
    {
        String serialPortName;
        ModbusCrc CrcInter = new ModbusCrc();
        Thread updatethread;
        Thread uartRcvThread;
        byte[] FileDataBuffer;
        byte[] FileCrc = new byte[2];
        UInt32 FileSize = 0;
        byte BinDataIndex = 0;
        byte ChoseUpdateID = 0;
        BlockingCollection<byte[]> RxQueue = new BlockingCollection<byte[]>(new ConcurrentQueue<byte[]>());
        BlockingCollection<string> ShowStrRxQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
        int UartRcvFlag = 0;
        List<byte> UartRcvData = new List<byte>();
        private object _lock = new object();
        string[] getPorts = null;
        public UartTabUserControl()
        {
            InitializeComponent();
        }

        private void UartTabUserControl_Load(object sender, EventArgs e)
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            getPorts = ports;
            SerialListBox.Items.AddRange(ports);
            SerialListBox.SelectedIndex = SerialListBox.Items.Count > 0 ? 0 : -1;
            BandListBox.Items.Add("9600");
            BandListBox.Items.Add("19200");
            BandListBox.Items.Add("38400");
            BandListBox.Items.Add("57600");
            BandListBox.Items.Add("115200");
            BandListBox.SelectedIndex = 1;
            ChoseUpdateDeviceBox.SelectedIndex = 0;
            DeviceBandListBox.SelectedIndex = 0;
            timer1000ms.Start();
/*            TimerOneMs = new HighPrecisionTimer();
            TimerOneMs.Callback = Timer1ms_CallBack;
            // 启动1ms定时器
            TimerOneMs.Start(1);*/
        }

        private void OpenSerialButton_Click(object sender, EventArgs e)
        {
            if (OpenSerialButton.Text == "打开串口")
            {
                try
                {
                    serialPort1.PortName = SerialListBox.Text;//获取comboBox1要打开的串口号
                    serialPortName = serialPort1.PortName;
                    serialPort1.BaudRate = int.Parse(BandListBox.Text);//获取comboBox2选择的波特率
                    serialPort1.DataBits = 8;//设置数据位
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Parity = Parity.None;
                    serialPort1.Open();//打开串口
                    OpenSerialButton.Text = "关闭串口";
                    SerialListBox.Enabled = false;
                    BandListBox.Enabled = false;
                    CheckSerialButton.Enabled = false;
                    uartRcvThread = new Thread(UartData_Recieve);
                    uartRcvThread.Start();
                }
                catch (Exception err)
                {
                    MessageBox.Show("打开失败" + err.ToString(), "提示!");
                }
            }
            else
            {
                try
                {
                    uartRcvThread.Abort();
                    serialPort1.Close();//关闭串口
                }
                catch (Exception) { }
                OpenSerialButton.Text = "打开串口";//按钮显示打开
                SerialListBox.Enabled = true;
                BandListBox.Enabled = true;
                CheckSerialButton.Enabled = true;
            }
        }

        private void CheckSerialButton_Click(object sender, EventArgs e)
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            SerialListBox.Items.Clear();
            SerialListBox.Items.AddRange(ports);
            SerialListBox.SelectedIndex = SerialListBox.Items.Count > 0 ? 0 : -1;
        }


        private void CheckSerialPortChange()
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            if (OpenSerialButton.Text == "关闭串口")
            {
                if (ports.Length == 0)
                {
                    try
                    {
                        serialPort1.Close();//关闭串口
                    }
                    catch (Exception) { }
                    OpenSerialButton.Text = "打开串口";//按钮显示打开
                    SerialListBox.Enabled = true;
                    BandListBox.Enabled = true;
                    CheckSerialButton.Enabled = true;
                }
                else
                {
                    for (UInt16 i = 0; i < ports.Length; i++)
                    {
                        if (serialPortName == ports[i])
                        {
                            break;
                        }

                        if (i == ports.Length - 1)
                        {
                            try
                            {
                                serialPort1.Close();//关闭串口
                            }
                            catch (Exception) { }
                            OpenSerialButton.Text = "打开串口";//按钮显示打开
                            SerialListBox.Enabled = true;
                            BandListBox.Enabled = true;
                            CheckSerialButton.Enabled = true;
                            SerialListBox.Items.Clear();
                            SerialListBox.Items.AddRange(ports);
                            SerialListBox.SelectedIndex = SerialListBox.Items.Count > 0 ? 0 : -1;
                        }
                    }

                }
            }
            else
            {
                if (getPorts.Length != ports.Length)
                {
                    getPorts = ports;
                SerialListBox.Items.Clear();
                SerialListBox.Items.AddRange(ports);
                    SerialListBox.SelectedIndex = SerialListBox.Items.Count > 0 ? 0 : -1;
                }
                else
                {
                    for (int i = 0; i < ports.Length; i++)
                    {
                        if (getPorts[i] != ports[i])
                        {
                            getPorts = ports;
                            SerialListBox.Items.Clear();
                            SerialListBox.Items.AddRange(ports);
                            SerialListBox.SelectedIndex = SerialListBox.Items.Count > 0 ? 0 : -1;
                            break;
                        }

                    }
                }
            }

        }

        private void timer1000ms_Tick(object sender, EventArgs e)
        {
            CheckSerialPortChange();
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

        private void UartDataShow(byte[] byteData, byte MsgType)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            if (CheckDataShowStyleBox.Checked)
            {
                Encoding gb2312 = Encoding.GetEncoding("GB2312", new EncoderExceptionFallback(), new DecoderExceptionFallback());
                string data;
                try
                {
                    data = gb2312.GetString(byteData);
                }
                catch
                {
                    // 如果有异常，替换所有不可解码字符为 ?
                    data = new string(
                        byteData.Select(b => (b >= 0x20 && b <= 0x7E) ? (char)b : '?').ToArray()
                    );
                }
                if (MsgType == 0)
                {
                    UartDataBox.AppendText('[' + timestamp + ']' + "收←◆" + data + Environment.NewLine);//对话框追加显示数据
                }
                else
                {
                    UartDataBox.AppendText('[' + timestamp + ']' + "发→◇" + data + Environment.NewLine);
                }
            }
            else
            {
                if (MsgType == 0)
                {
                    //16进制显示
                    UartDataBox.AppendText('[' + timestamp + ']' + "收←◆" + byteToHexStr(byteData) + Environment.NewLine);
                }
                else
                {
                    UartDataBox.AppendText('[' + timestamp + ']' + "发→◇" + byteToHexStr(byteData) + Environment.NewLine);
                }
            }

        }

        private void UartDataShow(byte[] byteData, byte MsgType, string timestamp)
        {
            if (CheckDataShowStyleBox.Checked)
            {
                Encoding gb2312 = Encoding.GetEncoding("GB2312", new EncoderExceptionFallback(), new DecoderExceptionFallback());
                string data;
                try
                {
                    data = gb2312.GetString(byteData);
                }
                catch
                {
                    // 如果有异常，替换所有不可解码字符为 ?
                    data = new string(
                        byteData.Select(b => (b >= 0x20 && b <= 0x7E) ? (char)b : '?').ToArray()
                    );
                }
                if (MsgType == 0)
                {
                    UartDataBox.AppendText('[' + timestamp + ']' + "收←◆" + data + Environment.NewLine);//对话框追加显示数据
                }
                else
                {
                    UartDataBox.AppendText('[' + timestamp + ']' + "发→◇" + data + Environment.NewLine);
                }
            }
            else
            {
                if (MsgType == 0)
                {
                    //16进制显示
                    UartDataBox.AppendText('[' + timestamp + ']' + "收←◆" + byteToHexStr(byteData) + Environment.NewLine);
                }
                else
                {
                    UartDataBox.AppendText('[' + timestamp + ']' + "发→◇" + byteToHexStr(byteData) + Environment.NewLine);
                }
            }

        }

        private void UartData_Recieve()
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            int len = 0;
            while (true)
            {
                try
                {
                    len = serialPort1.BytesToRead;//获取可以读取的字节数
                }
                catch (Exception) { }
                if (len > 0)
                {
                    byte[] buff = new byte[len];//创建缓存数据数组
                    serialPort1.Read(buff, 0, len);//把数据读取到buff数组
                    UartRcvData.AddRange(buff);
                    if (UartRcvFlag == 0)
                    {
                        UartRcvFlag = 1;
                        timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                    }
                }
                else
                {
                    if (UartRcvFlag == 1)
                    {
                        UartRcvFlag = 0;
                        byte[] Data = UartRcvData.ToArray();
                        UartRcvData = new List<byte>();
                        if (updatethread != null && updatethread.IsAlive)
                        {
                            RxQueue.Add(Data);
                        }
                        Invoke((Action)(() =>
                        {
                            UartDataShow(Data, 0, timestamp);
                        }));
                    }
                }
                Thread.Sleep(20);
            }
        }

        private void ChoseFileButton_Click(object sender, EventArgs e)
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
                    FilePathShowBox.Text = fileName;
                    FileSize = (UInt32)FileDataBuffer.Length;
                    FileCrc = CrcInter.Crc16(FileDataBuffer, FileSize);
                    //byte[] kk = FileDataBuffer.Skip(0).Take(3).ToArray();
                }
            }
        }

        private void ClearUartDataShowButton_Click(object sender, EventArgs e)
        {
            UartDataBox.Clear();
        }

        private void BandConfigButton_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[5];
            byte[] crc = new byte[2];
            data[0] = 0x55;
            data[1] = 0x55;
            data[2] = (byte)DeviceBandListBox.SelectedIndex;
            crc = CrcInter.Crc16(data, 3);
            data[3] = crc[0];
            data[4] = crc[1];
            try
            {
                serialPort1.Write(data, 0, data.Length);
                if (CheckDataShowStyleBox.Checked)
                {
                    UartDataShow(data, 0);
                }
                else
                {
                    UartDataShow(data, 1);
                }
            }
            catch (Exception) { }
        }

        private void CheckDeviceButton_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[5];
            byte[] crc = new byte[2];
            data[0] = 0xAA;
            data[1] = 0xAA;
            data[2] = (byte)(ChoseUpdateDeviceBox.SelectedIndex + 1);
            crc = CrcInter.Crc16(data, 3);
            data[3] = crc[0];
            data[4] = crc[1];
            try
            {
                serialPort1.Write(data, 0, data.Length);
                if (CheckDataShowStyleBox.Checked)
                {
                    UartDataShow(data, 0);
                }
                else
                {
                    UartDataShow(data, 1);
                }

            }
            catch (Exception) { }
        }

        private void StartUpdateButton_Click(object sender, EventArgs e)
        {
            if ((updatethread == null || !updatethread.IsAlive) && (FileSize != 0))
            {
                updatethread = new Thread(Update_Process);
                ChoseUpdateID = (byte)(ChoseUpdateDeviceBox.SelectedIndex + 1);
                updatethread.Start();
            }
        }

        private void Iap_Req()
        {
            byte[] data = new byte[14];
            data[0] = (byte)'i';
            data[1] = (byte)'a';
            data[2] = (byte)'p';
            data[3] = ChoseUpdateID;
            data[4] = (byte)(FileSize & 0xFF);
            data[5] = (byte)((FileSize >> 8) & 0xFF);
            data[6] = (byte)((FileSize >> 16) & 0xFF);
            data[7] = (byte)((FileSize >> 24) & 0xFF);
            data[8] = FileCrc[0];
            data[9] = FileCrc[1];
            data[10] = 0;
            data[11] = 0;
            byte[] crc = CrcInter.Crc16(data, 12);
            data[12] = crc[0];
            data[13] = crc[1];

            try
            {
                serialPort1.Write(data, 0, data.Length);
                Invoke((Action)(() =>
                {
                    if (CheckDataShowStyleBox.Checked)
                    {
                        UartDataShow(data, 0);
                    }
                    else
                    {
                        UartDataShow(data, 1);
                    }
                }));

            }
            catch (Exception) { }
        }

        private void Iap_Erase()
        {
            byte[] data = new byte[8];
            data[0] = (byte)'e';
            data[1] = (byte)'r';
            data[2] = (byte)'a';
            data[3] = (byte)'s';
            data[4] = (byte)'e';
            data[5] = ChoseUpdateID;

            byte[] crc = CrcInter.Crc16(data, 6);
            data[6] = crc[0];
            data[7] = crc[1];

            try
            {
                serialPort1.Write(data, 0, data.Length);
                Invoke((Action)(() =>
                {
                    if (CheckDataShowStyleBox.Checked)
                    {
                        UartDataShow(data, 0);
                    }
                    else
                    {
                        UartDataShow(data, 1);
                    }
                }));

            }
            catch (Exception) { }
        }

        private void Iap_Write(byte[] bindata)
        {
            byte[] data = new byte[bindata.Length + 8];
            data[0] = 0x55;
            data[1] = 0xaa;
            data[2] = ChoseUpdateID;
            data[3] = (byte)(bindata.Length >> 8);
            data[4] = (byte)(bindata.Length & 0xFF);
            data[5] = BinDataIndex;
            Buffer.BlockCopy(bindata, 0, data, 6, bindata.Length);
            byte[] crc = CrcInter.Crc16(data, (uint)(6 + bindata.Length));
            data[6 + bindata.Length] = crc[0];
            data[7 + bindata.Length] = crc[1];

            try
            {
                serialPort1.Write(data, 0, data.Length);
                Invoke((Action)(() =>
                {
                    if (CheckDataShowStyleBox.Checked)
                    {
                        UartDataShow(data, 0);
                    }
                    else
                    {
                        UartDataShow(data, 1);
                    }
                }));

            }
            catch (Exception) { }

        }

        private void Iap_Done()
        {
            byte[] data = new byte[7];
            data[0] = (byte)'d';
            data[1] = (byte)'o';
            data[2] = (byte)'n';
            data[3] = (byte)'e';
            data[4] = ChoseUpdateID;
            byte[] crc = CrcInter.Crc16(data, 5);
            data[5] = crc[0];
            data[6] = crc[1];

            try
            {
                serialPort1.Write(data, 0, data.Length);
                Invoke((Action)(() =>
                {
                    if (CheckDataShowStyleBox.Checked)
                    {
                        UartDataShow(data, 0);
                    }
                    else
                    {
                        UartDataShow(data, 1);
                    }
                }));

            }
            catch (Exception) { }
        }


        private void Update_Process()
        {
            byte UpdateState = 0;
            Invoke((Action)(() =>
            {
                StartUpdateButton.Enabled = false;
            }));

            while (true)
            {
                switch (UpdateState)
                {
                    case 0:
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Iap_Req();
                                byte[] frame = null;
                                RxQueue.TryTake(out frame, 1000);
                                if (frame != null && frame.Length == 6)
                                {
                                    byte[] crc = CrcInter.Crc16(frame, 4);
                                    if (frame[0] == (byte)'i' && frame[1] == (byte)'a' && frame[2] == (byte)'p'
                                        && frame[4] == crc[0] && frame[5] == crc[1])
                                    {
                                        UpdateState = 1;
                                        break;
                                    }
                                }

                                if (i == 2)
                                {
                                    Invoke((Action)(() =>
                                    {
                                        StartUpdateButton.Enabled = true;
                                        MessageBox.Show("未找到目标设备", "错误!");
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
                                byte[] frame = null;
                                RxQueue.TryTake(out frame, 1000);
                                if (frame != null)
                                {
                                    byte[] crc = CrcInter.Crc16(frame, (uint)(frame.Length - 2));
                                    if (frame[0] == (byte)'e' && frame[1] == (byte)'r' && frame[2] == (byte)'a' && frame[6] == 0
                                        && frame[frame.Length - 2] == crc[0] && frame[frame.Length - 1] == crc[1])
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
                                                StartUpdateButton.Enabled = true;
                                                MessageBox.Show("擦除失败", "错误!");
                                            }));
                                            return;
                                        }

                                    }
                                }

                                if (i == 2)
                                {
                                    Invoke((Action)(() =>
                                    {
                                        StartUpdateButton.Enabled = true;
                                        MessageBox.Show("未找到目标设备", "错误!");
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

                            BinDataIndex = 0;
                            int groupIndex = 0;
                            for (groupIndex = 0; groupIndex < Group; groupIndex++)
                            {
                                byte[] groupData = FileDataBuffer.Skip(groupIndex * 2048).Take(2048).ToArray();
                                for (int i = 0; i < 3; i++)
                                {
                                    Iap_Write(groupData);
                                    byte[] frame = null;
                                    RxQueue.TryTake(out frame, 1000);
                                    if (frame != null)
                                    {
                                        byte[] crc = CrcInter.Crc16(frame, (uint)(frame.Length - 2));
                                        if (frame[0] == 0x55 && frame[1] == 0xaa && frame[4] == 0
                                            && frame[frame.Length - 2] == crc[0] && frame[frame.Length - 1] == crc[1])
                                        {
                                            BinDataIndex++;
                                            Invoke((Action)(() =>
                                            {
                                                UpdateProgressBar.Value = groupIndex + 1;
                                            }));
                                            break;
                                        }
                                        else
                                        {
                                            if (i == 2)
                                            {
                                                Invoke((Action)(() =>
                                                {
                                                    StartUpdateButton.Enabled = true;
                                                    MessageBox.Show("烧录失败", "错误!");
                                                    UpdateProgressBar.Value = 0;
                                                }));
                                                return;
                                            }
                                        }
                                    }

                                    if (i == 2)
                                    {
                                        Invoke((Action)(() =>
                                        {
                                            StartUpdateButton.Enabled = true;
                                            MessageBox.Show("未找到目标设备", "错误!");
                                            UpdateProgressBar.Value = 0;
                                        }));
                                        return;
                                    }

                                }

                            }

                            if (remainSize > 0)
                            {
                                byte[] groupData = FileDataBuffer.Skip(groupIndex * 2048).Take(remainSize).ToArray();
                                for (int i = 0; i < 3; i++)
                                {
                                    Iap_Write(groupData);
                                    byte[] frame = null;
                                    RxQueue.TryTake(out frame, 1000);
                                    if (frame != null)
                                    {
                                        byte[] crc = CrcInter.Crc16(frame, (uint)(frame.Length - 2));
                                        if (frame[0] == 0x55 && frame[1] == 0xaa && frame[4] == 0
                                            && frame[frame.Length - 2] == crc[0] && frame[frame.Length - 1] == crc[1])
                                        {
                                            Invoke((Action)(() =>
                                            {
                                                UpdateProgressBar.Value = groupIndex + 1;
                                            }));
                                            break;
                                        }
                                        else
                                        {
                                            if (i == 2)
                                            {
                                                Invoke((Action)(() =>
                                                {
                                                    StartUpdateButton.Enabled = true;
                                                    MessageBox.Show("烧录失败", "错误!");
                                                    UpdateProgressBar.Value = 0;
                                                }));
                                                return;
                                            }

                                        }
                                    }

                                    if (i == 2)
                                    {
                                        Invoke((Action)(() =>
                                        {
                                            StartUpdateButton.Enabled = true;
                                            MessageBox.Show("未找到目标设备", "错误!");
                                            UpdateProgressBar.Value = 0;
                                        }));
                                        return;
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
                                byte[] frame = null;
                                RxQueue.TryTake(out frame, 1000);
                                if (frame != null)
                                {
                                    byte[] crc = CrcInter.Crc16(frame, (uint)(frame.Length - 2));
                                    if (frame[0] == (byte)'d' && frame[1] == (byte)'o' && frame[2] == (byte)'n' && frame[5] == 0
                                        && frame[frame.Length - 2] == crc[0] && frame[frame.Length - 1] == crc[1])
                                    {
                                        Invoke((Action)(() =>
                                        {
                                            StartUpdateButton.Enabled = true;
                                            MessageBox.Show("固件烧录成功", "提示!");
                                            UpdateProgressBar.Value = 0;
                                        }));
                                        return;
                                    }
                                    else
                                    {
                                        Invoke((Action)(() =>
                                        {
                                            StartUpdateButton.Enabled = true;
                                            MessageBox.Show("校验失败", "错误!");
                                            UpdateProgressBar.Value = 0;
                                        }));
                                        return;
                                    }
                                }

                                if (i == 2)
                                {
                                    Invoke((Action)(() =>
                                    {
                                        StartUpdateButton.Enabled = true;
                                        MessageBox.Show("未找到目标设备", "错误!");
                                        UpdateProgressBar.Value = 0;
                                    }));
                                    return;
                                }


                            }
                        }
                        break;

                    default:
                        { }
                        break;

                }
                Thread.Sleep(1);
            }

        }

        private void BootButton_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[4];
            data[0] = (byte)'s';
            data[1] = (byte)'t';
            data[2] = (byte)'o';
            data[3] = (byte)'p';
            try
            {
                serialPort1.Write(data, 0, data.Length);
                if (CheckDataShowStyleBox.Checked)
                {
                    UartDataShow(data, 0);
                }
                else
                {
                    UartDataShow(data, 1);
                }

            }
            catch (Exception) { }
        }

    }
}
