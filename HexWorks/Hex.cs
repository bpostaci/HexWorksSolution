﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Alias: bpostaci
// Date : 22/11/2023
// Ver  : 1.1


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HexWorks
{
    public class Hex
    {
        public static long ConvertHexStringToInt64(string hexString)
        {
            // Remove any leading "0x" if present
            hexString = hexString.TrimStart("0x".ToCharArray());

            // Parse the hex string to a long
            long result = long.Parse(hexString, System.Globalization.NumberStyles.HexNumber);

            return result;
        }
        public static string ConvertInt64ToHexString(long value, bool includePrefix = false)
        {
            string hexString = value.ToString("X");

            if (includePrefix)
            {
                hexString = "0x" + hexString;
            }

            return hexString;
        }
        public static bool IsHexadecimal(string input)
        {
            foreach (char c in input)
            {
                if (!IsHexDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
        private static bool IsHexDigit(char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'F') ||
                   (c >= 'a' && c <= 'f');
        }
        public static byte[] ConvertInt64ToByteArray(long value)
        {
            byte[] byteArray = BitConverter.GetBytes(value);
            return byteArray;
        }
        public static long ConvertByteArrayToInt64(byte[] byteArray)
        {
            if (byteArray.Length < 8)
            {
                throw new ArgumentException("Byte array length must be at least 8 bytes.");
            }

            return BitConverter.ToInt64(byteArray, 0);
        }
        public static string FormatWithSeperator(string source, string seperator, int offset)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                if (i > 0 && i % offset == 0)
                {
                    builder.Append(seperator);
                }
                builder.Append(source[i]);
            }
            return builder.ToString();
        }

        public static bool IsBinaryString(string input)
        {
            // Define the regular expression pattern
            string pattern = @"^[01]*b$";

            // Use Regex.IsMatch to check if the input matches the pattern
            return Regex.IsMatch(input, pattern);
        }

        public static ulong ConvertBinaryToUInt64(string binaryString)
        {
            // Convert binary string to ulong
            ulong result = Convert.ToUInt64(binaryString, 2);
            return result;
        }
        public static uint ConvertBinaryToUInt32(string binaryString)
        {
            // Convert binary string to ulong
            uint result = Convert.ToUInt32(binaryString, 2);
            return result;
        }
    }

   

}
