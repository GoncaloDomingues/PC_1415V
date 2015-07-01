using System;
using System.Threading;
using System.Threading.Tasks;

namespace EstudoPC_exame {

    class UnsafeSpinBarrier {
        public int toArrive;

        /*
         * os tipos primitivos não são thread-safe, pois as escritas
         * podem não ser atómicas.
         * Quando à resolução da Barrier os métodos addPartner 
         * e removePartner estão errados: em ambos é necessário 
         * garantir atomicidade do teste com a alteração do estado,
         * o que não está a fazer. (nota: não fazer toArrive.get() == 0)
         */

        public UnsafeSpinBarrier(int partners) {
            if (partners <= 0)
                throw new ArgumentException();
            toArrive = partners;
        }

        public void signalAndWait() {
            int oldToArrive = toArrive;
            if (Interlocked.CompareExchange(ref toArrive, 0, oldToArrive) == 0)
                throw new Exception();

            if (Interlocked.Decrement(ref toArrive) > 0)
                do {
                    //Thread.CurrentThread.yield;
                } while (!(Interlocked.CompareExchange(ref toArrive, 0, oldToArrive) == 0));
        }

        public void addPartner() {
            int oldToArrive = toArrive;
            if (Interlocked.CompareExchange(ref toArrive, 0, oldToArrive) == 0)
                throw new Exception();
            Interlocked.Increment(ref toArrive);
        }

        public void removePartner() {
            Interlocked.Decrement(ref toArrive);
            int oldToArrive = toArrive;
            if (Interlocked.CompareExchange(ref toArrive, 0, oldToArrive) == 0)
                throw new Exception();
        }
    }

    class Program {
        static void Main(string[] args) {
        }
    }
}
