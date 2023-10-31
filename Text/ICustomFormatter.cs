using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;
public interface ICustomFormatter<T>
{
    void Format(T obj, string? format, StringBuilder to);
}

public class BunanFormatter : ICustomFormatter<Chain<Bunan, IBunanWeaver>>
{
    public void Format(Chain<Bunan, IBunanWeaver> span, string? format, StringBuilder to)
    {
        span.Direct(new UTF16BunanWeaver(to));
    }
}