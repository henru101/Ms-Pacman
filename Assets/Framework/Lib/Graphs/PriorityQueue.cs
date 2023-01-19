using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PriorityQueue<T> {
    SortedList<Pair<double>, T> _list;
    int count;

    public PriorityQueue() {
        _list = new SortedList<Pair<double>, T>(new PairComparer<double>());
    }

    public void Enqueue(T item, double priority) {
        _list.Add(new Pair<double>(priority, count), item);
        count++;
    }

    public T Dequeue() {
        T item = _list[_list.Keys[0]];
        _list.RemoveAt(0);
        return item;
    }

    public int Count() {
        return _list.Count;
    }

    public bool Contains(T t) {
        return _list.ContainsValue(t);
    }
}

class Pair<T> {
    public T First { get; private set; }
    public T Second { get; private set; }

    public Pair(T first, T second) {
        First = first;
        Second = second;
    }

    public override int GetHashCode() {
        return First.GetHashCode() ^ Second.GetHashCode();
    }

    public override bool Equals(object other) {
        Pair<T> pair = other as Pair<T>;
        if (pair == null) {
            return false;
        }
        return (this.First.Equals(pair.First) && this.Second.Equals(pair.Second));
    }
}

class PairComparer<T> : IComparer<Pair<T>> where T : System.IComparable
{
    public int Compare(Pair<T> x, Pair<T> y) {
        if (x.First.CompareTo(y.First) < 0) {
            return -1;
        }
        else if (x.First.CompareTo(y.First) > 0) {
            return 1;
        }
        else {
            return x.Second.CompareTo(y.Second);
        }
    }
}