using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexWorks;


//Project-> Properties->  "Conditional compilation symbols"
#if X64 
    using MemoryAddress = HexWorks.MemoryAddress64;
#else
    using MemoryAddress = HexWorks.MemoryAddress32;
#endif 

namespace ExampleUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            Process proc = Process.GetCurrentProcess();
            IntPtr startOffset = proc.MainModule.BaseAddress;

            MemoryAddress address1 = startOffset;

            PrintAddress(address1);

            MemoryAddress offset = 0x1000;

            var nextAddress = address1 + offset;
            
            PrintAddress(nextAddress);

            /*
            MemoryAddress p1 = 0xFFAAEEEE99887766;
            MemoryAddress p2 = "0xFFAAEEEE99887766";
            MemoryAddress p3 = "FFAAEEEE`99887766";

            if (p1 == p3) Console.WriteLine("yes they are equal");
            */


            Console.ReadLine(); 
        }

        static void PrintAddress(MemoryAddress address1)
        {
            Console.WriteLine("");
            Console.WriteLine($"Prefix            :{address1.ToHexString(true, false)} ");
            Console.WriteLine($"Prefix+Capital    :{address1.ToHexString(true, true)} ");
            Console.WriteLine($"Plain             :{address1.ToHexString(false, false)} ");
            Console.WriteLine($"Only Capital      :{address1.ToHexString(false, true)} ");
            Console.WriteLine($"Bits              :{address1.ToBits()} ");
            Console.WriteLine($"Bits Formated     :{Hex.FormatWithSeperator(address1.ToBits(), ".", 8)} ");
            Console.WriteLine($"High Bytes        :{address1.HighBytes().ToHexString()}");
            Console.WriteLine($"Low  Bytes        :{address1.LowBytes().ToHexString()}");
        }
    }
}
