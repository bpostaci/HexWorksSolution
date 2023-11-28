# Hex data value object implementation

-  Contians 2 Hex data class for 64 bit use Hex64 and for 32bit use Hex32 class
-  Get rid off primitive obsession for using "ulong" or "uint" for hex data types.
-  Useful for debugger extensions, hex editors.

Features: 
  
- Supports Arithmetic, Logic, and Bitwise Operations.
- Supports Offseting.
- Supports Display Options.
- Immutable Type.


````
Example:
//Project-> Properties->  "Conditional compilation symbols"
#if X64 
    using ma64 = HexWorks.Hex64;
#else
    using ma32 = HexWorks.Hex32;
#endif 
````

USAGE: 

Example Assignments
````
  ASSINGMENT: 
            /*Example Of Assignments*/

             ma64 p1 = 0xFFAAEEEE99887766;     //Hex Number Assignment
             ma64 p2 = "0xFFAAEEEE99887766";   //Hex String Assignment
             ma64 p3 = "FFAAEEEE`99887766";    //Hex String Assignment
             ma64 p4 = "FFAAEEEE99887766";
             ma64 p5 = ma64.FromBinaryString("1011b");                //from Binary. 
             ma64 p6 = ma64.FromHexString("0xFFAAEEEE99887766");

...

````
Array Assignment
````
ma64 arr1 = new ma64(new byte[] { 0x12, 0x34, 0x56, 0x78, 0xAB, 0xBC, 0xDE, 0xF0 });
PrintAddress(arr1);

ma64 arr2 = new ma64(new byte[] { 0x12, 0x34, 0x56, 0x78, 0xAB, 0xBC, 0xDE, 0xF0 }, false);
PrintAddress(arr2);


        static void Main(string[] args)
        {
            Process proc = Process.GetCurrentProcess();
            IntPtr startOffset = proc.MainModule.BaseAddress;

            ma64 address1 = startOffset;

            PrintAddress(address1);

            ma64 offset = 0x1000;

            var nextAddress = address1 + offset;

            PrintAddress(nextAddress);

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
```

