﻿using HexWorks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestMemoryAddress
{
    [TestClass]
    public class UnitTest_Hex64
    {
        [TestMethod]

        public void Test_Assignments()
        {
            Hex64 address1 = "ffffb281e6565840";
            Hex64 address2 = "0xffffb281e6565840";
            Hex64 address3 = "ffffb281`e6565840";
            

            Assert.IsTrue(18446658869717784640 == address1.Value);
            Assert.IsTrue(18446658869717784640 == address2.Value);
            Assert.IsTrue(18446658869717784640 == address3.Value);
            

            Assert.IsTrue(address1 == address2);
            Assert.IsTrue(address2 == address3);
            Assert.IsTrue(address3 == address1);

            Assert.IsTrue(address1.ToHexString() == "ffffb281e6565840");
            Assert.IsTrue(address1.ToHexString(true, false) == "0xffffb281e6565840");

        }

        [TestMethod] public void Test_Create_From_BinaryString()
        {
            string validBinaryString = "10100101b";
            Hex64 result = Hex64.FromBinaryString(validBinaryString);
            Assert.IsNotNull(result);
            Assert.IsTrue(165 == result.Value);

            string invalid = "10330101b";
            Assert.ThrowsException<ArgumentException>(() =>
            {
                Hex64 test = Hex64.FromBinaryString(invalid);
            });

            string invalid2 = "asdadsb";
            Assert.ThrowsException<ArgumentException>(() =>
            {
                Hex64 test = Hex64.FromBinaryString(invalid2);
            });

            string validLengthBinaryString   = "1000100010001000100010001000100010001000100010001000100010001000b";
            Hex64 result2 = Hex64.FromBinaryString(validLengthBinaryString);
            Assert.IsNotNull(result2);


            string invalidLengthBinaryString = "10001000100010001000100010001000100010001000100010001000100010001b";
            Assert.ThrowsException<ArgumentException>(() =>
            {
                Hex64 test = Hex64.FromBinaryString(invalidLengthBinaryString);
            });


        }

        [TestMethod]
        public void Test_Different_Initializations()
        {
            Hex64 address1 = "ffffb281e6565840";
            Hex64 address2 = "0xffffb281e6565840";
            Hex64 address3 = "ffffb281`e6565840";
            Hex64 address4 = 0xffffb281e6565840;

            Assert.IsTrue(18446658869717784640 == address1.Value);
            Assert.IsTrue(18446658869717784640 == address2.Value);
            Assert.IsTrue(18446658869717784640 == address3.Value);
            Assert.IsTrue(18446658869717784640 == address4.Value);
        }

        [TestMethod]
        public void Test_String_vs_Number()
        {
            Hex64 address1 = 345;
            Hex64 address2 = "345";
            Hex64 address3 = 0x345;

            Assert.IsTrue(address1 != address2);
            Assert.IsTrue(address2 == address3);
        }


        [TestMethod]
        public void Test_Numbers()
        {
            Hex64 address1 = -1L;
            Assert.IsTrue(address1.ToHexString(false, true) == "FFFFFFFFFFFFFFFF");

            Hex64 address2 = "0xFFFFFFFFFFFFFFFF";
            Assert.IsTrue(address2 + 1L == 0);

            Hex64 address3 = Int64.MinValue;
            Assert.IsTrue(address3.ToHexString(false, true) == "8000000000000000");

            Hex64 address4 = Int64.MaxValue;
            Assert.IsTrue(address4.ToHexString(false, true) == "7FFFFFFFFFFFFFFF");

        }

        [TestMethod]

        public void Test_not_a_hex_string_initialization()
        {

            Assert.ThrowsException<ArgumentException>(() =>
            {
                Hex64 p = new Hex64("abg");
            });

        }

        [TestMethod]

        public void Test_hex_huge_string_initialization()
        {

            Assert.ThrowsException<ArgumentException>(() =>
            {
                Hex64 p = "0x1FFFFCCCC11114444";
            });

        }

        [TestMethod]
        public void Test_High_Low_bits()
        {
            Hex64 a1 = "0xFFEECCBB11223344";
            var high = a1.HighBytes();
            var low = a1.LowBytes();

            Assert.IsTrue(high == 0xffeeccbb);
            Assert.IsTrue(low == 0x11223344);

        }

        [TestMethod]

        public void Test_Reference_Equality()
        {
            Hex64 address1 = "ffffb281e6565840";
            Hex64 address2 = "0xffffb281e6565840";
            Hex64 address3 = "ffffb281`e6565840";
            

            Assert.IsTrue(address1 == address2);
            Assert.IsTrue(address2 == address3);
            Assert.IsTrue(address3 == address1);
        }

        [TestMethod]
        public void Test_Hex64_ValidInput_ConvertsCorrectly()
        {
            byte[] validByteArray = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            Hex64 address = new Hex64(validByteArray, false);

            Assert.IsTrue(0x123456789ABCDEF0 == address.Value);

            //Be Careful the little-endian is the default !!!
            byte[] validByteArray2 = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            Hex64 address2 = new Hex64(validByteArray2, true);

            Assert.IsTrue(0xF0DEBC9A78563412 == address2.Value);
        }

        [TestMethod]
        public void Test_Hex64_InvalidInput_ThrowsArgumentException()
        {
            byte[] invalidByteArray = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0xFF };
            Assert.ThrowsException<ArgumentException>(() => new Hex64(invalidByteArray));

            byte[] invalidByteArray2 = new byte[] { 0x12, 0x34 };
            Assert.ThrowsException<ArgumentException>(() => new Hex64(invalidByteArray2));

        }

        [TestMethod]
        public void Test_Sum_Substract()
        {
            Hex64 a1 = "0xffff";
            Hex64 a2 = "0x1";

            var result = a1 + a2;
            Assert.IsTrue(result == "0x10000");

            var result2 = a1 - a2;
            Assert.IsTrue(result2 == "0xfffe");

            var res3 = a1 + 0x100;
            Assert.IsTrue(res3 == 0x100ff);

            var res4 = a1 - 0x100;
            Assert.IsTrue(res4 == 0xfeff);

            var res5 = a2 - a1;


        }

        [TestMethod]
        public void Test_Multiply()
        {
            Hex64 a1 = "0xffffffffffffffff";
            Hex64 a2 = "0xffff";

            var result = a1 * a2;
            //Allows Overflow.
            Assert.IsTrue(0xffffffffffff0001 == result);
         }

        [TestMethod]
        public void Test_OperatorOverloads()
        {
            Hex64 address1 = new Hex64(10);
            Hex64 address2 = new Hex64(5);
            
            UInt32 value = 2;

            Hex64 result;

            // Addition operator
            result = address1 + address2;
            Assert.IsTrue(result.Value == 15);

            result = address1 + value;
            Assert.IsTrue(result.Value == 12);

            // Subtraction operator
            result = address1 - address2;
            Assert.IsTrue(result.Value == 5);

            result = address1 - value;
            Assert.IsTrue(result.Value == 8);

            // Bitwise AND operator
            result = address1 & address2;
            Assert.IsTrue(result.Value == 0);

            // Bitwise OR operator
            result = address1 | address2;
            Assert.IsTrue(result.Value == 15);

            // Bitwise XOR operator
            result = address1 ^ address2;
            Assert.IsTrue(result.Value == 15);

            // Left shift operator
            result = address1 << 2;
            Assert.IsTrue(result.Value == 40);

            // Right shift operator
            result = address1 >> 2;
            Assert.IsTrue(result.Value == 2);

            // Bitwise NOT operator
            result = ~address1;
            Assert.IsTrue(result.Value == 18446744073709551605);

            // Decrement operator
            Assert.ThrowsException<NotSupportedException>(() =>
            {
                result = --address1;
                Assert.IsTrue(result.Value == 9);

            });

            // Increment operator
            Assert.ThrowsException<NotSupportedException>(() =>
            {
                result = ++address1;
                Assert.IsTrue(result.Value == 10);
            });
        }


        [TestMethod]
        public void Test_Arithmetics()
        {
            Hex64 rax = new Hex64(0x100);
            Hex64 rbx = new Hex64(0x200);

            Assert.AreEqual(0x300, rax + rbx); // SUM
            Assert.AreEqual(0x100, rbx - rax); // SUB
            Assert.AreEqual(0x10000, rax * rax); // MUL

            Hex64 text_rax = ~rax;  //NOT
            Assert.AreEqual(0xFFFFFFFFFFFFFEFF, text_rax);


            rax = Hex64.FromBinaryString("1010b");
            rbx = Hex64.FromBinaryString("101b");


            Assert.AreEqual(0x0000, rax & rbx); // AND
            Assert.AreEqual(0xF, rax | rbx); // OR

            rax = Hex64.FromBinaryString("1010b");
            rbx = Hex64.FromBinaryString("1101b");

            Assert.AreEqual(0x7, rax ^ rbx); // XOR

            rax = 0xFF11;
            rbx = 0x11FF;
             
            var result = ~(rbx & rax);
            Assert.AreEqual(0xffffffffffffeeee, result);

            var t2 = rbx.NAND(rax);
            Assert.AreEqual(t2, result);

            result = ~(rbx ^ rax);
            t2 = rbx.XNOR(rax);
            Assert.AreEqual(0xFFFFFFFFFFFF1111, result);
            Assert.AreEqual(t2, result);

            result = ~(rbx | rax);
            t2 = rbx.NOR(rax);
            Assert.AreEqual(t2, result);

            /*
            Assert.AreEqual(0xFFFFFFFFFFFFFDFF, ~rbx); // NOT
            Assert.AreEqual(0xFFFFF800000002FF, ~(rbx & rax)); // NAND
            Assert.AreEqual(0xFFFFFDFFFFFFFFFDFF, ~(rax ^ rbx)); // XNOR
            */
        }

        [TestMethod]
        public void Test_ShiftOperations()
        {
            Hex64 result = 1UL << 63; // SHIFT_LEFT
            Assert.AreEqual(0x8000000000000000, result);
            result = result >> 32; // SHIFT_RIGHT
            Assert.AreEqual(0x0000000080000000, result);
        }

        [TestMethod]
        public void Test_OverFlow()
        {
            Hex64 address1 = "ffffffffffffffff";
            var p = address1 + 0x1;

            Assert.IsTrue(p == 0x0);

        }
    }
}
