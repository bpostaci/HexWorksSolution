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
    }
}
