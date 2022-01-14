using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace SageManifestHeaderFixer
{
    class Program
    {


        static readonly Byte[] GAME6 = { 0x00, 0x00, 0x00, 0x06 };
        static readonly Byte[] GAMEMOD6 = { 0x00, 0x01, 0x00 ,0x06 };
        static readonly Byte[] GAME7 = { 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x01 };
        static Byte[] FileBytes;



        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (args.Length > 1 || args.Length <= 0)
            {
                Console.WriteLine("ERROR ARGS!");
                Console.WriteLine("Input: <FILE|DIRECTORY> REQUIRED<.MANIFEST>");
            }
            else
            {
                try
                {
                    if (Directory.Exists(args[0]))
                    {
                        ChangeDirectory(new DirectoryInfo(args[0]));
                    }
                    else
                    if (File.Exists(args[0]))
                    {
                        ChangeFile(new FileInfo(args[0]));
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            sw.Stop();
            Console.WriteLine("Task Finished! Cost Time:" + sw.ElapsedMilliseconds);

        }



        static void ChangeDirectory(DirectoryInfo directory)
        {
            Console.WriteLine("M Open D:" + directory.FullName);
            foreach (FileInfo f in directory.GetFiles("*.manifest",SearchOption.AllDirectories))
            {
                ChangeFile(f);
            }



        }


        static void ChangeFile(FileInfo file)
        {
            Console.WriteLine("M Open F:" + file.FullName);
            FileBytes = File.ReadAllBytes(file.FullName);
            Console.WriteLine("File {0},Length:{1}", file.Name, FileBytes.Length);

            
            Byte[] byteRead = BytesSlice(FileBytes,0,8);
            Console.Write("Header Data:");

            foreach(byte b in byteRead)
            {
                Console.Write(Convert.ToString(b,16).ToUpper() + " ");
            }

            Console.WriteLine();


            if (DiffBytes(byteRead,GAME7)){
                Console.WriteLine("It's Version 7!");
                To6(file);
            }else
            if (DiffBytes(byteRead, GAME6) || DiffBytes(byteRead, GAMEMOD6))
            {
                Console.WriteLine("It's Version 6!");
                To7(file);
            }
            else
            {
                throw new Exception("It is not a Manifest!(Version 6 or Version 7)!");
            }

            Console.WriteLine("Done!");

        }

        static Byte[] BytesSlice(Byte[] bytes, int index, int length)
        {
            List<Byte> list = new List<Byte>();

            for(int i = index; i < index + length; i++)
            {

                list.Add(bytes[i]);
            }



            return list.ToArray();
        }


        static void To6(FileInfo file)
        {
            Console.WriteLine("Change to Version 6!");
            List<byte> list = new List<byte>(GAME6);

            for(int i = 8; i< file.Length; i++)
            {
                list.Add(FileBytes[i]);
            }

            File.WriteAllBytes(file.FullName,list.ToArray());

        }

        static void To7(FileInfo file)
        {
            Console.WriteLine("Change to Version 7!");
            List<byte> list = new List<byte>(GAME7);

            for (int i = 4; i < file.Length; i++)
            {
                list.Add(FileBytes[i]);
            }

            File.WriteAllBytes(file.FullName, list.ToArray());
        }


        static bool DiffBytes(Byte[] l, Byte[] s)
        {

            Console.Write("Diff Data:");

            foreach (byte b in l)
            {
                Console.Write(Convert.ToString(b, 16).ToUpper() + " ");
            }
            Console.Write("|");
            foreach (byte b in s)
            {
                Console.Write(Convert.ToString(b, 16).ToUpper() + " ");
            }
            Console.WriteLine();


            for (int i = 0; i < s.Length; i++)
            {

                if (s[i] != l[i])
                {
                    return false;
                }

            }

            return true;
        }
    }
}
