using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using STBuilder = System.Text.StringBuilder;

namespace Nonno.Text;
public class Text
{
    readonly ListTable<IWeaver> _table;
    IWeaver _weaver;
    int _current;

    protected Text()
    {
        _table = new();
        _weaver = null!;
        _current = -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal TWeaver GetWeaver<TWeaver>() where TWeaver : class, IWeaver
    {
        var i = ValueOf<TWeaver>.index;
        
        if (_current != i)
        {
            _weaver.Terminate();
            _current = i;
            _weaver = _table[i] ?? InitializeWeaver<TWeaver>();
        }

        Debug.Assert(_weaver is TWeaver);
        return Unsafe.As<TWeaver>(_weaver);
    }

    protected void SetWeaver<TWeaver>(TWeaver weaver) where TWeaver : IWeaver
    {
        _table[ValueOf<TWeaver>.index] = weaver;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Weave<TDirector, TWeaver>(TDirector director) where TDirector : IDirector<TWeaver> where TWeaver : class, IWeaver
    {
        var w = GetWeaver<TWeaver>();
        director.Direct(w);
    }

    protected virtual TWeaver InitializeWeaver<TWeaver>() where TWeaver : class, IWeaver => throw new NotSupportedException();

    static volatile int _max;

    static class ValueOf<T> where T : IWeaver
    {
        public static int index = Interlocked.Increment(ref _max);
    }

    public class ListTable<TValue>
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
}

public class StringText : Text
{
    readonly STBuilder _builder;

    StringWeaver? _oW;
    ICodeWeaver<Bunan>? _bW;

    protected override TWeaver InitializeWeaver<TWeaver>()
    {
        if (_oW is TWeaver r_o) return r_o;
        if (_bW is TWeaver r_b) return r_b;

        _oW ??= new(_builder);
        if (_oW is TWeaver r_o_) return r_o_;

        throw new NotSupportedException();
    }
}

public static partial class TextExtensions
{
    public static void Weave(this Text text, IFormattable obj, string? format = null)
    {
        text.GetWeaver<IObjectWeaver>().Append(obj, format);
    }
    public static void Weave<T>(this Text text, T obj, string? format = null) where T : IFormattable
    {
        text.GetWeaver<IObjectWeaver>().Append<T>(obj, format);
    }
}
