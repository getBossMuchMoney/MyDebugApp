using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MyDebugApp
{
    public class HighPrecisionTimer
    {
        public delegate void TimerCallback(uint id, uint msg, uint user, uint param1, uint param2);

        [DllImport("winmm.dll")]
        private static extern uint timeSetEvent(uint delay, uint resolution, TimerCallback callback, uint user, uint mode);

        [DllImport("winmm.dll")]
        private static extern uint timeKillEvent(uint timerId);

        private uint _timerId;
        private GCHandle _callbackHandle;

        public Action<uint, uint, uint, uint, uint> Callback { get; set; }

        public void Start(int interval)
        {
            if (_timerId != 0) return;

            // 将委托包装为 TimerCallback，并固定内存
            TimerCallback nativeCallback = (id, msg, user, param1, param2) =>
            {
                Callback?.Invoke(id, msg, user, param1, param2);
            };

            _callbackHandle = GCHandle.Alloc(nativeCallback, GCHandleType.Normal);
            _timerId = timeSetEvent((uint)interval, 0, nativeCallback, 0, 1);
        }

        public void Stop()
        {
            if (_timerId != 0)
            {
            timeKillEvent(_timerId);
                _timerId = 0;
        }

            if (_callbackHandle.IsAllocated)
        {
                _callbackHandle.Free();
        }
        }

        ~HighPrecisionTimer()
            {
                Stop();
            }
        }
}
