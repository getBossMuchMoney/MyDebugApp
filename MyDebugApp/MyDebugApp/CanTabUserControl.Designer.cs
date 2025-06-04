
namespace MyDebugApp
{
    partial class CanTabUserControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openBinFile = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CanBandListBox = new System.Windows.Forms.ComboBox();
            this.CanDevPassNumBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CanDevListBox = new System.Windows.Forms.ComboBox();
            this.OpenCanDevButton = new System.Windows.Forms.Button();
            this.ResetCanDevButton = new System.Windows.Forms.Button();
            this.ScanCanButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StartUpdateButton = new System.Windows.Forms.Button();
            this.UpdateProgressBar = new System.Windows.Forms.ProgressBar();
            this.ChoseBinFileButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.BinFilePathBox = new System.Windows.Forms.TextBox();
            this.ChoseDevListBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BootButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ClearRcvDataShowButton = new System.Windows.Forms.Button();
            this.ShowRcvDataBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // openBinFile
            // 
            this.openBinFile.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CanBandListBox);
            this.groupBox1.Controls.Add(this.CanDevPassNumBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CanDevListBox);
            this.groupBox1.Controls.Add(this.OpenCanDevButton);
            this.groupBox1.Controls.Add(this.ResetCanDevButton);
            this.groupBox1.Controls.Add(this.ScanCanButton);
            this.groupBox1.Location = new System.Drawing.Point(15, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 251);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "can设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "波特率";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "通道号";
            // 
            // CanBandListBox
            // 
            this.CanBandListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CanBandListBox.FormattingEnabled = true;
            this.CanBandListBox.Items.AddRange(new object[] {
            "125kbit",
            "250kbit",
            "500kbit"});
            this.CanBandListBox.Location = new System.Drawing.Point(72, 142);
            this.CanBandListBox.Name = "CanBandListBox";
            this.CanBandListBox.Size = new System.Drawing.Size(95, 23);
            this.CanBandListBox.TabIndex = 6;
            // 
            // CanDevPassNumBox
            // 
            this.CanDevPassNumBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CanDevPassNumBox.FormattingEnabled = true;
            this.CanDevPassNumBox.Items.AddRange(new object[] {
            "1通道",
            "2通道"});
            this.CanDevPassNumBox.Location = new System.Drawing.Point(72, 91);
            this.CanDevPassNumBox.Name = "CanDevPassNumBox";
            this.CanDevPassNumBox.Size = new System.Drawing.Size(95, 23);
            this.CanDevPassNumBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "设备序号";
            // 
            // CanDevListBox
            // 
            this.CanDevListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CanDevListBox.FormattingEnabled = true;
            this.CanDevListBox.Location = new System.Drawing.Point(72, 39);
            this.CanDevListBox.Name = "CanDevListBox";
            this.CanDevListBox.Size = new System.Drawing.Size(95, 23);
            this.CanDevListBox.TabIndex = 3;
            // 
            // OpenCanDevButton
            // 
            this.OpenCanDevButton.Location = new System.Drawing.Point(72, 203);
            this.OpenCanDevButton.Name = "OpenCanDevButton";
            this.OpenCanDevButton.Size = new System.Drawing.Size(95, 23);
            this.OpenCanDevButton.TabIndex = 2;
            this.OpenCanDevButton.Text = "打开分析仪";
            this.OpenCanDevButton.UseVisualStyleBackColor = true;
            this.OpenCanDevButton.Click += new System.EventHandler(this.OpenCanDevButton_Click);
            // 
            // ResetCanDevButton
            // 
            this.ResetCanDevButton.Location = new System.Drawing.Point(188, 203);
            this.ResetCanDevButton.Name = "ResetCanDevButton";
            this.ResetCanDevButton.Size = new System.Drawing.Size(103, 23);
            this.ResetCanDevButton.TabIndex = 1;
            this.ResetCanDevButton.Text = "复位分析仪";
            this.ResetCanDevButton.UseVisualStyleBackColor = true;
            this.ResetCanDevButton.Click += new System.EventHandler(this.ResetCanDevButton_Click);
            // 
            // ScanCanButton
            // 
            this.ScanCanButton.Location = new System.Drawing.Point(188, 38);
            this.ScanCanButton.Name = "ScanCanButton";
            this.ScanCanButton.Size = new System.Drawing.Size(75, 23);
            this.ScanCanButton.TabIndex = 0;
            this.ScanCanButton.Text = "扫描设备";
            this.ScanCanButton.UseVisualStyleBackColor = true;
            this.ScanCanButton.Click += new System.EventHandler(this.ScanCanButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.StartUpdateButton);
            this.groupBox2.Controls.Add(this.UpdateProgressBar);
            this.groupBox2.Controls.Add(this.ChoseBinFileButton);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.BinFilePathBox);
            this.groupBox2.Controls.Add(this.ChoseDevListBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.BootButton);
            this.groupBox2.Location = new System.Drawing.Point(15, 297);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 293);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "升级功能";
            // 
            // StartUpdateButton
            // 
            this.StartUpdateButton.Location = new System.Drawing.Point(21, 248);
            this.StartUpdateButton.Name = "StartUpdateButton";
            this.StartUpdateButton.Size = new System.Drawing.Size(75, 23);
            this.StartUpdateButton.TabIndex = 8;
            this.StartUpdateButton.Text = "开始升级";
            this.StartUpdateButton.UseVisualStyleBackColor = true;
            this.StartUpdateButton.Click += new System.EventHandler(this.StartUpdateButton_Click);
            // 
            // UpdateProgressBar
            // 
            this.UpdateProgressBar.Location = new System.Drawing.Point(21, 201);
            this.UpdateProgressBar.Name = "UpdateProgressBar";
            this.UpdateProgressBar.Size = new System.Drawing.Size(242, 23);
            this.UpdateProgressBar.TabIndex = 7;
            // 
            // ChoseBinFileButton
            // 
            this.ChoseBinFileButton.Location = new System.Drawing.Point(22, 147);
            this.ChoseBinFileButton.Name = "ChoseBinFileButton";
            this.ChoseBinFileButton.Size = new System.Drawing.Size(75, 23);
            this.ChoseBinFileButton.TabIndex = 6;
            this.ChoseBinFileButton.Text = "选择文件";
            this.ChoseBinFileButton.UseVisualStyleBackColor = true;
            this.ChoseBinFileButton.Click += new System.EventHandler(this.ChoseBinFileButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 15);
            this.label6.TabIndex = 5;
            // 
            // BinFilePathBox
            // 
            this.BinFilePathBox.Location = new System.Drawing.Point(111, 147);
            this.BinFilePathBox.Name = "BinFilePathBox";
            this.BinFilePathBox.Size = new System.Drawing.Size(152, 25);
            this.BinFilePathBox.TabIndex = 4;
            // 
            // ChoseDevListBox
            // 
            this.ChoseDevListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChoseDevListBox.FormattingEnabled = true;
            this.ChoseDevListBox.Items.AddRange(new object[] {
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
            "从机10",
            "从机11",
            "从机12"});
            this.ChoseDevListBox.Location = new System.Drawing.Point(111, 94);
            this.ChoseDevListBox.Name = "ChoseDevListBox";
            this.ChoseDevListBox.Size = new System.Drawing.Size(85, 23);
            this.ChoseDevListBox.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "选择设备";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "boot命令";
            // 
            // BootButton
            // 
            this.BootButton.Location = new System.Drawing.Point(111, 42);
            this.BootButton.Name = "BootButton";
            this.BootButton.Size = new System.Drawing.Size(75, 23);
            this.BootButton.TabIndex = 0;
            this.BootButton.Text = "boot";
            this.BootButton.UseVisualStyleBackColor = true;
            this.BootButton.Click += new System.EventHandler(this.BootButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ClearRcvDataShowButton);
            this.groupBox3.Controls.Add(this.ShowRcvDataBox);
            this.groupBox3.Location = new System.Drawing.Point(342, 14);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(718, 576);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据显示";
            // 
            // ClearRcvDataShowButton
            // 
            this.ClearRcvDataShowButton.Location = new System.Drawing.Point(7, 531);
            this.ClearRcvDataShowButton.Name = "ClearRcvDataShowButton";
            this.ClearRcvDataShowButton.Size = new System.Drawing.Size(75, 23);
            this.ClearRcvDataShowButton.TabIndex = 1;
            this.ClearRcvDataShowButton.Text = "清除数据";
            this.ClearRcvDataShowButton.UseVisualStyleBackColor = true;
            this.ClearRcvDataShowButton.Click += new System.EventHandler(this.ClearRcvDataShowButton_Click);
            // 
            // ShowRcvDataBox
            // 
            this.ShowRcvDataBox.Location = new System.Drawing.Point(7, 21);
            this.ShowRcvDataBox.Multiline = true;
            this.ShowRcvDataBox.Name = "ShowRcvDataBox";
            this.ShowRcvDataBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ShowRcvDataBox.Size = new System.Drawing.Size(705, 486);
            this.ShowRcvDataBox.TabIndex = 0;
            // 
            // CanTabUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CanTabUserControl";
            this.Size = new System.Drawing.Size(1085, 614);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openBinFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ResetCanDevButton;
        private System.Windows.Forms.Button ScanCanButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CanDevListBox;
        private System.Windows.Forms.Button OpenCanDevButton;
        private System.Windows.Forms.ComboBox CanBandListBox;
        private System.Windows.Forms.ComboBox CanDevPassNumBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BootButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox BinFilePathBox;
        private System.Windows.Forms.ComboBox ChoseDevListBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ChoseBinFileButton;
        private System.Windows.Forms.ProgressBar UpdateProgressBar;
        private System.Windows.Forms.Button StartUpdateButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button ClearRcvDataShowButton;
        private System.Windows.Forms.TextBox ShowRcvDataBox;
    }
}
