using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nonno.Assets;
using Nonno.Assets.Collections;

namespace Nonno.Text.Communication;
public interface IForum<TText> where TText : Text
{
    
    IAsyncCollection<ForumRecord<TText>> Records { get; }
    IAsyncCollection<Authority> Members { get; }
}

public class ForumRecord
{
    public UniqueIdentifier<ForumRecord> Identifier { get; }
    public Text Text { get; }
    public DateTime? CreationTime { get; init; }
    public DateTime? UpdatedTime { get; set; }
    public Authority? Author { get; init; }

    public ForumRecord(UniqueIdentifier<ForumRecord> identifier, Text text)
    {
        Identifier = identifier;
        Text = text;
    }
}

public class ForumRecord<TText> : ForumRecord where TText : Text
{
    public new TText Text => Unsafe.As<TText>(base.Text);

    public ForumRecord(UniqueIdentifier<ForumRecord> identifier, TText text) : base(identifier, text)
    {
    }
}

public abstract class Forum<TText> : IForum<TText> where TText : Text
{
    public IAsyncCollection<ForumRecord<TText>> Records { get; }
    public IAsyncCollection<Authority> Members => throw new NotImplementedException();

    public Forum()
    {
        Records = new RecordCollection(this);
    }

    public abstract ValueTask<bool> TryAdd(ForumRecord<TText> record);
    public abstract ValueTask<bool> TryRemove(ForumRecord<TText> record);
    public abstract ValueTask<bool> Contains(ForumRecord<TText> record);
    public abstract IAsyncEnumerator<ForumRecord<TText>> GetEnumerator(CancellationToken cancellationToken);

    public class RecordCollection : IAsyncCollection<ForumRecord<TText>>
    {
        readonly Forum<TText> _base;

        public int Count => throw new NotSupportedException();

        public RecordCollection(Forum<TText> @base) => _base = @base;

        public async Task AddAsync(ForumRecord<TText> item) { if (!await TryAddAsync(item)) throw new Exception(); }
        public Task ClearAsync() => throw new NotImplementedException();
        public ValueTask<bool> ContainsAsync(ForumRecord<TText> item) => _base.Contains(item);
        public IAsyncEnumerator<ForumRecord<TText>> GetAsyncEnumerator(CancellationToken cancellationToken = default) => _base.GetEnumerator(cancellationToken);
        public async Task RemoveAsync(ForumRecord<TText> item) { if (!await TryRemoveAsync(item)) throw new Exception(); }
        public ValueTask<bool> TryAddAsync(ForumRecord<TText> item) => _base.TryAdd(item);
        public ValueTask<bool> TryRemoveAsync(ForumRecord<TText> item) => _base.TryRemove(item);
    }
}
