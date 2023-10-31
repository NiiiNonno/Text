using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STBuilder = System.Text.StringBuilder;

namespace Nonno.Text;
public readonly struct StringBuilder : IObjectWeaver
{
    readonly STBuilder _builder;

    public STBuilder InternalBuilder => _builder;
    public IFormatProvider FormatProvider { get; init; }

    public int Length => _builder.Length;
    public int Capacity => _builder.Capacity;

    public StringBuilder()
    {
        _builder = new();
        FormatProvider = CultureInfo.CurrentCulture;
    }
    public StringBuilder(STBuilder builder, IFormatProvider? formatProvider = null)
    {
        _builder = builder;
        FormatProvider = formatProvider ?? CultureInfo.CurrentCulture;
    }

    public void Append(string? str) => _builder.Append(str);
    public void Append(object? obj) => _builder.Append(obj);
    public void Append(char @char) => _builder.Append(@char);
    public void Append(IFormattable obj, string? format = null) => _builder.Append(obj.ToString(format, FormatProvider));
    public void Append(ReadOnlySpan<char> str) => _builder.Append(str);
    public void Append(ReadOnlySpan<byte> str) => _builder.Append(Encoding.UTF8.GetString(str));
    public void AppendLine() => _builder.AppendLine();
    public void Clear() => _builder.Clear();
    public void EnsureCapacity(int capacity) => _builder.EnsureCapacity(capacity);
    public void Insert(Index at, string? str) => _builder.Insert(at.GetOffset(Length), str);
    public void Insert(Index at, object? obj) => _builder.Insert(at.GetOffset(Length), obj);
    public void Insert(Index at, char @char) => _builder.Insert(at.GetOffset(Length), @char);
    public void Insert(Index at, IFormattable obj, string? format = null) => _builder.Insert(at.GetOffset(Length), obj.ToString(format, FormatProvider));
    public void Insert(Index at, ReadOnlySpan<char> str) => _builder.Insert(at.GetOffset(Length), str);
    public void Insert(Index at, ReadOnlySpan<byte> str) => _builder.Insert(at.GetOffset(Length), Encoding.UTF8.GetString(str));
    public void Remove(Range range)
    {
        var (o, l) = range.GetOffsetAndLength(Length);
        _builder.Remove(o, l);
    }
    public void Remove(Index at) => _builder.Remove(at.GetOffset(Length), 1);
    public void Replace(char old, char neo) => _builder.Replace(old, neo);
    public void Replace(char old, char neo, Range range)
    {
        var (o, l) = range.GetOffsetAndLength(Length);
        _builder.Replace(old, neo, o, l);
    }
    public void Replace(string old, string neo) => _builder.Replace(old, neo);
    public void Replace(string old, string neo, Range range)
    {
        var (o, l) = range.GetOffsetAndLength(Length);
        _builder.Replace(old, neo, o, l);
    }
    public void Replace(ReadOnlySpan<char> old, ReadOnlySpan<char> neo) => Replace(new string(old), new string(neo));
    public void Replace(ReadOnlySpan<char> old, ReadOnlySpan<char> neo, Range range) => Replace(new string(old), new string(neo), range);
    public void Replace(ReadOnlySpan<byte> old, ReadOnlySpan<byte> neo) => Replace(Encoding.UTF8.GetString(old), Encoding.UTF8.GetString(neo));
    public void Replace(ReadOnlySpan<byte> old, ReadOnlySpan<byte> neo, Range range) => Replace(Encoding.UTF8.GetString(old), Encoding.UTF8.GetString(neo), range);

    public override string ToString() => _builder.ToString();

    public void Terminate() { }
}

public class StringWeaver : IObjectWeaver
{
    StringBuilder _base;

    public STBuilder InternalBuilder
    {
        get => _base.InternalBuilder;
        set => _base = new(value, FormatProvider);
    }
    public IFormatProvider FormatProvider 
    { 
        get => _base.FormatProvider; 
        set => _base = new(InternalBuilder, value);
    }

    public int Length => _base.Length;

    public int Capacity => _base.Capacity;

    public StringWeaver(STBuilder builder)
    {
        _base = new(builder);
    }

    public void Append(string? str) => _base.Append(str);
    public void Append(object? obj) => _base.Append(obj);
    public void Append(char @char) => _base.Append(@char);
    public void Append(IFormattable obj, string? format = null) => _base.Append(obj, format);
    public void Append(ReadOnlySpan<char> str) => _base.Append(str);
    public void Append(ReadOnlySpan<byte> str) => _base.Append(str);
    public void AppendLine() => _base.AppendLine();
    public void Clear() => _base.Clear();
    public void EnsureCapacity(int capacity) => _base.EnsureCapacity(capacity);
    public void Insert(Index at, string? str) => _base.Insert(at, str);
    public void Insert(Index at, object? obj) => _base.Insert(at, obj);
    public void Insert(Index at, char @char) => _base.Insert(at, @char);
    public void Insert(Index at, IFormattable obj, string? format = null) => _base.Insert(at, obj, format);
    public void Insert(Index at, ReadOnlySpan<char> str) => _base.Insert(at, str);
    public void Insert(Index at, ReadOnlySpan<byte> str) => _base.Insert(at, str);
    public void Remove(Range range) => _base.Remove(range);
    public void Remove(Index at) => _base.Remove(at);
    public void Replace(char old, char neo) => _base.Replace(old, neo);
    public void Replace(char old, char neo, Range range) => _base.Replace(old, neo, range);
    public void Replace(string old, string neo) => _base.Replace(old, neo);
    public void Replace(string old, string neo, Range range) => _base.Replace(old, neo, range);
    public void Replace(ReadOnlySpan<char> old, ReadOnlySpan<char> neo) => _base.Replace(old, neo);
    public void Replace(ReadOnlySpan<char> old, ReadOnlySpan<char> neo, Range range) => _base.Replace(old, neo, range);
    public void Replace(ReadOnlySpan<byte> old, ReadOnlySpan<byte> neo) => _base.Replace(old, neo);
    public void Replace(ReadOnlySpan<byte> old, ReadOnlySpan<byte> neo, Range range) => _base.Replace(old, neo, range);
    public void Terminate() => _base.Terminate();
}
