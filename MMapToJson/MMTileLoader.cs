using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MMapToJson
{
    static class MMTileLoader
    {
        public static MMTile LoadMMTile(string path)
        {
            var data = File.ReadAllBytes(path);

            unsafe
            {
                fixed (byte* dataPtr = data)
                {
                    var tile = new MMTile();
                    tile.CustomProperties.TotalBytes = data.Length;
                    IntPtr dataManagedPtr = (IntPtr)dataPtr;
                    IntPtr startPtr = dataManagedPtr;
                    tile.MmapTileHeader = ReadAndAdvance<MmapTileHeader>(ref dataManagedPtr);
                    tile.dtMeshHeader = ReadAndAdvance<dtMeshHeader>(ref dataManagedPtr);
                    tile.dtMeshTile.verts = ReadAndAdvance<XYZ>(ref dataManagedPtr, tile.dtMeshHeader.vertCount);
                    tile.dtMeshTile.polys = ReadAndAdvance<dtPoly>(ref dataManagedPtr, tile.dtMeshHeader.polyCount);
                    tile.dtMeshTile.links = ReadAndAdvance<dtLink>(ref dataManagedPtr, tile.dtMeshHeader.maxLinkCount);
                    tile.dtMeshTile.detailMeshes = ReadAndAdvance<dtPolyDetail>(ref dataManagedPtr, tile.dtMeshHeader.detailMeshCount);
                    tile.dtMeshTile.detailVerts = ReadAndAdvance<XYZ>(ref dataManagedPtr, tile.dtMeshHeader.detailVertCount);
                    tile.dtMeshTile.detailTris = ReadAndAdvance<DetailTri>(ref dataManagedPtr, tile.dtMeshHeader.detailTriCount);
                    //tile->bvTree = dtGetThenAdvanceBufferPointer<dtBVNode>(d, bvtreeSize);
                    //tile->offMeshCons = dtGetThenAdvanceBufferPointer<dtOffMeshConnection>(d, offMeshLinksSize);

                    tile.CustomProperties.ReadBytes = (int)(dataManagedPtr.ToInt64() - startPtr.ToInt64());
                    tile.CustomProperties.UnreadBytes = tile.CustomProperties.TotalBytes - tile.CustomProperties.ReadBytes;
                    return tile;
                }
            }
        }


        unsafe static T ReadAndAdvance<T>(ref IntPtr dataPtr)
        {
            T result = Marshal.PtrToStructure<T>(dataPtr);
            dataPtr += Marshal.SizeOf<T>();
            return result;
        }

        unsafe static T[] ReadAndAdvance<T>(ref IntPtr dataPtr, int count)
        {
            T[] result = new T[count];
            for(int index = 0; index < count; index++)
                result[index] = ReadAndAdvance<T>(ref dataPtr);
            return result;
        }
    }
}
