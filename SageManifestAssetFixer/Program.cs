using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Web;

namespace SageManifestAssetFixer
{
    class Program
    {



        struct HardDataBlock
        {

            public string Name_0;
            public string Name_1;
            public bool Unknown_0;
            public bool Unknown_1;
            public byte[] TypeID_0;
            public byte[] TypeHash_0;
            public byte[] TypeID_1;
            public byte[] TypeHash_1;
        }

        struct SimpleDataBlock
        {
            public string Name;
            public bool Unknown;
            public byte[] TypeID;
            public byte[] TypeHash;
        }

        struct DataBlock
        {
            public string Name;
            public byte[] TypeID;
            public byte[] InstanceID;
            public byte[] TypeHash;
            public byte[] InstanceHash;
        }




        static readonly Byte[] GAME7 = { 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x01 };
        static readonly string RA3_0 = "11280974,1507896455,75989905,2052546860,89767986,3054734349,91719417,2789100025,97402485,3806481831,104126924,3160055664,105587076,1802638191,113029208,3439477741,134745024,2853539982,165611890,1129081393,191082224,2620092030,241329492,1267843369,254471188,990517568,299416263,21805907,300273508,3959342059,315614191,570655738,343094706,1360895287,346077409,2270316023,354222972,3645429389,357806494,1889946602,376113229,1405623879,402082255,3814569307,417176643,655411104,442957564,1448492760,474482543,3791548904,485977567,1542859068,530081230,751047049,531798225,1251212385,534008255,2345543630,544776545,2695845168,565855655,2717629172,568797146,2673866970,582251659,339527263,587347702,2217996198,607123156,2144861754,608742960,1430609896,611126373,549522065,611271731,1517904520,680780553,1276225790,686292351,1465656250,694838069,4192036989,731899443,118801026,741706624,886642248,772462768,875756023,814796096,840636063,815686049,1966615229,838130914,1117246753,926814458,86137284,951036436,2082140658,970663416,2629630449,980180622,1679479598,1014034471,2969006564,1044178106,898281629,1047799468,1315713652,1138847078,1373916399,1151702845,1544456513,1184276792,1336753171,1213037816,981786901,1218703046,1075836011,1234485619,3237545121,1238702511,1469314420,1249197161,2873907483,1276968974,3983232506,1277109486,4038536099,1296329066,1187295208,1302035577,3170370516,1318336226,1815171461,1334543376,3689232783,1343682755,532337471,1345252658,1378748105,1350608344,2226564801,1371131236,197748432,1371977384,301556505,1431290504,174664052,1443425905,3631141684,1447728787,2530876419,1449288224,4177308976,1469055344,3973930514,1469498551,2736882942,1474300054,1098794956,1479531562,525928939,1482556238,3502529123,1499648070,1611277776,1507490649,2857307522,1532214660,4106878882,1546953756,2225127171,1559937240,2264980354,1566941920,2999033148,1626415288,890675481,1634186921,607961670,1641540160,3985956449,1644972043,232598955,1654297531,2835608034,1697313181,3604410282,1710207315,955128953,1795213437,1676252299,1802643394,304552910,1804891371,2510507971,1807540972,3038881679,1870180772,1460143826,1874610847,550595366,1883691512,130352738,1887864852,1145979021,1921726501,916455758,1931769760,1959031257,1956336137,693782642,1963196508,41691366,1968811301,2503358751,1974922393,276486244,2007776008,1553493937,2010562192,76343204,2014061824,3544626863,2047841880,3936894160,2067763016,2390364070,2070603733,1124141471,2074164365,3494287844,2101756272,1496519333,2131237081,903715207,2140592493,3288487200,2170044043,360514692,2178440954,1048263193,2219670431,1443638853,2254974584,1817802683,2278083954,3993871439,2281773838,4074957085,2283998413,2326182616,2343360052,2196796046,2359666647,3222028787,2361666546,2947372396,2368878637,2109862985,2383722691,1770851834,2384988189,320610464,2407383451,3647979872,2421413379,4189839782,2423780215,545326116,2430161325,3795761625,2467477932,4165504734,2486173485,2666354233,2496977262,4172139043,2505151835,96366804,2516466693,3122351307,2524893660,325319211,2525284163,1210708834,2525492603,1246977355,2590317961,1993395951,2591692061,1944261033,2599418799,4073471,2635272840,4048847353,2697587777,129995987,2728605516,469423908,2738273474,3442548366,2742786440,1752851487,2745675575,4154219145,2800139175,2481075826,2802565590,2668810035,2808773267,2375239281,2818967523,3311890413,2836737428,1088041681,2846460739,1685827550,2873949396,4072122752,2891878770,884927254,2893598307,3564957031,2901356964,1764459959,2905958645,187609259,2912385462,156475083,2912435838,2349512303,2913856066,1181471134,2914137727,1938882503,2915726794,3221193174,2957071942,3017648610,2958905921,2908721247,2964891936,3311886287,2987234945,1863152725,2995571269,1905565848,2996766068,2164060776,3048039928,3780991768,3057018816,18723467,3058114834,725292512,3072160547,44045492,3094553884,3862996978,3166846828,1029381398,3169690211,616355229,3175516876,4275530952,3188107749,1300486550,3266421346,3448375452,3278684478,1247287143,3279338544,1560001831,3319822471,3497422169,3339496789,1644774436,3359870461,73026796,3366599054,2875609897,3374519264,87133761,3408790761,2508525962,3420020972,1007090371,3423839194,1141570868,3476397162,3050031378,3477794083,3533302842,3516764358,738011492,3517887782,2961656853,3548787763,1873318063,3558134211,3525217272,3569007303,3617141059,3575834349,3529865568,3587539190,213154423,3604279694,257609346,3614134471,4192635734,3620764004,3267267860,3645205636,3852026165,3716369729,2581745695,3727134806,3386360116,3741098742,1229544125,3746130610,3018771479,3756168709,144321893,3784442503,334318473,3786401627,2818203443,3799634821,226620221,3809842710,1440657035,3810008068,1848047257,3827723072,1092854186,3898802054,4258032094,3899542881,2484138270,3901433122,1804668011,3906227957,2503036394,3919360587,815199407,3928762264,1830167803,3959844197,2330506892,3971904488,1878382208,3972178387,1310384366,3974617950,4161179766,3976885676,2900176681,3978455785,282482670,4007043648,855988878,4026878150,3889487240,4039592522,2797111417,4042295058,1637720895,4044322798,2789252562,4063352333,3941033891,4094578721,4253066583,4098431913,743017490,4102242385,3000504919,4140422551,3882873771,4141306833,133543122,4143989686,472971368,4157475773,903854585,4180354873,3463571609,4243804314,89086670,4262364347,333131708,4272453293,2097642646";
        static readonly string RA3_1 = "11280974,1507896455,39814226,314822194,75989905,2052546860,89767986,3054734349,91719417,2789100025,97402485,3806481831,104126924,3160055664,105587076,1802638191,113029208,1656677095,134745024,2312628881,165611890,1129081393,191082224,2620092030,241329492,1267843369,254471188,3410848344,277107587,2317778777,299416263,21805907,300273508,3959342059,315614191,103519233,322325003,2620066145,343094706,1360895287,346077409,2270316023,354222972,3645429389,357806494,1889946602,376113229,1405623879,402082255,3814569307,417176643,655411104,442957564,1448492760,474482543,3791548904,526310600,2888879139,530081230,751047049,531798225,1251212385,534008255,682443293,544776545,2695845168,565855655,2717629172,568797146,2673866970,582251659,339527263,587347702,2217996198,607123156,3226236577,608742960,1430609896,611126373,549522065,611271731,1517904520,680780553,1276225790,686292351,1465656250,694838069,4192036989,731899443,118801026,741706624,886642248,772462768,875756023,814796096,381021378,815686049,1966615229,838130914,1557773986,926814458,250226167,951036436,2082140658,970663416,2629630449,980180622,2699179992,1014034471,2969006564,1044178106,898281629,1047799468,615677912,1138847078,151653059,1151702845,3748836538,1184276792,1336753171,1213037816,1906897608,1218703046,1075836011,1234485619,3237545121,1238702511,1469314420,1249197161,1278519116,1276968974,3983232506,1277109486,4038536099,1280789292,607470343,1296329066,1187295208,1302035577,3170370516,1318336226,1815171461,1334543376,3689232783,1343682755,532337471,1345252658,1378748105,1350608344,2226564801,1371131236,197748432,1371977384,301556505,1431290504,3436385791,1443425905,3631141684,1447728787,2530876419,1449288224,4177308976,1469055344,1859711999,1469498551,2486913666,1474300054,1098794956,1479531562,525928939,1482556238,61741584,1507490649,2857307522,1532214660,1144322423,1546953756,2225127171,1559937240,2264980354,1566941920,2999033148,1601459053,417928170,1626415288,890675481,1634186921,607961670,1641540160,3985956449,1644972043,2203829159,1654297531,2835608034,1697313181,3604410282,1710207315,2376776400,1795213437,1676252299,1802643394,304552910,1804891371,3270592658,1807540972,3038881679,1870180772,1460143826,1874610847,3751675568,1883691512,130352738,1887864852,1145979021,1921726501,916455758,1931769760,1959031257,1956336137,693782642,1963196508,371552972,1968811301,2503358751,1974922393,276486244,2007776008,1553493937,2010562192,76343204,2014061824,3544626863,2047841880,3936894160,2067763016,2390364070,2070603733,687529096,2074164365,3494287844,2101756272,1964683919,2131237081,1090016465,2140592493,3288487200,2170044043,360514692,2178440954,3236296400,2219670431,1443638853,2254974584,397654061,2278083954,3993871439,2281773838,4074957085,2283998413,2326182616,2343360052,2196796046,2359666647,3222028787,2361666546,2947372396,2368878637,2109862985,2383722691,1770851834,2384988189,320610464,2403539258,3087440567,2407383451,3647979872,2421413379,2180997581,2423780215,1499516372,2430161325,3795761625,2467477932,4165504734,2486173485,3468295734,2496977262,669571460,2516466693,3122351307,2524893660,325319211,2525284163,1210708834,2525492603,1246977355,2590317961,1503451890,2591692061,1944261033,2599418799,4073471,2635272840,4048847353,2655847973,4249975357,2697587777,129995987,2728605516,469423908,2738273474,3442548366,2742786440,1752851487,2745675575,4154219145,2800139175,2481075826,2802565590,2668810035,2808773267,2375239281,2818967523,3311890413,2836737428,946454949,2846460739,1118458536,2873949396,4072122752,2891878770,2180208569,2893598307,3292022823,2901356964,1764459959,2905958645,3249516186,2912385462,156475083,2912435838,2349512303,2913856066,1181471134,2914137727,1938882503,2915726794,3221193174,2957071942,3017648610,2958905921,2908721247,2964891936,759840931,2987234945,1863152725,2995571269,1905565848,2996766068,2164060776,3048039928,3780991768,3057018816,1130654826,3058114834,725292512,3072160547,948454052,3094553884,3862996978,3166846828,1029381398,3169690211,616355229,3175516876,4275530952,3182468061,435433034,3188107749,1300486550,3266421346,3448375452,3278684478,1247287143,3279338544,1560001831,3282880182,2363093724,3319822471,1950506001,3339496789,1644774436,3359870461,73026796,3366599054,2875609897,3374519264,87133761,3408790761,2508525962,3420020972,1007090371,3423839194,1226594532,3424784030,3482723710,3476397162,3050031378,3477794083,558654915,3516764358,738011492,3517887782,2961656853,3548787763,45961792,3558134211,3525217272,3569007303,3617141059,3575309793,1143374314,3575834349,3529865568,3587539190,213154423,3604279694,3333529903,3614134471,4192635734,3620764004,3267267860,3645205636,3852026165,3716369729,2581745695,3727134806,3386360116,3737094024,2724763994,3741098742,1229544125,3746130610,3018771479,3756168709,144321893,3784442503,334318473,3786401627,1565904428,3799634821,226620221,3809842710,610414520,3810008068,1848047257,3827723072,1092854186,3898802054,4258032094,3899542881,343405281,3901433122,1804668011,3906227957,2503036394,3919360587,2610009338,3928762264,1830167803,3959844197,2330506892,3971904488,1878382208,3972178387,714445674,3974617950,4161179766,3976885676,2900176681,3978455785,282482670,4007043648,855988878,4026878150,3889487240,4039592522,1598727877,4042295058,1637720895,4044322798,2789252562,4063352333,1420218404,4094578721,4253066583,4098431913,743017490,4102242385,3000504919,4140422551,3882873771,4141306833,1240780838,4143989686,472971368,4157475773,903854585,4180354873,3463571609,4232364483,3130369778,4243804314,89086670,4262364347,333131708,4272453293,2097642646";
        static Byte[] FileBytes;
        static List<DataBlock> list = new List<DataBlock>();
        static Dictionary<string, DataBlock> map = new Dictionary<string, DataBlock>();
        static List<HardDataBlock> hdb_list = new List<HardDataBlock>();
        static Dictionary<string, HardDataBlock> hdb_map = new Dictionary<string, HardDataBlock>();
        static List<SimpleDataBlock> sdb_list = new List<SimpleDataBlock>();
        static List<HardDataBlock> last_list = new List<HardDataBlock>();

