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
        static string[] files, dirs;
        static void openPath(string p)
        {
            path = p;
            files = correctArrey(System.IO.Directory.GetFiles(path));
            dirs = correctArrey(System.IO.Directory.GetDirectories(path));
        }
        static void Main(string[] args)
        {
            
            openPath( "i:\\tdir");
            while (true)
            {
                int selected = 5, last = 0, totalLength = dirs.Length + files.Length;
                bool rewrite = false;
                Console.Title=path;
                Console.WriteLine("directories :\n");
                for (int i = 0; i < dirs.Length; i++)
                    Console.WriteLine( (i == selected ? "--> :  " : "") + "dir  : " + dirs[i] + (i == selected ? "  : <-- ": ""));
                Console.WriteLine("\nfiles :\n");
                for (int i = 0; i < files.Length; i++)
                    Console.WriteLine((selected >= dirs.Length && i == selected - dirs.Length ? "--> :  " : "") + "file : " + files[i] + (selected >= dirs.Length && i == selected - dirs.Length ? "  : <--" : ""));
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
                        break;
                    }
                    if (last == selected&&!rewrite)
                    {
                        continue;
                    }
                    if (selected < dirs.Length)
                    {
                        if (last < dirs.Length)
                        {
                            Console.SetCursorPosition(0, last + 2);
                            Console.WriteLine(emptyString(dirs[last].Length + 21));
                            Console.SetCursorPosition(0, last + 2);
                            Console.Write("dir  : " + dirs[last]);
                        }
                        else
                        {
                            Console.SetCursorPosition(0, last + 5);
                            Console.WriteLine(emptyString(files[last - dirs.Length].Length + 21));
                            Console.SetCursorPosition(0, last + 5);
                            Console.Write("file : " + files[last - dirs.Length]);
                        }
                        Console.SetCursorPosition(0, selected+2);
                        Console.Write("--> :  dir  : " + dirs[selected]+ "  : <--");
                    }
                    else
                    {
                        if (last < dirs.Length)
                        {
                            Console.SetCursorPosition(0, last + 2);
                            Console.WriteLine(emptyString(dirs[last].Length + 21));
                            Console.SetCursorPosition(0, last + 2);
                            Console.Write("dir  : " + dirs[last]);
                        }
                        else
                        {
                            Console.SetCursorPosition(0, last + 5);
                            Console.WriteLine(emptyString(files[last-dirs.Length].Length + 21));
                            Console.SetCursorPosition(0, last + 5);
                            Console.Write("file : " + files[last-dirs.Length]);
                        }
                        Console.SetCursorPosition(0, selected + 5);
                        Console.Write("--> :  file : " + files[selected-dirs.Length] + "  : <--");
                    }
                        /*
                    Console.Clear();
                    Console.WriteLine(path);//║╔╚═
                    Console.WriteLine("\ndirectories :\n");
                    for (int i = 0; i < dirs.Length; i++)
                        Console.WriteLine((i == selected ? "### :  " : "") + "dir  : " + dirs[i] + (i == selected ? "   : ###" : ""));
                    Console.WriteLine("\nfiles :\n");
                    for (int i = 0; i < files.Length; i++)
                        Console.WriteLine((selected >= dirs.Length && i == selected - dirs.Length ? "### :  " : "") + "file : " + files[i] + (selected >= dirs.Length && i == selected - dirs.Length ? "   : ###" : ""));*/
                    last = selected;
                }
            //innerWhileBreack:;
            }
        }
        static string[] correctArrey(string[] v)
        {
            for(int i = 0; i < v.Length; i++)
                v[i] = System.IO.Path.GetFileName(v[i]);
            return v;   
        }
        static string emptyString(int len)
        {
            string s = "";
            for(int i = 0; i < len; i++)
                s += " ";
            return s;
        }
    }
}




/*for (int i = 0; i < dirs.Length; i++)
                    Console.WriteLine( (i == selected ? "╔══════════════════════════════════════════\n║  " : "") + "dir  : " + dirs[i] + (i == selected ? "\n╚══════════════════════════════════════════" : ""));
                Console.WriteLine("\nfiles :\n");
                for (int i = 0; i < files.Length; i++)
                    Console.WriteLine((selected >= dirs.Length && i == selected - dirs.Length ? "╔══════════════════════════════════════════\n║  " : "") + "file : " + files[i] + (selected >= dirs.Length && i == selected - dirs.Length ? "\n╚══════════════════════════════════════════" : ""));*/
