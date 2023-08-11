using System;
using System.Collections.Generic;
using UnityEngine;

internal class MutliStackCache<T>
{
    public MutliStackCache(Func<T> generator)
    {
        this.generator = generator;
    }

    public void Add(Stack<T> list) => cache.Enqueue(list);

    public T Get()
    {
        if (cache.TryPeek(out Stack<T> t))
        {
            if (t.Count == 0)
            {
                Debug.Log("Bug");
                cache.Dequeue();
                return Get();
            }

            T result = t.Pop();

            if (t.Count == 0)
                cache.Dequeue();

            return result;
        }

        return generator();
    }

    private readonly Queue<Stack<T>> cache = new();

    private readonly Func<T> generator;
}