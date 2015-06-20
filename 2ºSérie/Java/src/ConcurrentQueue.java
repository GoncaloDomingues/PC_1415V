import java.util.concurrent.atomic.AtomicReference;

/* Implemente em Java e C# a classe ConcurrentQueue<T>que define um contentor com
disciplina FIFO (First­In­First­Out) suportado numa lista simplesmente ligada. A classe
disponibiliza as operações put, tryTakee isEmpty. A operação putcoloca no fim da fila o
elemento passado como argumento? a operação tryTakeretorna o elemento presente no
início da fila ou null, no caso da fila estar vazia? a operação isEmpty indica se a fila contém
elementos. A implementação suporta acessos concorrentes e nenhuma das operações
disponibilizadas bloqueia as threads invocantes.
*/
public class ConcurrentQueue<T>{

    private static class Node<T>{
        public final T item;
        final AtomicReference<Node<T>> next;

        public Node(T item, Node<T> next){
            this.item=item;
            this.next=new AtomicReference<>(next);
        }
    }

    private final Node<T> dummy = new Node<T>(null,null);

    /*
        inicialização dos ponteiros head e tail com referencia para o dummy
     */
    private final AtomicReference<Node<T>> head = new AtomicReference<Node<T>>(dummy);
    private final AtomicReference<Node<T>> tail = new AtomicReference<Node<T>>(dummy);

    /*
        A operação put coloca no fim da fila o elemento passado como argumento?
     */
    public boolean put(T item){
        Node<T> newNode = new Node<T>(item, null);
        while(true){
            Node<T> curTail = tail.get();
            Node<T> tailNext = curTail.next.get();

            //verificação da informaçao do tail.
            if(curTail==tail.get()){
                if(tailNext != null){
                    tail.compareAndSet(curTail,tailNext);
                }else{
                    if(curTail.next.compareAndSet(null, newNode))
                        tail.compareAndSet(curTail,newNode);
                    return true;
                }
            }
        }
    }

    public boolean isEmpty(){
        return head.get().next.get()==null;
    }

    /* 	A operação tryTake retorna o elemento presente no início da fila, ou null caso da fila estar vazia. */
    public T tryTake(){

        if (isEmpty())
            return null;

        while(true) {

            Node<T> curHead = head.get();

            if(head.compareAndSet(curHead, curHead.next.get()))
                return head.get().item;
        }
    }
}