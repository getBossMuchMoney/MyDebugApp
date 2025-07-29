using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MyDebugApp
{
    public static class CanCrossFileQueue
    {
        public static BlockingCollection<uint[]> CtrlSettingRxQueue = new BlockingCollection<uint[]>(new ConcurrentQueue<uint[]>());
        public static BlockingCollection<uint[]> AppTxQueue = new BlockingCollection<uint[]>(new ConcurrentQueue<uint[]>());
    }
}
