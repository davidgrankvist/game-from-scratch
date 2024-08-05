namespace GameFromScratch.App.Platform.Common
{
    internal class LruCache<TKey, TValue>
        where TKey: notnull
    {
        private readonly Dictionary<TKey, Node> cache;
        private readonly int capacity;

        public int Count => cache.Count;

        private Node? head;
        private Node? tail;

        public LruCache(int capacity)
        {
            cache = new Dictionary<TKey, Node>(capacity);
            this.capacity = capacity;

            head = null;
            tail = null;
        }

        public bool TryGet(TKey key, out TValue? value)
        {
            if (cache.TryGetValue(key, out var node))
            {
                MoveToFirst(node);

                value = node.value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public void Add(TKey key, TValue value)
        {
            var node = new Node(key, value);
            AddFirstAndRightShift(node);
        }

        private void AddFirstAndRightShift(Node node)
        {
            var nextCount = Count + 1;
            if (head == null || tail == null)
            {
                head = node;
                tail = node;
            }
            else if (nextCount <= capacity)
            {
                MoveToFirst(node);
            }
            else
            {
                RemoveLast();
                MoveToFirst(node);
            }

            cache.Add(node.key, node);
        }

        private void MoveToFirst(Node node)
        {
            if (head == null)
            {
                head = node;
            }
            else
            {
                node.next = head;
                node.prev = null;
                head.prev = node;

                head = node;
            }
        }

        private void RemoveLast()
        {
            cache.Remove(tail!.key);
            tail = tail?.prev;
        }

        private class Node
        {
            public readonly TKey key;
            public readonly TValue value;

            public Node? prev;
            public Node? next;

            public Node(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
