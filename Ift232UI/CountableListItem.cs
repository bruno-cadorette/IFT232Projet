using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ift232UI
{
    sealed class CountableListItem<T>
    {
        public T Item { get; private set; }
        public int Count { get; private set; }

        public CountableListItem(T item, int count)
        {
            Item = item;
            Count = count;
        }

        public override string ToString()
        {
            return Item.ToString() + " : " + Count;
        }
    }
}
