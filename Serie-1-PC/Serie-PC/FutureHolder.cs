using System;
using System.Threading;

namespace Ex1 {
    /*
     Implemente, em Java ou C#, o sincronizador future holder que se destina a ser usado para
    hospedar dados produzido por cálculos demorados. Defina a classe FutureHolder<T> cuja
    interface pública, definida em Java , é a seguinte: void setValue(T value), T
    getValue(long timeout) throws InterruptedException e boolean
    isValueAvailable().

     O método getValue bloqueia as threads evocantes até que os dados subjacentes ao
    sincronizador sejam disponibilizados através da chamada ao método setValue. (Como as
    instâncias desta classe são de utilização única, chamadas subsequentes ao método setValue
    produzem excepção a InvalidOperationException) . A operação getValue retorna os
    dados hospedados, ou null no caso de ocorrer timeout . O sincronizador deve suportar o
    cancelamento devido à interrupção das threads bloqueadas pelo método getValue
     */
    public class FutureHolder<T> {

        private readonly object lockObj = new object();

        // variável que permite saber se existe um valor atribuido
        private bool hasValue;

        private T val;

        public FutureHolder() {
            hasValue = false;
        }

        public void SetValue(T value) {
            lock (lockObj) {
                if (value == null)
                    throw new ArgumentException("Null value parameter!");
                if (IsValueAvailable())
                    throw new InvalidOperationException("Thread already got exclusive lock!");

                val = value;
                hasValue = true;
                //depois de ter um valor atribuido, notifica todas as threads.
                Monitor.PulseAll(lockObj);
            }
        }

        public T GetValue(int timeout) {
            lock (lockObj) {
                try {
                    if (timeout <= 0)
                        return val;
                    //espera para ter um valor
                    Monitor.Wait(lockObj, timeout);

                    int actual = Environment.TickCount;
                    timeout = SyncUtils.AdjustTimeout(ref actual, ref timeout);
                    //se ocorreu um timeout e não existe valor retorna null
                    if (timeout >= actual)
                        return default(T);

                    hasValue = false;
                    return val;
                } catch (ThreadInterruptedException) {
                    throw new ThreadInterruptedException();
                }
            }
        }

        private bool IsValueAvailable() {
            return hasValue;
        }

    }

}
