using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using uint32 = System.UInt32;

namespace MMapToJson
{
    unsafe struct MMTile
    {
        public MmapTileHeader Header;
    }

    unsafe struct MmapTileHeader
    {
        public uint32 mmapMagic;
        public uint32 dtVersion;
        public uint32 mmapVersion;
        public uint32 size;
        public char usesLiquids;
        fixed char padding[3];
    }
}