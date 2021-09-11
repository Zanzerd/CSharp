using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        public T[] array;
        public int totalLength;
        public int pointer;
        public const int startSize = 2;
        public Channel(int length)
        {
            array = new T[length];
            totalLength = length;
            pointer = 0;
        }
        public Channel() : this(startSize) { }
        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                if (pointer <= index)
                    return null;
                return array[index];
            }
            set
            {
                lock (this)
                {
                    if (pointer < index)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    else if (pointer == index)
                    {
                        if (pointer == totalLength)
                        {
                            totalLength = totalLength * startSize;
                            var arrayNext = new T[totalLength];
                            Array.Copy(array, arrayNext, array.Length);
                            array = arrayNext;
                        }
                        array[index] = value;
                        pointer = index + 1;
                    }
                    else
                    {
                        array[index] = value;
                        pointer = index + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            if (pointer > 0)
                return array[pointer - 1];
            return null;
        }

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            if (LastItem() == knownLastItem)
            {
                lock(this)
                {
                    this[pointer] = item;
                }
            }
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                return pointer;
            }
        }
    }
}