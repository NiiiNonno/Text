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

public interface IBunanWeaver : IWeaver
{
    IReadOnlyList<Word> Candidates { get; }

    void Pose(Bunan bunan);
    void Confirm(int index = 0);
}

public interface IMarkupWeaver : IWeaver
{
    
}

public interface IObjectWeaver : IWeaver
{
    IFormatProvider FormatProvider { get; }
    int Length { get; }
    int Capacity { get; }

    void Append(string? str);
    void Append(object? obj);
    void Append(char @char);
    void Append(IFormattable obj, string? format = null);
    void Append(ReadOnlySpan<char> str);
    void Append(ReadOnlySpan<byte> str);

    void Insert(Index at, string? str);
    void Insert(Index at, object? obj);
    void Insert(Index at, char @char);
    void Insert(Index at, IFormattable obj, string? format = null);
    void Insert(Index at, ReadOnlySpan<char> str);
    void Insert(Index at, ReadOnlySpan<byte> str);

    void Replace(char old, char neo);
    void Replace(char old, char neo, Range range);
    void Replace(string old, string neo);
    void Replace(string old, string neo, Range range);
    void Replace(ReadOnlySpan<char> old, ReadOnlySpan<char> neo);
    void Replace(ReadOnlySpan<char> old, ReadOnlySpan<char> neo, Range range);
    void Replace(ReadOnlySpan<byte> old, ReadOnlySpan<byte> neo);
    void Replace(ReadOnlySpan<byte> old, ReadOnlySpan<byte> neo, Range range);

    void AppendLine();
    void Clear();
    void EnsureCapacity(int capacity);
    void Remove(Range range);
    void Remove(Index at);
}

public static partial class TextExtensions
{
    public static void Append<T>(this IObjectWeaver @this, T obj, string? format = null) where T : IFormattable
    {
        if (obj is not null) @this.Append(obj.ToString(format, @this.FormatProvider));
    }
    //public static void Append<T>(this IObjectStringBuilder @this, T obj) where T : IFormattable
    //{
    //    if (obj is not null) @this.Append(obj.ToString(format: "G", @this.FormatProvider));
    //}
    public static void Insert<T>(this IObjectWeaver @this, Index at, T obj, string? format = null) where T : IFormattable
    {
        if (obj is not null) @this.Insert(at, obj.ToString(format, @this.FormatProvider));
    }
}
