using System;
using System.Collections;
using System.Collections.Generic;


public static class DequeTest
{
    public static IList<T> GetReverseView<T>(Deque<T> d)
    {
        return d.Reverse();
    }
}

public interface IDeque<T>
{
    void PushRight(T value);
    void PushLeft(T value);
    T PeekRight();
    T PeekLeft();
    T PopRight();
    T PopLeft();
    Deque<T> Reverse();
}

internal interface IChangeMarkingEnumerator
{
    void DequeChanged();
}

public class Deque<T> : IList<T>, IDeque<T>
{
    private sealed class ReverseDeque<RT> : Deque<RT>
    {
        private Deque<RT> _parent;

        //For some reason this is necessary... And that confuses me to no end
        public override int Count
        {
            get { return _parent.Count; }
        }

        public ReverseDeque(Deque<RT> parent)
        {
            _parent = parent;
        }

        public override Deque<RT> Reverse()
        {
            return _parent;
        }

        public override RT this[int index]
        {
            get { return _parent[(Count - 1) - index]; }

            set
            {
                _parent[(Count - 1) - index] = value;
            }
        }

        public override void PushRight(RT item)
        {
            _parent.PushLeft(item);
        }

        public override void PushLeft(RT item)
        {
            _parent.PushRight(item);
        }

        public override RT PopRight()
        {
            return _parent.PopLeft();
        }

        public override RT PopLeft()
        {
            return _parent.PopRight();
        }

        public override RT PeekRight()
        {
            return _parent.PeekLeft();
        }

        public override RT PeekLeft()
        {
            return _parent.PeekRight();
        }

        public override void RemoveAt(int index)
        {
            _parent.RemoveAt((Count - 1) - index);
        }

