using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    class WeightedQueue<T>
    {
        private SortedList<int,List<T>> queue; 
        public WeightedQueue()
        {
            queue = new SortedList<int, List<T>>();
        }

        public void Queue(int priority, T t)
        {
            if(queue[priority] == null)
                queue[priority] = new List<T>();
            queue[priority].Add(t);
        }

        public T Pop()
        {
            foreach (var list in queue.Values)
            {
                if (list.Any())
                {
                    T res = list.First();
                    list.RemoveAt(0);
                }
            }
            return default(T);
        }
    }
}
