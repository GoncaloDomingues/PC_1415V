using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FutureHolderNonBlocking
{
    class SyncUtils
    {

        public static int AdjustTimeout(ref int lastTime, ref int timeout)
        {
            if (timeout != Timeout.Infinite)
            {
                int now = Environment.TickCount;
                int elapsed = (now == lastTime) ? 1 : now - lastTime;
                if (elapsed >= timeout)
                {
                    timeout = 0;
                }
                else
                {
                    timeout -= elapsed;
                    lastTime = now;
                }
            }
            return timeout;
        }

        public static void EnterUninterruptibly(object mlock, out bool interrupted)
        {
            interrupted = false;

            do
            {
                try
                {
                    Monitor.Enter(mlock);
                    break;
                }
                catch (ThreadInterruptedException)
                {
                    interrupted = true;
                }
            } while (true);
        }

        public static void Notify(object mlock, object condition)
        {
            if (mlock == condition)
            {
                Monitor.Pulse(mlock);
                return;
            }

            bool interrupted;

            EnterUninterruptibly(condition, out interrupted);
            Monitor.Pulse(condition);
            Monitor.Exit(condition);

            if (interrupted)
                Thread.CurrentThread.Interrupt();
        }
    }

}
