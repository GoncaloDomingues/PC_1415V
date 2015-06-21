using System;
using System.Threading;

namespace Ex1 {
    /*
    Implemente em C# o sincronizador Exchanger<T>, que suporte a troca de mensagens,
    definidas por instâncias do tipo T , entre pares de threads . A classe que implementa o
    sincronizador define o método T Exchange(T myMsg, int timeout), que é chamado pelas
    threads para oferecer a mensagem myMsg e receber, no valor de retorno, a mensagem
    oferecida pela thread com que emparelharam. Quando a troca de mensagens não pode ser
    realizada de imediato (porque não existe ainda outra thread bloqueada), a thread corrente fica
    bloqueada até que: outra thread invoque Exchange; a espera seja interrompida, ou; expire o
    limite de tempo, especificado através do parâmetro timeout
     */

    public class Exchanger<T> {

        //TODO adicionar um par para mensagem/resposta
        private readonly object lockObj = new object();

        //estrutura que vai guardar a mensagem, para trocar com outra thread
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

        //mensagem a retornar, caso exista
        private Obj<T> obj;

        //mensagem a retornar, caso exista
       /* class PairMessage<T> {
            public Obj<T> msg1;
            public Obj<T> msg2;
        }*/

        public T Exchange(T myMsg, int timeout) {
            lock (lockObj) {
                Obj<T> aux = new Obj<T>(default(T));

                //se não existir mensagem de outra thread, a nova é atribuida
                if (obj.Data == null) {
                    obj = new Obj<T>(myMsg);
                    aux = obj;

                    //espera que haja outra mensagem, de outra thread
                    while (true) {
                        try {
                            int start = Environment.TickCount;
                            Monitor.Wait(lockObj, timeout);

                            //retorna a nossa mensagen
                            if (obj.Data == null)
                                return aux.Data;

                            if (timeout != 0) {
                                //se existir timeout retorna null
                                timeout = SyncUtils.AdjustTimeout(ref start, ref timeout);
                                if (timeout <= 0) {
                                    obj = new Obj<T>(default(T));
                                    return obj.Data;
                                }
                            }
                        } catch (ThreadInterruptedException) {
                            throw new ThreadInterruptedException();
                        }
                    }
                }

                //efectuar a troca de mensagens
                aux = obj;
                obj = new Obj<T>(default(T));
                T msg = aux.Data;
                aux.Data = myMsg;

                Monitor.PulseAll(lockObj);
                return msg;
            }
        }
    }

}
