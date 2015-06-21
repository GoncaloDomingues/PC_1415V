using System;
using System.Threading;
using System.Collections.Generic;

namespace Ex1 {

    /*
     
     Define, em Java ou C#, a classe MessageQueue<T> que implemente o sincronizador message
    queue para suportar a comunicação entre threads , através de mensagens, mas com
    possibilidade de na recepçao filtrar as mensagens a receber. As mensagens têm associado
    um tipo (inteiro positivo) e um campo de dados do tipo genérico T. A operação Send promove a
    entrega de uma mensagem à fila; esta operação nunca bloqueia a thread invocante. A
    operação Receive promove a recolha da mensagem mais antiga que satisfaça o predicado
    especificado através do parâmetro selector (do tipo Predicate<int>) , a aplicar ao tipo das
    mensagens que forem enviadas para a a fila. O sincronizador deve garantir que as mensagens
    do mesmo tipo são entregues pela ordem de chegada e que as threads receptoras também
    são servidas por ordem de chegada. A operação Receive deve suportar desistência por
    timeout e por cancelamento devido à interrupção des threads bloqueadas.
     
     */

    public struct Obj<T> {
        private int val;
        private T data;
        public Obj(int v, T info) {
            val = v;
            data = info;
        }
        public T Data {
            get { return data; }
            set { data = value; }
        }
        public int Val {
            get { return val; }
            set { val = value; }
        }
    }

    public class MessageQueue<T> {

        private readonly object lockObj = new object();

        private LinkedList<Obj<T>> messages;

        public MessageQueue() {
            messages = new LinkedList<Obj<T>>();
        }

        public void Send(Obj<T> msg) {
            lock (lockObj) {
                messages.AddLast(msg);
                Monitor.PulseAll(lockObj);
            }
        }

        public Obj<T> Receiver(Predicate<int> selector, int timeout) {
            lock (lockObj) {
                Obj<T> obj = new Obj<T>();

                try {
                    Monitor.Wait(lockObj, timeout);
                    int actual = Environment.TickCount;

                    timeout = SyncUtils.AdjustTimeout(ref actual, ref timeout);
                    if (timeout >= actual)
                        return obj;

                    if (messages.Count > 0) {
                        foreach(Obj<T> o in messages) {
                            if (selector(o.Val)) {
                                obj = o;
                                messages.Remove(o);
                                break;
                            }
                        }
                    }

                } catch (ThreadInterruptedException) {
                    throw new ThreadInterruptedException();
                }
                return obj;
            }
        }

    }

}

