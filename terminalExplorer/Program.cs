using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terminalExplorer
{
    class Program
    {
        static string path;
        static void Main(string[] args)
        {
            path = "C:\\users\\ali\\appdata";
            while (true)
            {
                string[] files =correctArrey( System.IO.Directory.GetFiles(path));
                string[] dirs = correctArrey(System.IO.Directory.GetDirectories(path));
                Console.Clear();
                Console.WriteLine(path);
                int selected = 0, totalLength=dirs.Length+files.Length;
                for (int i = 0; i < dirs.Length; i++)
                    Console.WriteLine((i == selected ? "______________________________\n##  " : "") + dirs[i] + (i == selected ? "  ##\n______________________________" : ""));
                for (int i = 0; i < dirs.Length; i++)
                    Console.WriteLine((selected>=dirs.Length&&i == selected-dirs.Length ? "______________________________\n##  " : "") + dirs[i] + (selected >= dirs.Length && i == selected - dirs.Length ? "  ##\n______________________________" : ""));
                while (true)
                {
                    ConsoleKeyInfo ck = Console.ReadKey(true);
                    if (ck.Key == ConsoleKey.DownArrow)
                    {
                        selected++;
                        if (selected > totalLength- 1)
                            selected = 0;
                    }
                    else if (ck.Key == ConsoleKey.UpArrow)
                    {
                        selected--;
                        if (selected < 0)
                            selected = totalLength - 1;
                    }
                    else if (ck.Key == ConsoleKey.Enter)
                    {
                        goto innerWhileBreack;
                    }
                    Console.Clear();
                    Console.WriteLine(path);
                    for (int i = 0; i < dirs.Length; i++)
                        Console.WriteLine((i == selected ? "______________________________\n##  " : "") + dirs[i] + (i == selected ? "  ##\n______________________________" : ""));
                    for (int i = 0; i < dirs.Length; i++)
                        Console.WriteLine((selected >= dirs.Length && i == selected - dirs.Length ? "______________________________\n##  " : "") + dirs[i] + (selected >= dirs.Length && i == selected - dirs.Length ? "  ##\n______________________________" : ""));
                }
            innerWhileBreack:;
            }
        }
        static string[] correctArrey(string[] v)
        {
            return v;   
        }
    }
}
