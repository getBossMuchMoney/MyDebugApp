using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MyDebugApp
{
    class HighPrecisionTimer
    {
        // 定义 TimerProc 委托签名（Win32 多媒体定时器需要）
        private delegate void TimerProc(uint id, uint msg, IntPtr user, uint dw1, uint dw2);

        [DllImport("winmm.dll")]
        private static extern uint timeSetEvent(uint delay, uint resolution, TimerProc callback, IntPtr user, uint mode);

        [DllImport("winmm.dll")]
        private static extern uint timeKillEvent(uint timerId);

        private uint _timerId;
        private GCHandle _gcHandle; // 用于保护 this 不被回收
        private Action _callback;
        private TimerProc _timerProc; // ⚠️ 关键：保存委托为类成员

        public Action Callback
        {
            get => _callback;
            set => _callback = value;
        }

        public void Start(int interval)
        {
            if (_timerId != 0)
                Stop();

            // 保持对 this 的引用，防止被 GC 回收
            _gcHandle = GCHandle.Alloc(this, GCHandleType.Normal);

            // ⚠️ 将委托保存为成员变量，防止被 GC 回收
            _timerProc = OnTimer;

            // 注册非托管定时器
            _timerId = timeSetEvent((uint)interval, 0, _timerProc, GCHandle.ToIntPtr(_gcHandle), 1);
        }

        private static void OnTimer(uint id, uint msg, IntPtr user, uint dw1, uint dw2)
        {
            // 恢复对象实例
            GCHandle handle = GCHandle.FromIntPtr(user);
            HighPrecisionTimer timer = (HighPrecisionTimer)handle.Target;

            // 调用外部回调
            timer._callback?.Invoke();
        }

        public void Stop()
        {
            if (_timerId != 0)
            {
                timeKillEvent(_timerId);
                _timerId = 0;
            }

            if (_gcHandle.IsAllocated)
            {
                _gcHandle.Free();
            }

            // 显式置空委托，帮助调试并避免误用
            _timerProc = null;
        }

        ~HighPrecisionTimer()
        {
            Stop();
        }
    }
}