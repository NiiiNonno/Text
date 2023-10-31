using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;
public class Word
{
    readonly string _utf16;
    readonly Stroke[] _strs;
    readonly Bunan[] _buns;

    public override string ToString() => _utf16;
}

public interface IBunanWord
{

}
