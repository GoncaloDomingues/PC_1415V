package ex1;

import java.util.concurrent.atomic.AtomicReference;


/*

Implemente em Java e C# a classe C oncurrentQueue<T> que define um contentor com
disciplina FIFO (F irstInFirstOut) suportado numa lista simplesmente ligada. A classe
disponibiliza as operações p ut, t ryTake e i sEmpty. A operação p ut coloca no fim da fila o
elemento passado como argumento; a operação t ryTake retorna o elemento presente no
início da fila ou n ull, no caso da fila estar vazia; a operação i sEmpty indica se a fila contém
elementos. A implementação suporta acessos concorrentes e nenhuma das operações
disponibilizadas bloqueia as threads invocantes.
NOTA: Para a implementação considere a explicação sobre a l ockfree
queue , proposta por Michael e Scott, do capítulo 15 do livro Java Concurrency in Practice .

*/
public class ConcurrentQueue<T> {

	private static class Node<T> {
		final T item;
		final AtomicReference<Node<T>> next;
		
		public Node(T item, Node<T> next) {
			this.item = item;
			this.next = new AtomicReference<Node<T>>(next);
		}
	}

	private final Node<T> dummy;
	private final AtomicReference<Node<T>> head;
	private final AtomicReference<Node<T>> tail;
	
	public ConcurrentQueue() {
		dummy = new Node<T>(null, null);
		head = new AtomicReference<Node<T>>(dummy);
		tail = new AtomicReference<Node<T>>(dummy);
	}
	
	//lista vazia quando o head e o tail, apontam para o dummy (true)
	public boolean isEmpty() {
		return head.get() == tail.get();
	}
	
	public boolean put(T item) {
		Node<T> newReq = new Node<T>(item, null);
		
		while(true) {
			
			Node<T> currentTail = tail.get();
			Node<T> tailNext = currentTail.next.get();
			
			if (currentTail == tail.get()) {
				if (tailNext != null) {
					//avançar na queue
					tail.compareAndSet(currentTail, tailNext);
				} else {
					//inserção do novo pedido
					if (currentTail.next.compareAndSet(null, newReq)) {
						tail.compareAndSet(currentTail, newReq);
						return true;
					}
				}
			}
			
		}
		
	}
	
	public T tryTake() {
		
		if (isEmpty()) {
			return null;
		}
		
		Node<T> currentHead = head.get();
		Node<T> headNext = currentHead.next.get(); //valor a retornar
		Node<T> currentTail = tail.get();
		Node<T> newTake = headNext.next.get();
		
		while(true) {
			
			if (currentHead == head.get()) {
				if (headNext == null) {
					//lista vazia
					tail.compareAndSet(currentTail, currentHead);
					return null;
				} else {
					if (currentHead.next.compareAndSet(headNext, newTake)) {
						if (newTake == null) {
							//lista vazia
							tail.compareAndSet(currentTail, currentHead);
						}
						return headNext.item;
					}
				}
			}
			
		}
		
	}
	
	
}
