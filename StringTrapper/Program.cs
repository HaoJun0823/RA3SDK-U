using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StringTrapper
{
    class Program
    {

        static FileInfo OriginFile;
        static FileInfo ResultTXT;
        static long StartOffset = 0;
        static Byte[] SplitCode;
        static Byte[] EndCode;
        static List<string> list = new List<string>();

        static void Main(string[] args)
        {

            Console.Write("YOUR INPUT:");
            PrintArray(args);
            if (args.Length != 5)
            {
                Console.WriteLine("ERORR! NEED INPUT:1-[FILE];2-[START OFFSET HEX];3-[RESULT TXT FILE];4-[SPLIT CODE];5-[END CODE]");

                
                return;
            }
            else
            {
                try
                {
                    OriginFile = new FileInfo(args[0]);
                    StartOffset = Convert.ToInt64(args[1],16);
                    ResultTXT = new FileInfo(args[2]);
                    SplitCode = StringToByteArray(args[3]);
                    EndCode = StringToByteArray(args[4]);
                    DoTask();
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR:" + e.Message+"\n"+e.StackTrace);
                }
            }

        }

        static Byte[] StringToByteArray(string str)
        {

            if (str.Length == 2)
            {
                return new byte[] { Convert.ToByte(str,16) };
            }

            string[] arr = str.Split('-');

            List<byte> listx = new List<byte>();

            for (int i = 0; i < arr.Length; i++)
            {
                listx.Add(Convert.ToByte(arr[i],16));
            }

            return listx.ToArray();

        }

        static void PrintArray(Object[] args)
        {
            foreach (Object o in args)
            {
                Console.Write("{0} ", o);
            }
            Console.WriteLine();
        }

        static void DoTask()
        {
            Console.WriteLine("Task On!");
            using (BinaryReader br = new BinaryReader(OriginFile.OpenRead()))
            {
                int endcount = 0;
                int splitcount = 0;
                br.BaseStream.Position = StartOffset;
                Console.WriteLine("Base Position:{0}",br.BaseStream.Position);
                bool flag = false;
                StringBuilder strBuild = new StringBuilder();
                StringBuilder strTempS = new StringBuilder();
                StringBuilder strTempE = new StringBuilder();
                while (endcount < EndCode.Length)
                {

                    if (splitcount >= SplitCode.Length)
                    {

                        if (strBuild.Length <= 0)
                        {
                            Console.WriteLine("NULL OR INVAILD");
                            strBuild.Clear();
                            strTempS.Clear();
                            splitcount = 0;
                            continue;
                        }
                        
                        list.Add(strBuild.ToString());
                        

                        Console.WriteLine("Add [{0}]",strBuild.ToString());
                        strBuild.Clear();
                        strTempS.Clear();
                        splitcount = 0;
                        continue;
                    }

                    Console.Write("Next Position:{0}-", br.BaseStream.Position+1);
                    byte data = br.ReadByte();
                    Console.Write("C:{0}|D:{1}|S:{2},{3}|E:{4},{5}", Convert.ToChar(data),data, splitcount, SplitCode[splitcount], endcount, EndCode[endcount]);
                    if (data.Equals(SplitCode[splitcount]))
                    {
                       
                        splitcount++;
                        Console.Write("+SPLIT:{0}", splitcount);


                        strTempS.Append(Convert.ToChar(data));
                    }
                    else
                    {
                        splitcount = 0;

                        if (strTempS.Length != 0)
                        {
                            strBuild.Append(strTempS);
                        }

                        
                        strTempS.Clear();
                        flag = true;
                    }

                    if (data.Equals(EndCode[endcount]))
                    {
                        
                        endcount++;
                        Console.Write("+END:{0}", endcount);
                        strTempE.Append(Convert.ToChar(data));
                    }
                    else
                    {
                        endcount = 0;
                        if (flag&&strTempE.Length!=0)
                        {
                            strBuild.Append(strTempE);
                        }


                        strTempE.Clear();
                    }

                    if (splitcount == 0 && endcount == 0)
                    {
                        
                        strBuild.Append(Convert.ToChar(data));
                        Console.Write("+APPEND:{0}", strBuild.ToString());
                    }
                    flag = false;
                    Console.WriteLine();





                }




            }
            ResultTXT.Delete();
            using (StreamWriter sw = new StreamWriter(ResultTXT.OpenWrite()))
            {
                foreach (string s in list)
                {
                    sw.WriteLine(s);
                }

            }


        }


    }
}
