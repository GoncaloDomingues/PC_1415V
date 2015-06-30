using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DataAggregator
{
    public class DataAggregator<D>
    {

        private readonly LinkedList<D> items = new LinkedList<D>();
        private readonly LinkedList<bool> consumers = new LinkedList<bool>();

        public void Put(D data)
        {

            lock (this)
            {
                LinkedListNode<D> newItem = new LinkedListNode<D>(data);

                items.AddLast(newItem);

                if (consumers.Count > 0)
                {
                    consumers.First.Value=true;
                }
                Monitor.PulseAll(this);
            }
        }
        public List<D> TakeAll()
        {
            lock (this)
            {


                LinkedList<D> toRet;

                if (consumers.Count == 0 && items.Count!=0)
                {
                    toRet = items;
                    items.Clear();
                    return toRet.ToList();
                }

                if (items.Count == 0)
                {
                    LinkedListNode<bool> newConsumer = new LinkedListNode<bool>(false);
                    consumers.AddFirst(newConsumer);

                    do
                    {
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (ThreadInterruptedException)
                        {
                            if (newConsumer.Value)
                            {
                                Thread.CurrentThread.Interrupt();

                                if (items.Count > 0)
                                {
                                    consumers.Remove(newConsumer);
                                    return;
                                }
                        
                            throw;
                        }

                        if (consumers.First.Value)
                        {
                            consumers.RemoveFirst();

                            toRet = items;
                            items.Clear();
                            return toRet.ToList();
                        }

                    } while (true);
                } return null;               
            }
        }
    }
}
