
namespace MyDebugApp
{
    partial class DebugForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            this.timer1000ms = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.openBinFile = new System.Windows.Forms.OpenFileDialog();
            this.tabCan = new System.Windows.Forms.TabPage();
            this.CanDataRcvBox = new System.Windows.Forms.TextBox();
            this.OpenCanDeviceButton = new System.Windows.Forms.Button();
            this.tabUart = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SerialListBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BandListBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CheckSerialButton = new System.Windows.Forms.Button();
            this.OpenSerialButton = new System.Windows.Forms.Button();
            this.CheckDataShowStyleBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.UartDataBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CheckDeviceButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DeviceBandListBox = new System.Windows.Forms.ComboBox();
            this.BandConfigButton = new System.Windows.Forms.Button();
            this.StartUpdateButton = new System.Windows.Forms.Button();
            this.UpdateProgressBar = new System.Windows.Forms.ProgressBar();
            this.BootButton = new System.Windows.Forms.Button();
            this.ChoseFileButton = new System.Windows.Forms.Button();
            this.FilePathShowBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ChoseUpdateDeviceBox = new System.Windows.Forms.ComboBox();
            this.ClearUartDataShowButton = new System.Windows.Forms.Button();
            this.DebugTab = new System.Windows.Forms.TabControl();
            this.tabCan.SuspendLayout();
            this.tabUart.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.DebugTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1000ms
            // 
            this.timer1000ms.Interval = 1000;
            this.timer1000ms.Tick += new System.EventHandler(this.timer1000ms_Tick);
            // 
            // openBinFile
            // 
            this.openBinFile.FileName = "openFileDialog1";
            // 
            // tabCan
            // 
            this.tabCan.Controls.Add(this.OpenCanDeviceButton);
            this.tabCan.Controls.Add(this.CanDataRcvBox);
            this.tabCan.Location = new System.Drawing.Point(4, 25);
            this.tabCan.Name = "tabCan";
            this.tabCan.Padding = new System.Windows.Forms.Padding(3);
            this.tabCan.Size = new System.Drawing.Size(1085, 614);
            this.tabCan.TabIndex = 1;
            this.tabCan.Text = "CAN调试";
            this.tabCan.UseVisualStyleBackColor = true;
            // 
            // CanDataRcvBox
            // 
            this.CanDataRcvBox.Location = new System.Drawing.Point(369, 27);
            this.CanDataRcvBox.Multiline = true;
            this.CanDataRcvBox.Name = "CanDataRcvBox";
            this.CanDataRcvBox.ReadOnly = true;
            this.CanDataRcvBox.Size = new System.Drawing.Size(656, 483);
            this.CanDataRcvBox.TabIndex = 0;
            // 
            // OpenCanDeviceButton
            // 
            this.OpenCanDeviceButton.Location = new System.Drawing.Point(196, 148);
            this.OpenCanDeviceButton.Name = "OpenCanDeviceButton";
            this.OpenCanDeviceButton.Size = new System.Drawing.Size(75, 23);
            this.OpenCanDeviceButton.TabIndex = 1;
            this.OpenCanDeviceButton.Text = "打开分析仪";
            this.OpenCanDeviceButton.UseVisualStyleBackColor = true;
            // 
            // tabUart
            // 
            this.tabUart.Controls.Add(this.ClearUartDataShowButton);
            this.tabUart.Controls.Add(this.ChoseUpdateDeviceBox);
            this.tabUart.Controls.Add(this.label3);
            this.tabUart.Controls.Add(this.FilePathShowBox);
            this.tabUart.Controls.Add(this.ChoseFileButton);
            this.tabUart.Controls.Add(this.groupBox3);
            this.tabUart.Controls.Add(this.groupBox2);
            this.tabUart.Controls.Add(this.groupBox1);
            this.tabUart.Location = new System.Drawing.Point(4, 25);
            this.tabUart.Name = "tabUart";
            this.tabUart.Padding = new System.Windows.Forms.Padding(3);
            this.tabUart.Size = new System.Drawing.Size(1085, 614);
            this.tabUart.TabIndex = 0;
            this.tabUart.Text = "串口调试";
            this.tabUart.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CheckDataShowStyleBox);
            this.groupBox1.Controls.Add(this.OpenSerialButton);
            this.groupBox1.Controls.Add(this.CheckSerialButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.BandListBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SerialListBox);
            this.groupBox1.Location = new System.Drawing.Point(33, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 215);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口设置";
            // 
            // SerialListBox
            // 
            this.SerialListBox.FormattingEnabled = true;
            this.SerialListBox.Location = new System.Drawing.Point(81, 24);
            this.SerialListBox.Name = "SerialListBox";
            this.SerialListBox.Size = new System.Drawing.Size(121, 23);
            this.SerialListBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "串口号";
            // 
            // BandListBox
            // 
            this.BandListBox.FormattingEnabled = true;
            this.BandListBox.Location = new System.Drawing.Point(81, 72);
            this.BandListBox.Name = "BandListBox";
            this.BandListBox.Size = new System.Drawing.Size(121, 23);
            this.BandListBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(9, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "波特率";
            // 
            // CheckSerialButton
            // 
            this.CheckSerialButton.Location = new System.Drawing.Point(13, 127);
            this.CheckSerialButton.Name = "CheckSerialButton";
            this.CheckSerialButton.Size = new System.Drawing.Size(75, 23);
            this.CheckSerialButton.TabIndex = 4;
            this.CheckSerialButton.Text = "扫描串口";
            this.CheckSerialButton.UseVisualStyleBackColor = true;
            this.CheckSerialButton.Click += new System.EventHandler(this.CheckSerialButton_Click);
            // 
            // OpenSerialButton
            // 
            this.OpenSerialButton.Location = new System.Drawing.Point(127, 127);
            this.OpenSerialButton.Name = "OpenSerialButton";
            this.OpenSerialButton.Size = new System.Drawing.Size(75, 23);
            this.OpenSerialButton.TabIndex = 5;
            this.OpenSerialButton.Text = "打开串口";
            this.OpenSerialButton.UseVisualStyleBackColor = true;
            this.OpenSerialButton.Click += new System.EventHandler(this.OpenSerialButton_Click);
            // 
            // CheckDataShowStyleBox
            // 
            this.CheckDataShowStyleBox.AutoSize = true;
            this.CheckDataShowStyleBox.Location = new System.Drawing.Point(127, 174);
            this.CheckDataShowStyleBox.Name = "CheckDataShowStyleBox";
            this.CheckDataShowStyleBox.Size = new System.Drawing.Size(89, 19);
            this.CheckDataShowStyleBox.TabIndex = 6;
            this.CheckDataShowStyleBox.Text = "字符显示";
            this.CheckDataShowStyleBox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.UartDataBox);
            this.groupBox2.Location = new System.Drawing.Point(364, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(697, 452);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据接收";
            // 
            // UartDataBox
            // 
            this.UartDataBox.BackColor = System.Drawing.SystemColors.Window;
            this.UartDataBox.Location = new System.Drawing.Point(14, 24);
            this.UartDataBox.Multiline = true;
            this.UartDataBox.Name = "UartDataBox";
            this.UartDataBox.ReadOnly = true;
            this.UartDataBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UartDataBox.Size = new System.Drawing.Size(670, 415);
            this.UartDataBox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BootButton);
            this.groupBox3.Controls.Add(this.UpdateProgressBar);
            this.groupBox3.Controls.Add(this.StartUpdateButton);
            this.groupBox3.Controls.Add(this.BandConfigButton);
            this.groupBox3.Controls.Add(this.DeviceBandListBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.CheckDeviceButton);
            this.groupBox3.Location = new System.Drawing.Point(33, 245);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(295, 335);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "设置功能";
            // 
            // CheckDeviceButton
            // 
            this.CheckDeviceButton.Location = new System.Drawing.Point(197, 286);
            this.CheckDeviceButton.Name = "CheckDeviceButton";
            this.CheckDeviceButton.Size = new System.Drawing.Size(75, 23);
            this.CheckDeviceButton.TabIndex = 2;
            this.CheckDeviceButton.Text = "查询设备";
            this.CheckDeviceButton.UseVisualStyleBackColor = true;
            this.CheckDeviceButton.Click += new System.EventHandler(this.CheckDeviceButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "选择波特率";
            // 
            // DeviceBandListBox
            // 
            this.DeviceBandListBox.FormattingEnabled = true;
            this.DeviceBandListBox.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.DeviceBandListBox.Location = new System.Drawing.Point(108, 247);
            this.DeviceBandListBox.Name = "DeviceBandListBox";
            this.DeviceBandListBox.Size = new System.Drawing.Size(121, 23);
            this.DeviceBandListBox.TabIndex = 4;
            // 
            // BandConfigButton
            // 
            this.BandConfigButton.Location = new System.Drawing.Point(93, 286);
            this.BandConfigButton.Name = "BandConfigButton";
            this.BandConfigButton.Size = new System.Drawing.Size(90, 23);
            this.BandConfigButton.TabIndex = 5;
            this.BandConfigButton.Text = "设置波特率";
            this.BandConfigButton.UseVisualStyleBackColor = true;
            this.BandConfigButton.Click += new System.EventHandler(this.BandConfigButton_Click);
            // 
            // StartUpdateButton
            // 
            this.StartUpdateButton.Location = new System.Drawing.Point(12, 200);
            this.StartUpdateButton.Name = "StartUpdateButton";
            this.StartUpdateButton.Size = new System.Drawing.Size(75, 23);
            this.StartUpdateButton.TabIndex = 8;
            this.StartUpdateButton.Text = "开始升级";
            this.StartUpdateButton.UseVisualStyleBackColor = true;
            this.StartUpdateButton.Click += new System.EventHandler(this.StartUpdateButton_Click);
            // 
            // UpdateProgressBar
            // 
            this.UpdateProgressBar.Location = new System.Drawing.Point(12, 159);
            this.UpdateProgressBar.Name = "UpdateProgressBar";
            this.UpdateProgressBar.Size = new System.Drawing.Size(250, 23);
            this.UpdateProgressBar.TabIndex = 6;
            // 
            // BootButton
            // 
            this.BootButton.Location = new System.Drawing.Point(93, 36);
            this.BootButton.Name = "BootButton";
            this.BootButton.Size = new System.Drawing.Size(75, 23);
            this.BootButton.TabIndex = 9;
            this.BootButton.Text = "boot";
            this.BootButton.UseVisualStyleBackColor = true;
            this.BootButton.Click += new System.EventHandler(this.BootButton_Click);
            // 
            // ChoseFileButton
            // 
            this.ChoseFileButton.Location = new System.Drawing.Point(45, 361);
            this.ChoseFileButton.Name = "ChoseFileButton";
            this.ChoseFileButton.Size = new System.Drawing.Size(75, 23);
            this.ChoseFileButton.TabIndex = 4;
            this.ChoseFileButton.Text = "选择固件";
            this.ChoseFileButton.UseVisualStyleBackColor = true;
            this.ChoseFileButton.Click += new System.EventHandler(this.ChoseFileButton_Click);
            // 
            // FilePathShowBox
            // 
            this.FilePathShowBox.Location = new System.Drawing.Point(126, 361);
            this.FilePathShowBox.Name = "FilePathShowBox";
            this.FilePathShowBox.Size = new System.Drawing.Size(169, 25);
            this.FilePathShowBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "选择设备";
            // 
            // ChoseUpdateDeviceBox
            // 
            this.ChoseUpdateDeviceBox.FormattingEnabled = true;
            this.ChoseUpdateDeviceBox.Items.AddRange(new object[] {
            "主机",
            "从机1",
            "从机2",
            "从机3",
            "从机4",
            "从机5",
            "从机6",
            "从机7",
            "从机8",
            "从机9",
            "从机10"});
            this.ChoseUpdateDeviceBox.Location = new System.Drawing.Point(126, 323);
            this.ChoseUpdateDeviceBox.Name = "ChoseUpdateDeviceBox";
            this.ChoseUpdateDeviceBox.Size = new System.Drawing.Size(121, 23);
            this.ChoseUpdateDeviceBox.TabIndex = 7;
            // 
            // ClearUartDataShowButton
            // 
            this.ClearUartDataShowButton.Location = new System.Drawing.Point(378, 510);
            this.ClearUartDataShowButton.Name = "ClearUartDataShowButton";
            this.ClearUartDataShowButton.Size = new System.Drawing.Size(75, 23);
            this.ClearUartDataShowButton.TabIndex = 9;
            this.ClearUartDataShowButton.Text = "清空数据";
            this.ClearUartDataShowButton.UseVisualStyleBackColor = true;
            this.ClearUartDataShowButton.Click += new System.EventHandler(this.ClearUartDataShowButton_Click);
            // 
            // DebugTab
            // 
            this.DebugTab.Controls.Add(this.tabUart);
            this.DebugTab.Controls.Add(this.tabCan);
            this.DebugTab.Location = new System.Drawing.Point(-2, 0);
            this.DebugTab.Name = "DebugTab";
            this.DebugTab.SelectedIndex = 0;
            this.DebugTab.Size = new System.Drawing.Size(1093, 643);
            this.DebugTab.TabIndex = 0;
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1092, 642);
            this.Controls.Add(this.DebugTab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugForm";
            this.Text = "调试助手";
            this.Load += new System.EventHandler(this.DebugForm_Load);
            this.tabCan.ResumeLayout(false);
            this.tabCan.PerformLayout();
            this.tabUart.ResumeLayout(false);
            this.tabUart.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.DebugTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1000ms;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.OpenFileDialog openBinFile;
        private System.Windows.Forms.TabPage tabCan;
        private System.Windows.Forms.Button OpenCanDeviceButton;
        private System.Windows.Forms.TextBox CanDataRcvBox;
        private System.Windows.Forms.TabPage tabUart;
        private System.Windows.Forms.Button ClearUartDataShowButton;
        private System.Windows.Forms.ComboBox ChoseUpdateDeviceBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FilePathShowBox;
        private System.Windows.Forms.Button ChoseFileButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button BootButton;
        private System.Windows.Forms.ProgressBar UpdateProgressBar;
        private System.Windows.Forms.Button StartUpdateButton;
        private System.Windows.Forms.Button BandConfigButton;
        private System.Windows.Forms.ComboBox DeviceBandListBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CheckDeviceButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox UartDataBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CheckDataShowStyleBox;
        private System.Windows.Forms.Button OpenSerialButton;
        private System.Windows.Forms.Button CheckSerialButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox BandListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SerialListBox;
        private System.Windows.Forms.TabControl DebugTab;
    }
}

