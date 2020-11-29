using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using uint32 = System.UInt32;

#pragma warning disable CS0649

namespace MMapToJson
{
    unsafe struct MMTile
    {
        public MmapTileHeader MmapTileHeader;
        public dtMeshHeader dtMeshHeader;
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

    unsafe struct dtMeshHeader
	{
		public int magic;
		public int version;
		public int x;
		public int y;
		public int layer;
		public uint userId;
		public int polyCount;
		public int vertCount;
		public int maxLinkCount;
		public int detailMeshCount;
		public int detailVertCount;
		public int detailTriCount;
		public int bvNodeCount;
		public int offMeshConCount;
		public int offMeshBase;
		public float walkableHeight;
		public float walkableRadius;
		public float walkableClimb;
		public XYZ bmin;
		public XYZ bmax;
		public float bvQuantFactor;
	};

	unsafe struct XYZ
    {
		public float x;
		public float y;
		public float z;
    }
}