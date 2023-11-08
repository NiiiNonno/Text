using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Nonno.Assets.Collections;
using SysGC = System.Collections.Generic;
using NAC = Nonno.Assets.Collections;
using System.Linq;

namespace Nonno.Text.Transformation;
public abstract class BunanTrieNode<TNode> : NAC::ITable<Bunan, TNode>
{
    static readonly ArrayList<Bunan> KEYS = new(GetCapitals().ToArray());

    readonly TNode?[] _children;

    public NAC.ISet<Bunan> Keys => KEYS;

    public TNode? this[Bunan code] 
    { 
        get => TryGetValue(code, out var r) ? r : throw new KeyNotFoundException();
        set { if (!TrySetValue(code, value)) throw new KeyNotFoundException(); }
    }

    public BunanTrieNode()
    {
        _children = new TNode[Bunan.CODE_CAPITAL_MAX];
    }

    public bool ContainsKey(Bunan key) => key.Code < _children.Length;
    public IEnumerator<KeyValuePair<Bunan, TNode?>> GetEnumerator() => Keys.Select(x => new KeyValuePair<Bunan, TNode?>(x, this[x])).GetEnumerator();
    public bool TryGetValue(Bunan key, [MaybeNullWhen(false)] out TNode value)
    {
        if (key.Code < _children.Length)
        {
            value = _children[key.Code];
            if (value is null) _children[key.Code] = value = CreateNode(key);
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }
    public bool TrySetValue(Bunan key, TNode? value)
    {
        if (key.Code >= _children.Length) return false;
        _children[key.Code] = value;
        return true;
    }

    public abstract TNode CreateNode(Bunan key);

    static IEnumerable<Bunan> GetCapitals()
    {
        for (byte i = Bunan.CODE_CAPITAL_MIN; i < Bunan.CODE_CAPITAL_MAX; i++)
        {
            yield return new(i);
        }
    }
}

public class UnicodeBunanWeaver : ICodeWeaver<Bunan>
{
    readonly Trie<ArraySegmentReverseEnumerator<Bunan>, INode, Bunan> _trie;
    readonly ArrayList<Bunan> _key;
    StringWeaver _builder;

    public UnicodeBunanWeaver(StringWeaver builder)
    {
        _trie = new(new InternalNode(new()));
        _key = new();
        _builder = builder;
    }

    public void Pose(Bunan bunan)
    {
        _key.Add(bunan);
        if (!_trie.TryGetValue(_key[..].GetReverseEnumerator(), out var node))
        {
            _trie.TrySetValue(_key[..].GetReverseEnumerator(), node = bunan.IsSeparator switch
            {
                true => new LeafNode(_trie.Root),
                false => new InternalNode(_key[..]),
            });
        }

        if (bunan.IsSeparator)
        {
            Terminate();
        }
    }

    public void Terminate()
    {

    }

    public interface INode : ITable<Bunan, INode>
    {
        
    }

    public class InternalNode : BunanTrieNode<INode>, INode
    {
        readonly ArraySegment<Bunan> _key;

        public InternalNode(ArraySegment<Bunan> key) : base()
        {
            _key = key;
        }
    }

    public class LeafNode : INode
    {
        readonly INode _root;

        public LeafNode(INode root)
        {
            _root = root;
        }

        public INode? this[Bunan key] 
        { 
            get => _root[key]; 
            set => _root[key] = value; 
        }

        public NAC.ISet<Bunan> Keys => Bunan.Capitals;
    }
}