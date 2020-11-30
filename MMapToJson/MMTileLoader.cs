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
                    tile.dtMeshTile.verts = ReadAndAdvance<float>(ref dataManagedPtr, tile.dtMeshHeader.vertCount);
                    //tile->polys = dtGetThenAdvanceBufferPointer<dtPoly>(d, polysSize);
                    //tile->links = dtGetThenAdvanceBufferPointer<dtLink>(d, linksSize);
                    //tile->detailMeshes = dtGetThenAdvanceBufferPointer<dtPolyDetail>(d, detailMeshesSize);
                    //tile->detailVerts = dtGetThenAdvanceBufferPointer<float>(d, detailVertsSize);
                    //tile->detailTris = dtGetThenAdvanceBufferPointer < unsigned char> (d, detailTrisSize);
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
