using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie_2
{
    class ConcurrentQueue<T>
    {

        private class Node<T>
        {
            public readonly object item;
            public Node<T> next;

            public Node(object item)
            {
                this.item = item;

            }
        }

        private volatile Node<T> dummy;
        private volatile Node<T> tail;
        private volatile Node<T> head;

        public static ConcurrentQueue()
        {
            dummy = new Node<T>(null);
            dummy.next = null;

            tail = new Node<T>(null);
            tail.next = dummy;

            head = new Node<T>(null);
            head.next = dummy;
        }

        public bool Put(object item)
        {
            Node<T> toAdd = new Node<T>(item);
            Node<T> curTail = tail;

            while (true)
            {
                if (curTail == tail)
                {
                    if (curTail.next == null)
                    {//If the value in location == expectedValue, then newValue is exchanged. 
                       //Either way, the value in location (before exchange) is returned.

                        if (Interlocked.CompareExchange(ref toAdd, null, curTail.next) == null)
                        {
                            Interlocked.CompareExchange(ref curTail, toAdd, tail);
                            return true;
                        }
                    }
                    else
                    {
                        Interlocked.CompareExchange(ref curTail, curTail.next, tail);
                    }
                }
            }
        }

        public object TryTake()
        {
            if (Is_Empty())
                return default(T);

            Node<T> curHead = head;
            while (true)
            {
                if (curHead == head)
                {

                    Interlocked.CompareExchange(ref curHead, curHead.next, head);
                    return curHead.item;
                }
            }
        }

        public bool Is_Empty()
        {
            return head == tail;
        }
    }
}


