namespace Ift232UI
{
    internal sealed class CountableListItem<T>
    {
        public CountableListItem(T item, int count)
        {
            Item = item;
            Count = count;
        }

        public T Item { get; private set; }
        public int Count { get; private set; }

        public override string ToString()
        {
            return Item + " : " + Count;
        }
    }
}