using System;
using System.Collections.Generic;
using System.Threading;

namespace Ex1 {

    /*
     Implemente, em Java ou C#, o sincronizador synchronous queue , definindo a classe
    SynchronousQueue<T>, cuja interface pública, definida em Java , é a seguinte: T take()
    throws InterruptedException e void put(T msg) throws InterruptedException. 
    Este sincronizador suporta a comunicação entre threads produtoras e threads consumidoras
    através de mensagens do tipo genérico T .
    Ao invocar o método put, uma thread produtora oferece uma mensagem para recepção, mas 
    aguarda que a mensagem seja recebida, bloqueando se necessário. As threads consumidoras
    declaram a sua disponibilidade para receber mensagens invocando o método take, sendo bloqueadas 
    até que exista uma mensagem disponível ou a espera seja interrompida. As mensagens devem ser 
    entregues por ordem de chegada (FIFO), sendo a selecção das threads consumidoras feita por ordem
    inversa (LIFO). O sincronizador deve suportar o cancelamento devido à interrupção das
    threads bloqueadas por qualquer dos métodos.
     */
    public class SynchronousQueue<T> {

        private readonly object lockObj = new object();

        //estruta que vai guardar o pedido para ser processado
        private struct Obj<T> {
            private T data;
            public Obj(T info) {
                data = info;
            }
            public T Data {
                get { return data; }
                set { data = value; }
            }
        }

        private bool hasMsg;

        //lista que vai conter as mensagens
        private LinkedList<Obj<T>> queue;

        public SynchronousQueue() {
            queue = new LinkedList<Obj<T>>();
            hasMsg = false;
        }

        public T Take() {
            lock (lockObj) {
                Obj<T> obj = new Obj<T>(default(T));


                while (queue.Count == 0) {
                    try {
                        Monitor.Wait(lockObj);
                    } catch (ThreadInterruptedException) {
                        Monitor.PulseAll(lockObj);
                        throw new ThreadInterruptedException();
                    } 
                }
                try {
                    if (queue.Count > 0) {
                        obj = queue.First.Value;
                        hasMsg = false;
                    }
                    Monitor.PulseAll(lockObj);
                } finally {
                    if (!hasMsg && queue.Count > 0) {
                        queue.RemoveFirst();
                    }
                }
                return obj.Data;
            }

        }

        public void Put(T msg) {
            lock (lockObj) {    
                try {
            
                    if (msg.Equals(default(T)))
                        throw new ArgumentException("Null value parameter!");
            
                    
                    Obj<T> newObj = new Obj<T>(msg);

                    queue.AddLast(newObj); //queue.AddLast(new Obj<T>(msg));
                    hasMsg = true;
                    Monitor.PulseAll(lockObj);
                    Monitor.Wait(lockObj);
                } catch (ThreadInterruptedException) {
                    throw new ThreadInterruptedException();
                }
            }
        }

    }
}
