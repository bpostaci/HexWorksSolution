// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Alias: bpostaci
// Date : 22/11/2023

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
        public ulong Value => _value;

        #region CONSTRUCTORS
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
        /// <summary>
        /// Constructs MemoryAddress from byte array
        /// </summary>
        /// <param name="byteArray"> By Default it should be litteendian for 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 => 0xF0DEBC9A78563412 </param>
        /// <param name="IsLittleEndian">to make it reverse make it false;for 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 => 0x123456789ABCDEF0 </param>
        /// <exception cref="ArgumentException">Array size must be 8 bytes.</exception>
        public MemoryAddress64(byte[] byteArray,bool IsLittleEndian = true)
        {
            if (byteArray.Length != 8) throw new ArgumentException("byte array size must fit with 64 bit (8 bytes)");
            if(!IsLittleEndian) Array.Reverse(byteArray);   
            _value =  (ulong) BitConverter.ToInt64(byteArray, 0);
        }

        /// <summary>
        /// Constructs MemoryAddress object from hex string
        /// </summary>
        /// <param name="hexString">Hex data as string exp. "0x42" or "42" </param>
        /// <exception cref="ArgumentException"></exception>
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

        #endregion

        #region EQUALITY


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

        #endregion

        #region OPERATOR OVERLOADS


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

        public static MemoryAddress64 operator *(MemoryAddress64 address1, MemoryAddress64 address2)
        {
            ulong result = address1._value * address2._value; 
            return new MemoryAddress64(result);
        }
        public static MemoryAddress64 operator *(MemoryAddress64 address1, long value)
        {
                ulong result = (ulong)( (long) address1._value * value);
                return new MemoryAddress64(result);
           
        }

        public static MemoryAddress64 operator &(MemoryAddress64 address1, MemoryAddress64 address2)
        {
            ulong op = address1._value & address2._value;
            return new MemoryAddress64(op);
        }
        public static MemoryAddress64 operator |(MemoryAddress64 address1, MemoryAddress64 address2)
        {
            ulong op = address1._value | address2._value;
            return new MemoryAddress64(op);
        }
        public static MemoryAddress64 operator ^(MemoryAddress64 address1, MemoryAddress64 address2)
        {
            ulong op = address1._value ^ address2._value;
            return new MemoryAddress64(op);
        }
        public static MemoryAddress64 operator <<(MemoryAddress64 address1, int shiftamount)
        {
            ulong op = address1._value << shiftamount;
            return new MemoryAddress64(op);
        }
        public static MemoryAddress64 operator >>(MemoryAddress64 address1, int shiftamount)
        {
            ulong op = address1._value >> shiftamount;
            return new MemoryAddress64(op);
        }

        public static MemoryAddress64 operator ~(MemoryAddress64 address1)
        {
            return new MemoryAddress64(~address1._value);
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

        #endregion

        #region BITWISE OPERATIONS
        /// <summary>
        /// Toggles a single bit for given index 
        /// </summary>
        /// <param name="bitPosition"> Bit position must be between 0 and 63 (inclusive).</param>
        /// <returns> returns a new MemoryAddress64 object </returns>
        public MemoryAddress64 ToggleBit(int bitPosition)
        {
            if (bitPosition < 0 || bitPosition >= 64)
            {
                throw new ArgumentOutOfRangeException(nameof(bitPosition), "Bit position must be between 0 and 63 (inclusive).");
            }

            ulong bitmask = (ulong)1 << bitPosition;
            return new MemoryAddress64(_value ^ bitmask); 
        }

        /// <summary>
        /// Get the Bit of given Index 
        /// </summary>
        /// <param name="bitPosition">"Bit position must be between 0 and 63 (inclusive).</param>
        /// <returns>an integer which 1 or 0 </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int GetBit(int bitPosition)
        {
            if (bitPosition < 0 || bitPosition >= 64)
            {
                throw new ArgumentOutOfRangeException(nameof(bitPosition), "Bit position must be between 0 and 63 (inclusive).");
            }

            ulong mask = 1UL << bitPosition;
            return (_value & mask) == 0 ? 0 : 1;
        }

        public MemoryAddress64 SetBit(int bitPosition, int newBitValue)
        {
            if (bitPosition < 0 || bitPosition >= 64)
            {
                throw new ArgumentOutOfRangeException(nameof(bitPosition), "Bit position must be between 0 and 63 (inclusive).");
            }

            ulong clearMask = ~(1UL << bitPosition);
            ulong setMask = (ulong)newBitValue << bitPosition;
            ulong result = (_value & clearMask) | setMask;
            return new MemoryAddress64(result);
        }

        /// <summary>
        /// Returns a new MemoryAddress64 for high 32 bits .
        /// </summary>
        /// <returns>returns a new MemoryAddress64 object </returns>
        public MemoryAddress64 HighBytes()
        {
            var q = _value >> 32;
            MemoryAddress64 x = new MemoryAddress64(q);
            return x;
        }

        /// <summary>
        /// Returns a new MemoryAddress64 for low 32 bits .
        /// </summary>
        /// <returns> returns a new MemoryAddress64 object</returns>
        public MemoryAddress64 LowBytes()
        {
            var q = (_value << 32) >> 32;
            MemoryAddress64 x = new MemoryAddress64(q);
            return x;

        }
        /// <summary>
        /// Clears the given number of the bits from begining low end-side. 
        /// </summary>
        /// <param name="numBits">Number of bits to clear </param>
        /// <returns> returns a new MemoryAddress64 object </returns>
        /// <exception cref="ArgumentException"></exception>
        public MemoryAddress64 ClearEndBits(int numBits)
        {
            if (numBits > 64)
                throw new ArgumentException("Can not be bigger than 64 bit");


            var c = (_value >> numBits) << numBits;
            return new MemoryAddress64(c);

        }

        public MemoryAddress64 NAND(MemoryAddress64 address)
        {
            ulong result = ~(_value & address._value);
            return new MemoryAddress64(result); 
        }

        public MemoryAddress64 XNOR(MemoryAddress64 address)
        {
            return new MemoryAddress64( ~(_value ^ address._value));
        }

        
        public MemoryAddress64 NOR(MemoryAddress64 address)
        {
            return new MemoryAddress64(~(_value | address._value));
        }


        #endregion

        public long ToLong()
        {
            return (long)(_value); 
        }

        public string ToSignedHexString(bool includePrefix, bool trim =true)
        {
            long raw = (long)_value;
            string _prefix = "";
            if (includePrefix) _prefix = "0x";
            string result = "";

            if (raw >= 0 )
            {
                
                if(trim)
                {
                    result = raw.ToString("X");
                }
                else
                {
                    result = raw.ToString("X16");
                }
                return "+" + _prefix + result; 
            }
            else
            {
                raw = raw * -1;
                if (trim)
                {
                    result = raw.ToString("X");
                }
                else
                {
                    result = raw.ToString("X16");
                }
                return "-" + _prefix + result; 
            }
            
        }

        public string ToHexString()
        {
            return ToHexString(false, false); 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="includePrefix"> it will add "0x" at the beginning </param>
        /// <param name="isCapitalLetters"> makes it capital letters </param>
        /// <returns></returns>
        public string ToHexString(bool includePrefix,bool isCapitalLetters = false)
        {
            return ToString(includePrefix, isCapitalLetters); 
        }

        public string ToBits()
        {
            long v = (long)_value; 
            return Convert.ToString(v, toBase: 2).PadLeft(64, '0'); 
        }
        public string ToBits(int SeparateByBitCount, string seperator)
        {
            long v = (long)_value;
            string result = Convert.ToString(v, toBase: 2).PadLeft(64, '0');

            return Hex.FormatWithSeperator(result, seperator, SeparateByBitCount);
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
