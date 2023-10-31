using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using STBuilder = System.Text.StringBuilder;

namespace Nonno.Text;
public abstract class Text
{
    readonly ListTable<IWeaver> _table;
    IWeaver _weaver;
    int _current;

    public bool RecordDirectors { get; protected set; }

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
        if (RecordDirectors) Record<TDirector, TWeaver>(director);
        director.Direct(w);
    }

    protected virtual void Record<TDirector, TWeaver>(TDirector director) where TDirector : IDirector<TWeaver> where TWeaver : class, IWeaver { }

    protected virtual TWeaver InitializeWeaver<TWeaver>() where TWeaver : class, IWeaver => throw new NotSupportedException();

    public abstract void Clear();

    public Text Copy();

    static volatile int _max;

    public static TText Brank<TText>() where TText : Text
    {

    }

    static class ValueOf<T> where T : IWeaver
    {
        public static int index = Interlocked.Increment(ref _max);
    }

    static class ValueOf_2<T> where T : Text
    {
        public static T _instance;
    }
}

public class StringText : Text
{
    readonly STBuilder _builder;

    StringWeaver? _oW;
    IBunanWeaver? _bW;

    public override void Clear()
    {
        _builder.Clear();
    }

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
