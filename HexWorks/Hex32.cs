// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Alias: bpostaci
// Date : 22/11/2023
// Ver  : 1.1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexWorks
{

    /// <summary>
    /// Hex32 is an immutable value object thats stores memory address for 32 bit.
    /// </summary>
    public class Hex32
    {
        private readonly UInt32 _value;
        public UInt32 Value => _value;

        public static Hex32 FromHexString(string hexString)
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

            if (hexString.Length > 8)
            {
                throw new ArgumentException("Hex string size is bigger than 64bit");
            }
            uint result = Convert.ToUInt32(hexString, 16);
            return new Hex32(result);
        }

        public static Hex32 FromBinaryString(string binaryString)
        {
            const int MaxBinaryLength = 32; 
            const int MinBinaryLength = 1;


            if (!IsValidBinaryString(binaryString))
                throw new ArgumentException("Input string is not a valid binary string. Example: \"1010b\"");


            int length = binaryString.Length;
            if (length > MaxBinaryLength + 1 || length < MinBinaryLength)
                throw new ArgumentException($"Input string length should be between {MinBinaryLength} and {MaxBinaryLength + 1}");


            string raw = binaryString.TrimEnd('b');
            uint result = Hex.ConvertBinaryToUInt32(raw);
            return new Hex32(result);
        }

        private static bool IsValidBinaryString(string binaryString)
        {
            return binaryString.Length >= 2 && binaryString.EndsWith("b") && binaryString.Substring(0, binaryString.Length - 1).All(c => c == '0' || c == '1');
        }


        #region CONSTRUCTORS
        public Hex32(UInt32 value)
        {
            _value = value;
        }

        public Hex32(int value)
        {
            _value = (UInt32)value;
        }

        public Hex32(IntPtr ptr)
        {
            _value = (UInt32)ptr;
        }
        public Hex32(UIntPtr ptr)
        {
            _value = (UInt32)ptr;
        }
        /// <summary>
        /// Constructor for a MemoryAddress from bytearray
        /// </summary>
        /// <param name="byteArray"> Default Should be little endian 0x12345678 => { 0x78, 0x56, 0x34, 0x12 } </param>
        /// <param name="IsLitteEndian">To change it make it false 0x12345678 => { 0x12 0x34 0x56 0x78 } </param>
        /// <exception cref="ArgumentException"></exception>
        public Hex32(byte[] byteArray,bool isLittleEndian = true)
        {
            if (byteArray == null ||byteArray.Length != 4) throw new ArgumentException("byte array size must fit with 32 bit (4 bytes)");

            if (BitConverter.IsLittleEndian != isLittleEndian)
            {
                // Reverse the byte array if endianness does not match the system's endianness
                Array.Reverse(byteArray);
            }


            _value = (UInt32)BitConverter.ToUInt32(byteArray, 0);
        }

        public Hex32(string hexString)
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
        public Hex32 HighBytes()
        {
            var q = _value >> 16;
            Hex32 x = new Hex32(q);
            return x;
        }

        public Hex32 LowBytes()
        {
            var q = (_value << 16) >> 16;
            Hex32 x = new Hex32(q);
            return x;

        }

        public Hex32 GetBaseAddress(int OffsetSizeAsBits)
        {
            if (OffsetSizeAsBits < 0 || OffsetSizeAsBits >= 32)
            {
                throw new ArgumentOutOfRangeException(nameof(OffsetSizeAsBits), "Offset size must be between 0 and 31 (inclusive).");
            }


            var c = (_value >> OffsetSizeAsBits) << OffsetSizeAsBits;
            return new Hex32(c);

        }
        /// <summary>
        /// Toggles a single bit for given index 
        /// </summary>
        /// <param name="bitIndex"> Should between 0-31 </param>
        /// <returns></returns>
        public Hex32 ToggleBit(int bitIndex)
        {
            if (bitIndex < 0 || bitIndex >= 32)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), "Bit position must be between 0 and 31 (inclusive).");
            }

            uint bitmask = (uint)1 << bitIndex; 
            return new Hex32(_value ^ bitmask);
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
        public Hex32 SetBit(int bitPosition, int newBitValue)
        {
            if (bitPosition < 0 || bitPosition >= 32)
            {
                throw new ArgumentOutOfRangeException(nameof(bitPosition), "Bit position must be between 0 and 31 (inclusive).");
            }

            uint clearMask = ~( (uint)1 << bitPosition);
            uint setMask = (uint)newBitValue << bitPosition;
            uint result = (_value & clearMask) | setMask;
            return new Hex32(result);   
        }

        public Hex32 Offset(int offset)
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

            return new Hex32(result);
        }

        public Hex32 NAND(Hex32 address)
        {
            uint result = ~(_value & address._value);
            return new Hex32(result);
        }

        public Hex32 XNOR(Hex32 address)
        {
            return new Hex32(~(_value ^ address._value));
        }


        public Hex32 NOR(Hex32 address)
        {
            return new Hex32(~(_value | address._value));
        }

        public bool TestBit(uint data)
        {
            if ((this.Value & data) == data)
                return true;
            else
                return false;
        }

        #endregion

        #region EQUALITY
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((Hex32)obj);
        }

        public bool Equals(Hex32 other)
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
        public static bool operator ==(Hex32 address1, Hex32 address2)
        {
            if (ReferenceEquals(address1, address2))
                return true;

            if (ReferenceEquals(address1, null) || ReferenceEquals(address2, null))
                return false;

            return address1.Equals(address2);
        }

        public static bool operator !=(Hex32 address1, Hex32 address2)
        {
            return !(address1 == address2);
        }

        public static Hex32 operator +(Hex32 address1, Hex32 address2)
        {
            UInt32 sum = address1._value + address2._value;
            return new Hex32(sum);
        }
        public static Hex32 operator +(Hex32 address1, UInt32 value)
        {
            UInt32 sum = address1._value + value;
            return new Hex32(sum);
        }
        public static Hex32 operator -(Hex32 address1, Hex32 address2)
        {
            UInt32 sum = address1._value - address2._value;
            return new Hex32(sum);
        }
        public static Hex32 operator -(Hex32 address1, UInt32 value)
        {
            UInt32 sum = address1._value - value;
            return new Hex32(sum);
        }

        public static Hex32 operator *(Hex32 address1, Hex32 address2)
        {
            uint result = address1._value * address2._value;
            return new Hex32(result);
        }
        public static Hex32 operator *(Hex32 address1, int value)
        {
            checked
            {
                uint result = (uint)((int)address1._value * value);
                return new Hex32(result);
            }
        }


        public static Hex32 operator &(Hex32 address1, Hex32 address2)
        {
            UInt32 op = address1._value & address2._value;
            return new Hex32(op);
        }
        public static Hex32 operator |(Hex32 address1, Hex32 address2)
        {
            UInt32 op = address1._value | address2._value;
            return new Hex32(op);
        }
        public static Hex32 operator ^(Hex32 address1, Hex32 address2)
        {
            UInt32 op = address1._value ^ address2._value;
            return new Hex32(op);
        }
        public static Hex32 operator <<(Hex32 address1, int shiftamount)
        {
            UInt32 op = address1._value << shiftamount;
            return new Hex32(op);
        }
        public static Hex32 operator >>(Hex32 address1, int shiftamount)
        {
            UInt32 op = address1._value >> shiftamount;
            return new Hex32(op);
        }

        public static Hex32 operator ~(Hex32 address1)
        {
            return new Hex32(~address1._value);
        }

        public static Hex32 operator --(Hex32 address)
        {
            UInt32 decrementedValue = address._value - 1;
            return new Hex32(decrementedValue);
        }
        public static Hex32 operator ++(Hex32 address)
        {
            UInt32 decrementedValue = address._value + 1;
            return new Hex32(decrementedValue);
        }


        public static implicit operator Hex32(string hexString)
        {
            return new Hex32(hexString);
        }

        public static implicit operator Hex32(UInt32 value)
        {
            return new Hex32(value);
        }

        public static implicit operator Hex32(int value)
        {
            return new Hex32((UInt32)value);
        }

        public static implicit operator Hex32(UIntPtr value)
        {
            return new Hex32((UInt32)value);
        }
        public static implicit operator Hex32(IntPtr value)
        {
            return new Hex32((UInt32)value);
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
