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
        static int prevpathlen = 20;
        static List<string> prevPath = new List<string>(prevpathlen + 1);
        static void openPath(string p, bool prev = true)
        {
            if (p!="" && prev)
            {
                if (prevPath.Count == prevpathlen)
                    prevPath.RemoveAt(0);
                prevPath.Add(p);
            }
            if (p == "")
            {
                Console.Clear();
                path = menu(System.IO.Directory.GetLogicalDrives(), "Terminal Explorer");
                if (prev)
                {
                    if (prevPath.Count == prevpathlen)
                        prevPath.RemoveAt(0);
                    prevPath.Add(path);
                }
                files = correctArrey(System.IO.Directory.GetFiles(path));
                dirs = correctArrey(System.IO.Directory.GetDirectories(path));
            }
            else
            {
                path = p;
                while (path[path.Length - 1] == '\\')
                    path = path.Remove(path.Length - 1);
                if (path.Length < 3) path += '\\';
                try
                {
                    files = correctArrey(System.IO.Directory.GetFiles(path));
                    dirs = correctArrey(System.IO.Directory.GetDirectories(path));
                }
                catch
                {
                    if (prevPath.Count != 0)
                        prevPath.RemoveAt(prevPath.Count - 1);
                    if (prevPath.Count == 0) openPath("");
                    else openPath(prevPath[prevPath.Count - 1]);
                };
            }
        }
        static void welcomeMes(string mes)
        {
            int latency = 10;
            Console.Write("░");
            System.Threading.Thread.Sleep(latency);
            Console.SetCursorPosition(0, 0);
            Console.Write("▒░");
            System.Threading.Thread.Sleep(latency);
            Console.SetCursorPosition(0, 0);
            Console.Write("▓▒░");
            System.Threading.Thread.Sleep(latency);
            Console.SetCursorPosition(0, 0);
            Console.Write("█▓▒░");
            System.Threading.Thread.Sleep(latency);
            Console.SetCursorPosition(0, 0);
            Console.Write("▓█▓▒░");
            System.Threading.Thread.Sleep(latency);
            Console.SetCursorPosition(0, 0);
            Console.Write("▒▓█▓▒░");
            System.Threading.Thread.Sleep(latency);
            Console.SetCursorPosition(0, 0);
            Console.Write("░▒▓█▓▒░");
            System.Threading.Thread.Sleep(latency);
            for (int i = 0; i < mes.Length; i++)
            {
                Console.SetCursorPosition(i, 0);
                if (i < mes.Length - 6)
                    Console.Write(mes[i].ToString() + "░▒▓█▓▒░");
                else Console.Write(mes[i].ToString() + "░▒▓█▓▒░".Substring(0, mes.Length - i));
                System.Threading.Thread.Sleep(latency);
            }
            Console.SetCursorPosition(mes.Length,0);
            Console.Write("  ");
            System.Threading.Thread.Sleep(700);
        }
        static void Main(string[] args)
        {
            welcomeMes("press tab to open menu ");
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
                        {
                            openPath((path.Length > 3) ? path + '\\' + dirs[selected] : path + dirs[selected]);
                            break;
                        }
                        else
                            try
                            {
                                System.Diagnostics.Process.Start(path + '\\' + files[selected - dirs.Length]);
                            }
                            catch { }
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
                        string op1 = (selected < dirs.Length) ? "Open selected in explorer" : "Run width argument";
                        string mes = menu(new string[] { "Back", "Goto", "Home", "Info (path)", "info (selected)" ,"Delete","Copy", op1,"Open path in explorer", "Previous", "Exit" }, "select an item then press enter\n");
                        {
                            if (mes == "Back")
                                ex = true;
                            else if (mes == "Goto")
                            {
                                Console.Clear();
                                ex = true;
                                Console.Write("enter a path to go to it (nothing to cancel) : ");
                                string p = Console.ReadLine();
                                while (p.Length > 1 && !System.IO.Directory.Exists(p))
                                {
                                    Console.Write("enter a valid path to go to it (nothing to cancel) : ");
                                    p = Console.ReadLine();
                                }
                                openPath(p);
                            }
                            else if (mes == "Home")
                            {
                                ex = true;
                                openPath("");
                            }
                            else if (mes == "Info (path)")
                            {
                                Console.Clear();
                                ex = true;
                                Console.WriteLine(path);
                                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                                Console.WriteLine("CreationTime : " + di.CreationTime.ToLongDateString() + "  " + di.CreationTime.ToLongTimeString());
                                Console.WriteLine("LastAccessTime : " + di.LastAccessTime.ToLongDateString() + "  " + di.LastAccessTime.ToLongTimeString());
                                Console.WriteLine("LastWriteTime : " + di.LastWriteTime.ToLongDateString() + "  " + di.LastWriteTime.ToLongTimeString());
                                Console.WriteLine(di.Attributes.ToString());
                                Console.Write("press any key to continue");
                                Console.ReadKey(true);
                            }
                            else if (mes == "info (selected)")
                            {
                                Console.Clear();
                                ex = true;
                                if (selected < dirs.Length)
                                {
                                    Console.WriteLine(path+"\\"+dirs[selected]);
                                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path + "\\" + dirs[selected]);
                                    Console.WriteLine("directory CreationTime : " + di.CreationTime.ToLongDateString() + "  " + di.CreationTime.ToLongTimeString());
                                    Console.WriteLine("directory LastAccessTime : " + di.LastAccessTime.ToLongDateString() + "  " + di.LastAccessTime.ToLongTimeString());
                                    Console.WriteLine("directory LastWriteTime : " + di.LastWriteTime.ToLongDateString() + "  " + di.LastWriteTime.ToLongTimeString());
                                    Console.WriteLine(di.Attributes.ToString());
                                    Console.Write("press any key to continue");
                                    Console.ReadKey(true);
                                }
                                else
                                {

                                    Console.WriteLine(path + "\\" + files[selected - dirs.Length]);
                                    System.IO.FileInfo di = new System.IO.FileInfo(path + "\\" + files[selected - dirs.Length]);
                                    float gbFileSize = (float)(di.Length / 1024.0 / 1024.0 );
                                    Console.WriteLine("file size : " + di.Length.ToString() + " Bytes            "+((gbFileSize>=921.6)?"("+(gbFileSize/1024.0).ToString()+" GB)":((gbFileSize >= 0.9216)? "(" + (gbFileSize).ToString() + " MB)" : "(" + (gbFileSize * 1024.0).ToString() + " KB)")));
                                    Console.WriteLine("file CreationTime : " + di.CreationTime.ToLongDateString() + "  " + di.CreationTime.ToLongTimeString());
                                    Console.WriteLine("file LastAccessTime : " + di.LastAccessTime.ToLongDateString() + "  " + di.LastAccessTime.ToLongTimeString());
                                    Console.WriteLine("fileLastWriteTime : " + di.LastWriteTime.ToLongDateString() + "  " + di.LastWriteTime.ToLongTimeString());
                                    Console.WriteLine(di.Attributes.ToString());
                                    Console.Write("press any key to continue");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (mes == "Delete")
                            {
                                Console.Clear();
                                ex = true;
                                Console.Write("Are you sure? (yes=yes , something else=no)");
                                if (Console.ReadLine() == "yes")
                                {
                                    if (selected < dirs.Length)
                                        delete(path + "\\" + dirs[selected]);
                                    else
                                        delete(path + "\\" + files[selected - dirs.Length]);
                                    openPath(path);
                                }
                                System.Threading.Thread.Sleep(1000);
                            }

                            else if (selected>=dirs.Length&&mes == "Copy")
                            {
                                Console.Clear();
                                ex = true;
                                Console.Write("enter path to move (dont forget file name and extention) : ");
                                string path2 = Console.ReadLine();
                                try
                                {
                                    System.IO.File.Copy(path + "\\" + files[selected - dirs.Length], path2);
                                    Console.Write("ok");
                                }
                                catch(Exception exe) { Console.Write(exe.Message); }
                                System.Threading.Thread.Sleep(1000);
                            }
                            else if (mes == op1)
                            {
                                if (selected < dirs.Length)
                                {
                                    ex = true; 
                                    System.Diagnostics.Process.Start(path + '\\' + dirs[selected]);
                                }
                                else
                                {
                                    Console.Clear();
                                    ex = true;
                                    Console.Write("enter argument : ");
                                    System.Diagnostics.Process.Start(path + '\\' + files[selected - dirs.Length], Console.ReadLine());
                                }
                            }
                            else if(mes== "Open path in explorer")
                            {
                                ex = true;
                                System.Diagnostics.Process.Start(path + '\\' + dirs[selected]);
                            }
                            else if (mes == "Previous")
                            {
                                ex = true;
                                prevPath.Insert(0, "Back");
                                int innerMes = menu2(prevPath.ToArray(), "select an item to go to it\n") - 1;
                                prevPath.RemoveAt(0);
                                if (innerMes != -1)
                                {
                                    string p = prevPath[innerMes];
                                    prevPath.RemoveAt(innerMes);
                                    openPath(p);
                                }
                            }
                            else if (mes == "Exit")
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                        }
                        if (ex)
                            break;
                    }
                    else
                    {
                        if (selected < dirs.Length)
                        {
                            for (int n=(selected<dirs.Length-1)?selected+1:0; n < dirs.Length; n++)
                            {
                                if (Char.ToLower(dirs[n][0]) == Char.ToLower(ck.KeyChar))
                                {
                                    selected = n;
                                    break;
                                }
                                else if (n == dirs.Length - 1)
                                {
                                    for (int m = 0; m < dirs.Length; m++)
                                        if (Char.ToLower(dirs[m][0]) == Char.ToLower(ck.KeyChar))
                                        {
                                            selected = m;
                                            break;
                                        }
                                }

                            }
                        }
                        else
                        {

                            for (int n = (selected-dirs.Length < files.Length - 1) ? selected-dirs.Length + 1 : dirs.Length; n < files.Length; n++)
                            {
                                if (Char.ToLower(files[n][0]) == Char.ToLower(ck.KeyChar))
                                {
                                    selected = dirs.Length + n;
                                    break;
                                }
                                else if (n == files.Length - 1)
                                {
                                    for (int m = 0; m < files.Length; m++)
                                        if (Char.ToLower(files[m][0]) == Char.ToLower(ck.KeyChar))
                                        {
                                            selected = m+dirs.Length;
                                            break;
                                        }
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
        static bool dirDelete(string path)
        {
            bool ret = true;
            foreach (string x in System.IO.Directory.GetFiles(path))
            {
                try { System.IO.File.Delete(x); }
                catch { ret = false; };
            }
            foreach (string x in System.IO.Directory.GetFiles(path))
            {
                if (!dirDelete(x))
                    ret = false;
            }
            try
            {
                if (ret)
                    System.IO.Directory.Delete(path);
                return ret;
            }
            catch { return false; }
        }
        static bool delete(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                if (dirDelete(path))
                {
                    Console.Write("ok. the directory was deleted");
                    return true;
                }
                else
                {
                    Console.Write("something went wrong !!!");
                    return true;
                }
            }
            else
            {
                try { System.IO.File.Delete(path); return true; }
                catch { return false; };
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
        static int menu2(string[] items, string title)
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
                    return selected;
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


