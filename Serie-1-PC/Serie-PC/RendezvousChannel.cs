using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex1 {

    interface Token<T> {
        T Value();
    }

    public class RendezvousChannel<S,R> {

        //TimeoutException e InterruptedException
        public R Request(S service, int timeout) {
            return default(R);
        }

        

    }

}
