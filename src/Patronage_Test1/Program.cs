using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Patronage_Test1
{
    public class Program
    {
        enum Errors { ErrorAccess, ErrorFound, NoError }

        public static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.Write("Aplikacja została uruchomiona bez parametru. Następuje zakończenie programu.");
                return;
            }
            if (!ValidatePath(args[0]))
            {
                Console.Write("Format ścieżki jest niezgodny z wykorzystywanym systemem operacyjnym. Następuje zakończenie programu.");
                return;
            }

            string path = args[0];
            Errors err = ExistPath(ref path);
            if (err == Errors.ErrorAccess)
            {
                Console.Write("Brak uprawnień dostępu do wskazanego katalogu. Następuje zakończenie programu.");
                return;
            }
            else if (err == Errors.ErrorFound)
            {
                Console.Write("Wskazana ścieżka nie istnieje. Następuje zakończenie programu.");
                return;
            }
            else
            {
                if (path[path.Length - 1] != Path.DirectorySeparatorChar)
                    path += Path.DirectorySeparatorChar;
            }
            FileSystemInfo fsi = new DirectoryInfo(path);
            Tree tree = new Tree(fsi, ObjectType.Directory);
            BuildTree(ref tree);
            tree.PrintTreeData();
            Console.Write("Aby zakończyć naciśnij dowolny przycisk...");
            Console.ReadKey();
        }

        // Funkcja zwraca informację czy skazana ścieżka istnieje
        private static Errors ExistPath(ref string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                di.GetDirectories();
            }
            catch (DirectoryNotFoundException)
            {
                return Errors.ErrorFound;
            }
            catch (UnauthorizedAccessException)
            {
                return Errors.ErrorAccess;
            }
            return Errors.NoError;
        }

        // Tworzenie drzewa
        private static void BuildTree(ref Tree tree)
        {
            DirectoryInfo dir = new DirectoryInfo(tree.Object.FullName);
            DirectoryInfo[] dirTable = dir.GetDirectories();
            FileInfo[] fileTable = dir.GetFiles();
            if (fileTable != null)
            {
                foreach (DirectoryInfo di in dirTable)
                {
                    Tree tempTree = tree.AddChild(di, ObjectType.Directory);
                    BuildTree(ref tempTree);
                }
                foreach (FileInfo fi in fileTable)
                {
                    tree.AddChild(fi, ObjectType.File);
                }
            }
        }

        private static bool ValidatePath(string path)
        {
            // Walidacja ścieżki dla systemu Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Wyrażenie regularne uwzględnia tradycyjną ścieżkę (np. C:\) oraz przydział sieciowy lecz lokalizacja podawana jest z nazwą hosta, nie waliduje możliwości podania błędnego adresu IP (np. 256.256.256.256 będzie dla niego poprawnym adresem)
                Regex reg = new Regex(@"^(([a-zA-Z]:\\)|\\\\)([^\\\/:*?\<>|\r\n]+(\\)?)*$");
                return reg.IsMatch(path);
            }
            // Walidacja ścieżki dla systemu Linux
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Regex reg = new Regex(@"^(\/){1}([^\/\0]+(\/)?)*$");
                return reg.IsMatch(path);
            }
            // Walidacja ścieżki dla systemu OSX
            else
            {
                // Wyrażenie regularne uwzględnia typowy dla systemu Unix format ścieżki gdzie separatorem jest "/", nie uwzględnia tradycyjnej formy z separatorem ":" (wykorzystywanym do wersji OS X 8.1)
                Regex reg = new Regex(@"^(\/){1}([^\/\0:]+(\/)?)*$");
                return reg.IsMatch(path);
            }
        }
    }
}
