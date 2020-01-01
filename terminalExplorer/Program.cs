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
            if (p == "")
            {
                Console.Clear();
                path = menu(System.IO.Directory.GetLogicalDrives(), "Terminal Explorer");
                files = correctArrey(System.IO.Directory.GetFiles(path));
                dirs = correctArrey(System.IO.Directory.GetDirectories(path));
            }
            else
            {
                path = p;
                while (path[path.Length - 1] == '\\')
                    path = path.Remove(path.Length - 1);
                if (path.Length < 3) path += '\\';
                files = correctArrey(System.IO.Directory.GetFiles(path));
                dirs = correctArrey(System.IO.Directory.GetDirectories(path));
            }
        }
        static void Main(string[] args)
        {
            if (args.Length > 0 && System.IO.Directory.Exists(args[0]))
                openPath(args[0]);
            else
                openPath("");
            while (true)
            {
                int selected = 0, last = 0, totalLength = dirs.Length + files.Length;
                bool rewrite = false;
                Console.Title = path;
                Console.Clear();
                Console.WriteLine("directories :\n");
                for (int i = 0; i < dirs.Length; i++)
                    Console.WriteLine((i == selected ? "      --->d : " : "dir  : ") + dirs[i] + (i == selected ? " : <--" : ""));
                Console.WriteLine("\nfiles :\n");
                for (int i = 0; i < files.Length; i++)
                    Console.WriteLine((selected >= dirs.Length && i == selected - dirs.Length ? "      --->f : " : "file : ") + files[i] + (selected >= dirs.Length && i == selected - dirs.Length ? " : <--" : ""));
                while (true)
                {
                    ConsoleKeyInfo ck = Console.ReadKey(true);
                    if (ck.Key == ConsoleKey.DownArrow)
                    {
                        selected++;
                        if (selected > totalLength - 1)
                            selected = 0;
                    }
                    else if (ck.Key == ConsoleKey.UpArrow)
                    {
                        selected--;
                        if (selected < 0)
                            selected = totalLength - 1;
                    }
                    else if (ck.Key == ConsoleKey.End)
                    {
                        selected = (selected < dirs.Length) ? dirs.Length - 1 : dirs.Length + files.Length - 1;
                    }
                    else if (ck.Key == ConsoleKey.Home)
                    {
                        selected = (selected < dirs.Length) ? 0 : (files.Length == 0) ? 0 : dirs.Length;
                    }
                    else if (ck.Key == ConsoleKey.Enter)
                    {
                        if (selected < dirs.Length)
                            openPath((path.Length > 3) ? path + '\\' + dirs[selected] : path + dirs[selected]);
                        else
                            try
                            {
                                System.Diagnostics.Process.Start(path + '\\' + files[selected - dirs.Length]);
                            }
                            catch { }
                        break;
                    }
                    else if (ck.Key == ConsoleKey.Backspace)
                    {
                        if (path.Length > 3)
                        {
                            openPath(cutLast(path));
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            openPath("");
                            break;
                        }
                    }
                    else if (ck.Key == ConsoleKey.Tab)
                    {
                        bool ex = false;
                        switch (menu(new string[] { "Back","Home" ,"Exit"}, "select an item then press enter\n"))
                        {
                            case "Back":
                                ex = true;
                                break;
                            case "Home":
                                openPath("");
                                break;
                            case "Exit":
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                                break;
                        }
                        if (ex)
                            break;
                    }
                    else
                    {
                        if(selected<dirs.Length)
                        {
                            for(int n = 0; n < dirs.Length; n++)
                            {
                                if (Char.ToLower(dirs[n][0]) == Char.ToLower(ck.KeyChar))
                                {
                                    selected = n;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int n = 0; n < files.Length; n++)
                            {
                                if (Char.ToLower(files[n][0]) == Char.ToLower(ck.KeyChar))
                                {
                                    selected = dirs.Length + n;
                                    break;
                                }
                            }
                        }
                    }
                    if (last == selected && !rewrite)
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
                        Console.SetCursorPosition(0, selected + 2);
                        Console.Write("      --->d : " + dirs[selected] + " : <--");
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
                            Console.WriteLine(emptyString(files[last - dirs.Length].Length + 21));
                            Console.SetCursorPosition(0, last + 5);
                            Console.Write("file : " + files[last - dirs.Length]);
                        }
                        Console.SetCursorPosition(0, selected + 5);
                        Console.Write("      --->f : " + files[selected - dirs.Length] + " : <--");
                    }
                    last = selected;
                }
                //innerWhileBreack:;
            }
        }
        static string cutLast(string v)
        {
            int i = v.Length - 1;
            while (v[i] != '\\')
                i--;
            if (i > 0)
                return v.Substring(0, i);
            return v;
        }
        static string[] correctArrey(string[] v)
        {
            for (int i = 0; i < v.Length; i++)
                v[i] = System.IO.Path.GetFileName(v[i]);
            return v;
        }
        static string emptyString(int len)
        {
            string s = "";
            for (int i = 0; i < len; i++)
                s += " ";
            return s;
        }
        static int entersCount(string v)
        {
            int i = 0;
            foreach (char x in v)
                if (x == '\n')
                    i++;
            return i;
        }
        static string menu(string[] items, string title)
        {
            Console.Clear();
            Console.WriteLine(title);
            int enterCount = entersCount(title) + 1;
            int selected = 0, last = 0;
            for (int i = 0; i < items.Length; i++)
                Console.WriteLine((i == selected ? "  -->  " : "") + items[i] + (i == selected ? "  <--" : ""));
            while (true)
            {
                ConsoleKeyInfo ck = Console.ReadKey(true);
                if (ck.Key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected > items.Length - 1)
                        selected = 0;
                }
                else if (ck.Key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected < 0)
                        selected = items.Length - 1;
                }
                else if (ck.Key == ConsoleKey.Enter)
                {
                    if (selected == -1)
                        return "";
                    else return items[selected];
                }
                if (selected != last)
                {
                    Console.SetCursorPosition(0, enterCount + last);
                    Console.Write(emptyString(12 + items[last].Length));
                    Console.SetCursorPosition(0, enterCount + last);
                    Console.Write(items[last]);
                    Console.SetCursorPosition(0, enterCount + selected);
                    Console.Write("  -->  " + items[selected] + "  <--");
                }
                last = selected;
            }
        }
    }
}




/*for (int i = 0; i < dirs.Length; i++)
                    Console.WriteLine( (i == selected ? "╔══════════════════════════════════════════\n║  " : "") + "dir  : " + dirs[i] + (i == selected ? "\n╚══════════════════════════════════════════" : ""));
                Console.WriteLine("\nfiles :\n");
                for (int i = 0; i < files.Length; i++)
                    Console.WriteLine((selected >= dirs.Length && i == selected - dirs.Length ? "╔══════════════════════════════════════════\n║  " : "") + "file : " + files[i] + (selected >= dirs.Length && i == selected - dirs.Length ? "\n╚══════════════════════════════════════════" : ""));*/
