using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Reflection;

namespace AssetTypeHashHeaderFixer
{

    struct DataBlock
    {
        public string Name;
        public byte[] TypeID;
        public byte[] InstanceID;
        public byte[] TypeHash;
        public byte[] InstanceHash;
    }

    class Program
    {
        static List<DataBlock> list = new List<DataBlock>();
        static Dictionary<string, DataBlock> map = new Dictionary<string, DataBlock>();

        static void init()
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Properties.Resources.ResourceManager.GetObject("TypeHashDataGroup") as string);
            XmlNode xn = xd.GetElementsByTagName("Group")[0];
            foreach(XmlNode xe in xn)
            {
                if(xe.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }
                XmlElement xex = (XmlElement)xe;
                DataBlock db = new DataBlock();
                db.Name = xex.GetAttribute("Name");
                db.TypeID = String2ByteArray(xex.GetAttribute("TypeID"));
                db.InstanceID = String2ByteArray(xex.GetAttribute("InstanceID"));
                db.TypeHash = String2ByteArray(xex.GetAttribute("TypeHash"));
                db.InstanceHash = String2ByteArray(xex.GetAttribute("InstanceHash"));

                Console.WriteLine("Register {0} ID: {1}", db.Name, xex.GetAttribute("TypeID"));
                map.Add(xex.GetAttribute("TypeID"),db);
                
            }
            

        }

        static Byte[] String2ByteArray(String data)
        {
            Byte[] result = new byte[4];

            string[] str = data.Split('-');

            for(int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(str[i], 16);
            }



            return result;
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            if (args.Length != 1)
            {
                Console.WriteLine("ERROR:<FILE>|<DIRECTORY>");
            }
            else
            {

                try
                {
                    init();

                    if (File.Exists(args[0]))
                    {
                        FileInfo file = new FileInfo(args[0]);
                        DoTask(file);
                    }
                    else if (Directory.Exists(args[0]))
                    {
                        DirectoryInfo directory = new DirectoryInfo(args[0]);
                        DoMultipleTask(directory);
                    }

                    


                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR:" + e.Message+"\n"+e.StackTrace);
                }

            }
            sw.Stop();
            Console.WriteLine("Be One With Yuri! Cost:" + sw.ElapsedMilliseconds);
        }


        static void DoTask(FileInfo file)
        {
            DataBlock db = new DataBlock();
            using (BinaryReader br = new BinaryReader(file.OpenRead()))
            {
                Byte[] arr = new byte[4];

                for(int i = 0; i < arr.Length; i++)
                {
                    arr[i] = br.ReadByte();
                }
                string header = ByteStringBuilder(arr);
                Console.WriteLine("File:{0} Header:{1}",file.Name,header);

                br.BaseStream.Position = 8;
                Console.WriteLine("Set Position...");
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = br.ReadByte();
                }



                if (map.ContainsKey(header))
                {
                    db = map[header];
                    Console.WriteLine("Origin File:{0} TypeHash:{1}", file.Name, ByteStringBuilder(arr));
                    Console.WriteLine("Target File:{0} TypeHash:{1}", file.Name, ByteStringBuilder(db.TypeHash));

                    if (ByteStringBuilder(arr).Equals(ByteStringBuilder(db.TypeHash)))
                    {
                        Console.WriteLine("Information:{0} Don't Need Fix.",file.Name);
                        return;
                    }

                }
                else
                {
                    Console.WriteLine("ERROR! This is not a vaild asset!({0} MISS HEADER? {1})",file.Name,header);
                    return;
                }


            }

            if(db.Name.Length<=0)
            {
                Console.WriteLine("ERROR! File Data Block Name <= 0!({0})", file.Name);
                return;
            }
            Console.WriteLine("Found Data {0} Cook For File {1}",db.Name,file.Name);
            using (BinaryWriter bw = new BinaryWriter(file.OpenWrite()))
            {
                bw.BaseStream.Position = 8;
                Console.WriteLine("Write To {0}",file.Name);

                for(int i = 0; i < db.TypeHash.Length; i++)
                {
                    Console.WriteLine("Write Data Name By {0} Type Hash By {1} At {2}",db.Name,db.TypeHash[i].ToString("X2"), bw.BaseStream.Position);
                    bw.Write(db.TypeHash[i]);
                    
                }
                bw.Flush();


            }

            Console.WriteLine("Done!");




        }


        static void DoMultipleTask(DirectoryInfo directory)
        {

            foreach (FileInfo file in directory.GetFiles())
            {
                DoTask(file);
            }


        }

        static string ByteStringBuilder(byte[] bytes)
        {
            StringBuilder strBuild = new StringBuilder();


            for (int i = 0; i < bytes.Length; i++)
            {

                strBuild.Append(bytes[i].ToString("X2"));
                if (i != bytes.Length - 1)
                {
                    strBuild.Append("-");
                }


            }

            return strBuild.ToString();

        }

        static DataBlock Search(Byte[] arr)
        {

            foreach (DataBlock db in list)
            {
                for (int i = 0; i < db.TypeID.Length; i++)
                {
                    if (db.TypeID[i] != arr[i])
                    {
                        break;
                    }
                    else if (i == db.TypeID.Length - 1)
                    {
                        return db;
                    }

                }
                
            }
            return new DataBlock();


        }

    }
}
