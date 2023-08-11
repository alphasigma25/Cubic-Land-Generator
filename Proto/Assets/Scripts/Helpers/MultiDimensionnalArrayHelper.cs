using System;
using System.Runtime.InteropServices;

internal static class MultiDimensionnalArrayHelper
{
    public static void Fill<T>(this T[,,] array, T value)
    {
        Span<T> data = MemoryMarshal.CreateSpan(ref array[0, 0, 0], array.Length);
        data.Fill(value);
    }
}