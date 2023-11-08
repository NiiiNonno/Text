using System;
using System.Globalization;

namespace Nonno.Text;
public interface IDirector<TTarget>
{
    void Direct(TTarget target);
}

public readonly struct Chain<THandler, TTarget> : IDirector<TTarget>, IFormattable where THandler : IDirector<TTarget>
{
    readonly THandler[] _handlers;

    public Chain(params THandler[] handlerParams)
    {
        _handlers = handlerParams;
    }

    public void Direct(TTarget target)
    {
        foreach (var item in _handlers)
        {
            item.Direct(target);
        }
    }

    public override string ToString() => ToString(null, null);
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        formatProvider ??= CultureInfo.CurrentCulture;
        if (formatProvider.GetFormat(typeof(ICustomFormatter<Chain<THandler, TTarget>>)) is ICustomFormatter<Chain<THandler, TTarget>> f)
        {
            var r = new StringWeaver();
            f.Format(this, format, to: r);
            return r.ToString();
        }
        else
        {
            return string.Concat(_handlers);
        }
    }
}
