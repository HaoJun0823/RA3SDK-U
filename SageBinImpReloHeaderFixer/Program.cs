using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SageBinImpReloHeaderFixer
{
    class Program
    {

        static readonly Byte[] GAME7_BIN = { 0x00, 0x00, 0xBB, 0xBA };
        static readonly Byte[] GAME7_IMP = { 0x00, 0x00, 0xB1, 0xBA };
        static readonly Byte[] GAME7_RELO = { 0x00, 0x00, 0xBE, 0xBA };

        static Byte[] FileBytes;

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (args.Length > 1 || args.Length <= 0)
            {
                Console.WriteLine("ERROR ARGS!");
                Console.WriteLine("Input: <FILE|DIRECTORY> REQUIRED<.BIN|.IMP|.RELO>");
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
                    if (File.Exists(args[0])){
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
            Console.WriteLine("BIR Open D:" + directory.FullName);

            
            foreach (FileInfo f in directory.GetFiles("*.bin",SearchOption.AllDirectories))
            {
                Console.WriteLine("Bin Open D:" + directory.FullName);
                ChangeFile(f);
            }

            foreach (FileInfo f in directory.GetFiles("*.imp", SearchOption.AllDirectories))
            {
                Console.WriteLine("Imp Open D:" + directory.FullName);
                ChangeFile(f);
            }

            foreach (FileInfo f in directory.GetFiles("*.relo", SearchOption.AllDirectories))
            {
                Console.WriteLine("Relo Open D:" + directory.FullName);
                ChangeFile(f);
            }



        }

        static void ChangeFile(FileInfo file)
        {

            Console.WriteLine("BIR Open F:" + file.FullName);
            FileBytes = File.ReadAllBytes(file.FullName);
            Console.WriteLine("File {0},Length:{1}", file.Name, FileBytes.Length);


            Byte[] byteRead = BytesSlice(FileBytes, 0, 4);
            Console.Write("Header Data:");

            foreach (byte b in byteRead)
            {
                Console.Write(Convert.ToString(b, 16).ToUpper() + " ");
            }

            Console.WriteLine("");

            bool flag = true;

            Console.WriteLine("Check Bin Version 7...");
            if (flag&&DiffBytes(byteRead, GAME7_BIN))
            {
                To6(file);
                flag = false;
            }
            else
            {
                Console.WriteLine("Not Bin Version 7...");
            }

            Console.WriteLine("Check Relo Version 7...");
            if (flag && DiffBytes(byteRead, GAME7_RELO))
            {
                To6(file);
                flag = false;
            }
            else
            {
                Console.WriteLine("Not Relo Version 7...");
            }

            Console.WriteLine("Check Imp Version 7...");
            if (flag && DiffBytes(byteRead, GAME7_IMP))
            {
                To6(file);
                flag = false;
            }
            else
            {
                Console.WriteLine("Not Imp Version 7...");
            }

            if (flag)
            {

                string ext = Path.GetExtension(file.FullName).ToUpper();
                Console.WriteLine("Extension:"+ext);
                switch (ext)
                {
                    case ".BIN":
                        To7(file, GAME7_BIN);
                        break;
                    case ".IMP":
                        To7(file, GAME7_IMP);
                        break;
                    case ".RELO":
                        To7(file, GAME7_RELO);
                        break;
                    default:
                        Console.WriteLine("It is not a bin or imp or relo!(Version 6 or Version 7)!");
                        break;

                }
            }

            Console.WriteLine("Done!");

        }

        static Byte[] BytesSlice(Byte[] bytes, int index, int length)
        {
            List<Byte> list = new List<Byte>();

            for (int i = index; i < index + length; i++)
            {

                list.Add(bytes[i]);
            }



            return list.ToArray();
        }


        static void To6(FileInfo file)
        {
            Console.WriteLine("Change to Version 6!");

            File.WriteAllBytes(file.FullName, BytesSlice(FileBytes, 4, FileBytes.Length - 4));

        }

        static void To7(FileInfo file, Byte[] header)
        {
            Console.WriteLine("Change to Version 7!");
            List<byte> list = new List<byte>(header);

            for (int i = 0; i < file.Length; i++)
            {
                list.Add(FileBytes[i]);
            }

            File.WriteAllBytes(file.FullName, list.ToArray());
        }


        static bool DiffBytes(Byte[] l, Byte[] s)
        {

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
