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
                    return (MMTile)Marshal.PtrToStructure((IntPtr)dataPtr, typeof(MMTile));
                }
            }
        }
    }
}
