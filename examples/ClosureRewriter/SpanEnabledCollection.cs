using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClosureRewriter
{
    public class SpanEnabledCollection<T> : Collection<T>
    {
        private readonly SpanEnabledList _items;

        private SpanEnabledCollection(SpanEnabledList list) : base(list)
        {
            _items = new SpanEnabledList();
        }

        internal static SpanEnabledCollection<T> Create()
        {
            return new SpanEnabledCollection<T>(new SpanEnabledList());
        }

        internal ReadOnlySpan<T> GetSpan() => _items.GetSpan();

        internal ReadOnlyMemory<T> GetMemory() => _items.GetMemory();

        private class SpanEnabledList : IList<T>
        {
            private int _size;

            private T[] _items = Array.Empty<T>();

            public T this[int index]
            {
                get => _items[index];
                set => _items[index] = value;
            }

            public int Count => _size;

            public bool IsReadOnly => false;

            public int Capacity
            {
                get
                {
                    return _items.Length;
                }
                set
                {
                    if (value < _size)
                    {
                        return;
                    }

                    if (value != _items.Length)
                    {
                        if (value > 0)
                        {
                            T[] array = new T[value];
                            if (_size > 0)
                            {
                                Array.Copy(_items, 0, array, 0, _size);
                            }

                            _items = array;
                            return;
                        }

                        _items = Array.Empty<T>();
                    }
                }
            }

            internal ReadOnlySpan<T> GetSpan()
            {
                return _items.AsSpan(0, _size);
            }

            internal ReadOnlyMemory<T> GetMemory()
            {
                return _items.AsMemory(0, _size);
            }

            private void EnsureCapacity(int min)
            {
                if (_items.Length >= min)
                {
                    return;
                }

                int num = (this._items.Length == 0) ? 4 : (this._items.Length * 2);
                if (num > 2146435071)
                {
                    num = 2146435071;
                }

                if (num < min)
                {
                    num = min;
                }

                Capacity = num;
            }

            public void Add(T item)
            {
                T[] items = _items;
                int size = _size;
                if (size < items.Length)
                {
                    _size = size + 1;
                    items[size] = item;
                    return;
                }

                EnsureCapacity(size + 1);
                _size = size + 1;
                items[size] = item;
            }

            public void Clear()
            {
                int size = _size;
                if (size > 0)
                {
                    Array.Clear(_items, 0, size);
                    _size = 0;
                }
            }

            public bool Contains(T item)
            {
                return Array.IndexOf(_items, item, 0, _size) != -1;
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _items.CopyTo(array, arrayIndex);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ((IEnumerable<T>)_items).GetEnumerator();
            }

            public int IndexOf(T item)
            {
                return Array.IndexOf(_items, item, 0, _size);
            }

            public void Insert(int index, T item)
            {
                if (index > _size)
                {
                    // ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
                }

                if (_size == _items.Length)
                {
                    EnsureCapacity(_size + 1);
                }

                if (index < _size)
                {
                        Array.Copy(_items, index, _items, index + 1, _size - index);
                }

                _items[index] = item;
                _size++;
            }

            public bool Remove(T item)
            {
                int index = IndexOf(item);
                if (index >= 0)
                {
                    RemoveAt(index);
                    return true;
                }

                return false;
            }

            public void RemoveAt(int index)
            {
                if (index >= _size)
                {
                    // ThrowHelper.ThrowArgumentOutOfRange_IndexException();
                }

                _size--;
                if (index < _size)
                {
                    Array.Copy(_items, index + 1, _items, index, _size - index);
                }

                // if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                // {
                //         _items[_size] = default(T);
                // }
                // _version++;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
