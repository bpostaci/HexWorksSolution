using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexWorks
{
    /// <summary>
    /// MemoryAddress64 is an immutable value object thats stores memory address for 64 bit.
    /// </summary>
    public class MemoryAddress64 : IEquatable<MemoryAddress64>
    {
        private readonly ulong _value;

        public MemoryAddress64(ulong value)
        {
            _value = value;
        }

        public MemoryAddress64(long value)
        {
            _value = (ulong)value;
        }

        public MemoryAddress64(IntPtr ptr)
        {
            _value = (ulong)ptr;
        }

        public MemoryAddress64(UIntPtr ptr)
        {
            _value = (ulong)ptr;
        }

        public MemoryAddress64(byte[] byteArray)
        {   
            _value =  (ulong) BitConverter.ToInt64(byteArray, 0);
        }

        public MemoryAddress64(string hexString)
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

            if(hexString.Length > 16)
            {
                throw new ArgumentException("Hex string size is bigger than 64bit");
            }

            _value = Convert.ToUInt64(hexString, 16);
        }

        public ulong Value => _value;

        public MemoryAddress64 HighBytes()
        {
            var q = _value >> 32;
            MemoryAddress64 x = new MemoryAddress64(q);
            return x; 
        }

        public MemoryAddress64 LowBytes()
        {
            var q = (_value << 32) >> 32;
            MemoryAddress64 x = new MemoryAddress64(q);
            return x;

        }

        public MemoryAddress64 ClearEndBits( int numBits)
        {
            if (numBits > 64)
                throw new ArgumentException("Can not be bigger than 64 bit");

            
            var c = ( _value >> numBits) << numBits;
            return new MemoryAddress64(c); 

        }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((MemoryAddress64)obj);
        }

        public bool Equals(MemoryAddress64 other)
        {
            if (other == null)
                return false;

            return _value == other._value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(MemoryAddress64 address1, MemoryAddress64 address2)
        {
            if (ReferenceEquals(address1, address2))
                return true;

            if (ReferenceEquals(address1, null) || ReferenceEquals(address2, null))
                return false;

            return address1.Equals(address2);
        }

        public static bool operator !=(MemoryAddress64 address1, MemoryAddress64 address2)
        {
            return !(address1 == address2);
        }

        public static MemoryAddress64 operator +(MemoryAddress64 address1, MemoryAddress64 address2)
        {
            ulong sum = address1._value + address2._value;
            return new MemoryAddress64(sum);
        }
        public static MemoryAddress64 operator +(MemoryAddress64 address1, ulong value)
        {
            ulong sum = address1._value + value;
            return new MemoryAddress64(sum);
        }
        public static MemoryAddress64 operator -(MemoryAddress64 address1, MemoryAddress64 address2)
        {
            ulong sum = address1._value - address2._value;
            return new MemoryAddress64(sum);
        }
        public static MemoryAddress64 operator -(MemoryAddress64 address1, ulong value)
        {
            ulong sum = address1._value - value;
            return new MemoryAddress64(sum);
        }


        public static MemoryAddress64 operator --(MemoryAddress64 address)
        {
            ulong decrementedValue = address._value - 1;
            return new MemoryAddress64(decrementedValue);
        }
        public static MemoryAddress64 operator ++(MemoryAddress64 address)
        {
            ulong decrementedValue = address._value +1;
            return new MemoryAddress64(decrementedValue);
        }


        public static implicit operator MemoryAddress64(string hexString)
        {
            return new MemoryAddress64(hexString);
        }

        public static implicit operator MemoryAddress64(ulong value)
        {
            return new MemoryAddress64(value);
        }

        public static implicit operator MemoryAddress64(long value)
        {
            return new MemoryAddress64((ulong)value);
        }

        public static implicit operator MemoryAddress64(int value)
        {
            return new MemoryAddress64((ulong)value);
        }

        public static implicit operator MemoryAddress64(uint value)
        {
            return new MemoryAddress64((ulong)value);
        }
        public static implicit operator MemoryAddress64(IntPtr value)
        {
            return new MemoryAddress64((ulong)value);
        }
        public static implicit operator MemoryAddress64(UIntPtr value)
        {
            return new MemoryAddress64((ulong)value);
        }



        public string ToHexString()
        {
            return ToHexString(false, false); 
        }
        public string ToHexString(bool includePrefix,bool isCapitalLetters = false)
        {
            return ToString(includePrefix, isCapitalLetters); 
        }

        public string ToBits()
        {
            long v = (long)_value; 
            return Convert.ToString(v, toBase: 2).PadLeft(64, '0'); 
        }

        public byte[] ToByteArray()
        {
            return BitConverter.GetBytes(_value); 
        }

        public override string ToString()
        {
            return ToString(includePrefix: false);
        }

        public string ToString(bool includePrefix,bool isCapital = false)
        {
            string hexString = _value.ToString("X16");
            if(!isCapital) hexString = hexString.ToLower(); 

            if (includePrefix)
            {
                hexString = "0x" + hexString;
            }

            return hexString;
        }


    }


}
