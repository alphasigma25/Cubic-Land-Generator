using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Merci a toute personne raisonable (particulièrement anne so) de ne pas chercher
/// a comprendre ce que fait cette class ou comment elle marche
/// </summary>
internal static class MultiDimensionnalArrayHelper
{
    public static void Fill<T>(this T[,,] array, T value)
    {
        Span<T> data = MemoryMarshal.CreateSpan(ref Unsafe.As<T, T>(ref array[0, 0, 0]), array.Length);
        data.Fill(value);
    }
}