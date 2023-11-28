using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexWorks;
using HexWorks.Extensions; 

using MemoryAddress64 = HexWorks.Hex64;

namespace TestHexWorks
{
    class Program
    {
        static void Main(string[] args)
        {

            Hex64 rax = 0xFF11;
            Hex64 rbx = 0x11FF;


            var result = ~(rbx ^ rax);
            var t2 = rbx.XNOR(rax); 
            
            PrintAddress(result);
            PrintAddress(t2);


            Console.WriteLine(result.ToBits(8, "-"));

            result = rax | rbx;

            Console.WriteLine(result.ToBits(8, "-"));

            byte[] validByteArray = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            Hex32 address = new Hex32(validByteArray, false);
            Console.WriteLine(address.ToHexString());
            byte[] validByteArray2 = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            Hex32 a2 = new Hex32(validByteArray2);
            Console.WriteLine(a2.ToHexString());

            PteEntry64 e = new PteEntry64(0x800000041BFB4963);
            Console.WriteLine(e.ToBits());
            MemoryAddress64 f = (ulong)e.Flags;
            Console.WriteLine(f.ToBits());

            Console.WriteLine(e.GetFlagsAsString());



            MemoryAddress64 address1 = 0xffffba88921845a0;

            Console.WriteLine($"Prefix            :{address1.ToHexString(true, false)} ");
            Console.WriteLine($"Prefix+Capital    :{address1.ToHexString(true, true)} ");
            Console.WriteLine($"Plain             :{address1.ToHexString(false, false)} ");
            Console.WriteLine($"Only Capital      :{address1.ToHexString(false, true)} ");
            Console.WriteLine($"Bits              :{address1.ToBits()} ");
            Console.WriteLine($"Bits Formated     :{Hex.FormatWithSeperator(address1.ToBits(), ".", 8)} ");
            Console.WriteLine($"Bits Formated2    :{address1.ToBits(8, "-")}");


            var t = address1.ToggleBit(15);
            Console.WriteLine($"Bits Formated2    :{t.ToBits(8, "-")}");
            /*OUTPUT: 
             * Prefix            :0xffffba88921845a0
               Prefix+Capital    :0xFFFFBA88921845A0
               Plain             :ffffba88921845a0
               Only Capital      :FFFFBA88921845A0
             */



            Console.WriteLine("TEST 32 BIT");

            Hex32 address2 = 0xFFFF0000;
            Console.WriteLine($"Prefix            :{address2.ToHexString(true, false)} ");
            Console.WriteLine($"Prefix+Capital    :{address2.ToHexString(true, true)} ");
            Console.WriteLine($"Plain             :{address2.ToHexString(false, false)} ");
            Console.WriteLine($"Only Capital      :{address2.ToHexString(false, true)} ");
            Console.WriteLine($"Bits              :{address2.ToBits()} ");
            Console.WriteLine($"Bits Formated     :{Hex.FormatWithSeperator(address2.ToBits(), ".", 8)} ");
            Console.WriteLine($"Bits Formated2    :{address2.ToBits(8, "-")}");

            Console.ReadLine();

        }

        static void PrintAddress(Hex64 address1)
        {
            Console.WriteLine("");
            Console.WriteLine($"Prefix            :{address1.ToHexString(true, false)} ");
            Console.WriteLine($"Prefix+Capital    :{address1.ToHexString(true, true)} ");
            Console.WriteLine($"Plain             :{address1.ToHexString(false, false)} ");
            Console.WriteLine($"Only Capital      :{address1.ToHexString(false, true)} ");
            Console.WriteLine($"Signed Hex        :{address1.ToSignedHexString(true)}");
            Console.WriteLine($"Signed Hex(notrim):{address1.ToSignedHexString(true, false)}");
            Console.WriteLine($"Bits              :{address1.ToBits()} ");
            Console.WriteLine($"Bits Formated     :{Hex.FormatWithSeperator(address1.ToBits(), ".", 8)} ");
            Console.WriteLine($"Bits Formated2    :{address1.ToBits(8, "-")}");
            Console.WriteLine($"High Bytes        :{address1.HighBytes().ToHexString()}");
            Console.WriteLine($"Low  Bytes        :{address1.LowBytes().ToHexString()}");
        }
    }

   
}