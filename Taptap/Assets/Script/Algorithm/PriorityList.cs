using System;
using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    public class PriorityList<T> where T : IComparable<T>
    {
        private List<T> list;

        public bool IsEmpty { get => list.Count == 0; }

        public PriorityList()
        {
            list = new List<T>();
        }


        public void Enqueue(T t)
        {
            list.Add(t);
        }

        public T Dequeue()
        {
            if (list.Count > 0)
            {
                list.Sort();
                T t = list[0];
                list.RemoveAt(0);
                return t;
            }
            return default(T);
        }
    }
}
