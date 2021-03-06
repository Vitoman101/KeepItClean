﻿using System.IO;
using Syroot.Windows.IO;

namespace KeepItClean
{
    static class Download
    {
        public readonly static string DOWNLOAD = new KnownFolder(KnownFolderType.Downloads).Path;
        public readonly static DirectoryInfo dir = new DirectoryInfo(DOWNLOAD);
        public static void Clean()
        {
            DirectoryInfo di = new DirectoryInfo(DOWNLOAD);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                try
                {
                    file.Delete();
                } 
                catch
                {
                    continue;
                }
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch 
                {
                    continue;
                }
            }
        }
    }
}
