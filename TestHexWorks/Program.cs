using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexWorks; 

namespace TestHexWorks
{
    class Program
    {
        static void Main(string[] args)
        {

            MemoryAddress32 test = "0x1234";
            var arr = test.ToByteArray();


            byte[] validByteArray = new byte[] { 0x12, 0x34 };
            MemoryAddress32 address = new MemoryAddress32(validByteArray,false);
            Console.WriteLine( address.ToHexString());
            byte[] validByteArray2 = new byte[] { 0x12, 0x34, };
            MemoryAddress32 a2 = new MemoryAddress32(validByteArray2);
            Console.WriteLine(a2.ToHexString());

            PteEntry64 e = new PteEntry64(0x800000041BFB4963);
            Console.WriteLine(e.ToBits());
            MemoryAddress64 f = (ulong)e.Flags; 
            Console.WriteLine(f.ToBits());

            Console.WriteLine(e.GetFlagsAsString());



            MemoryAddress64 address1 = 0xffffba88921845a0;
            
            Console.WriteLine($"Prefix            :{address1.ToHexString(true,false)} ");
            Console.WriteLine($"Prefix+Capital    :{address1.ToHexString(true, true)} ");
            Console.WriteLine($"Plain             :{address1.ToHexString(false, false)} ");
            Console.WriteLine($"Only Capital      :{address1.ToHexString(false, true)} ");
            Console.WriteLine($"Bits              :{address1.ToBits()} ");
            Console.WriteLine($"Bits Formated     :{Hex.FormatWithSeperator(address1.ToBits(),".",8)} ");

            /*OUTPUT: 
             * Prefix            :0xffffba88921845a0
               Prefix+Capital    :0xFFFFBA88921845A0
               Plain             :ffffba88921845a0
               Only Capital      :FFFFBA88921845A0
             */

            Console.WriteLine("TEST 32 BIT");

            MemoryAddress32 address2 = 0xFFFF0000;
            Console.WriteLine($"Prefix            :{address2.ToHexString(true, false)} ");
            Console.WriteLine($"Prefix+Capital    :{address2.ToHexString(true, true)} ");
            Console.WriteLine($"Plain             :{address2.ToHexString(false, false)} ");
            Console.WriteLine($"Only Capital      :{address2.ToHexString(false, true)} ");
            Console.WriteLine($"Bits              :{address2.ToBits()} ");
            Console.WriteLine($"Bits Formated     :{Hex.FormatWithSeperator(address2.ToBits(), ".", 8)} ");

            Console.ReadLine(); 

        }

       
    }
}
