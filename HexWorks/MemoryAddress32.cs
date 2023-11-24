// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Alias: bpostaci
// Date : 22/11/2023


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
        public UInt32 Value => _value;

        #region CONSTRUCTORS
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
        /// <summary>
        /// Constructor for a MemoryAddress from bytearray
        /// </summary>
        /// <param name="byteArray"> Default Should be little endian 0x12345678 => { 0x78, 0x56, 0x34, 0x12 } </param>
        /// <param name="IsLitteEndian">To change it make it false 0x12345678 => { 0x12 0x34 0x56 0x78 } </param>
        /// <exception cref="ArgumentException"></exception>
        public MemoryAddress32(byte[] byteArray,bool IsLitteEndian =true)
        {
            if (byteArray.Length != 4) throw new ArgumentException("byte array size must fit with 32 bit (4 bytes)"); 
            
            if(!IsLitteEndian)
            {
                Array.Reverse(byteArray);
            }
            
            _value = (UInt32)BitConverter.ToUInt32(byteArray, 0);
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
        #endregion

        #region BITWISE OPERATIONS
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

        public MemoryAddress32 GetBaseAddress(int OffsetSizeAsBits)
        {
            if (OffsetSizeAsBits < 0 || OffsetSizeAsBits >= 32)
            {
                throw new ArgumentOutOfRangeException(nameof(OffsetSizeAsBits), "Offset size must be between 0 and 31 (inclusive).");
            }


            var c = (_value >> OffsetSizeAsBits) << OffsetSizeAsBits;
            return new MemoryAddress32(c);

        }
        /// <summary>
        /// Toggles a single bit for given index 
        /// </summary>
        /// <param name="bitIndex"> Should between 0-31 </param>
        /// <returns></returns>
        public MemoryAddress32 ToggleBit(int bitIndex)
        {
            if (bitIndex < 0 || bitIndex >= 32)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), "Bit position must be between 0 and 31 (inclusive).");
            }

            uint bitmask = (uint)1 << bitIndex; 
            return new MemoryAddress32(_value ^ bitmask);
        }

        public int GetBit(int bitPosition)
        {
            if (bitPosition < 0 || bitPosition >= 32 )
            {
                throw new ArgumentOutOfRangeException(nameof(bitPosition), "Bit position must be between 0 and 31 (inclusive).");
            }

            uint mask = (uint)1 << bitPosition;
            return (_value & mask) == 0 ? 0 : 1;
        }
        public MemoryAddress32 SetBit(int bitPosition, int newBitValue)
        {
            if (bitPosition < 0 || bitPosition >= 32)
            {
                throw new ArgumentOutOfRangeException(nameof(bitPosition), "Bit position must be between 0 and 31 (inclusive).");
            }

            uint clearMask = ~( (uint)1 << bitPosition);
            uint setMask = (uint)newBitValue << bitPosition;
            uint result = (_value & clearMask) | setMask;
            return new MemoryAddress32(result);   
        }

        public MemoryAddress32 Offset(int offset)
        {
            uint result = _value;
            if (offset < 0)
            {
                result = result - (uint)(-1 * offset);
            }
            else
            {
                result = result + (uint)offset;
            }

            return new MemoryAddress32(result);
        }

        public MemoryAddress32 NAND(MemoryAddress32 address)
        {
            uint result = ~(_value & address._value);
            return new MemoryAddress32(result);
        }

        public MemoryAddress32 XNOR(MemoryAddress32 address)
        {
            return new MemoryAddress32(~(_value ^ address._value));
        }


        public MemoryAddress32 NOR(MemoryAddress32 address)
        {
            return new MemoryAddress32(~(_value | address._value));
        }


        #endregion

        #region EQUALITY
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

        #endregion

        #region OPERATOR OVERLOADS
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

        public static MemoryAddress32 operator *(MemoryAddress32 address1, MemoryAddress32 address2)
        {
            uint result = address1._value * address2._value;
            return new MemoryAddress32(result);
        }
        public static MemoryAddress32 operator *(MemoryAddress32 address1, int value)
        {
            checked
            {
                uint result = (uint)((int)address1._value * value);
                return new MemoryAddress32(result);
            }
        }


        public static MemoryAddress32 operator &(MemoryAddress32 address1, MemoryAddress32 address2)
        {
            UInt32 op = address1._value & address2._value;
            return new MemoryAddress32(op);
        }
        public static MemoryAddress32 operator |(MemoryAddress32 address1, MemoryAddress32 address2)
        {
            UInt32 op = address1._value | address2._value;
            return new MemoryAddress32(op);
        }
        public static MemoryAddress32 operator ^(MemoryAddress32 address1, MemoryAddress32 address2)
        {
            UInt32 op = address1._value ^ address2._value;
            return new MemoryAddress32(op);
        }
        public static MemoryAddress32 operator <<(MemoryAddress32 address1, int shiftamount)
        {
            UInt32 op = address1._value << shiftamount;
            return new MemoryAddress32(op);
        }
        public static MemoryAddress32 operator >>(MemoryAddress32 address1, int shiftamount)
        {
            UInt32 op = address1._value >> shiftamount;
            return new MemoryAddress32(op);
        }

        public static MemoryAddress32 operator ~(MemoryAddress32 address1)
        {
            return new MemoryAddress32(~address1._value);
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

        #endregion 

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
        public string ToBits(int SeparateByBitCount,string seperator )
        {
            int v = (int)_value;
            string result = Convert.ToString(v, toBase: 2).PadLeft(32, '0');

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
