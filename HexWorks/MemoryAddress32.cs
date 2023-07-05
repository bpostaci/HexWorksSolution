using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexWorks
{
    /// <summary>
    /// MemoryAddress32 is an immutable value object thats stores memory address for 32 bit.
    /// </summary>
    public class MemoryAddress32
    {
        private readonly UInt32 _value;

        public MemoryAddress32(UInt32 value)
        {
            _value = value;
        }

        public MemoryAddress32(int value)
        {
            _value = (UInt32)value;
        }

        public MemoryAddress32(IntPtr ptr)
        {
            _value = (UInt32)ptr;
        }
        public MemoryAddress32(UIntPtr ptr)
        {
            _value = (UInt32)ptr;
        }

        public MemoryAddress32(byte[] byteArray)
        {
            _value = (UInt32)BitConverter.ToInt64(byteArray, 0);
        }

        public MemoryAddress32(string hexString)
        {

            hexString = hexString.Replace("`", "");

            if (hexString.StartsWith("0x"))
            {
                hexString = hexString.Substring(2);
            }

            if (!Hex.IsHexadecimal(hexString))
            {
                throw new ArgumentException("Input string is not a hex string");
            }

            if(hexString.Length > 8)
            {
                throw new ArgumentException("Hex string is bigger than size of 32 bit"); 
            }

            _value = Convert.ToUInt32(hexString, 16);
        }

        public UInt32 Value => _value;

        public MemoryAddress32 HighBytes()
        {
            var q = _value >> 16;
            MemoryAddress32 x = new MemoryAddress32(q);
            return x;
        }

        public MemoryAddress32 LowBytes()
        {
            var q = (_value << 16) >> 16;
            MemoryAddress32 x = new MemoryAddress32(q);
            return x;

        }

        public MemoryAddress32 ClearEndBits(int numBits)
        {
            if (numBits > 32)
                throw new ArgumentException("Can not be bigger than 64 bit");


            var c = (_value >> numBits) << numBits;
            return new MemoryAddress32(c);

        }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((MemoryAddress32)obj);
        }

        public bool Equals(MemoryAddress32 other)
        {
            if (other == null)
                return false;

            return _value == other._value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(MemoryAddress32 address1, MemoryAddress32 address2)
        {
            if (ReferenceEquals(address1, address2))
                return true;

            if (ReferenceEquals(address1, null) || ReferenceEquals(address2, null))
                return false;

            return address1.Equals(address2);
        }

        public static bool operator !=(MemoryAddress32 address1, MemoryAddress32 address2)
        {
            return !(address1 == address2);
        }

        public static MemoryAddress32 operator +(MemoryAddress32 address1, MemoryAddress32 address2)
        {
            UInt32 sum = address1._value + address2._value;
            return new MemoryAddress32(sum);
        }
        public static MemoryAddress32 operator +(MemoryAddress32 address1, UInt32 value)
        {
            UInt32 sum = address1._value + value;
            return new MemoryAddress32(sum);
        }
        public static MemoryAddress32 operator -(MemoryAddress32 address1, MemoryAddress32 address2)
        {
            UInt32 sum = address1._value - address2._value;
            return new MemoryAddress32(sum);
        }
        public static MemoryAddress32 operator -(MemoryAddress32 address1, UInt32 value)
        {
            UInt32 sum = address1._value - value;
            return new MemoryAddress32(sum);
        }


        public static MemoryAddress32 operator --(MemoryAddress32 address)
        {
            UInt32 decrementedValue = address._value - 1;
            return new MemoryAddress32(decrementedValue);
        }
        public static MemoryAddress32 operator ++(MemoryAddress32 address)
        {
            UInt32 decrementedValue = address._value + 1;
            return new MemoryAddress32(decrementedValue);
        }


        public static implicit operator MemoryAddress32(string hexString)
        {
            return new MemoryAddress32(hexString);
        }

        public static implicit operator MemoryAddress32(UInt32 value)
        {
            return new MemoryAddress32(value);
        }

        public static implicit operator MemoryAddress32(int value)
        {
            return new MemoryAddress32((UInt32)value);
        }

        public static implicit operator MemoryAddress32(UIntPtr value)
        {
            return new MemoryAddress32((UInt32)value);
        }
        public static implicit operator MemoryAddress32(IntPtr value)
        {
            return new MemoryAddress32((UInt32)value);
        }

        public string ToHexString()
        {
            return ToHexString(false, false);
        }
        public string ToHexString(bool includePrefix, bool isCapitalLetters = false)
        {
            return ToString(includePrefix, isCapitalLetters);
        }

        public string ToBits()
        {
            int v = (int)_value;
            return Convert.ToString(v, toBase: 2).PadLeft(32,'0');
        }

        public byte[] ToByteArray()
        {
            return BitConverter.GetBytes(_value);
        }

        public override string ToString()
        {
            return ToString(includePrefix: false);
        }

        public string ToString(bool includePrefix, bool isCapital = false)
        {
            string hexString = _value.ToString("X8");
            if (!isCapital) hexString = hexString.ToLower();

            if (includePrefix)
            {
                hexString = "0x" + hexString;
            }

            return hexString;
        }

    }
}
