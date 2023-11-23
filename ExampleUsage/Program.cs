
// This project has generating to expose features of MemoryAddress object. 


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            Console.WriteLine("This project has generating to expose features of MemoryAddress object.");
            Console.WriteLine();

            /* ALLOCATE AN ADDRESS THAT WE USE */
            Process proc = Process.GetCurrentProcess();
            IntPtr startOffset = proc.MainModule.BaseAddress;


            /*Example Of Assignments*/

            MemoryAddress p1 = 0xFFAAEEEE99887766;     //Hex Number Assignment
            MemoryAddress p2 = "0xFFAAEEEE99887766";   //Hex String Assignment
            MemoryAddress p3 = "FFAAEEEE`99887766";    //Hex String Assignment

            MemoryAddress a1 = 840;   //-> Decimal Assingment
            MemoryAddress a2 = "840"; // -> Hexedecimal Assignment
            if(a1 != a2 ) Console.WriteLine($"{a1.ToHexString()} not equals {a2.ToHexString() }");

            //Array Assignment (Must be 8 byte for MemoryAddress64) 
            MemoryAddress arr1 = new MemoryAddress(new byte[] { 0x12, 0x34, 0x56, 0x78, 0xAB, 0xBC, 0xDE, 0xF0 });


            /* Example Of Offseting */
            MemoryAddress address1 = startOffset;
            PrintAddress(address1);
            MemoryAddress offset = 0x1000;
            var nextAddress = address1 + offset;
            PrintAddress(nextAddress);


            for (int i = 0; i < 4; i++)
            {
                nextAddress = address1 + (offset * (i * -8));
                Console.WriteLine($"Offset { (offset * (i * -8)).ToSignedHexString(true,false) } => { nextAddress.ToHexString()}");
            }
            /*
             * ( ToSignedHexString no_trim ) 
             Offset +0x0000000000000000 => 000001c852e50000
             Offset -0x0000000000008000 => 000001c852e48000
             Offset -0x0000000000010000 => 000001c852e40000
             Offset -0x0000000000018000 => 000001c852e38000
             
             (Defult ToSignedHexString trim = true; ) 
             Offset +0x0 => 000001a33a290000
             Offset -0x8000 => 000001a33a288000
             Offset -0x10000 => 000001a33a280000
             Offset -0x18000 => 000001a33a278000

             */


            /* BITWISE OPERATINS */

            MemoryAddress rax = 0x100;
            MemoryAddress rbx = 0x200;

            /*Artimetichs*/
            var result = rax + rbx; // SUM 
            result = rax - rbx;
            result = rax * rax;  // May create Overflow exception if exceeded ulong !!!

            result = rax++;  // !! Be Careful it is an immutable type actual value doesn't changes !!!
            result = rbx--;  // !! Be Careful it is an immutable type actual value doesn't changes !!!

            /*Logic*/
            result = rax & rbx; //AND
            result = rbx ^ rbx; //XOR 
            result = rax | rbx; //OR
            result = ~rbx;        //NOT
            result = ~(rbx & rax); //NAND
            result = ~(rax ^ rbx); //XNOR

            result = rbx.NAND(rax);
            result = rbx.XNOR(rax);
            result = rbx.NOR(rax);

            /*Shifts*/
            result = 1UL << 63;     //0x1000000000000000
            result = result >> 32;  //0x0000000010000000


            /*Bit Operations*/
            Console.WriteLine();
            Console.WriteLine("Bit Operations");

            MemoryAddress flag = 0xF;

            Console.WriteLine("FLAG: " +  flag.ToBits(8,"-"));
            MemoryAddress nextFlag = 0; 
            flag = flag.SetBit(0, 0); // make last bit 0 => 0xE
            Console.WriteLine("FLAG: " + flag.ToBits(8, "-"));
            flag = flag.ToggleBit(3); // makes 4th bit 0 => 0x6 (note: bits starts with index 0 ) 
            Console.WriteLine("FLAG: " + flag.ToBits(8, "-"));

            for(int i = 0; i< 4; i++)
            {
                Console.WriteLine($"Bit{i}: {flag.GetBit(i)}");
            }
            /*
             Bit Operations
                FLAG: 00000000-00000000-00000000-00000000-00000000-00000000-00000000-00001111
                FLAG: 00000000-00000000-00000000-00000000-00000000-00000000-00000000-00001110
                FLAG: 00000000-00000000-00000000-00000000-00000000-00000000-00000000-00000110
                Bit0: 0
                Bit1: 1
                Bit2: 1
                Bit3: 0
             */

            for(int i = 0; i< 32; i++)
            {
                flag = flag.SetBit(i, 1); 
            }
            Console.WriteLine("FLAG: " + flag.ToBits(8, "-"));
            
            
            flag = flag.ClearEndBits(4);
            Console.WriteLine("FLAG: " + flag.ToBits(8, "-"));
            /*
               FLAG: 00000000-00000000-00000000-00000000-11111111-11111111-11111111-11111111
               FLAG: 00000000-00000000-00000000-00000000-11111111-11111111-11111111-11110000
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
            Console.WriteLine($"Signed Hex        :{address1.ToSignedHexString(true)}");
            Console.WriteLine($"Signed Hex(notrim):{address1.ToSignedHexString(true,false)}");
            Console.WriteLine($"Bits              :{address1.ToBits()} ");
            Console.WriteLine($"Bits Formated     :{Hex.FormatWithSeperator(address1.ToBits(), ".", 8)} ");
            Console.WriteLine($"Bits Formated2    :{address1.ToBits(8, "-")}");
            Console.WriteLine($"High Bytes        :{address1.HighBytes().ToHexString()}");
            Console.WriteLine($"Low  Bytes        :{address1.LowBytes().ToHexString()}");

            /* OUTPUT
                Prefix            :0x0000026c28af1000
                Prefix + Capital  :0x0000026C28AF1000
                Plain             :0000026c28af1000
                Only Capital      :0000026C28AF1000
                Bits              :0000000000000000000000100110110000101000101011110001000000000000
                Bits Formated     :00000000.00000000.00000010.01101100.00101000.10101111.00010000.00000000
                Bits Formated2    :00000000-00000000-00000010-01101100-00101000-10101111-00010000-00000000
                High Bytes        :000000000000026c
                Low  Bytes        :0000000028af1000
            */
        }
    }
}
