using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;

public interface ITable<TKey, TValue>
{
    TValue? this[TKey key] { get; set; }

    bool ContainsKey(TKey key);
    bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);
    bool TrySetValue(TKey key, TValue value);
}

public class DictionaryTable<TKey, TValue> : ITable<TKey, TValue>
{
    readonly IDictionary<TKey, TValue> _dict;

    public TValue? this[TKey key]
    {
        get
        {
            if (_dict.TryGetValue(key, out var r)) return r;
            return default;
        }
        set
        {
            switch (_dict.ContainsKey(key), value is not null)
            {
            case (true, true):
                _dict[key] = value!;
                return;
            case (true, false):
                _dict.Remove(key);
                return;
            case (false, true):
                _dict.Add(key, value!);
                return;
            case (false, false):
                return;
            }
        }
    }

    public DictionaryTable(IDictionary<TKey, TValue> dictionary) : this()
    {
        if (default(TValue) is not null) throw new ArgumentException("有照型を引数に取ることはできません。", nameof(TValue));
        _dict = dictionary;
    }
    public DictionaryTable()
    {
        if (default(TValue) is not null) throw new ArgumentException("有照型を引数に取ることはできません。", nameof(TValue));
        _dict = new Dictionary<TKey, TValue>();
    }

    public bool ContainsKey(TKey key) => true;
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _dict.TryGetValue(key, out value);
    public bool TrySetValue(TKey key, TValue value) => _dict.TryAdd(key, value);
}

public class ListTable<TValue> : ITable<int, TValue>
{
    TValue?[] _a = Array.Empty<TValue?>();

    public TValue? this[int key]
    { 
        get
        {
            var i = unchecked((uint)key);
            if (i < _a.Length) return _a[i];
            return default;
        }
        set
        {
            if (key < 0) throw new IndexOutOfRangeException();

            if (value is null)
            {
                if (key < _a.Length) return;
                _a[key] = default;
            }
            else
            {
                if (key < _a.Length) Array.Resize(ref _a, Math.Max(key, _a.Length << 1));
                _a[key] = value;
            }
        }
    }

    public bool ContainsKey(int key) => key > 0;
    public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value)
    {
        if (this[key] is { } v) 
        { 
            value = v; 
            return true; 
        }
        else
        {
            value = default;
            return false;
        }
    }
    public bool TrySetValue(int key, TValue value)
    {
        if (key < 0) return false;

        if (key < _a.Length) Array.Resize(ref _a, Math.Max(key, _a.Length << 1));
        _a[key] = value;

        return true;
    }
}
