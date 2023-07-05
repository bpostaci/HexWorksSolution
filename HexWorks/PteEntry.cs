using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexWorks
{
    [Flags]
    public enum HardwarePteFlags : ulong
    {
        Valid = 1UL,
        Write = 1UL << 1,
        Owner = 1UL << 2,
        WriteThrough = 1UL << 3,
        CacheDisable = 1UL << 4,
        Accessed = 1UL << 5,
        Dirty = 1UL << 6,
        LargePage = 1UL << 7,
        Global = 1UL << 8,
        CopyOnWrite = 1UL << 9,
        Prototype = 1UL << 10,
        Reserved0 = 1UL << 11,
        NoExecute = 1UL << 63
    }

    

    public class PteEntry64 : MemoryAddress64
    {
        public const int PTE_WORKING_SET_BITS = 11;
        public PteEntry64(ulong value) : base(value)
        {
        }
        public HardwarePteFlags Flags => (HardwarePteFlags)(this.Value & 0x8000000000000FFFUL);

        public ulong Pfn => (this.Value >> 12) & 0xFFFFFFFFFUL; // 9xF = 2 ^ 36 -> pfn size is 36 bit.

        public ulong SoftwareWsIndex
        {
            get
            {
                var killfirstbit = (this.Value << 1) >> 1; //get rid of the NoExecute bit (bit number 63). 
                // 12 bit for flags
                // 36 bit for pfn 
                // (16 - PTE_WORKING_SET_BITS) -> for reserved1 which is 5bit.
                return killfirstbit >> (12 + 36 + (16 - PTE_WORKING_SET_BITS));
            }
        }
        public bool IsValid()
        {
            return Flags.HasFlag(HardwarePteFlags.Valid); 
        }

        public bool IsWrite()
        {
            return Flags.HasFlag(HardwarePteFlags.Write);
        }
        public bool IsOwner()
        {
            return Flags.HasFlag(HardwarePteFlags.Owner);
        }
        public bool IsWriteThrough()
        {
            return Flags.HasFlag(HardwarePteFlags.WriteThrough);
        }

        public bool IsCacheDisable()
        {
            return Flags.HasFlag(HardwarePteFlags.CacheDisable);
        }
        public bool IsAccessed()
        {
            return Flags.HasFlag(HardwarePteFlags.Accessed);
        }

        public bool IsDirty()
        {
            return Flags.HasFlag(HardwarePteFlags.Dirty);
        }
        public bool IsLargePage()
        {
            return Flags.HasFlag(HardwarePteFlags.LargePage);
        }

        public bool IsGlobal()
        {
            return Flags.HasFlag(HardwarePteFlags.Global);
        }

        public bool IsCopyOnWrite()
        {
            return Flags.HasFlag(HardwarePteFlags.CopyOnWrite);
        }
        public bool IsPrototype()
        {
            return Flags.HasFlag(HardwarePteFlags.Prototype);
        }

        public static PteEntry64 Create(ulong pageFrame, HardwarePteFlags flags)
        {
            ulong pteAddress=0;
            pteAddress = (pteAddress & 0xFFF) | (pageFrame << 12);
            pteAddress = (pteAddress & ~0xFFFUL) | ((ulong)flags & 0xFFF);
            return new PteEntry64(pteAddress); 
        }

        public string GetFlagsAsString()
        {
            var flagNames = Enum.GetNames(typeof(HardwarePteFlags));
            var setFlags = Enum.GetValues(typeof(HardwarePteFlags)).Cast<HardwarePteFlags>()
                               .Where(f => Flags.HasFlag(f))
                               .Select(f => GetFlagName(f, flagNames));
            var flagsAsString = string.Join(", ", setFlags);
            return flagsAsString;
        }

        private string GetFlagName(HardwarePteFlags flag, string[] flagNames)
        {
            ulong flagValue = (ulong)flag;
            for (int i = 0; i < flagNames.Length; i++)
            {
                ulong value = (ulong)Enum.Parse(typeof(HardwarePteFlags), flagNames[i]);
                if (value == flagValue)
                    return flagNames[i];
            }
            return string.Empty;
        }

    }
}
