using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using HexWorks;

namespace UnitTestMemoryAddress
{
    /// <summary>
    /// Summary description for UnitTest_MemoryAddress32
    /// </summary>
    [TestClass]
    public class UnitTest_MemoryAddress32
    {
        public UnitTest_MemoryAddress32()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Test_Initialization()
        {
            MemoryAddress32 a1 = 0xFFEE9922;
            MemoryAddress32 a2 = "0xFFEE9922";
            MemoryAddress32 offset1 = 0x1;



            Assert.ThrowsException<ArgumentException>(() =>
            {
                MemoryAddress32 a3 = "0xFFEEDDCC00112244";
            });
        }
        [TestMethod]
        public void MemoryAddress32_ValidInput_ConvertsCorrectly()
        {
            byte[] validByteArray = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            MemoryAddress32 address = new MemoryAddress32(validByteArray,false);

            Assert.IsTrue(0x12345678 == address.Value);

            byte[] validByteArray2 = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            MemoryAddress32 address2 = new MemoryAddress32(validByteArray2, true);

            Assert.IsTrue(0x78563412 == address2.Value);
        }

        [TestMethod]
        public void MemoryAddress32_InvalidInput_ThrowsArgumentException()
        {
            byte[] invalidByteArray = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A };
            Assert.ThrowsException<ArgumentException>(() => new MemoryAddress32(invalidByteArray));

            byte[] invalidByteArray2 = new byte[] { 0x12, 0x34 };
            Assert.ThrowsException<ArgumentException>(() => new MemoryAddress32(invalidByteArray2));

        }

        [TestMethod]
        public void Test_High_Low_bits()
        {
            MemoryAddress32 a1 = 0xFFEE9922;
            var high = a1.HighBytes();
            var low = a1.LowBytes();

            Assert.IsTrue(high == 0xffee);
            Assert.IsTrue(low == 0x9922); 

        }

        [TestMethod]

        public void Test_Reference_Equality()
        {
            MemoryAddress32 address1 = "ffffb281";
            MemoryAddress32 address2 = "0xffffb281";
            MemoryAddress32 address3 = 0xffffb281;

            Assert.IsTrue(address1 == address2);
            Assert.IsTrue(address2 == address3);
            Assert.IsTrue(address3 == address1);
        }

        [TestMethod]
        public void Test_OverFlow()
        {
            MemoryAddress32 address1 = "ffffffff";
            var p = address1 + 0x1;

            Assert.IsTrue(p == 0x0); 

        }

        [TestMethod]
        public void Test_Underflow()
        {
            MemoryAddress32 address1 = "0x00000000";
            var p = address1 - 0x1;

            Assert.IsTrue(p == 0xFFFFFFFF);

        }

        [TestMethod]
        public void Test_Sum_Substract()
        {
            MemoryAddress32 a1 = "0xffff";
            MemoryAddress32 a2 = "0x1";

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
            MemoryAddress32 address1 = new MemoryAddress32(10);
            MemoryAddress32 address2 = new MemoryAddress32(5);
            UInt32 value = 2;

            MemoryAddress32 result;

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
            Assert.IsTrue(result.Value == 4294967285);

            // Decrement operator
            result = --address1;
            Assert.IsTrue(result.Value == 9);

            // Increment operator
            result = ++address1;
            Assert.IsTrue(result.Value == 10);
        }
        [TestMethod]
        public void Test_Numbers()
        {
            MemoryAddress32 address1 = -1;
            Assert.IsTrue(address1.ToHexString(false, true) == "FFFFFFFF");

            MemoryAddress32 address2 = "0xFFFFFFFF";
            Assert.IsTrue(address2 + 1 == 0);

            MemoryAddress32 address3 = Int32.MinValue;
            Assert.IsTrue(address3.ToHexString(false, true) == "80000000");

            MemoryAddress32 address4 = Int32.MaxValue;
            Assert.IsTrue(address4.ToHexString(false, true) == "7FFFFFFF");

        }
    }
}
