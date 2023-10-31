using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;
public class CachedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    readonly IDictionary<TKey, TValue> _cache;
    readonly IReadOnlyDictionary<TKey, TValue> _base;

    public CachedDictionary(IDictionary<TKey, TValue> cache, IReadOnlyDictionary<TKey, TValue> @base)
    {
        _cache = cache;
        _base = @base;
    }

    public TValue this[TKey key] => TryGetValue(key, out var r) ? r : throw new KeyNotFoundException();
    public IEnumerable<TKey> Keys => _base.Keys;
    public IEnumerable<TValue> Values => _base.Values;
    public int Count => _base.Count;

    public bool ContainsKey(TKey key)
    {
        if (_cache.ContainsKey(key)) return true;
        if (_base.TryGetValue(key, out var value)) { _cache.Add(key, value); return true; }
        return false;
    }
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _base.GetEnumerator();
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (_cache.TryGetValue(key, out value)) return true;
        if (_base.TryGetValue(key, out value)) { _cache.Add(key, value); return true; }
        return false;
    }
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_base).GetEnumerator();
}

public class DelegatedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    readonly Func<TKey, TValue> _func;

    public DelegatedDictionary(Func<TKey, TValue> func)
    {
        _func = func;
    }

    public TValue this[TKey key] => _func(key);

    public bool ContainsKey(TKey key) => true;
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) { value = _func(key); return true; }

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw new NotSupportedException();
    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => throw new NotSupportedException();
    int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => throw new NotSupportedException();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => throw new NotSupportedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
}