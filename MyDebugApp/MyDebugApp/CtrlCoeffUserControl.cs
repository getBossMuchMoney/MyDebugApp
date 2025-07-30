using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDebugApp
{
    public partial class CtrlCoeffUserControl : UserControl
    {
        public event EventHandler<int> OnDeviceSelected; // 可以传递设备名称或其他参数
        public CtrlCoeffUserControl()
        {
            InitializeComponent();
            ChoseDevListBox.SelectedIndex = 1;
            CalPointBox.SelectedIndex = 0;
        }
        private void PllKpButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PllKpBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x34;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void PllKiButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PllKiBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x36;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void PfcDrKpButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PfcDrKpBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x38;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void PfcDrKiButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PfcDrKiBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x3A;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void PfcQrKpButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PfcQrKpBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x3C;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void PfcQrKiBoxButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PfcQrKiBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x3E;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void PfcDumResButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PfcDumResBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x40;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);
        }

        private void PfcKdButton_Click(object sender, EventArgs e)
        {

        }

        private void PfcKzButton_Click(object sender, EventArgs e)
        {

        }

        private void PfcVoltKpButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PfcVoltKpBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x46;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);
        }

        private void PfcVoltKiButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(PfcVoltKiBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x48;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);
        }

        private void DcCurrKpButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(DcCurrKpBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x4A;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);
        }

        private void DcCurrKiButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(DcCurrKiBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x4C;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void DcCurrAverKpButton_Click(object sender, EventArgs e)
        {


        }

        private void DcCurrAverKiButton_Click(object sender, EventArgs e)
        {


        }

        private void DcVoltKpButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(DcVoltKpBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x52;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void DcVoltKiButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(DcVoltKiBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x54;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void DroopRatioButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            uint u32value = 0;

            if (float.TryParse(DroopRatioBox.Text, out fvalue))
            {
                u32value = (uint)(fvalue * 10000);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x56;
            data[2] = 2;
            data[3] = (u32value >> 8) & 0xFF;
            data[4] = u32value & 0xFF;
            data[5] = (u32value >> 24) & 0xFF;
            data[6] = (u32value >> 16) & 0xFF;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void ReadCoeffButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data1 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data2 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data3 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data4 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data5 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data6 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data7 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data8 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data9 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data10 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            id.DesId = 0;
            id.FuncCode = 0x21;
            id.SlaverFlag = 1;
            id.ActFlag = 1;

            data1[0] = id.IdFrame;
            data1[1] = 0x34;
            data1[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data1);

            data2[0] = id.IdFrame;
            data2[1] = 0x37;
            data2[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data2);

            data3[0] = id.IdFrame;
            data3[1] = 0x3A;
            data3[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data3);

            data4[0] = id.IdFrame;
            data4[1] = 0x3D;
            data4[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data4);

            data5[0] = id.IdFrame;
            data5[1] = 0x40;
            data5[2] = 0x02;
            CanCrossFileQueue.AppTxQueue.Add(data5);

            data6[0] = id.IdFrame;
            data6[1] = 0x46;
            data6[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data6);

            data7[0] = id.IdFrame;
            data7[1] = 0x49;
            data7[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data7);

            data8[0] = id.IdFrame;
            data8[1] = 0x4C;
            data8[2] = 0x02;
            CanCrossFileQueue.AppTxQueue.Add(data8);

            data9[0] = id.IdFrame;
            data9[1] = 0x52;
            data9[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data9);

            data10[0] = id.IdFrame;
            data10[1] = 0x55;
            data10[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data10);
        }

        private void SaveCoeffButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            CMD_WD cmd = new CMD_WD() { all = 0 };

            cmd.DataSave = 1;
            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x07;
            data[2] = 0x01;
            data[3] = (uint)(cmd.all >> 8);
            data[4] = (uint)(cmd.all & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void RecovCoeffButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            CMD_WD cmd = new CMD_WD() { all = 0 };

            cmd.ResetCtrlPara = 1;
            id.DesId = 0;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x07;
            data[2] = 0x01;
            data[3] = (uint)(cmd.all >> 8);
            data[4] = (uint)(cmd.all & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void ChoseDevListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectDeviceIndex = ChoseDevListBox.SelectedIndex;
            OnDeviceSelected?.Invoke(this, SelectDeviceIndex); // 触发事件
        }

        private void Cal1RefButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalRef1Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x11;
            }
            else
            {
                data[1] = 0x11 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal1ActButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalAct1Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x12;
            }
            else
            {
                data[1] = 0x12 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal2RefButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalRef2Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x13;
            }
            else
            {
                data[1] = 0x13 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal2ActButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalAct2Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x14;
            }
            else
            {
                data[1] = 0x14 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal3RefButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalRef3Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x15;
            }
            else
            {
                data[1] = 0x15 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal3ActButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalAct3Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x16;
            }
            else
            {
                data[1] = 0x16 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);
        }

        private void Cal4RefButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalRef4Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x17;
            }
            else
            {
                data[1] = 0x17 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal4ActButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalAct4Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x18;
            }
            else
            {
                data[1] = 0x18 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal5RefButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalRef5Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x19;
            }
            else
            {
                data[1] = 0x19 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal5ActButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalAct5Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x1a;
            }
            else
            {
                data[1] = 0x1a + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal6RefButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalRef6Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x1b;
            }
            else
            {
                data[1] = 0x1b + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);
        }

        private void Cal6ActButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalAct6Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x1c;
            }
            else
            {
                data[1] = 0x1c + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal7RefButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalRef7Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x1d;
            }
            else
            {
                data[1] = 0x1d + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal7ActButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalAct7Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x1e;
            }
            else
            {
                data[1] = 0x1e + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);
        }

        private void Cal8RefButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalRef8Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x1f;
            }
            else
            {
                data[1] = 0x1f + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void Cal8ActButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            float fvalue = 0;
            ushort u16value = 0;

            if (NoCalButton.Checked == true)
            {
                return;
            }

            if (float.TryParse(CalAct8Box.Text, out fvalue))
            {
                u16value = (ushort)(fvalue * 10);
            }
            else
            {
                MessageBox.Show("输入非法参数", "错误!");
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data[1] = 0x20;
            }
            else
            {
                data[1] = 0x20 + 0x10;
            }
            data[2] = 0x01;
            data[3] = (uint)(u16value >> 8);
            data[4] = (uint)(u16value & 0xFF);
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void RunCalButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (NoCalButton.Checked == true)
            {
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x0A;
            data[2] = 0x01;
            data[3] = (uint)(CalPointBox.SelectedIndex + 1);
            data[4] = 1;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void FinishCalButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (NoCalButton.Checked == true)
            {
                return;
            }

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x0A;
            data[2] = 0x01;
            data[3] = 0;
            data[4] = 2;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void ResetCalButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x0A;
            data[2] = 0x01;
            data[3] = 0;
            data[4] = 3;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void ReadCalValueButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data1 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data2 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data3 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data4 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data5 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            uint[] data6 = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (NoCalButton.Checked == true)
            {
                return;
            }

            id.DesId = 0;
            id.FuncCode = 0x21;
            id.SlaverFlag = 1;
            id.ActFlag = 1;

            data1[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data1[1] = 0x11;
            }
            else
            {
                data1[1] = 0x11 + 0x10;
            }
            data1[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data1);

            data2[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data2[1] = 0x14;
            }
            else
            {
                data2[1] = 0x14 + 0x10;
            }
            data2[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data2);

            data3[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data3[1] = 0x17;
            }
            else
            {
                data3[1] = 0x17 + 0x10;
            }
            data3[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data3);

            data4[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data4[1] = 0x1a;
            }
            else
            {
                data4[1] = 0x1a + 0x10;
            }
            data4[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data4);

            data5[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data5[1] = 0x1d;
            }
            else
            {
                data5[1] = 0x1d + 0x10;
            }
            data5[2] = 0x03;
            CanCrossFileQueue.AppTxQueue.Add(data5);

            data6[0] = id.IdFrame;
            if (CalCurrButton.Checked == true)
            {
                data6[1] = 0x20;
            }
            else
            {
                data6[1] = 0x20 + 0x10;
            }
            data6[2] = 0x01;
            CanCrossFileQueue.AppTxQueue.Add(data6);

        }

        private void NoCalButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x09;
            data[2] = 0x01;
            data[3] = 0;
            data[4] = 0;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void CalCurrButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x09;
            data[2] = 0x01;
            data[3] = 0;
            data[4] = 1;
            CanCrossFileQueue.AppTxQueue.Add(data);

        }

        private void CalVoltButton_Click(object sender, EventArgs e)
        {
            CanAppId id = new CanAppId() { IdFrame = 0 };
            uint[] data = new uint[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            id.DesId = (byte)ChoseDevListBox.SelectedIndex;
            id.FuncCode = 0x20;
            id.SlaverFlag = 1;
            data[0] = id.IdFrame;
            data[1] = 0x09;
            data[2] = 0x01;
            data[3] = 0;
            data[4] = 2;
            CanCrossFileQueue.AppTxQueue.Add(data);
        }
    }
}
