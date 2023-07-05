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
