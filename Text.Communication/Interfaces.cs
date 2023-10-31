using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text.Communication;
public interface IPersistent<T>
{
    event EventHandler Updated;

    T? Latest { get; }
}