        static Dictionary<int, byte> origin_data = new Dictionary<int, byte>();
        static Dictionary<int, byte> target_data = new Dictionary<int, byte>();

        static List<DataBlock> ManifestBlockList = new List<DataBlock>();

        static int ManifestBlockNumber = 0;

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            init();
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
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                }

            }


            Console.WriteLine("Result:");

            foreach (KeyValuePair<int,byte> pair in origin_data)
            {
                Console.WriteLine("Key:{0},value:{1}", pair.Key,pair.Value);
            }

            foreach (KeyValuePair<int, byte> pair in target_data)
            {
                Console.WriteLine("Key:{0},value:{1}", pair.Key, pair.Value);
            }
            sw.Stop();
            Console.WriteLine("Task Finished! Cost Time:" + sw.ElapsedMilliseconds);
        }

        static void init()
        {

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Properties.Resources.ResourceManager.GetObject("TypeHashDataGroup") as string);
            XmlNode xn = xd.GetElementsByTagName("Group")[0];
            foreach (XmlNode xe in xn)
            {
                if (xe.NodeType == XmlNodeType.Comment)
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
                list.Add(db);
                map.Add(xex.GetAttribute("TypeID"), db);

            }
            init_s();




        }


        static void init_s()
        {
            string[] sra3_0 = RA3_0.Split(',');
            string[] sra3_1 = RA3_1.Split(',');

            Console.WriteLine("MAP X DATA LENGTH:{0}", list.Count);
            Console.WriteLine("RA3 0 DATA LENGTH:{0}", sra3_0.Length / 2);
            Console.WriteLine("RA3 1 DATA LENGTH:{0}", sra3_1.Length / 2);

            for (int i = 0; i < sra3_1.Length; i += 2)
            {

                long id = Convert.ToInt64(sra3_1[i]);
                long type = Convert.ToInt64(sra3_1[i + 1]);

                byte[] b_id = BitConverter.GetBytes(id);
                byte[] b_type = BitConverter.GetBytes(type);

                byte[] b_id4 = new byte[4];
                byte[] b_type4 = new byte[4];

                HardDataBlock hdb = new HardDataBlock();

                for (int x = 0; x < 4; x++)
                {
                    b_id4[x] = b_id[x];
                    b_type4[x] = b_type[x];
                }

                hdb.TypeID_1 = b_id4;
                hdb.TypeHash_1 = b_type4;

                string map_id;
                if (map.ContainsKey(ByteStringBuilder(b_id4)))
                {
                    map_id = map[ByteStringBuilder(b_id4)].Name;
                    hdb.Unknown_1 = false;
                }
                else
                {
                    map_id = "MISS:" + ByteStringBuilder(b_id4);
                    hdb.Unknown_1 = true;
                }



                hdb.Name_1 = map_id;


                Console.WriteLine("1:Name {2} ID {0} Hash {1}", ByteStringBuilder(b_id4), ByteStringBuilder(b_type4), map_id);


                hdb_list.Add(hdb);
                hdb_map.Add(hdb.Name_1, hdb);


            }

            for (int i = 0; i < sra3_0.Length; i += 2)
            {

                long id = Convert.ToInt64(sra3_0[i]);
                long type = Convert.ToInt64(sra3_0[i + 1]);

                byte[] b_id = BitConverter.GetBytes(id);
                byte[] b_type = BitConverter.GetBytes(type);

                byte[] b_id4 = new byte[4];
                byte[] b_type4 = new byte[4];

                SimpleDataBlock sdb = new SimpleDataBlock();

                for (int x = 0; x < 4; x++)
                {
                    b_id4[x] = b_id[x];
                    b_type4[x] = b_type[x];
                }

                sdb.TypeID = b_id4;
                sdb.TypeHash = b_type4;

                string map_id;
                if (map.ContainsKey(ByteStringBuilder(b_id4)))
                {
                    map_id = map[ByteStringBuilder(b_id4)].Name;
                    sdb.Unknown = false;
                }
                else
                {
                    map_id = "MISS:" + ByteStringBuilder(b_id4);
                    sdb.Unknown = true;
                }



                sdb.Name = map_id;


                Console.WriteLine("0:Name {2} ID {0} Hash {1}", ByteStringBuilder(b_id4), ByteStringBuilder(b_type4), map_id);


                sdb_list.Add(sdb);


            }


            //BUG BUT DON'T NEED CARES
            foreach (SimpleDataBlock sdb in sdb_list)
            {

                if (hdb_map.ContainsKey(sdb.Name))
                {
                    HardDataBlock hdb = hdb_map[sdb.Name];
                    hdb.Name_0 = sdb.Name;
                    hdb.TypeHash_0 = sdb.TypeHash;
                    hdb.TypeID_0 = sdb.TypeID;
                    hdb.Unknown_0 = sdb.Unknown;
                    last_list.Add(hdb);
                }
                else
                {

                    HardDataBlock hdb = new HardDataBlock();
                    hdb.Name_0 = sdb.Name;
                    hdb.TypeHash_0 = sdb.TypeHash;
                    hdb.TypeID_0 = sdb.TypeID;
                    hdb.Unknown_0 = sdb.Unknown;
                    last_list.Add(hdb);


                }




            }
            Console.WriteLine("LAST LIST:");
            Console.WriteLine("NAME_0,NAME_1,TYPEID_0,TYPEID_1,TYPEHASH_0,TYPEHASH_1,UNKNOWN_0,UNKNOWN_1");
            foreach (HardDataBlock hdb in last_list)
            {

                StringBuilder strBuild = new StringBuilder();





                if (hdb.Name_0 != null)
                {
                    strBuild.Append(hdb.Name_0);
                }

                strBuild.Append(',');

                if (hdb.Name_1 != null)
                {
                    strBuild.Append(hdb.Name_1);
                }

                strBuild.Append(',');

                if (hdb.TypeID_0 != null)
                {
                    strBuild.Append(ByteStringBuilder(hdb.TypeID_0));

                }

                strBuild.Append(',');

                if (hdb.TypeID_1 != null)
                {
                    strBuild.Append(ByteStringBuilder(hdb.TypeID_1));

                }

                strBuild.Append(',');

                if (hdb.TypeHash_0 != null)
                {
                    strBuild.Append(ByteStringBuilder(hdb.TypeHash_0));

                }

                strBuild.Append(',');

                if (hdb.TypeHash_1 != null)
                {
                    strBuild.Append(ByteStringBuilder(hdb.TypeHash_1));

                }

                strBuild.Append(',');

                if (hdb.Unknown_0)
                {
                    strBuild.Append(true);

                }
                else
                {

                    strBuild.Append(false);

                }

                strBuild.Append(',');

                if (hdb.Unknown_1)
                {
                    strBuild.Append(true);
                }
                else
                {
                    strBuild.Append(false);
                }

                Console.WriteLine(strBuild);

                hdb_map[hdb.Name_0] = hdb;

            }



        }

        static Byte[] String2ByteArray(String data)
        {
            Byte[] result = new byte[4];

            string[] str = data.Split('-');

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(str[i], 16);
            }



            return result;
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


        static void ChangeDirectory(DirectoryInfo directory)
        {
            Console.WriteLine("M Open D:" + directory.FullName);
            foreach (FileInfo f in directory.GetFiles("*.manifest", SearchOption.AllDirectories))
            {
                ChangeFile(f);
            }



        }

        static void BuildManifest()
        {

            int start = 0 + 47 + 4 + 1;
            Console.WriteLine("Start:{0}",start);
            DataBlock db = new DataBlock();

            //for (int c = 0; c < FileBytes.Length; c++)
            //{
            //    Console.Write("{1}:{0:X2} ", FileBytes[0 + c], c + 0);
            //}
            //Console.WriteLine();

            for (int i = 0; i < ManifestBlockNumber; i++)
            {
                byte[] tid = new byte[4];
                byte[] iid = new byte[4];
                byte[] th = new byte[4];
                byte[] ih = new byte[4];

                //Console.WriteLine("{1}tid:{0}", start + (i * 48) + 0, i);
                //Console.WriteLine("{1}tid:{0}", start + (i * 48) + 1, i);
                //Console.WriteLine("{1}tid:{0}", start + (i * 48) + 2, i);
                //Console.WriteLine("{1}tid:{0}", start + (i * 48) + 3, i);

                //Console.WriteLine("{1}iid:{0}", start + (i * 48) + 4, i);
                //Console.WriteLine("{1}iid:{0}", start + (i * 48) + 5, i);
                //Console.WriteLine("{1}iid:{0}", start + (i * 48) + 6, i);
                //Console.WriteLine("{1}iid:{0}", start + (i * 48) + 7, i);

                //Console.WriteLine("{1}th:{0}", start + (i * 48) + 8, i);
                //Console.WriteLine("{1}th:{0}", start + (i * 48) + 9, i);
                //Console.WriteLine("{1}th:{0}", start + (i * 48) + 10, i);
                //Console.WriteLine("{1}th:{0}", start + (i * 48) + 11, i);

                //Console.WriteLine("{1}ih:{0}", start + (i * 48) + 12, i);
                //Console.WriteLine("{1}ih:{0}", start + (i * 48) + 13, i);
                //Console.WriteLine("{1}ih:{0}", start + (i * 48) + 14, i);
                //Console.WriteLine("{1}ih:{0}", start + (i * 48) + 15, i);

                int offset = i * 48;

                for (int c = 0; c < 16; c++)
                {
                    Console.Write("{1}:{0:X2} ",FileBytes[start +offset + c],c + start +offset );
                }
                Console.WriteLine();

                tid[0] = FileBytes[start + offset + 0];
                tid[1] = FileBytes[start + offset + 1];
                tid[2] = FileBytes[start + offset + 2];
                tid[3] = FileBytes[start + offset + 3];

                iid[0] = FileBytes[start + offset + 4];
                iid[1] = FileBytes[start + offset + 5];
                iid[2] = FileBytes[start + offset + 6];
                iid[3] = FileBytes[start + offset + 7];

                th[0] = FileBytes[start + offset + 8];
                th[1] = FileBytes[start + offset + 9];
                th[2] = FileBytes[start + offset + 10];
                th[3] = FileBytes[start + offset + 11];

                ih[0] = FileBytes[start + offset + 12];
                ih[1] = FileBytes[start + offset + 13];
                ih[2] = FileBytes[start + offset + 14];
                ih[3] = FileBytes[start + offset + 15];

                db.TypeID = tid;
                db.InstanceHash = ih;
                db.InstanceID = iid;
                db.TypeHash = th;


                Console.WriteLine("{4}:TypeId {0} InstanceId {1} TypeHash {2} InstanceHash {3}", ByteStringBuilder(tid), ByteStringBuilder(iid), ByteStringBuilder(th), ByteStringBuilder(ih), i);

                

                if (map.ContainsKey(ByteStringBuilder(db.TypeID)))
                {

                    db.Name = map[ByteStringBuilder(db.TypeID)].Name;
                    DataBlock ndb = map[ByteStringBuilder(db.TypeID)];

                    Console.WriteLine("Found {0}",db.Name);
                    Console.WriteLine("!Write!:{0} To {1}", ByteStringBuilder(db.TypeHash), ByteStringBuilder(ndb.TypeHash));

                    FileBytes[start + offset + 8] = ndb.TypeHash[0];
                    FileBytes[start + offset + 9] = ndb.TypeHash[1];
                    FileBytes[start + offset + 10] = ndb.TypeHash[2];
                    FileBytes[start + offset + 11] = ndb.TypeHash[3];

                }
                else
                {
                    Console.WriteLine("!Warning!:{0} Is not alive in map.",ByteStringBuilder(db.TypeID));
;                }

                ManifestBlockList.Add(db);

            }




        }


        static void ChangeFile(FileInfo file)
        {
            Console.WriteLine("M Open F:" + file.FullName);
            FileBytes = File.ReadAllBytes(file.FullName);
            Console.WriteLine("File {0},Length:{1}", file.Name, FileBytes.Length);

            byte[] arr = new byte[8];

            using (BinaryReader br = new BinaryReader(file.OpenRead()))
            {

                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = br.ReadByte();
                }

                Console.WriteLine("File {0} Header {1}", file.Name, ByteStringBuilder(arr));




            }

            if (DiffBytes(arr, GAME7))
            {
                Console.WriteLine("THIS IS VERSION 7 MANIFEST.");
                FileBytes = File.ReadAllBytes(file.FullName);

                byte[] b = new byte[4];
                b[0] = FileBytes[15 + 1];
                b[1] = FileBytes[15 + 2];
                b[2] = FileBytes[15 + 3];
                b[3] = FileBytes[15 + 4];

                

                ManifestBlockNumber = BitConverter.ToInt32(b,0);
                Console.WriteLine("TypeHash Number:{0} {1}", ByteStringBuilder(b),ManifestBlockNumber);
                DoTask();
                File.WriteAllBytes(file.FullName, FileBytes);
            }
            else
            {
                Console.WriteLine("THIS IS NOT VERSION 7 MANIFEST.");
            }



            Console.WriteLine("Done!");

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

        static void DoTask()
        {
            Console.WriteLine("[SLOW]Try To Hack...");
            BuildManifest();
            return;
            //int number = 0;



            //foreach (DataBlock db in list)
            //{
            //    Console.WriteLine("[{1}/{0}]Search {2} Header {3}", list.Count, number + 1, db.Name, ByteStringBuilder(db.TypeID));
            //    HardDataBlock hdb = new HardDataBlock();
            //    if (!hdb_map.ContainsKey(db.Name))
            //    {
            //        Console.WriteLine("Break {0} Beacause This is not in HDB List!", db.Name);

            //        continue;
            //    }
            //    else
            //    {

            //        hdb = hdb_map[db.Name];
            //        Console.WriteLine("Get {0} Name {1}", db.Name, hdb.Name_0);

            //        if (String.IsNullOrEmpty(hdb.Name_0))
            //        {
            //            Console.WriteLine("Wrong!Pass!", db.Name, hdb.Name_0);
            //            continue;
            //        }

            //    }

            //    int count = 0;

            //    for (int i = 0; i < FileBytes.Length; i++)
            //    {

            //        if (FileBytes[i] == db.TypeID[count])
            //        {
            //            count++;
            //            Console.WriteLine("Match {0} Data {1} At {2}", count, FileBytes[i].ToString("X2"), i);
            //        }

            //        if (count > db.TypeID.Length)
            //        {
            //            throw new Exception("Count > " + db.TypeID.Length);
            //        }

            //        if (count == db.TypeID.Length)
            //        {
            //            Console.WriteLine("Match {0}", db.Name);
            //            Console.WriteLine("SKIP 4 NEW CURSOR: {0}", i);

            //            byte[] checkD = new byte[4];
            //            byte[] checkA = new byte[4];
            //            byte[] checkB = new byte[4];
            //            byte[] checkC = new byte[4];


            //            checkD[0] = FileBytes[i - 4 + 1 + 0];
            //            checkD[1] = FileBytes[i - 4 + 1 + 1];
            //            checkD[2] = FileBytes[i - 4 + 1 + 2];
            //            checkD[3] = FileBytes[i - 4 + 1 + 3];

            //            checkA[0] = FileBytes[i + 0 + 1 + 0];
            //            checkA[1] = FileBytes[i + 0 + 1 + 1];
            //            checkA[2] = FileBytes[i + 0 + 1 + 2];
            //            checkA[3] = FileBytes[i + 0 + 1 + 3];

            //            checkB[0] = FileBytes[i + 4 + 1 + 0];
            //            checkB[1] = FileBytes[i + 4 + 1 + 1];
            //            checkB[2] = FileBytes[i + 4 + 1 + 2];
            //            checkB[3] = FileBytes[i + 4 + 1 + 3];

            //            checkC[0] = FileBytes[i + 8 + 1 + 0];
            //            checkC[1] = FileBytes[i + 8 + 1 + 1];
            //            checkC[2] = FileBytes[i + 8 + 1 + 2];
            //            checkC[3] = FileBytes[i + 8 + 1 + 3];

            //            Console.WriteLine("{0} {1} {2} {3}",ByteStringBuilder(checkD), ByteStringBuilder(checkB), ByteStringBuilder(checkA), ByteStringBuilder(checkC));

            //            Console.WriteLine("Check Data {0} With {1}", ByteStringBuilder(checkB), ByteStringBuilder(hdb.TypeHash_0));

            //            if (checkB.Equals(hdb.TypeHash_0))
            //            {
            //                Console.WriteLine("Can Write!");

            //                for (count = 0; count < db.TypeHash.Length; count++)
            //                {
            //                    if (FileBytes[i + 4 + 1].Equals(db.TypeHash[count]))
            //                    {
            //                        Console.WriteLine("Pass {0} Beacause {1}  At {2}", FileBytes[i].ToString("X2"), db.TypeHash[count].ToString("X2"), i);
            //                    }
            //                    else
            //                    {


            //                        Console.WriteLine("Write {0} To {1}  At {2}", FileBytes[i].ToString("X2"), db.TypeHash[count].ToString("X2"), i);
            //                        origin_data.Add(i + 4 + 1, FileBytes[i + 4 + 1]);
            //                        target_data.Add(i + 4 + 1, db.TypeHash[count]);
            //                        FileBytes[i + 4 + 1] = db.TypeHash[count];
            //                    }



            //                    i++;
            //                }

            //            }
            //            else
            //            {
            //                Console.WriteLine("Cannot Write!");
            //            }



            //            count = 0;


            //        }


            //    }
            //    number++;
            //}




        }

    }
}
