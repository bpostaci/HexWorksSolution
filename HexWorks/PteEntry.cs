using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexWorks
{
    [Flags]
    public enum PteFlags : ulong
    {
        Present = 1UL,
        ReadWrite = 1UL << 1,
        UserSupervisor = 1UL << 2,
        PageLevelCacheDisable = 1UL << 4,
        PageLevelWriteThrough = 1UL << 3,
        Dirty = 1UL << 6,
        Accessed = 1UL << 7,
        Global = 1UL << 8
    }

    public class PteEntry64 : MemoryAddress64
    {
        public PteEntry64(ulong value) : base(value)
        {
        }

        public static PteEntry64 Create(ulong pageFrame,PteFlags flags)
        {
            ulong pteAddress=0;
            pteAddress = (pteAddress & 0xFFF) | (pageFrame << 12);
            pteAddress = (pteAddress & ~0xFFFUL) | ((ulong)flags & 0xFFF);
            return new PteEntry64(pteAddress); 
        }

        public MemoryAddress64 PageFrame
        {
            get
            {
                return new MemoryAddress64(this.Value >> 12);
            }
        }

        public PteFlags Flags
        {
            get
            {
                return (PteFlags)(this.Value & 0xFFF);
            }
        }


    }
}
