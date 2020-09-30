namespace KeepItClean
{
    class Program
    {
        static void Main(string[] args)
        {
            Download.deleteEverything();
            Desktop.moveImagesToPictures();
            Desktop.moveDocumentsToDocuments();
            Desktop.moveAllFoldersInOne();
        }

        
    }
}
