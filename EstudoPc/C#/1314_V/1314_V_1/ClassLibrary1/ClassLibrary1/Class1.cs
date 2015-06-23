using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    
    /*public class UnsafeCountdownLatch {
        private int count;
        private readonly ManualResetEvent waitEvent = new ManualResetEvent(false); !
            
     * public UnsafeCountdownLatch(int c) {
         if (c <= 0) throw new InvalidOperationException();
            count = c;
        }

     * public bool Wait(int timeout) {
        if (count == 0) return true;
        waitEvent.WaitOne(timeout);
            return count == 0;
        } 

     *  public void Signal() {
            if (count == 0) throw new InvalidOperationException();
            if (--count == 0) waitEvent.Set();
        }

     *  public void AddCount() {
            if (count == 0 || count + 1 < 0) throw new InvalidOperationException();
                count++;
        }
}*/

    // A classe nao é thread safe pois nao garante que o contador da classe lido é o que foi escrito pela ultima vez, visto a variavel 
    //count nao ser volatile

    //Volatile - usado para impedir que o compilador faça optimizaçoes,ignorando algum codiogo. Garantimos tbm
     //          que o valor lido é sempre o ultimo que foi escrito na varivel.


    // As operaçoes efectuadas sobre a variavel count tbm nao sao atomicas, sendo estas necessárias pq fazem com que tudo seja feito
    // numa unica operaçao. 

      #pragma warning disable 420
    public class UnsafeCountdownLatch
    {
        private volatile int count;
        private readonly ManualResetEvent waitEvent = new ManualResetEvent(false);
        public UnsafeCountdownLatch(int c)
        {
            if (c <= 0) throw new InvalidOperationException();
            int lastValue = count;
            Interlocked.CompareExchange(ref count, c, lastValue);

        }
        public bool Wait(int timeout)
        {
            if (count == 0) return true;
            waitEvent.WaitOne(timeout);
            return count == 0;
        }
        public void Signal()
        {
            if (count == 0) throw new InvalidOperationException();
            if (count == 0)
            {
                Interlocked.Decrement(ref count);
                waitEvent.Set();
            }
        }
        public void AddCount()
        {

            int lastValue = count;
            if (lastValue == 0 || lastValue + 1 < 0) throw new InvalidOperationException();

            Interlocked.Increment(ref count);
        }
    }
}
