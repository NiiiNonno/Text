using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text.Communication;
public interface IForum<TText> where TText : Text
{
    event TextAddedEventHandler<TText>? TextAdded;

    void AddText(TText text);
    Task AddTextAsync(TText text)
    {
        AddText(text);
        return Task.CompletedTask;
    }
}

public delegate void TextAddedEventHandler<TText>(object? sender, TText text) where TText : Text;

public class ChatForum<TText> : IForum<TText> where TText : Text
{
    readonly LinkedList<Record> _record = new();

    public void AddText(TText text)
    {
        _record.AddLast(new Record(text, DateTime.Now));
    }

    public record Record(TText Text, DateTime DateTime);
}
