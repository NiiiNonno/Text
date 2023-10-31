using Index = Nonno.Text.Struct128<Nonno.Text.Bunan>;

namespace Nonno.Text;
public abstract class BunanWeaver : IBunanWeaver
{
    readonly CachedDictionary<Index, IReadOnlyList<Word>> _words;
    Index _c;
    int _i;

    public IReadOnlyList<Word> Candidates => _words[_c];

    public bool IsFinished { get; private set; }

    public BunanWeaver()
    {
        _c = Index.AllBitsSet;
        _words = new(new Dictionary<Index, IReadOnlyList<Word>>(), new DelegatedDictionary<Index, IReadOnlyList<Word>>(i =>
        {
            return Array.Empty<Word>();
        }));
    }

    public void Pose(Bunan bunan)
    {
        if (!_c.TrySet(_i++, bunan)) throw new Exception();
    }

    public void Confirm(int index = 0)
    {
        Confirm(Candidates[index]);
        _c = Index.AllBitsSet;
        _i = 0;
    }
    protected abstract void Confirm(Word bunanWord);

    public virtual void Terminate()
    {
        Confirm();
        IsFinished = true;
    }
    public virtual void Reset() => IsFinished = false;
}

public class UTF16BunanWeaver : BunanWeaver
{
    StringBuilder _builder;

    public UTF16BunanWeaver(StringBuilder builder)
    {
        _builder = builder;
    }

    protected override void Confirm(Word bunanWord)
    {
        _builder.Append(bunanWord);
    }

    public override void Reset()
    {
        _builder.Clear();

        base.Reset();
    }
}