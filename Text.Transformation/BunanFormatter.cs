namespace Nonno.Text.Transformation;

public class BunanFormatter : ICustomFormatter<Chain<Bunan, ICodeWeaver<Bunan>>>
{
    public void Format(Chain<Bunan, ICodeWeaver<Bunan>> span, string? format, StringWeaver to)
    {
        span.Direct(new UTF16BunanWeaver(to));
    }
}