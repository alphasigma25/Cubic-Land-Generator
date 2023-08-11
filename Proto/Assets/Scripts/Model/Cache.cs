using System;
using System.Collections.Generic;

internal class TriCache<T>
{
    public TriCache(Func<int, int, int, T> generator)
    {
        this.generator = generator;
        cache = new();
    }

    public T this[int x, int y, int z]
    {
        get
        {
            if (cache.TryGetValue(x, out Dictionary<int, Dictionary<int, T>> n1))
            {
                if (n1.TryGetValue(y, out Dictionary<int, T> n2))
                {
                    if (n2.TryGetValue(z, out T n3))
                        return n3;

                    T gen = generator(x, y, z);
                    n2[z] = gen;
                    return gen;
                }
                else
                {
                    T gen = generator(x, y, z);
                    n1[y] = new() { { z, gen } };
                    return gen;
                }
            }
            else
            {
                T gen = generator(x, y, z);
                Dictionary<int, T> res1 = new() { { z, gen } };
                cache[x] = new() { { y, res1 } };
                return gen;
            }
        }
    }

    private readonly Dictionary<int, Dictionary<int, Dictionary<int, T>>> cache;

    private readonly Func<int, int, int, T> generator;
}

internal class BiCache<T>
{
    public BiCache(Func<int, int, T> generator)
    {
        this.generator = generator;
        cache = new();
    }

    public T this[int x, int y]
    {
        get
        {
            if (cache.TryGetValue(x, out Dictionary<int, T> n2))
            {
                if (n2.TryGetValue(y, out T n3))
                    return n3;

                T gen = generator(x, y);
                n2[y] = gen;
                return gen;
            }
            else
            {
                T gen = generator(x, y);
                cache[x] = new() { { y, gen } };
                return gen;
            }
        }
    }

    private readonly Dictionary<int, Dictionary<int, T>> cache;

    private readonly Func<int, int, T> generator;
}