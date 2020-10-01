using System.Threading.Tasks;

namespace KeepItClean
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Parallel.Invoke(
                () => Download.Clean(),
                () => Temp.Clean(),
                () => Desktop.Clean()
                );

        }
    }
}
