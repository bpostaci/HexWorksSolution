MemoryAddress value object implementation

-  Contians 2 MemoryAddress class for 64 bit use MemoryAddress64 and for 32bit use MemoryAddress32 class
-  Use for cross compile: "Conditional compilation symbols" X64
-  
Example:
#if X64 
    using MemoryAddress = HexWorks.MemoryAddress64;
#else
    using MemoryAddress = HexWorks.MemoryAddress32;
#endif 


USAGE: 

            /*
            MemoryAddress p1 = 0xFFAAEEEE99887766;
            MemoryAddress p2 = "0xFFAAEEEE99887766";
            MemoryAddress p3 = "FFAAEEEE`99887766";

            if (p1 == p3) Console.WriteLine("yes they are equal");
            */

        static void Main(string[] args)
        {
            Process proc = Process.GetCurrentProcess();
            IntPtr startOffset = proc.MainModule.BaseAddress;

            MemoryAddress address1 = startOffset;

            PrintAddress(address1);

            MemoryAddress offset = 0x1000;

            var nextAddress = address1 + offset;

            PrintAddress(nextAddress);

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

OUTPUT:

Prefix            :0x000001e849be0000
Prefix+Capital    :0x000001E849BE0000
Plain             :000001e849be0000
Only Capital      :000001E849BE0000
Bits              :0000000000000000000000011110100001001001101111100000000000000000
Bits Formated     :00000000.00000000.00000001.11101000.01001001.10111110.00000000.00000000
High Bytes        :00000000000001e8
Low  Bytes        :0000000049be0000

Prefix            :0x000001e849be1000
Prefix+Capital    :0x000001E849BE1000
Plain             :000001e849be1000
Only Capital      :000001E849BE1000
Bits              :0000000000000000000000011110100001001001101111100001000000000000
Bits Formated     :00000000.00000000.00000001.11101000.01001001.10111110.00010000.00000000
High Bytes        :00000000000001e8
Low  Bytes        :0000000049be1000
