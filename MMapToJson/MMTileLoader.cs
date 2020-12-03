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
                    tile.MmapTileHeader = ReadAndAdvanceSingle<MmapTileHeader>(ref dataManagedPtr);
                    tile.dtMeshHeader = ReadAndAdvance<dtMeshHeader>(ref dataManagedPtr);
                    if (tile.dtMeshHeader.magic != 0x444e4156)
                        throw new ArgumentException("The loaded tile has wrong dtMeshHeader.magic value");
                    tile.dtMeshTile.verts = ReadAndAdvanceArray<XYZ>(ref dataManagedPtr, tile.dtMeshHeader.vertCount);
                    tile.dtMeshTile.polys = ReadAndAdvanceArray<dtPoly>(ref dataManagedPtr, tile.dtMeshHeader.polyCount);
                    tile.dtMeshTile.links = ReadAndAdvanceArray<dtLink>(ref dataManagedPtr, tile.dtMeshHeader.maxLinkCount);
                    tile.dtMeshTile.detailMeshes = ReadAndAdvanceArray<dtPolyDetail>(ref dataManagedPtr, tile.dtMeshHeader.detailMeshCount);
                    tile.dtMeshTile.detailVerts = ReadAndAdvanceArray<XYZ>(ref dataManagedPtr, tile.dtMeshHeader.detailVertCount);
                    tile.dtMeshTile.detailTris = ReadAndAdvanceArray<DetailTri>(ref dataManagedPtr, tile.dtMeshHeader.detailTriCount);
                    tile.dtMeshTile.bvTree = ReadAndAdvanceArray<dtBVNode>(ref dataManagedPtr, tile.dtMeshHeader.bvNodeCount);
                    tile.dtMeshTile.offMeshCons = ReadAndAdvanceArray<dtOffMeshConnection>(ref dataManagedPtr, tile.dtMeshHeader.offMeshConCount);

                    tile.CustomProperties.ReadBytes = (int)(dataManagedPtr.ToInt64() - startPtr.ToInt64());
                    tile.CustomProperties.UnreadBytes = tile.CustomProperties.TotalBytes - tile.CustomProperties.ReadBytes;
                    return tile;
                }
            }
        }

        unsafe static T ReadAndAdvanceSingle<T>(ref IntPtr dataPtr)
        {
            Console.WriteLine($"Type '{typeof(T).Name}' has size {Marshal.SizeOf<T>()}");
            return ReadAndAdvance<T>(ref dataPtr);
        }


        unsafe static T[] ReadAndAdvanceArray<T>(ref IntPtr dataPtr, int count)
        {
            Console.WriteLine($"Type '{typeof(T).Name}' has size {Marshal.SizeOf<T>()}");
            T[] result = new T[count];
            for(int index = 0; index < count; index++)
                result[index] = ReadAndAdvance<T>(ref dataPtr);
            return result;
        }

        unsafe static T ReadAndAdvance<T>(ref IntPtr dataPtr)
        {
            T result = Marshal.PtrToStructure<T>(dataPtr);
            dataPtr += Marshal.SizeOf<T>();
            return result;
        }
    }
}
