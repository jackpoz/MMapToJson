using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using uint32 = System.UInt32;
using dtPolyRef = System.UInt64;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

namespace MMapToJson
{
    unsafe struct MMTile
    {
        public MmapTileHeader MmapTileHeader;
        public dtMeshHeader dtMeshHeader;
		public dtMeshTile dtMeshTile;
		public CustomProperties CustomProperties;
	}

	unsafe struct MmapTileHeader
    {
        public uint32 mmapMagic;
        public uint32 dtVersion;
        public uint32 mmapVersion;
        public uint32 size;
        public byte usesLiquids;
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
		//public uint salt;
		//public uint linksFreeList;
		//public dtMeshHeader header;
		public dtPoly[] polys;
		public XYZ[] verts;
		public dtLink[] links;
		public dtPolyDetail[] detailMeshes;
		public XYZ[] detailVerts;
		public DetailTri[] detailTris;
		public dtBVNode[] bvTree;
		public dtOffMeshConnection[] offMeshCons;
		//public byte[] data;
		//public int dataSize;
		//public int flags;
		//public dtMeshTile next;
	};

	unsafe struct dtPoly
	{
		public uint firstLink;
		public PolygonVertices verts;
		public PolygonVertices neis;
		public ushort flags;
		public byte vertCount;
		public byte areaAndtype;
	}

	unsafe struct dtLink
	{
		public dtPolyRef @ref;
		public uint next;
		public byte edge;
		public byte side;
		public byte bmin;
		public byte bmax;
	};

	struct dtPolyDetail
	{
		public uint vertBase;
		public uint triBase;
		public byte vertCount;
		public byte triCount;
	};

	unsafe struct dtBVNode
	{
		public XYZ<ushort> bmin;
		public XYZ<ushort> bmax;
		public int i;
	};

	unsafe struct dtOffMeshConnection
	{
		public XYZ posA;
		public XYZ posB;
		public float rad;
		public byte flags;
		public byte side;
		public uint userId;
	};

	unsafe struct XYZ
    {
		public float x;
		public float y;
		public float z;
    }

	unsafe struct XYZ<T>
	{
		public T x;
		public T y;
		public T z;
	}

	unsafe struct PolygonVertices
    {
		public ushort v1;
		public ushort v2;
		public ushort v3;
		public ushort v4;
		public ushort v5;
		public ushort v6;
	}

	unsafe struct DetailTri
    {
		public byte vertA;
		public byte vertB;
		public byte vertC;
		public byte triFlags;
    }

	struct CustomProperties
    {
		public int TotalBytes;
		public int ReadBytes;
		public int UnreadBytes;
    }
}