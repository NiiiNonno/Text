using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace Nonno.Text;
[StructLayout(LayoutKind.Explicit)]
public unsafe readonly struct Struct128<T> : IEquatable<Struct128<T>>, IComparable<Struct128<T>> where T : unmanaged
{
    const int SIZE_64 = 2;
    const int SIZE_8 = 16;

    [FieldOffset(0)]
    readonly Vector128<byte> _v;
    [FieldOffset(0)]
    readonly Vector128<long> _s;

    private Struct128(Vector128<byte> v)
    {
        _v = v;
    }

    public T this[int index]
    {
        get
        {
            if (unchecked((uint)index) >= Length) throw new IndexOutOfRangeException();
            fixed (void* p = &this) return ((T*)p)[index];
        }
        set
        {
            if (unchecked((uint)index) >= Length) throw new IndexOutOfRangeException();
            fixed (void* p = &this) ((T*)p)[index] = value;
        }
    }

    public bool TryGet(int index, out T value)
    {
        if (unchecked((uint)index) >= Length) { value = default; return false; }
        fixed (void* p = &this) value = ((T*)p)[index];
        return true;
    }
    public bool TrySet(int index, T value)
    {
        if (unchecked((uint)index) >= Length) return false;
        fixed (void* p = &this) ((T*)p)[index] = value;
        return true;
    }
    public ref Struct128<U> AsRef<U>() where U : unmanaged
    {
        fixed (void* p = &this) return ref Unsafe.AsRef<Struct128<U>>(p);
    }
    public Struct128<U> As<U>() where U : unmanaged => new(_v);
    public override bool Equals(object? obj) => obj is Struct128<T> @struct && Equals(@struct);
    public bool Equals(Struct128<T> other) => _v.Equals(other._v);
    public override int GetHashCode() => HashCode.Combine(_v);
    public int CompareTo(Struct128<T> other)
    {
        int r;
        r = _s[0].CompareTo(other._s[0]);
        if (r != 0) return r;
        r = _s[1].CompareTo(other._s[1]);
        return r;
    }

    public static bool operator ==(Struct128<T> left, Struct128<T> right) => left.Equals(right);
    public static bool operator !=(Struct128<T> left, Struct128<T> right) => !(left == right);

    public static bool operator <(Struct128<T> left, Struct128<T> right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Struct128<T> left, Struct128<T> right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Struct128<T> left, Struct128<T> right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Struct128<T> left, Struct128<T> right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static int Length { get; } = SIZE_8 / sizeof(T);
    public static Struct128<T> AllBitsSet => new(Vector128<byte>.AllBitsSet);
}
