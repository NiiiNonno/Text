using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text.Communication;
public class SyncedForum<TText> : IForum<TText> where TText : Text
{
    readonly List<IForum<TText>> _forums;

    public ICollection<IForum<TText>> Forums => _forums;

    public void AddText(TText text)
    {
        
    }
}
