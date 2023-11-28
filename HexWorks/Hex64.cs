// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Alias: bpostaci
// Date : 22/11/2023
// Ver  : 1.1


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HexWorks
{
    /// <summary>
    /// Hex64 is an immutable value object thats stores memory address for 64 bit.
    /// </summary>
    public class Hex64 : IEquatable<Hex64>
    {

        public static Hex64 FromHexString(string hexString)
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

            if (hexString.Length > 16)
            {
                throw new ArgumentException("Hex string size is bigger than 64bit");
            }
            ulong result = Convert.ToUInt64(hexString, 16);
            return new Hex64(result);
        }

        public static Hex64 FromBinaryString(string binaryString)
        {
            const int MaxBinaryLength = 64; // Assuming Hex64 is a 64-bit value
            const int MinBinaryLength = 1;


            if (!IsValidBinaryString(binaryString))
                throw new ArgumentException("Input string is not a valid binary string. Example: \"1010b\"");


            int length = binaryString.Length;
            if (length > MaxBinaryLength + 1 || length < MinBinaryLength)
                throw new ArgumentException($"Input string length should be between {MinBinaryLength} and {MaxBinaryLength + 1}");


            string raw = binaryString.TrimEnd('b'); 
            ulong result = Hex.ConvertBinaryToUInt64(raw);
            return new Hex64(result);
        }

        private static bool IsValidBinaryString(string binaryString)
        {
            return binaryString.Length >= 2 && binaryString.EndsWith("b") && binaryString.Substring(0, binaryString.Length - 1).All(c => c == '0' || c == '1');
        }


        private readonly ulong _value;
        public ulong Value => _value;

        #region CONSTRUCTORS
        public Hex64(ulong value)
        {
            _value = value;
        }

        public Hex64(long value)
        {
            _value = (ulong)value;
        }

        public Hex64(IntPtr ptr)
        {
            _value = (ulong)ptr;
        }

        public Hex64(UIntPtr ptr)
        {
            _value = (ulong)ptr;
        }
        /// <summary>
        /// Constructs MemoryAddress from byte array
        /// </summary>
        /// <param name="byteArray"> By Default it should be litteendian for 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 => 0xF0DEBC9A78563412 </param>
        /// <param name="IsLittleEndian">to make it reverse make it false;for 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 => 0x123456789ABCDEF0 </param>
        /// <exception cref="ArgumentException">Array size must be 8 bytes.</exception>
        public Hex64(byte[] byteArray,bool isLittleEndian = true)
        {
            if (byteArray == null || byteArray.Length != 8) throw new ArgumentException("byte array size must fit with 64 bit (8 bytes)");
            if (BitConverter.IsLittleEndian != isLittleEndian)
            {
                // Reverse the byte array if endianness does not match the system's endianness
                Array.Reverse(byteArray);
            }
            _value =  (ulong) BitConverter.ToInt64(byteArray, 0);
        }

        /// <summary>
        /// Constructs MemoryAddress object from hex string
        /// </summary>
        /// <param name="hexString">Hex data as string exp. "0x42" or "42" </param>
        /// <exception cref="ArgumentException"></exception>
        public Hex64(string hexString)
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

            return Equals((Hex64)obj);
        }

        public bool Equals(Hex64 other)
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


        public static bool operator ==(Hex64 address1, Hex64 address2)
        {
            if (ReferenceEquals(address1, address2))
                return true;

            if (ReferenceEquals(address1, null) || ReferenceEquals(address2, null))
                return false;

            return address1.Equals(address2);
        }

        public static bool operator !=(Hex64 address1, Hex64 address2)
        {
            return !(address1 == address2);
        }

        public static Hex64 operator +(Hex64 address1, Hex64 address2)
        {
            ulong sum = address1._value + address2._value;
            return new Hex64(sum);
        }
        public static Hex64 operator +(Hex64 address1, ulong value)
        {
            ulong sum = address1._value + value;
            return new Hex64(sum);
        }
        public static Hex64 operator -(Hex64 address1, Hex64 address2)
        {
            ulong sum = address1._value - address2._value;
            return new Hex64(sum);
        }
        public static Hex64 operator -(Hex64 address1, ulong value)
        {
            ulong sum = address1._value - value;
            return new Hex64(sum);
        }

        public static Hex64 operator *(Hex64 address1, Hex64 address2)
        {
            ulong result = address1._value * address2._value; 
            return new Hex64(result);
        }
        public static Hex64 operator *(Hex64 address1, long value)
        {
                ulong result = (ulong)( (long) address1._value * value);
                return new Hex64(result);
           
        }

        public static Hex64 operator &(Hex64 address1, Hex64 address2)
        {
            ulong op = address1._value & address2._value;
            return new Hex64(op);
        }
        public static Hex64 operator |(Hex64 address1, Hex64 address2)
        {
            ulong op = address1._value | address2._value;
            return new Hex64(op);
        }
        public static Hex64 operator ^(Hex64 address1, Hex64 address2)
        {
            ulong op = address1._value ^ address2._value;
            return new Hex64(op);
        }
        public static Hex64 operator <<(Hex64 address1, int shiftamount)
        {
            ulong op = address1._value << shiftamount;
            return new Hex64(op);
        }
        public static Hex64 operator >>(Hex64 address1, int shiftamount)
        {
            ulong op = address1._value >> shiftamount;
            return new Hex64(op);
        }

        public static Hex64 operator ~(Hex64 address1)
        {
            return new Hex64(~address1._value);
        }

        public static Hex64 operator --(Hex64 address)
        {
            ulong decrementedValue = address._value - 1;
            return new Hex64(decrementedValue);
        }
        public static Hex64 operator ++(Hex64 address)
        {
            ulong decrementedValue = address._value +1;
            return new Hex64(decrementedValue);
        }


        public static implicit operator Hex64(string hexString)
        {
            return new Hex64(hexString);
        }

        public static implicit operator Hex64(ulong value)
        {
            return new Hex64(value);
        }

        public static implicit operator Hex64(long value)
        {
            return new Hex64((ulong)value);
        }

        public static implicit operator Hex64(int value)
        {
            return new Hex64((ulong)value);
        }

        public static implicit operator Hex64(uint value)
        {
            return new Hex64((ulong)value);
        }
        public static implicit operator Hex64(IntPtr value)
        {
            return new Hex64((ulong)value);
        }
        public static implicit operator Hex64(UIntPtr value)
        {
            return new Hex64((ulong)value);
        }

        #endregion

        #region BITWISE OPERATIONS
        /// <summary>
        /// Toggles a single bit for given index 
        /// </summary>
        /// <param name="bitPosition"> Bit position must be between 0 and 63 (inclusive).</param>
        /// <returns> returns a new Hex64 object </returns>
        public Hex64 ToggleBit(int bitPosition)
        {
            if (bitPosition < 0 || bitPosition >= 64)
            {
                throw new ArgumentOutOfRangeException(nameof(bitPosition), "Bit position must be between 0 and 63 (inclusive).");
            }

            ulong bitmask = (ulong)1 << bitPosition;
            return new Hex64(_value ^ bitmask); 
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

        public Hex64 SetBit(int bitPosition, int newBitValue)
        {
            if (bitPosition < 0 || bitPosition >= 64)
            {
                throw new ArgumentOutOfRangeException(nameof(bitPosition), "Bit position must be between 0 and 63 (inclusive).");
            }

            ulong clearMask = ~(1UL << bitPosition);
            ulong setMask = (ulong)newBitValue << bitPosition;
            ulong result = (_value & clearMask) | setMask;
            return new Hex64(result);
        }

        /// <summary>
        /// Returns a new Hex64 for high 32 bits .
        /// </summary>
        /// <returns>returns a new Hex64 object </returns>
        public Hex64 HighBytes()
        {
            var q = _value >> 32;
            Hex64 x = new Hex64(q);
            return x;
        }

        /// <summary>
        /// Returns a new Hex64 for low 32 bits .
        /// </summary>
        /// <returns> returns a new Hex64 object</returns>
        public Hex64 LowBytes()
        {
            var q = (_value << 32) >> 32;
            Hex64 x = new Hex64(q);
            return x;

        }
        /// <summary>
        /// Clears the given number of the bits from begining low end-side. 
        /// </summary>
        /// <param name="numBits">Number of bits to clear </param>
        /// <returns> returns a new Hex64 object </returns>
        /// <exception cref="ArgumentException"></exception>
        public Hex64 GetBaseAddress(int offsetSizeAsBit)
        {
            if (offsetSizeAsBit < 0 || offsetSizeAsBit >= 64)
            {
                throw new ArgumentOutOfRangeException(nameof(offsetSizeAsBit), "Offset size must be between 0 and 63 (inclusive).");
            }


            var c = (_value >> offsetSizeAsBit) << offsetSizeAsBit;
            return new Hex64(c);

        }

        public Hex64 Offset(long offset)
        {
            ulong result = _value; 
            if( offset < 0)
            {
                result = result - (ulong)(-1 * offset); 
            }
            else
            {
                result = result + (ulong)offset; 
            }
                
            return new Hex64(result); 
        }

        public bool TestBit(ulong data)
        {
            if ( (this.Value & data) == data)
                return true;
            else
                return false; 
        }

        public bool TestBit(uint data)
        {
            if ((this.Value & data) == data)
                return true;
            else
                return false;
        }

        public Hex64 NAND(Hex64 address)
        {
            ulong result = ~(_value & address._value);
            return new Hex64(result); 
        }

        public Hex64 XNOR(Hex64 address)
        {
            return new Hex64( ~(_value ^ address._value));
        }
        public Hex64 NOR(Hex64 address)
        {
            return new Hex64(~(_value | address._value));
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
