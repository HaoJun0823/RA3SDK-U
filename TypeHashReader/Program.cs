using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace TypeHashReader
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

        static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("ERROR:<DIRECTIORY>");
            }
            else
            {

                try
                {

                    DoTask(new DirectoryInfo(args[0]));


                }catch(Exception e)
                {
                    Console.WriteLine("Error:" + e.Message);
                }

            }


        }

        static string ByteStringBuilder(byte[] bytes)
        {
            StringBuilder strBuild = new StringBuilder();


            for(int i = 0; i < bytes.Length; i++)
            {

                strBuild.Append(bytes[i].ToString("X2"));
                if (i != bytes.Length - 1)
                {
                    strBuild.Append("-");
                }
                

            }

            return strBuild.ToString();

        }


        static void DoTask(DirectoryInfo dir)
        {

            foreach(FileInfo file in dir.GetFiles())
            {
                DataBlock temp = new DataBlock();
                temp.TypeID = new byte[4];
                temp.InstanceID = new byte[4];
                temp.TypeHash = new byte[4];
                temp.InstanceHash = new byte[4];

                temp.Name = file.Name;

                using (BinaryReader br = new BinaryReader(file.OpenRead()))
                {

                    for(int i = 0; i < 4; i++)
                    {
                        temp.TypeID[i] = br.ReadByte();
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        temp.InstanceID[i] = br.ReadByte();
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        temp.TypeHash[i] = br.ReadByte();
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        temp.InstanceHash[i] = br.ReadByte();
                    }




                }


                list.Add(temp);


            }

            XmlDocument xd = new XmlDocument();
            XmlElement xel = xd.CreateElement("Group");


            foreach(DataBlock temp in list)
            {
                XmlElement xe = xd.CreateElement("Name");
                xe.SetAttribute("Name",temp.Name);
                xe.SetAttribute("TypeID", ByteStringBuilder(temp.TypeID));
                xe.SetAttribute("InstanceID", ByteStringBuilder(temp.InstanceID));
                xe.SetAttribute("TypeHash", ByteStringBuilder(temp.TypeHash));
                xe.SetAttribute("InstanceHash", ByteStringBuilder(temp.InstanceHash));
                xel.AppendChild(xe);


            }
            xd.AppendChild(xel);
            xd.Save("result.xml");


        }
    }
}
