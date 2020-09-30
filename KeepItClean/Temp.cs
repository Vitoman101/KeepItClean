using System.IO;

namespace KeepItClean
{
    static class Temp
    {
        public static readonly string tempPath = Path.GetTempPath();
        public static readonly DirectoryInfo dir = new DirectoryInfo(tempPath);

        public static void Clean()
        {
            foreach (FileInfo file in dir.EnumerateFiles())
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
            foreach (DirectoryInfo dir in dir.EnumerateDirectories())
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
