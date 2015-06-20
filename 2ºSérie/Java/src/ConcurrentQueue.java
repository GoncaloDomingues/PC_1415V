import java.util.concurrent.atomic.AtomicReference;

/* Implemente em Java e C# a classe ConcurrentQueue<T>que define um contentor com
disciplina FIFO (First�In�First�Out) suportado numa lista simplesmente ligada. A classe
disponibiliza as opera��es put, tryTakee isEmpty. A opera��o putcoloca no fim da fila o
elemento passado como argumento? a opera��o tryTakeretorna o elemento presente no
in�cio da fila ou null, no caso da fila estar vazia? a opera��o isEmpty indica se a fila cont�m
elementos. A implementa��o suporta acessos concorrentes e nenhuma das opera��es
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
        inicializa��o dos ponteiros head e tail com referencia para o dummy
     */
    private final AtomicReference<Node<T>> head = new AtomicReference<Node<T>>(dummy);
    private final AtomicReference<Node<T>> tail = new AtomicReference<Node<T>>(dummy);

    /*
        A opera��o put coloca no fim da fila o elemento passado como argumento?
     */
    public boolean put(T item){
        Node<T> newNode = new Node<T>(item, null);
        while(true){
            Node<T> curTail = tail.get();
            Node<T> tailNext = curTail.next.get();

            //verifica��o da informa�ao do tail.
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

    /* 	A opera��o tryTake retorna o elemento presente no in�cio da fila, ou null caso da fila estar vazia. */
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