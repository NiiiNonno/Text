using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;

[StructLayout(LayoutKind.Explicit)]
public readonly struct Stroke
{
    [FieldOffset(0)]
    readonly sbyte _x;
    [FieldOffset(1)]
    readonly sbyte _y;
    [FieldOffset(2)]
    readonly sbyte _z;
    [FieldOffset(3)]
    readonly sbyte _w;
    [FieldOffset(0)]
    readonly int _v;

    public sbyte X => _x;
    public sbyte Y => _y; 
    public sbyte Z => _z; 
    public sbyte W => _w;
    
    public int Code => _v;

    public Stroke(int code)
    {
        _v = code;
    }
    public Stroke(sbyte x, sbyte y, sbyte z, sbyte w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }
}
