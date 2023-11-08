#if !NETSTANDARD2_1_OR_GREATER
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Diagnostics.CodeAnalysis;
[AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
internal sealed class MaybeNullWhenAttribute : Attribute
{
    public MaybeNullWhenAttribute(bool reeturnValue)
    {
        
    }
}

internal sealed class NotNullWhenAttribute : Attribute
{
    public NotNullWhenAttribute(bool reeturnValue)
    {

    }
}
#endif