        public override IEnumerator<RT> GetEnumerator()
        {
            var toReturn = new DequeEnumerator<RT>(this);
            enumerators.Add(toReturn);
            return toReturn;
        }
    }
    private struct Position
    {
        public int mapIndex;
        public int subArrayIndex;
    }
    private sealed class DequeEnumerator<ET> : IEnumerator<ET>, IChangeMarkingEnumerator
    {
        int position = -1;
        private bool disposed = false;
        private bool dequeChanged = false;
        private Deque<ET> deque;
        public DequeEnumerator(Deque<ET> deque)
        {
            this.deque = deque;
        }
        public ET Current
        {
            get
            {
                CheckDisposedAndDequeChanged();
                try
                {
                    return deque[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            CheckDisposedAndDequeChanged();
            deque.UnlistEnumerator(this);
            deque = null;
            disposed = true;
        }

        public bool MoveNext()
        {
            CheckDisposedAndDequeChanged();
            position++;
            return position < deque.Count;
        }

        public void Reset()
        {
            CheckDisposedAndDequeChanged();
            position = -1;
        }

        public void DequeChanged()
        {
            dequeChanged = true;
        }

        private void CheckDisposedAndDequeChanged()
        {
            if (dequeChanged) throw new InvalidOperationException("Deque changed while enumerating");
            if (disposed) throw new ObjectDisposedException("Enumerator was already disposed");
        }
    }

    //Default capacity of Deque block
    private const int defaultBlockSize = 5;
    private readonly int blockSize;
    private T[][] map;
    private Position left;
    private Position right;
    private readonly List<IChangeMarkingEnumerator> enumerators = new List<IChangeMarkingEnumerator>();

    public Deque(int blockSize)
    {
        this.blockSize = blockSize;
        if (blockSize < 1)
            throw new ArgumentOutOfRangeException("Block size must be greater than 0.");
        map = new T[blockSize][];
        map[blockSize / 2] = new T[blockSize];
        left = new Position
        {
            mapIndex = blockSize / 2,
            subArrayIndex = blockSize / 2
        };
        right = left;
    }

    //Constructor for default size
    public Deque()
        : this(defaultBlockSize)
    {
    }

    public virtual Deque<T> Reverse()
    {
        return new ReverseDeque<T>(this);
    }

    public virtual T this[int index]
    {
        get
        {
            CheckIndex(index);
            var position = PositionForIndex(index);
            return map[position.mapIndex][position.subArrayIndex];
        }

        set
        {
            AlertEnumeratorsOfChange();
            CheckIndex(index);
            var position = PositionForIndex(index);
            map[position.mapIndex][position.subArrayIndex] = value;
        }
    }

    public virtual int Count { get; private set; }

    public bool IsReadOnly => false;

    public virtual void Add(T item)
    {
        PushRight(item);
    }

    public void Clear()
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == null) continue;
            for (int y = 0; y < map[i].Length; y++)
            {
                map[i][y] = default(T);
            }
        }
        left = new Position
        {
            mapIndex = map.Length / 2,
            subArrayIndex = blockSize / 2
        };
        right = left;
        Count = 0;
        AlertEnumeratorsOfChange();
    }

    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < Count; i++)
        {
            if (comparer.Equals(this[i], item))
            {
                return true;
            }
        }
        return false;
    }

    public virtual void CopyTo(T[] destination, int index)
    {
        if (destination == null)
            throw new ArgumentNullException("destination", "Destination cannot be null.");
        CheckRangeArguments(destination.Length, index, Count);
        for (int i = 0; i < Count; ++i)
        {
            destination[index + i] = this[i];
        }
        AlertEnumeratorsOfChange();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public virtual IEnumerator<T> GetEnumerator()
    {
        var toReturn = new DequeEnumerator<T>(this);
        enumerators.Add(toReturn);
        return toReturn;
    }
    private void UnlistEnumerator(IChangeMarkingEnumerator enumerator)
    {
        enumerators.Remove(enumerator);
    }

    private void AlertEnumeratorsOfChange()
    {
        if (enumerators.Count < 1) return;
        foreach (var enumerator in enumerators)
        {
            enumerator.DequeChanged();
        }
        enumerators.Clear();
    }
    /*public virtual IEnumerator<T> GetEnumerator()
    {
        int count = Count;
        for (int i = 0; i != count; ++i)
        {
            yield return _circularBuffer[ConvertIndex(i)];
        }
    }*/

    public virtual int IndexOf(T item)
    {
        return IndexOf(item, EqualityComparer<T>.Default);
    }

    private class ComparerEqualityComparer<ECT> : EqualityComparer<ECT>
    {
        private IComparer<ECT> comparer;
        public ComparerEqualityComparer(IComparer<ECT> comparer){
            this.comparer = comparer;
        }
        public override bool Equals(ECT x, ECT y)
        {
            return comparer.Compare(x, y) == 0;
        }

        public override int GetHashCode(ECT obj)
        {
            return obj.GetHashCode();
        }
    }

    public virtual int IndexOf(T item, IComparer<T> comparer)
    {
        var equalityComparer = new ComparerEqualityComparer<T>(comparer);
        return IndexOf(item, equalityComparer);
    }

    public virtual int IndexOf(T item, IEqualityComparer<T> comparer)
    {
        for (int i = 0; i < Count; i++)
        {
            if (comparer.Equals(this[i], item))
            {
                return i;
            }
        }
        return -1;
    }

    public virtual void Insert(int index, T item)
    {
        if (index == 0)
        {
            PushLeft(item);
            return;
        }

        CheckIndex(index);

        var saveRight = map[right.mapIndex][right.subArrayIndex];
        var walker = left;
        for (int i = Count - 1; i > index; i--)
        {
            this[i] = this[i - 1];
        }

        this[index] = item;

        PushRight(saveRight);
        AlertEnumeratorsOfChange();
    }

    public virtual T PeekRight()
    {
        return this[Count - 1];
    }

    public virtual T PeekLeft()
    {
        return this[0];
    }

    public virtual T PopRight()
    {
        if (Count == 0)
            throw new InvalidOperationException("The deque is empty.");
        T toReturn = this[Count - 1];
        --Count;
        DecRight();
        AlertEnumeratorsOfChange();
        return toReturn;
    }

    public virtual T PopLeft()
    {
        if (Count == 0)
            throw new InvalidOperationException("The deque is empty.");
        T toReturn = this[0];
        --Count;
        IncLeft();
        AlertEnumeratorsOfChange();
        return toReturn;
    }

    public virtual void PushRight(T value)
    {
        IncRight();
        map[right.mapIndex][right.subArrayIndex] = value;
        Count++;
        AlertEnumeratorsOfChange();
    }

    public virtual void PushLeft(T value)
    {
        DecLeft();
        map[left.mapIndex][left.subArrayIndex] = value;
        Count++;
        AlertEnumeratorsOfChange();
    }

    public virtual bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index > -1)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public virtual void RemoveAt(int index)
    {
        CheckIndex(index);
        if (index == 0)
        {
            PopLeft();
            return;
        }
        if (index == Count - 1)
        {
            PopRight();
            return;
        }
        //Need to move all items in buffer after index 1 left              
        for (int i = index; i < Count - 1; i++)
        {
            this[i] = this[i + 1];
        }
        --Count;
        DecRight();
        AlertEnumeratorsOfChange();
    }

    private void CheckIndex(int index)
    {
        if (index < 0 || index >= Count)
        {
            throw new IndexOutOfRangeException("Invalid index " + index + " for deque length " + Count);
        }
    }

    private Position PositionForIndex(int index)
    {
        var toReturn = left;
        toReturn.mapIndex += index / blockSize;
        toReturn.subArrayIndex += index % blockSize;
        if (toReturn.subArrayIndex >= blockSize)
        {
            toReturn.mapIndex++;
            toReturn.subArrayIndex -= blockSize;
        }

        return toReturn;
    }

    private static void CheckRangeArguments(int sourceLength, int offset, int count)
    {
        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException("Invalid offset " + offset);
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException("Invalid count " + count);
        }

        if (sourceLength - offset < count)
        {
            throw new ArgumentException("Invalid offset (" + offset + ") or count + (" + count + ") for source length " + sourceLength);
        }
    }
    private void DecRight()
    {
        right.subArrayIndex--;
        if (right.subArrayIndex < 0)
        {
            right.mapIndex--;
            right.subArrayIndex += blockSize;
        }
        CheckShrink();
    }

    private void IncRight()
    {
        if (Count == 0) return;

        right.subArrayIndex++;
        if (right.subArrayIndex >= blockSize)
        {
            right.subArrayIndex -= blockSize;
            right.mapIndex++;
        }
        if (right.mapIndex >= map.Length)
        {
            Grow();
        }
        if (map[right.mapIndex] == null) map[right.mapIndex] = new T[blockSize];
    }

    private void IncLeft()
    {
        left.subArrayIndex++;
        if (left.subArrayIndex >= blockSize)
        {
            left.subArrayIndex -= blockSize;
            left.mapIndex++;
        }
        CheckShrink();
    }

    private void DecLeft()
    {
        if (Count == 0) return;

        left.subArrayIndex--;
        if (left.subArrayIndex < 0)
        {
            left.mapIndex--;
            left.subArrayIndex += blockSize;
        }
        if (left.mapIndex < 0)
        {
            Grow();
        }
        if (map[left.mapIndex] == null) map[left.mapIndex] = new T[blockSize];
    }

    private void CheckShrink()
    {
        if (map.Length <= blockSize) return;

        if ((right.mapIndex - left.mapIndex) > (map.Length / 9)) return;

        var oldMap = map;
        map = new T[oldMap.Length / 3][];
        var y = map.Length / 3;
        for (int i = left.mapIndex; i <= right.mapIndex; i++)
        {
            map[y] = oldMap[i];
            y++;
        }
        right.mapIndex = map.Length / 3 + right.mapIndex - left.mapIndex;
        left.mapIndex = map.Length / 3;
    }

    private void Grow()
    {
        var oldMap = map;
        map = new T[oldMap.Length * 3][];
        for (int i = 0; i < oldMap.Length; i++)
        {
            map[oldMap.Length + i] = oldMap[i];
        }
        left.mapIndex += oldMap.Length;
        right.mapIndex += oldMap.Length;
    }
}

