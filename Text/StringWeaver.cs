using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STBuilder = System.Text.StringBuilder;

namespace Nonno.Text;
public class StringWeaver : IObjectWeaver, ITreeWeaver
{
    public const char LINE_FEED = '\n';
    public const char TAB = '\t';

    readonly STBuilder _builder;
    int _depth;

    public STBuilder InternalBuilder => _builder;
    public IFormatProvider FormatProvider { get; init; }
    public int Length => _builder.Length;
    public int Capacity => _builder.Capacity;
    public int Depth
    {
        get => _depth;
        set
        {
            _depth = value;
        }
    }

    public StringWeaver()
    {
        _builder = new();
        FormatProvider = CultureInfo.CurrentCulture;
    }
    public StringWeaver(STBuilder builder, IFormatProvider? formatProvider = null)
    {
        _builder = builder;
        FormatProvider = formatProvider ?? CultureInfo.CurrentCulture;
    }

#if NETSTANDARD2_1_OR_GREATER
    public void Append(string? str) => _builder.Append(str);
    public void Append(object? obj) => _builder.Append(obj);
    public void Append(char @char) => _builder.Append(@char);
    public void Append(IFormattable obj, string? format = null) => _builder.Append(obj.ToString(format, FormatProvider));
    public void Append(ReadOnlySpan<char> str) => _builder.Append(str);
    public void Append(ReadOnlySpan<byte> str) => _builder.Append(Encoding.UTF8.GetString(str));
    public void AppendLine()
    {
        var s = (stackalloc char[1 + Depth]);
        s.Fill(TAB);
        s[0] = LINE_FEED;
        _builder.Append(s);
    }
    public void Clear() => _builder.Clear();
    public void EnsureCapacity(int capacity) => _builder.EnsureCapacity(capacity);
    public void Insert(int at, char @char) => _builder.Insert(at, @char);
    public void Insert(Index at, string? str) => _builder.Insert(at.GetOffset(Length), str);
    public void Insert(Index at, object? obj) => _builder.Insert(at.GetOffset(Length), obj);
    public void Insert(Index at, char @char) => _builder.Insert(at.GetOffset(Length), @char);
    public void Insert(Index at, IFormattable obj, string? format = null) => _builder.Insert(at.GetOffset(Length), obj.ToString(format, FormatProvider));
    public void Insert(Index at, ReadOnlySpan<char> str) => _builder.Insert(at.GetOffset(Length), str);
    public void Insert(Index at, ReadOnlySpan<byte> str) => _builder.Insert(at.GetOffset(Length), Encoding.UTF8.GetString(str));
    public void InsertLine(Index at)
    {
        var s = (stackalloc char[1 + Depth]);
        s.Fill(TAB);
        s[0] = LINE_FEED;
        _builder.Insert(at.GetOffset(_builder.Length), s);
    }
    public char Remove(int at) 
    {
        var r = _builder[at];
        _builder.Remove(at, 1);
        return r;
    }
    public ReadOnlySpan<char> Remove(Range range)
    {
        var (o, l) = range.GetOffsetAndLength(Length);
        _builder.Remove(o, l);
    }
    public char Remove(Index at) => _builder.Remove(at.GetOffset(Length), 1);
    public void Replace(char old, char neo, int start, int count) => _builder.Replace(old, neo, start, count);
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

    public int GetIndex(char of, int start, int count) => throw new NotSupportedException();
    public Index GetIndex(char of, Range range) => throw new NotSupportedException();
    public Range GetRange(ReadOnlySpan<char> of, Range range) => throw new NotSupportedException();
    void ICodeWeaver<char>.Pose(char code) => Append(code);
#else
    public void Append(string? str) => _builder.Append(str);
    public void Append(object? obj) => _builder.Append(obj);
    public void Append(char @char) => _builder.Append(@char);
    public void Append(IFormattable obj, string? format = null) => _builder.Append(obj.ToString(format, FormatProvider));
    public void AppendLine()
    {
        _builder.Append(TAB);
        for (int i = 0; i < Depth; i++) _builder.Append(LINE_FEED);
    }
    public void Clear() => _builder.Clear();
    public void EnsureCapacity(int capacity) => _builder.EnsureCapacity(capacity);
    public void Insert(int at, char @char) => _builder.Insert(at, @char);
    public char Remove(int at)
    {
        var r = _builder[at];
        _builder.Remove(at, 1);
        return r;
    }
    public void Replace(char old, char neo, int start, int count) => _builder.Replace(old, neo, start, count);
    public void Replace(char old, char neo) => _builder.Replace(old, neo);
    public void Replace(string old, string neo) => _builder.Replace(old, neo);

    public override string ToString() => _builder.ToString();

    public void Terminate() { }

    public int GetIndex(char of, int start, int count) => throw new NotSupportedException();
    void ICodeWeaver<char>.Pose(char code) => Append(code);
#endif
}
