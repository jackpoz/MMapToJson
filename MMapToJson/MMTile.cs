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
		public dtMeshTile dtMeshTile;
	}

    unsafe struct MmapTileHeader
    {
        public uint32 mmapMagic;
        public uint32 dtVersion;
        public uint32 mmapVersion;
        public uint32 size;
        public byte usesLiquids;
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

	unsafe struct dtMeshTile
	{
		public uint salt;
		public uint linksFreeList;
		//public dtMeshHeader header;
		//public dtPoly[] polys;
		public float[] verts;
		//public dtLink[] links;
		//public dtPolyDetail* detailMeshes;
		public float[] detailVerts;
		public byte[] detailTris;
		//public dtBVNode* bvTree;
		//public dtOffMeshConnection* offMeshCons;
		public byte[] data;
		public int dataSize;
		public int flags;
		//public dtMeshTile next;
	};

	unsafe struct XYZ
    {
		public float x;
		public float y;
		public float z;
    }
}