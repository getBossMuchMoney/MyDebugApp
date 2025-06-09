
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            this.tabCan = new System.Windows.Forms.TabPage();
            this.tabUart = new System.Windows.Forms.TabPage();
            this.DebugTab = new System.Windows.Forms.TabControl();
            this.DebugTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCan
            // 
            this.tabCan.Location = new System.Drawing.Point(4, 25);
            this.tabCan.Name = "tabCan";
            this.tabCan.Padding = new System.Windows.Forms.Padding(3);
            this.tabCan.Size = new System.Drawing.Size(1085, 614);
            this.tabCan.TabIndex = 1;
            this.tabCan.Text = "CAN升级";
            this.tabCan.UseVisualStyleBackColor = true;
            // 
            // tabUart
            // 
            this.tabUart.Location = new System.Drawing.Point(4, 25);
            this.tabUart.Name = "tabUart";
            this.tabUart.Padding = new System.Windows.Forms.Padding(3);
            this.tabUart.Size = new System.Drawing.Size(1085, 614);
            this.tabUart.TabIndex = 0;
            this.tabUart.Text = "串口升级";
            this.tabUart.UseVisualStyleBackColor = true;
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
            this.Text = "在线升级助手";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DebugForm_FormClosed);
            this.DebugTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabCan;
        private System.Windows.Forms.TabPage tabUart;
        private System.Windows.Forms.TabControl DebugTab;
    }
}

