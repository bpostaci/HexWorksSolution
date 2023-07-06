using HexWorks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestMemoryAddress
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        
        public void TestMethod1()
        {
            MemoryAddress64 address1 = "ffffb281e6565840";
            MemoryAddress64 address2 = "0xffffb281e6565840";
            MemoryAddress64 address3 = "ffffb281`e6565840";


            Assert.IsTrue(18446658869717784640 == address1.Value);
            Assert.IsTrue(18446658869717784640 == address2.Value);
            Assert.IsTrue(18446658869717784640 == address3.Value);

            Assert.IsTrue(address1 == address2);
            Assert.IsTrue(address2 == address3);
            Assert.IsTrue(address3 == address1);

            Assert.IsTrue(address1.ToHexString() == "ffffb281e6565840");
            Assert.IsTrue(address1.ToHexString(true,false) == "0xffffb281e6565840");

        }

        [TestMethod]
        public void Test_Different_Initializations()
        {
            MemoryAddress64 address1 = "ffffb281e6565840";
            MemoryAddress64 address2 = "0xffffb281e6565840";
            MemoryAddress64 address3 = "ffffb281`e6565840";
            MemoryAddress64 address4 = 0xffffb281e6565840;

            Assert.IsTrue(18446658869717784640 == address1.Value);
            Assert.IsTrue(18446658869717784640 == address2.Value);
            Assert.IsTrue(18446658869717784640 == address3.Value);
            Assert.IsTrue(18446658869717784640 == address4.Value);
        }

        [TestMethod]
        
        public void Test_not_a_hex_string_initialization()
        {
         
            Assert.ThrowsException<ArgumentException>(() =>
            {
                MemoryAddress64 p = new MemoryAddress64("abg");
            });

        }

        [TestMethod]

        public void Test_hex_huge_string_initialization()
        {

            Assert.ThrowsException<ArgumentException>(() =>
            {
                MemoryAddress64 p = "0x1FFFFCCCC11114444";
            });

        }

        [TestMethod]
        public void Test_High_Low_bits()
        {
            MemoryAddress64 a1 = "0xFFEECCBB11223344";
            var high = a1.HighBytes();
            var low = a1.LowBytes();

            Assert.IsTrue(high == 0xffeeccbb);
            Assert.IsTrue(low  == 0x11223344);

        }

        [TestMethod]

        public void Test_Reference_Equality()
        {
            MemoryAddress64 address1 = "ffffb281e6565840";
            MemoryAddress64 address2 = "0xffffb281e6565840";
            MemoryAddress64 address3 = "ffffb281`e6565840";

            Assert.IsTrue(address1 == address2);
            Assert.IsTrue(address2 == address3);
            Assert.IsTrue(address3 == address1);
        }

        [TestMethod]
        public void MemoryAddress64_ValidInput_ConvertsCorrectly()
        {
            byte[] validByteArray = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            MemoryAddress64 address = new MemoryAddress64(validByteArray, false);

            Assert.IsTrue(0x123456789ABCDEF0 == address.Value);

            //Be Careful the little-endian is the default !!!
            byte[] validByteArray2 = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            MemoryAddress64 address2 = new MemoryAddress64(validByteArray2, true);

            Assert.IsTrue(0xF0DEBC9A78563412 == address2.Value);
        }

        [TestMethod]
        public void MemoryAddress64_InvalidInput_ThrowsArgumentException()
        {
            byte[] invalidByteArray = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0xFF };
            Assert.ThrowsException<ArgumentException>(() => new MemoryAddress64(invalidByteArray));

            byte[] invalidByteArray2 = new byte[] { 0x12, 0x34 };
            Assert.ThrowsException<ArgumentException>(() => new MemoryAddress64(invalidByteArray2));

        }

        [TestMethod]
        public void Test_Sum_Substract()
        {
            MemoryAddress64 a1 = "0xffff";
            MemoryAddress64 a2 = "0x1";

            var result = a1 + a2;
            Assert.IsTrue(result == "0x10000");

            var result2 = a1 - a2;
            Assert.IsTrue(result2 == "0xfffe");

            var res3 = a1 + 0x100;
            Assert.IsTrue(res3 == 0x100ff);

            var res4 = a1 - 0x100;
            Assert.IsTrue(res4 == 0xfeff); 

        }

        [TestMethod]
        public void OperatorOverloads_Test()
        {
            MemoryAddress64 address1 = new MemoryAddress64(10);
            MemoryAddress64 address2 = new MemoryAddress64(5);
            UInt32 value = 2;

            MemoryAddress64 result;

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
            result = --address1;
            Assert.IsTrue(result.Value == 9);

            // Increment operator
            result = ++address1;
            Assert.IsTrue(result.Value == 10);
        }
    }
}
