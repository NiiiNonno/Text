using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;
public interface IWeaver
{
    void Terminate();
}

public interface ICodeWeaver<T> : IWeaver
{
    void Pose(T code);
}

public interface ITreeWeaver : IWeaver
{
    int Depth { get; set; }
#if NETSTANDARD2_1_OR_GREATER
    void IWeaver.Terminate() => Depth = 0;
#endif
}

public interface IStringWeaver<T> : ICodeWeaver<T>
{
    int Length { get; }

    void Append(T code);
    void Insert(int at, T @char);
    T Remove(int at);
    void Replace(T old, T neo, int start, int count);
    int GetIndex(T of, int start, int count);
    void Clear();

#if NETSTANDARD2_1_OR_GREATER
    void Append(ReadOnlySpan<T> str)
    {
        foreach (var code in str) Append(code);
    }

    void Insert(Index at, T @char) => Insert(at.GetOffset(Length));
    void Insert(Index at, ReadOnlySpan<T> str)
    {
        int i = at.Value;
        if (at.IsFromEnd)
        {
            foreach (var code in str) Insert(new(i - 1, true), code);
        }
        else
        {
            foreach (var code in str) Insert(new(i + 1, false), code);
        }
    }

    ReadOnlySpan<T> Remove(Range range);
    T Remove(Index at) => Remove(at.GetOffset(Length));

    void Replace(T old, T neo)
    {
        var i = GetIndex(of: old);
        Remove(at: i);
        Insert(at: i, neo);
    }
    void Replace(T old, T neo, Range range)
    {
        var i = GetIndex(of: old, range);
        Remove(at: i);
        Insert(at: i, neo);
    }
    void Replace(ReadOnlySpan<T> old, ReadOnlySpan<T> neo)
    {
        var i = GetRange(of: old);
        Remove(i);
        Insert(i.Start, neo);
    }
    void Replace(ReadOnlySpan<T> old, ReadOnlySpan<T> neo, Range range)
    {
        var i = GetRange(of: old, range);
        Remove(i);
        Insert(i.Start, neo);
    }

    Index GetIndex(T of) => GetIndex(of, ..);
    Index GetIndex(T of, Range range);
    Range GetRange(ReadOnlySpan<T> of) => GetRange(of, ..);
    Range GetRange(ReadOnlySpan<T> of, Range range);


    void ICodeWeaver<T>.Pose(T code) => Append(code);
#endif
}

public interface IObjectWeaver : IStringWeaver<char>
{
    IFormatProvider FormatProvider { get; }

#if NETSTANDARD2_1_OR_GREATER
    void Append(string? str) => Append(str.AsSpan());
    void Append(object? obj) => Append(obj?.ToString());
    void Append(IFormattable obj, string? format = null) => Append(obj.ToString(format, FormatProvider));
    void AppendLine();

    void Insert(Index at, string? str) => Insert(at, str.AsSpan());
    void Insert(Index at, object? obj) => Insert(at, obj?.ToString());
    void Insert(Index at, IFormattable obj, string? format = null) => Insert(at, obj.ToString(format, FormatProvider));
    void InsertLine(Index at);

    void Replace(string old, string neo);
    void Replace(string old, string neo, Range range) => Replace(old.AsSpan(), neo.AsSpan(), range);
#else

    void Append(string? str);
    void Append(object? obj);
    void Append(IFormattable obj, string? format = null);
    void AppendLine();

    void Replace(string old, string neo);
#endif
}

public static partial class TextExtensions
{
    public static void Append<T>(this IObjectWeaver @this, T obj, string? format = null) where T : IFormattable
    {
        if (obj is not null) @this.Append(obj.ToString(format, @this.FormatProvider));
    }
#if NETSTANDARD2_1_OR_GREATER
    public static void Insert<T>(this IObjectWeaver @this, Index at, T obj, string? format = null) where T : IFormattable
    {
        if (obj is not null) @this.Insert(at, obj.ToString(format, @this.FormatProvider));
    }
#endif

    public static void AppendLine(this IObjectWeaver @this, string? str)
    {
        @this.AppendLine();
        @this.Append(str);
    }
    public static void AppendLine(this IObjectWeaver @this, object? obj)
    {
        @this.AppendLine();
        @this.Append(obj);
    }
    public static void AppendLine(this IObjectWeaver @this, IFormattable obj, string? format = null)
    {
        @this.AppendLine();
        @this.Append(obj, format);
    }
    public static void AppendLine<T>(this IObjectWeaver @this, T obj, string? format = null) where T : IFormattable
    {
        @this.AppendLine();
        @this.Append(obj, format);
    }

#if NETSTANDARD2_1_OR_GREATER
    public static void InsertLine(this IObjectWeaver @this, Index at, string? str)
    {
        @this.Insert(at, str);
        @this.InsertLine(at);
    }
    public static void InsertLine(this IObjectWeaver @this, Index at, object? obj)
    {
        @this.Insert(at, obj);
        @this.InsertLine(at);
    }
    public static void InsertLine(this IObjectWeaver @this, Index at, IFormattable obj, string? format = null)
    {
        @this.Insert(at, obj, format);
        @this.InsertLine(at);
    }
    public static void InsertLine<T>(this IObjectWeaver @this, Index at, T obj, string? format = null) where T : IFormattable
    {
        @this.Insert(at, obj, format);
        @this.InsertLine(at);
    }
#endif
}