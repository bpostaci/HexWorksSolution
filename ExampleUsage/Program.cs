// This project has generating to expose features of MemoryAddress object. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HexWorks;


//Project-> Properties->  "Conditional compilation symbols"
#if X64 
    using MemoryAddress = HexWorks.Hex64;
#else
    using MemoryAddress = HexWorks.MemoryAddress32;
#endif 

using ma64 = HexWorks.Hex64; 



namespace ExampleUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This project has generating to expose features of Hex object.");
            Console.WriteLine();

            /*Example Of Assignments*/

            ma64 p1 = 0xFFAAEEEE99887766;     //Hex Number Assignment
            ma64 p2 = "0xFFAAEEEE99887766";   //Hex String Assignment
            ma64 p3 = "FFAAEEEE`99887766";    //Hex String Assignment
            ma64 p4 = "FFAAEEEE99887766";
            ma64 p5 = ma64.FromBinaryString("1011b");                //from Binary. 
            ma64 p6 = ma64.FromHexString("0xFFAAEEEE99887766");



            ma64 a1 = 840;   //-> Decimal Assingment
            ma64 a2 = "840"; // -> Hexedecimal Assignment
            if (a1 != a2) Console.WriteLine($"{a1.ToHexString()} not equals {a2.ToHexString()}");
            /* OUTPUT 
             0000000000000348 not equals 0000000000000840
            */

            //Array Assignment (Must be 8 byte for Hex64) 
            //Array Assignment (Must be 4 byte for Hex32) 
            ma64 arr1 = new ma64(new byte[] { 0x12, 0x34, 0x56, 0x78, 0xAB, 0xBC, 0xDE, 0xF0 });
            PrintAddress(arr1);

            /* OUTPUT
                Prefix            :0xf0debcab78563412
                Prefix+Capital    :0xF0DEBCAB78563412
                Plain             :f0debcab78563412
                Only Capital      :F0DEBCAB78563412
                Signed Hex        :-0xF21435487A9CBEE
                Signed Hex(notrim):-0x0F21435487A9CBEE
                Bits              :1111000011011110101111001010101101111000010101100011010000010010
                Bits Formated     :11110000.11011110.10111100.10101011.01111000.01010110.00110100.00010010
                Bits Formated2    :11110000-11011110-10111100-10101011-01111000-01010110-00110100-00010010
                High Bytes        :00000000f0debcab
                Low  Bytes        :0000000078563412
             */

            //LittleEndian (Default = true) 
            ma64 arr2 = new ma64(new byte[] { 0x12, 0x34, 0x56, 0x78, 0xAB, 0xBC, 0xDE, 0xF0 },false );
            PrintAddress(arr2);

             /* OUTPUT
                Prefix            :0x12345678abbcdef0
                Prefix+Capital    :0x12345678ABBCDEF0
                Plain             :12345678abbcdef0
                Only Capital      :12345678ABBCDEF0
                Signed Hex        :+0x12345678ABBCDEF0
                Signed Hex(notrim):+0x12345678ABBCDEF0
                Bits              :0001001000110100010101100111100010101011101111001101111011110000
                Bits Formated     :00010010.00110100.01010110.01111000.10101011.10111100.11011110.11110000
                Bits Formated2    :00010010-00110100-01010110-01111000-10101011-10111100-11011110-11110000
                High Bytes        :0000000012345678
                Low  Bytes        :00000000abbcdef0 

                */


            /* ALLOCATE AN ADDRESS THAT WE USE */
            Process proc = Process.GetCurrentProcess();
            IntPtr startOffset = proc.MainModule.BaseAddress;

            /* Example Of Offseting */
            ma64 address1 = startOffset;
            PrintAddress(address1);

            ma64 offset = 0x1000;
            var nextAddress = address1 + offset;
            PrintAddress(nextAddress);


            for (int i = 0; i < 4; i++)
            {
                nextAddress = address1 + (offset * (i * -8));
                //OR
                nextAddress = address1.Offset((i * -8) * 0x1000); 


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

            ma64 rax = 0x100;
            ma64 rbx = 0x200;

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

            ma64 flag = 0xF;

            Console.WriteLine("FLAG: " +  flag.ToBits(8,"-"));
            ma64 nextFlag = 0; 
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


            /*Set 32 bits of "1" */
            for(int i = 0; i< 32; i++)
            {
                flag = flag.SetBit(i, 1); 
            }
            Console.WriteLine("FLAG: " + flag.ToBits(8, "-"));
            
            
            flag = flag.GetBaseAddress(4);
            Console.WriteLine("FLAG: " + flag.ToBits(8, "-"));
            /*
               ADDRESS      : 00000000-00000000-00000000-00000000-11111111-11111111-11111111-11111111
               BASE ADDRESS : 00000000-00000000-00000000-00000000-11111111-11111111-11111111-11110000
             */


            /* FLAG ENUMS */

            ma64 eflags_register = GetEflagRegister(); 
                
            /*test flags*/
            if( eflags_register.TestBit((uint) EFlags.CF) ) Console.WriteLine("Carry Flag Present" );
            if (eflags_register.TestBit((uint) EFlags.ZF)) Console.WriteLine("Zero Flag Present");


            Console.WriteLine("End of Demo"); 
            Console.ReadLine(); 
        }

        static void PrintAddress(ma64 address1)
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
             
                Prefix            :0x12345678abbcdef0
                Prefix+Capital    :0x12345678ABBCDEF0
                Plain             :12345678abbcdef0
                Only Capital      :12345678ABBCDEF0
                Signed Hex        :+0x12345678ABBCDEF0
                Signed Hex(notrim):+0x12345678ABBCDEF0
                Bits              :0001001000110100010101100111100010101011101111001101111011110000
                Bits Formated     :00010010.00110100.01010110.01111000.10101011.10111100.11011110.11110000
                Bits Formated2    :00010010-00110100-01010110-01111000-10101011-10111100-11011110-11110000
                High Bytes        :0000000012345678
                Low  Bytes        :00000000abbcdef0 

            */
        }

        static ma64 GetEflagRegister()
        {
            return new ma64((ulong)(EFlags.CF | EFlags.ZF | EFlags.PF)); 
        }
    }
}
