using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexWorks.Extensions
{
    [Flags]
    public enum EFlags : uint
    {
        CF = 0x0001,  // Carry flag
        Reserved1 = 0x0002,  // Reserved, always 1 in EFLAGS
        PF = 0x0004,  // Parity flag
        Reserved2 = 0x0008,  // Reserved
        AF = 0x0010,  // Auxiliary Carry flag
        Reserved3 = 0x0020,  // Reserved
        ZF = 0x0040,  // Zero flag
        SF = 0x0080,  // Sign flag
        TF = 0x0100,  // Trap flag (single step)
        IF = 0x0200,  // Interrupt enable flag
        DF = 0x0400,  // Direction flag
        OF = 0x0800,  // Overflow flag
        IOPL = 0x3000,  // I/O privilege level
        NT = 0x4000,  // Nested task flag
        MD = 0x8000,  // Mode flag

        RF = 0x00010000,  // Resume flag
        VM = 0x00020000,  // Virtual 8086 mode flag
        AC = 0x00040000,  // Alignment Check
        VIF = 0x00080000,  // Virtual interrupt flag
        VIP = 0x00100000,  // Virtual interrupt pending
        ID = 0x00200000,  // Able to use CPUID instruction

        Reserved4 = 0x3FC00000,  // Reserved

        None = 0x40000000,  // (none) AES key schedule loaded flag
        AI = 0x80000000   // Alternate Instruction Set enabled
    }
}
