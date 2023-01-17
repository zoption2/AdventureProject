using System.Linq;
using System;


namespace System.Collections.Generic
{
    public class GameQueue<T>
    {
        private T[] _array = new T[0];
        private int _indexer = 0;

        public int Count => _array.Length;

        public void Enqueue(T element)
        {
            _indexer++;
            if (_array.Length < _indexer)
            {
                T[] newArray = new T[_indexer];
                for (int i = 0, j = _array.Length; i < j; i++)
                {
                    newArray[i] = _array[i];
                }
                newArray[newArray.Length - 1] = element;
                _array = newArray;
            }
        }

        public T Dequeue()
        {
            if (_array.Length > 0)
            {
                _indexer--;
                T selected = _array[0];
                T[] newArray = new T[_indexer];
                for (int i = 1, j = _array.Length; i < j; i++)
                {
                    newArray[i - 1] = _array[i];
                }
                _array = newArray;
                return selected;
            }
            else
            {
                return default;
            }
        }

        public void Banish(T element)
        {
            if (_array.Contains(element))
            {
                _indexer--;
                int banishedIndex = Array.IndexOf<T>(_array, element);
                T[] newArray = new T[_indexer];
                int offsetIndex = 0;
                for (int i = 0, j = newArray.Length; i < j; i++)
                {
                    if (i < banishedIndex)
                    {
                        offsetIndex = i;
                    }
                    else
                    {
                        offsetIndex = i + 1;
                    }
                    newArray[i] = _array[offsetIndex];
                }
                _array = newArray;
            }
        }
    }
}
    


