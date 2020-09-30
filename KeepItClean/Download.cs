using System.IO;
using Syroot.Windows.IO;

namespace KeepItClean
{
    static class Download
    {
        public static void deleteEverything()
        {
            DirectoryInfo di = new DirectoryInfo(new KnownFolder(KnownFolderType.Downloads).Path);
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
