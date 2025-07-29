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
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
            LoadUartUserControlToTab();
            LoadCanUserControlToTab();
        }
        private void LoadUartUserControlToTab()
        {
            // 确保 UserControl 实例化
            var userControl = new UartTabUserControl(); // 替换为你的 UserControl 类型

            // 设置 Dock 布局以填充整个 TabPage
            userControl.Dock = DockStyle.Fill;

            // 清除 TabPage 中原有控件（可选）
            tabUart.Controls.Clear();

            // 将 UserControl 添加到已有的 TabPage 中
            tabUart.Controls.Add(userControl);
        }

        private void LoadCanUserControlToTab()
        {
            // 确保 UserControl 实例化
            var userControl = new CtrlCoeffUserControl(); // 替换为你的 UserControl 类型

            // 设置 Dock 布局以填充整个 TabPage
            userControl.Dock = DockStyle.Fill;

            // 清除 TabPage 中原有控件（可选）
            tabCtrlCoeff.Controls.Clear();

            // 将 UserControl 添加到已有的 TabPage 中
            tabCtrlCoeff.Controls.Add(userControl);

            var userControl2 = new CanTabUserControl(userControl); // 替换为你的 UserControl 类型

            // 设置 Dock 布局以填充整个 TabPage
            userControl2.Dock = DockStyle.Fill;

            // 清除 TabPage 中原有控件（可选）
            tabCan.Controls.Clear();

            // 将 UserControl 添加到已有的 TabPage 中
            tabCan.Controls.Add(userControl2);


        }

        private void DebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
        }
    }
