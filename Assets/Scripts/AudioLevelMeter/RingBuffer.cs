namespace ouchi
{
    public class RingBuffer<T>
    {
        public RingBuffer(int capacity) {
            this.capacity = capacity;
            size = 0;
            begin_ = 0;
        }
        public T this[int i]{
            get
            {
                return buffer_[(begin_ + i) % capacity];
            }
            set
            {
                buffer_[(begin_ + i) % capacity] = value;
            }
        }
        public void Add(T value)
        {
            if(size < capacity)
            {
                buffer_[(begin_ + size) % capacity] = value;
                ++size;
            }
            else
            {
                buffer_[begin_] = value;
                begin_ = (begin_ + 1) % capacity;
            }
        }
        public T Front()
        {
            if (size == 0) throw new System.IndexOutOfRangeException("size is 0");
            return this[0];
        }
        public T Back()
        {
            if (size == 0) throw new System.IndexOutOfRangeException("size is 0");
            return this[size - 1];
        }
        public void PopBack(int count)
        {
            size -= System.Math.Min(count, size);
        }

        public int capacity {
            get { return capacity_; }
            set {
                capacity_ = value;
                var nb = new T[capacity];
                for (int i = 0; i < System.Math.Min(size, capacity_); ++i)
                    nb[i] = this[i];
                buffer_ = nb;
                begin_ = 0;
                size = System.Math.Min(size, capacity_);
            } }
        public int size { get; private set; }
        private int capacity_;
        private int begin_;
        private T[] buffer_;

    }
}
