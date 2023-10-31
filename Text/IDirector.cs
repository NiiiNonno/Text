using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;
public interface IDirector<TTarget>
{
    void Direct(TTarget target);
}

public readonly struct Chain<THandler, TTarget> : IDirector<TTarget>, IFormattable where THandler : IDirector<TTarget>
{
    readonly ImmutableArray<THandler> _handlers;

    public Chain(ImmutableArray<THandler> handlers)
    {
        _handlers = handlers;
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
            var r = new StringBuilder();
            f.Format(this, format, to: r);
            return r.ToString();
        }
        else
        {
            return string.Concat(_handlers);
        }
    }
}
