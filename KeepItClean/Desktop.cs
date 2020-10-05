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
        ".xls", ".xlsx", ".ods", ".ppt", ".pptx", ".txt", 
        ".dita", ".json", ".xml"};

        public static readonly List<string> compressionExtension = new List<string>
        {".zip", ".rar", ".7z", ".deb", ".pkg", ".rpm",
        ".tar.gz", ".z"};

        public static readonly List<string> videoExtension = new List<string>
        {".mp4", ".m4a", ".m4v", ".f4v", ".f4a", ".m4b",
        ".m4r", ".f4b",".mov", ".3gp",".3gp2", ".3g2",
        ".3gpp", ".3gpp2",".ogg", ".oga",".ogv", ".ogx",
        ".wmv", ".wma",".asf", ".webm",".flv", ".avi",
        ".hdv", ".mvx"};

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

        public readonly static string DESKTOP = new KnownFolder(KnownFolderType.Desktop).Path;
        public readonly static string PICTURES = new KnownFolder(KnownFolderType.Pictures).Path;
        public readonly static string DOCUMENTS = new KnownFolder(KnownFolderType.Documents).Path;
        public readonly static string VIDEOS = new KnownFolder(KnownFolderType.Videos).Path;

        readonly static DirectoryInfo dir = new DirectoryInfo(DESKTOP);

        private static void moveImagesToPictures()
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
                            if (Comparison.FilesAreEqual(file, new FileInfo(imagePath)))
                            {
                                file.Delete();
                            }
                            else
                            {
                                var dateEdited = File.GetLastWriteTime(file.FullName).ToString();
                                var dateReplace = dateEdited.Replace('/', '-').Replace(" ", "-").Replace(":", "-").Substring(0, dateEdited.Length - 3);
                                var fileRenamed = Path.Combine(PICTURES, file.Name.Replace(Path.GetExtension(file.FullName), "") + "-" + dateReplace + Path.GetExtension(file.FullName));
                                File.Move(file.FullName, fileRenamed);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }   
                }        
            }
        }

        private static void moveDocumentsToDocuments()
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
                            if(Comparison.FilesAreEqual(file, new FileInfo(documentPath)))
                            {
                                file.Delete();
                            } 
                            else
                            {
                                var dateEdited = File.GetLastWriteTime(file.FullName).ToString();
                                var dateReplace = dateEdited.Replace('/', '-').Replace(" ", "-").Replace(":","-").Substring(0, dateEdited.Length - 3);
                                var fileRenamed = Path.Combine(DOCUMENTS, file.Name.Replace(Path.GetExtension(file.FullName), "") + "-" + dateReplace + Path.GetExtension(file.FullName));
                                File.Move(file.FullName, fileRenamed);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        private static void createDirIfNotExist()
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

        private static void moveFoldersInOne()
        {         
            var envFolder = Path.Combine(DESKTOP, Environment.UserName);
            foreach (DirectoryInfo dir in dir.EnumerateDirectories())
            {
                var desktopFolder = Path.Combine(DESKTOP, Environment.UserName, dir.Name);
                bool equals = Comparison.PathEquals(dir.FullName, envFolder);
                if (!equals || directoriesNotToMove.Contains(dir.FullName))
                {
                    try
                    {
                        if (Directory.GetFileSystemEntries(dir.FullName).Length == 0 && !dir.Name.Equals("ZIP") || Directory.Exists(desktopFolder))
                        {
                            dir.Delete(true);
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
        }

        private static void moveZipFilesInOne()
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
                            if (Comparison.FilesAreEqual(file, new FileInfo(zipPath)))
                            {
                                file.Delete();
                            }
                            else
                            {
                                var dateEdited = File.GetLastWriteTime(file.FullName).ToString();
                                var dateReplace = dateEdited.Replace('/', '-').Replace(" ", "-").Replace(":", "-").Substring(0, dateEdited.Length - 3);
                                var fileRenamed = Path.Combine(DESKTOP, "ZIP", file.Name.Replace(Path.GetExtension(file.FullName), "") + "-" + dateReplace + Path.GetExtension(file.FullName));
                                File.Move(file.FullName, fileRenamed);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        private static void moveVideosToVideos()
        {
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (videoExtension.Contains(file.Extension.ToLowerInvariant()))
                {
                    var videoPath = Path.Combine(VIDEOS, file.Name);
                    if (!File.Exists(videoPath))
                    {
                        try
                        {
                            File.Move(file.FullName, videoPath);
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
                            if (Comparison.FilesAreEqual(file, new FileInfo(videoPath)))
                            {
                                file.Delete();
                            }
                            else
                            {
                                var dateEdited = File.GetLastWriteTime(file.FullName).ToString();
                                var dateReplace = dateEdited.Replace('/', '-').Replace(" ", "-").Replace(":", "-").Substring(0, dateEdited.Length - 3);
                                var fileRenamed = Path.Combine(VIDEOS, "ZIP", file.Name.Replace(Path.GetExtension(file.FullName), "") + "-" + dateReplace + Path.GetExtension(file.FullName));
                                File.Move(file.FullName, fileRenamed);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        public static void Clean()
        {
            moveDocumentsToDocuments();
            moveImagesToPictures();
            createDirIfNotExist();
            moveFoldersInOne();
            moveZipFilesInOne();
            moveVideosToVideos();
        }
    }
}
