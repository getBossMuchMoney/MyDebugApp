using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MyDebugApp
{
    class HighPrecisionTimer : IDisposable
    {
        #region Win32 API Definitions

        [DllImport("winmm.dll")]
        private static extern uint timeSetEvent(uint delay, uint resolution, TimerCallback handler, IntPtr user, uint eventType);

        [DllImport("winmm.dll")]
        private static extern uint timeKillEvent(uint timerId);

        private const uint TIME_ONESHOT = 0;
        private const uint TIME_PERIODIC = 1;

        private delegate void TimerCallback(uint id, uint msg, IntPtr user, IntPtr param1, IntPtr param2);

        #endregion

        #region Private Fields

        private uint _timerId;
        private bool _isRunning;
        private readonly TimerCallback _callback;

        #endregion

        #region Constructor / Finalizer

        public HighPrecisionTimer()
        {
            _callback = OnTimerCallback;
        }

        ~HighPrecisionTimer()
        {
            Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 启动定时器
        /// </summary>
        /// <param name="interval">间隔时间（毫秒）</param>
        public void Start(int interval)
        {
            if (_isRunning) return;

            // 设置周期性定时器，精度为1ms
            _timerId = timeSetEvent((uint)interval, 0, _callback, IntPtr.Zero, TIME_PERIODIC);

            if (_timerId == 0)
                throw new Exception("无法启动多媒体定时器，请检查权限或系统资源。");

            _isRunning = true;
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        public void Stop()
        {
            if (!_isRunning) return;

            timeKillEvent(_timerId);
            _isRunning = false;
        }

        #endregion

        #region Callback Handler

        /// <summary>
        /// 定时器触发时调用的回调方法
        /// </summary>
        private void OnTimerCallback(uint id, uint msg, IntPtr user, IntPtr param1, IntPtr param2)
        {
            Callback?.Invoke();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 定时器触发事件回调
        /// </summary>
        public Action Callback { get; set; }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }

        #endregion
    }
}
