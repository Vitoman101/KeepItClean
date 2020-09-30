using Syroot.Windows.IO;
using System;
using System.Collections.Generic;
using System.IO;



namespace KeepItClean
{
    static class Desktop
    {
        public static readonly List<string> imagesExtension = new List<string> 
        {".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi",
        ".png", ".gif", ".webp", ".tiff", ".tif", ".psd",
        ".raw", ".arw", ".cr2", ".nrw", ".k25", ".bmp",
        ".dib", ".heif", ".heic", ".ind", ".indd", ".indt",
        ".jp2", ".j2k", ".jpf", ".jpx", ".jpm", ".mj2", ".svg",
        ".svgz", ".ai", ".esp"};

        public static readonly List<string> documentsExtension = new List<string> 
        {".doc", ".docx", ".html", ".htm", ".odt", ".pdf",
        ".xls", ".xlsx", ".ods", ".ppt", ".pptx", ".txt" };

        public static readonly List<string> compressionExtension = new List<string>
        {".zip", ".rar", ".7z", ".deb", ".pkg", ".rpm",
        ".tar.gz", ".z"};

        public static readonly List<string> directoriesNotToMove = new List<string> 
        {   Environment.GetFolderPath(Environment.SpecialFolder.MyComputer).ToString(), 
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString(),
            Environment.GetFolderPath(Environment.SpecialFolder.MyMusic).ToString(),
            Environment.GetFolderPath(Environment.SpecialFolder.MyVideos).ToString(),
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString(),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).ToString(),
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString(),
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop).ToString(),
        };

        readonly static string DESKTOP = new KnownFolder(KnownFolderType.Desktop).Path;
        readonly static string PICTURES = new KnownFolder(KnownFolderType.Pictures).Path;
        readonly static string DOCUMENTS = new KnownFolder(KnownFolderType.Documents).Path;

        readonly static DirectoryInfo dir = new DirectoryInfo(DESKTOP);
        public static void moveImagesToPictures()
        {
            
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (imagesExtension.Contains(file.Extension.ToLowerInvariant()))
                {
                    var imagePath = Path.Combine(PICTURES, file.Name);
                    if (!File.Exists(imagePath))
                    {
                        try
                        {
                            File.Move(file.FullName, imagePath);
                        }
                        catch 
                        {
                            continue;
                        }
                    } 
                    else
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
                }        
            }
        }

        public static void moveDocumentsToDocuments()
        {          
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (documentsExtension.Contains(file.Extension.ToLowerInvariant()))
                {
                    var documentPath = Path.Combine(DOCUMENTS, file.Name);                   
                    if (!File.Exists(documentPath))
                    {
                        try
                        {
                            File.Move(file.FullName, documentPath);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    else
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
                }
            }
        }

        public static void createDirIfNotExist()
        {
            var desktopFolder = Path.Combine(DESKTOP, Environment.UserName);
            var zipFolder = Path.Combine(DESKTOP, "ZIP");
            try
            {
                if (!Directory.Exists(desktopFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(desktopFolder);
                }
                if (!Directory.Exists(zipFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(zipFolder);
                }
            } catch
            {           
                Environment.Exit(1);
            }
        }

        public static bool PathEquals(this string path1, string path2)
        {
            return Path.GetFullPath(path1)
                .Equals(Path.GetFullPath(path2), StringComparison.InvariantCultureIgnoreCase);
        }

        public static void moveAllFoldersInOne()
        {
            createDirIfNotExist();            
            var dontInclude = Path.Combine(DESKTOP, Environment.UserName);
            foreach (DirectoryInfo dir in dir.EnumerateDirectories())
            {
                var desktopFolder = Path.Combine(DESKTOP, Environment.UserName, dir.Name);
                bool equals = dir.FullName.PathEquals(dontInclude);
                if (!equals || directoriesNotToMove.Contains(dir.FullName))
                {
                    try
                    {
                        if (Directory.GetFileSystemEntries(dir.FullName).Length == 0 && !dir.Name.Equals("ZIP"))
                        {
                            dir.Delete();
                        } 
                        else if(!dir.Name.Equals("ZIP"))
                        {
                            Directory.Move(dir.FullName, desktopFolder);
                        }
                                            
                    }
                    catch
                    {                      
                        continue;
                    }
                }             
            }
            moveAllZipFilesInOne();
        }

        public static void moveAllZipFilesInOne()
        {            
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (compressionExtension.Contains(file.Extension.ToLowerInvariant()))
                {
                    var zipPath = Path.Combine(DESKTOP, "ZIP", file.Name);
                    if (!File.Exists(zipPath))
                    {
                        try
                        {
                            File.Move(file.FullName, zipPath);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    else
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
                }
            }
        }
    }
}
