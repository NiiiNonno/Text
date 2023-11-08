using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;
public interface ICustomFormatter<T>
{
    void Format(T obj, string? format, StringWeaver to);
}
