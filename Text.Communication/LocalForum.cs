using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nonno.Text.Communication;
public class LocalForum<TText> : Forum<TText> where TText : Text
{
    public override ValueTask<bool> Contains(ForumRecord<TText> record) => throw new NotImplementedException();
    public override IAsyncEnumerator<ForumRecord<TText>> GetEnumerator(CancellationToken cancellationToken) => throw new NotImplementedException();
    public override ValueTask<bool> TryAdd(ForumRecord<TText> record) => throw new NotImplementedException();
    public override ValueTask<bool> TryRemove(ForumRecord<TText> record) => throw new NotImplementedException();
}
