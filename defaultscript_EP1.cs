//===================================================//
//===== ABOUT =======================================//
//===================================================//
//                                                   //
// Extended build script for the Red Alert 3 Mod SDK //
//                                                   //
// Author: Bibber & qiuyuewu1987 & yangqs & WCAK47 //
// eMail: m.bibber@web.de (Bibber) //
//           qiuyuewu1987@163.com (qiuyuewu1987) //
//           876143917@qq.com (WCAK47) //
// EP1 Hijack By HaoJun0823
// modder@haojun0823.xyz
//===================================================//
//===== PROGRAM =====================================//
//===================================================//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Microsoft.Win32;

namespace EALABuild
{
    class BuildScript: EALABuildScript
    {
		#region Member variables
        Dictionary<string, Object> currentGUIData;
        static int MOD_BUILD_STEPS = 22;
        static string DEFAULT_GAME_VERSION = "1.0";
		static string DEFAULT_MOD_VERSION = "1.0";
		static string DEFAULT_SKUDEF_NAME = "中文限五字";
		
		// Variables
		object Reg;
		string Cmd = Environment.GetEnvironmentVariable("comspec");
		string PersonalDirectory;
		string SDKDirectory;
		string UserDataLeafName;
		
		string BuiltModsPath;
		string BinaryAssetBuilder;
		string AssetResolver;
		string HashFix;
		string LoDStreamBuilder;
		string MakeBig;
		string AssetMerger;
		
		string Mod;
		string ModPath;
		string ModAdditionalFilesPath;
		string ModAssetsPath;
		string ModDataPath;
		string BuiltModPath;
		string BuiltModDataPath;
		string ModInstallPath;

		string EP1ManifestFixerPath;
		string EP1BIRFixerPath;
		string EP1AssetFixerPath;
		string EP1ManifestAssetFixerPath;
		string EP1CSFPath;
		
		string ModBabProj;
		string ModXml;
		string ModBig;
		string ModSkudef;
		#endregion

		#region Implementation of EALABuildScript interface.

		public void Hijacked()
        {
        
			BuildHelper.DisplayLine("https://github.com/HaoJun0823/RA3SDK-U");
			BuildHelper.DisplayLine("RED ALERT 3 MOD SDK-U FOR UPRISING BY HAOJUN0823 VERSION:???");
			BuildHelper.DisplayLine("Thanks for utunnels & Kamijou Yary & You~");
			BuildHelper.DisplayLine("Warning:Unstable!!!!!");
			
			
			BuildHelper.DisplayLine("Hijacked StaticPaths...");
			_Hijacked("BUILT_MODS_STRING", "BuiltMods_EP1");
			_Hijacked("MOD_FOLDER_PATH", "Mods_EP1");
			_Hijacked("SAGEXML", "SageXml_EP1");
			BuildHelper.DisplayLine("Done!");
			

		}

		private void _Hijacked(string prop,string value)
        {


			BindingFlags flag = BindingFlags.Static | BindingFlags.NonPublic;
			string value2;

			FieldInfo fi = typeof(StaticPaths).GetField(prop, flag);

			value2 = fi.GetValue(typeof(StaticPaths)).ToString();

			

			BuildHelper.DisplayLine(prop+" Origin Data:" + value2);


			fi.SetValue(typeof(StaticPaths).GetProperty(prop), value);
			value2 = fi.GetValue(typeof(StaticPaths)).ToString();

			BuildHelper.DisplayLine(prop+" Target Data:" + value2);


		}


        public static void CopyFolders(string source, string destination)
        {
			if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
			
            DirectoryInfo di = new DirectoryInfo(source);
            CopyFiles(source, destination);

            foreach (DirectoryInfo d in di.GetDirectories())
            {
                string newDir = Path.Combine(destination, d.Name);
                CopyFolders(d.FullName, newDir);
            }
        }

        public static void CopyFiles(string source, string destination)
        {
            DirectoryInfo di = new DirectoryInfo(source);
            FileInfo[] files = di.GetFiles();

            foreach (FileInfo f in files)
            {
                string sourceFile = f.FullName;
                string destFile = Path.Combine(destination, f.Name);
                File.Copy(sourceFile, destFile, true);
            }
        }

		// This function is called immediately after the script constructor in the GUI load process.
        public void initialize()
        {
			/*
			* This will automatically reset the counter in BuildHelper, and the GUI will
			* set the type of build in BuildHelper.  If it is not enabled, call 
			* BuildHelper.Reset, and set BuildHelper.CurrentBuildType = to either BuildType.ModBuild
			* or BuildType.MapBuild in the buildMap and buildMod functions
			*/
			


            BuildHelper.AutomaticallyResetBuildHelper = true;
            BuildHelper.EnableBenchmarking = true;

			Hijacked();

			// Personal Directory
			PersonalDirectory = Environment.ExpandEnvironmentVariables(Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders", "Personal", "").ToString());
			
			// SDK Directory
			Reg = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{F6A3F605-7B10-4939-8D3D-4594332C1649}", "InstallLocation", null); //HaoJun0823: This is RA3 InstallWizard Data.
			if (Reg == null) { 
				Reg = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{F6A3F605-7B10-4939-8D3D-4594332C1649}", "InstallLocation", null);
				if (Reg == null) { 
					SDKDirectory = StaticPaths.CurrentDirectory;
				} else {
					SDKDirectory = Reg.ToString();
				}
			} else {
				SDKDirectory = Reg.ToString();
			}
			
			// User Data Leaf Name
			Reg = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Electronic Arts\\Electronic Arts\\Red Alert 3 Uprising", "UserDataLeafName", null);
			if (Reg == null) { 
				Reg = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Wow6432Node\\Electronic Arts\\Electronic Arts\\Red Alert 3 Uprising", "UserDataLeafName", null);
				if (Reg == null) { 
					UserDataLeafName = "Red Alert 3 Uprising";
				} else {
					UserDataLeafName = Reg.ToString();
				}
			} else {
				UserDataLeafName = Reg.ToString();
			}
			//CNC Launcher 3 Support
			UserDataLeafName = "Red Alert 3 Uprising";
			
			// Variables
			BuiltModsPath = SDKDirectory + "\\builtmods_ep1";
			BinaryAssetBuilder = SDKDirectory + "\\tools_ep1\\binaryassetbuilder.exe";
			AssetResolver = SDKDirectory + "\\tools_ep1\\modassetresolver.exe";
			HashFix = SDKDirectory + "\\tools_ep1\\hashfix.exe";
			LoDStreamBuilder = SDKDirectory + "\\tools\\lodstreambuilder.exe";
			MakeBig = SDKDirectory + "\\tools\\makebig.exe";
			AssetMerger = SDKDirectory + "\\tools\\assetmerge.exe";
			EP1AssetFixerPath = SDKDirectory + "\\tools_ep1\\AssetTypeHashHeaderFixer.exe";
			EP1ManifestFixerPath = SDKDirectory + "\\tools_ep1\\SageManifestHeaderFixer.exe";
			EP1BIRFixerPath = SDKDirectory + "\\tools_ep1\\SageBinImpReloHeaderFixer.exe";
			EP1ManifestAssetFixerPath = SDKDirectory + "\\tools_ep1\\SageManifestAssetFixer.exe";
			EP1CSFPath = SDKDirectory + "\\tools_ep1\\GameStrings.csf";
		}

		// This function is called on the successful completion of a build step.
        public void onStepSuccess(int stepId)
        {
            if (BuildHelper.GetNextExecutableStep() != -1)
            {
                executeModBuildStep(BuildHelper.CurrentStep);
            }
            else
            {
                BuildHelper.OnBuildComplete();
            }
        }

		// This function is called on the failure of a build step.
        public void onStepFailure(int stepId)
        {
            BuildHelper.DisplayLine(String.Format("Build failed on step {0}", BuildHelper.CurrentStep-1));
        }

		// This function is called to determine if a step should execute.
        public bool shouldExecuteStep(int stepId)
        {
            if (currentGUIData.ContainsKey("step" + stepId))
            {
                if ((bool)currentGUIData["step" + stepId])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

		// This function is called when the user elects to build a mod
        public void buildMod(string modName, Dictionary<string, Object> GUIResults)
        {
            currentGUIData = GUIResults;
            BuildHelper.CurrentBuildSteps = MOD_BUILD_STEPS;
			
			//Variables
			Mod = BuildHelper.BuildTarget;
			ModPath = SDKDirectory + "\\mods_ep1\\" + Mod;
			ModAdditionalFilesPath = ModPath + "\\additional";
			ModAssetsPath = ModPath + "\\assets";
			ModDataPath = ModPath + "\\data";
			BuiltModPath = BuiltModsPath + "\\mods_ep1\\" + Mod;
			BuiltModDataPath = BuiltModPath + "\\data";
			ModInstallPath = PersonalDirectory + "\\" + UserDataLeafName + "\\Mods\\" + Mod;
			
			ModBabProj = ModPath + "\\mod.babproj";
			ModXml = ModDataPath + "\\mod.xml";
			ModBig = Mod + "_" + (string)currentGUIData["modversion"] + ".big";
			
			if (((string)currentGUIData["skudefname"] == "中文限五字") || ((string)currentGUIData["skudefname"] == "")) {
				ModSkudef = Mod + "_" + (string)currentGUIData["modversion"] + "_U.skudef";
			} else {
				ModSkudef = (string)currentGUIData["skudefname"] + "_" + (string)currentGUIData["modversion"] + ".ep.skudef";
			}
			
            executeModBuildStep(BuildHelper.GetNextExecutableStep());
        }
		
		/*
		* This function is called during the GUI initialization to generate
		* the GUI for the mod build options section.
		*/
        public List<GUIElementData> GetModBuildGUIData()
        {
            List<GUIElementData> modGUIData = new List<GUIElementData>();
			modGUIData.Add(new GUIElementData("step1", "01.清理MOD暂存文件", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step2", "02.清空缓存", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("sp1", "", GUIElementData.LabelType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("step3", "03.建立AptUI[必选04|05]", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step4", "04.修复Assets", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step5", "05.链接Binary", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step6", "06.建立全局数据[必选07|08]", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step7", "07.修复Assets", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step8", "08.链接Binary", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step9", "09.建立基础数据[必选10|11]", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step10", "10.修复Assets", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step11", "11.链接Binary", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step12", "12.合并Assets资产", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("step13", "13.修复中立资产[损坏]", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("sp2", "", GUIElementData.LabelType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step14", "14.复制额外文件", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step15", "15.修复Manifest[必选]", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step16", "16.修复BIR[必选]", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step17", "17.Manifest修复[必选]", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step18", "18.拷贝起义CSF", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step19", "19.建立Big文件", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step20", "20.建立Skudef文件", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("sp3", "", GUIElementData.LabelType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step21", "21.建立全屏INI文件", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("step22", "22.建立窗口INI文件", GUIElementData.CheckBoxType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("sp4", "", GUIElementData.LabelType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("mv", "Mod版本:", GUIElementData.LabelType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("modversion", DEFAULT_MOD_VERSION, GUIElementData.InputBoxType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("sn", "Skudef名称:", GUIElementData.LabelType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("skudefname", DEFAULT_SKUDEF_NAME, GUIElementData.InputBoxType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("sp5", "", GUIElementData.LabelType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("label", "基于 SDK-X中文扩展1.3版", GUIElementData.LabelType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("label2", "SDK-U 1.0", GUIElementData.LabelType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("bbs2", "RA2DIY.com", GUIElementData.LabelType, ScriptIndex.ModScript));
            modGUIData.Add(new GUIElementData("bbs3", "RA3MOD制作大学", GUIElementData.LabelType, ScriptIndex.ModScript));
			modGUIData.Add(new GUIElementData("text4", "起义时刻 SDK", GUIElementData.LabelType, ScriptIndex.ModScript));
			return modGUIData;
        }
        #endregion

        private void executeModBuildStep(int stepId)
        {
            switch(stepId)
            {
                case 1:
					BuildHelper.DisplayLine(String.Format("Step {0}: [清理MOD暂存文件](Clearing built mod)",
						stepId));
                    RunExecutableArguments clearMod = new RunExecutableArguments(Cmd);
                    clearMod.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (for /R \"{1}\" %I in (\"*.*\") do ("
							+ "(if not \"%~xI\" == \".asset\" (del \"%I\" /F /Q))"
						+ "))",
						SDKDirectory, BuiltModPath);
					BuildHelper.DisplayLine(clearMod.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, clearMod);
                    break;
                    
                case 2:
					BuildHelper.DisplayLine(String.Format("Step {0}: [清空缓存](Clearing cache)",
						stepId));
                    RunExecutableArguments clearCache = new RunExecutableArguments(Cmd);
                    clearCache.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (if exist \"{1}\\builtmods\" (rd \"{1}\\builtmods\" /S /Q))"
						+ " & (if exist \"{1}\\cache\" (rd \"{1}\\cache\" /S /Q))"
						+ " & (for /R \"{2}\" %I in (\"*.asset\") do (del \"%I\" /F /Q))"
						+ " & (if exist \"{1}\\binaryassetbuilder.sessioncache.xml\" (del \"{1}\\binaryassetbuilder.sessioncache.xml\" /F /Q))"
						+ " & (if exist \"{1}\\binaryassetbuilder.sessioncache.xml.old\" (del \"{1}\\binaryassetbuilder.sessioncache.xml.old\" /F /Q))"
						+ " & (if exist \"{1}\\binaryassetbuilder.sessioncache.xml.deflate\" (del \"{1}\\binaryassetbuilder.sessioncache.xml.deflate\" /F /Q))"
						+ " & (if exist \"{1}\\binaryassetbuilder.sessioncache.xml.deflate.old\" (del \"{1}\\binaryassetbuilder.sessioncache.xml.deflate.old\" /F /Q))"
						+ " & (if exist \"{1}\\stringhashes.xml\" (del \"{1}\\stringhashes.xml\" /F /Q))",
						SDKDirectory, BuiltModsPath, BuiltModPath);
					BuildHelper.DisplayLine(clearCache.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, clearCache);
                    break;
                    
                case 3:
					BuildHelper.DisplayLine(String.Format("Step {0}: [建立AptUI](Building AptUI)",
						stepId));
					RunExecutableArguments argsAptUI = new RunExecutableArguments(Cmd);
					argsAptUI.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (for %I in (\"{1}\\aptui\\*\") do (del \"%I\" /F /Q))"
						+ " & (for %I in (\"{3}\\aptui\\*.xml\") do ("
							+ "(\"{2}\" \"%I\" /od:\"{4}\" /iod:\"{4}\" /csc:false /ls:true /osh:false /pc:true /res:true /slowclean:true /ss:true /art:\".;.\\Mods_EP1\\{5}\\Art;.\\Mods_EP1;.\\Art\" /audio:\".;.\\Mods_EP1\\{5}\\Audio;.\\Mods_EP1;.\\Audio\" /data:\".;.\\Mods_EP1\\{5}\\Data;.\\Mods_EP1;.\\SageXml_EP1\")"
							+ " & (if exist \"{1}\\aptui\\%~nI.manifest\" (echo. >\"{1}\\aptui\\%~nI.version\"))"
						+ "))",
						SDKDirectory, BuiltModDataPath, BinaryAssetBuilder, ModDataPath, BuiltModsPath, Mod);
					BuildHelper.DisplayLine(argsAptUI.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsAptUI);
					break;
				case 4:
					BuildHelper.DisplayLine(String.Format("Step {0}: [修复Assets](Fix Assets)",
stepId));

					RunExecutableArguments argsFA1 = new RunExecutableArguments(Cmd);
					argsFA1.Arguments = String.Format("/C (@echo off) & (\"{0}\" \"{1}\\data\\mod\\assets\") ", EP1AssetFixerPath, BuiltModPath);
					BuildHelper.DisplayLine(argsFA1.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsFA1);



					break;
				case 5:
					BuildHelper.DisplayLine(String.Format("Step {0}: [链接Binary](Linked Binary)",
	stepId));
					RunExecutableArguments argsAptUIF = new RunExecutableArguments(Cmd);
					argsAptUIF.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (for %I in (\"{1}\\aptui\\*\") do (del \"%I\" /F /Q))"
						+ " & (for %I in (\"{3}\\aptui\\*.xml\") do ("
							+ "(\"{2}\" \"%I\" /od:\"{4}\" /iod:\"{4}\" /csc:false /ls:true /osh:false /pc:true /res:true /slowclean:true /ss:true /art:\".;.\\Mods_EP1\\{5}\\Art;.\\Mods_EP1;.\\Art\" /audio:\".;.\\Mods_EP1\\{5}\\Audio;.\\Mods_EP1;.\\Audio\" /data:\".;.\\Mods_EP1\\{5}\\Data;.\\Mods_EP1;.\\SageXml_EP1\")"
							+ " & (if exist \"{1}\\aptui\\%~nI.manifest\" (echo. >\"{1}\\aptui\\%~nI.version\"))"
						+ "))",
						SDKDirectory, BuiltModDataPath, BinaryAssetBuilder, ModDataPath, BuiltModsPath, Mod);
					BuildHelper.DisplayLine(argsAptUIF.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsAptUIF);


					break;
				case 6:
					BuildHelper.DisplayLine(String.Format("Step {0}: [建立全局数据](Building global data)",
						stepId));
					RunExecutableArguments argsGlobal = new RunExecutableArguments(Cmd);
					argsGlobal.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (for %I in (\"{1}\\additionalmaps\\mapmetadata_*\") do (del \"%I\" /F /Q))"
						+ " & (for %I in (\"{3}\\additionalmaps\\mapmetadata_*.xml\") do ("
							+ "(\"{2}\" \"%I\" /od:\"{4}\" /iod:\"{4}\" /csc:false /ls:true /osh:false /pc:true /res:true /slowclean:true /ss:true /art:\".;.\\Mods_EP1\\{5}\\Art;.\\Mods_EP1;.\\Art\" /audio:\".;.\\Mods_EP1\\{5}\\Audio;.\\Mods_EP1;.\\Audio\" /data:\".;.\\Mods_EP1\\{5}\\Data;.\\Mods_EP1;.\\SageXml_EP1\")"
						+ "))",
						SDKDirectory, BuiltModDataPath, BinaryAssetBuilder, ModDataPath, BuiltModsPath, Mod);
					BuildHelper.DisplayLine(argsGlobal.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsGlobal);
					break;
				case 7:
					BuildHelper.DisplayLine(String.Format("Step {0}: [修复Assets](Fix Assets)",
stepId));

					RunExecutableArguments argsFA2 = new RunExecutableArguments(Cmd);
					argsFA2.Arguments = String.Format("/C (@echo off) & (\"{0}\" \"{1}\\data\\mod\\assets\") ", EP1AssetFixerPath, BuiltModPath);
					BuildHelper.DisplayLine(argsFA2.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsFA2);

					break;
				case 8:

					BuildHelper.DisplayLine(String.Format("Step {0}: [链接Binary](Linked Binary)",
						stepId));
					RunExecutableArguments argsGlobalF = new RunExecutableArguments(Cmd);
					argsGlobalF.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (for %I in (\"{1}\\additionalmaps\\mapmetadata_*\") do (del \"%I\" /F /Q))"
						+ " & (for %I in (\"{3}\\additionalmaps\\mapmetadata_*.xml\") do ("
							+ "(\"{2}\" \"%I\" /od:\"{4}\" /iod:\"{4}\" /csc:false /ls:true /osh:false /pc:true /res:true /slowclean:true /ss:true /art:\".;.\\Mods_EP1\\{5}\\Art;.\\Mods_EP1;.\\Art\" /audio:\".;.\\Mods_EP1\\{5}\\Audio;.\\Mods_EP1;.\\Audio\" /data:\".;.\\Mods_EP1\\{5}\\Data;.\\Mods_EP1;.\\SageXml_EP1\")"
						+ "))",
						SDKDirectory, BuiltModDataPath, BinaryAssetBuilder, ModDataPath, BuiltModsPath, Mod);
					BuildHelper.DisplayLine(argsGlobalF.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsGlobalF);


					break;
                case 9:
					BuildHelper.DisplayLine(String.Format("Step {0}: [建立基础数据](Building static data)",
						stepId));
                    RunExecutableArguments args = new RunExecutableArguments(Cmd);
					args.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (if exist \"{1}\\mod.bin\" (del \"{1}\\mod.bin\" /F /Q))"
						+ " & (if exist \"{1}\\mod.imp\" (del \"{1}\\mod.imp\" /F /Q))"
						+ " & (if exist \"{1}\\mod.manifest\" (del \"{1}\\mod.manifest\" /F /Q))"
						+ " & (if exist \"{1}\\mod.relo\" (del \"{1}\\mod.relo\" /F /Q))"
						+ " & (if exist \"{1}\\mod.version\" (del \"{1}\\mod.version\" /F /Q))"
						+ " & (for %I in (\"{1}\\mod_*\") do (del \"%I\" /F /Q))"
						+ " & (\"{2}\" \"{3}\" /od:\"{4}\" /iod:\"{4}\" /csc:false /ls:true /osh:false /pc:true /res:true /slowclean:true /ss:true /art:\".;.\\Mods_EP1\\{5}\\Art;.\\Mods_EP1;.\\Art\" /audio:\".;.\\Mods_EP1\\{5}\\Audio;.\\Mods_EP1;.\\Audio\" /data:\".;.\\Mods_EP1\\{5}\\Data;.\\Mods_EP1;.\\SageXml_EP1\")",
						SDKDirectory, BuiltModDataPath, BinaryAssetBuilder, ModXml, BuiltModsPath, Mod);
						
					if (((bool)currentGUIData["step6"] == false) && ((bool)currentGUIData["step7"] == false)) {
						args.Arguments = args.Arguments + String.Format(" & (\"{0}\" \"{1}\\mod.manifest\")",
							LoDStreamBuilder, BuiltModDataPath);
					}
					BuildHelper.DisplayLine(args.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, args);
                    break;
				case 10:

					BuildHelper.DisplayLine(String.Format("Step {0}: [修复Assets](Fix Assets)",
stepId));

					RunExecutableArguments argsFA3 = new RunExecutableArguments(Cmd);
					argsFA3.Arguments = String.Format("/C (@echo off) & (\"{0}\" \"{1}\\data\\mod\\assets\") ", EP1AssetFixerPath, BuiltModPath);
					BuildHelper.DisplayLine(argsFA3.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsFA3);


					break;
				case 11:

					BuildHelper.DisplayLine(String.Format("Step {0}: [链接Binary](Linked Binary)",
						stepId));
					RunExecutableArguments argsF = new RunExecutableArguments(Cmd);
					argsF.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (if exist \"{1}\\mod.bin\" (del \"{1}\\mod.bin\" /F /Q))"
						+ " & (if exist \"{1}\\mod.imp\" (del \"{1}\\mod.imp\" /F /Q))"
						+ " & (if exist \"{1}\\mod.manifest\" (del \"{1}\\mod.manifest\" /F /Q))"
						+ " & (if exist \"{1}\\mod.relo\" (del \"{1}\\mod.relo\" /F /Q))"
						+ " & (if exist \"{1}\\mod.version\" (del \"{1}\\mod.version\" /F /Q))"
						+ " & (for %I in (\"{1}\\mod_*\") do (del \"%I\" /F /Q))"
						+ " & (\"{2}\" \"{3}\" /od:\"{4}\" /iod:\"{4}\" /csc:false /ls:true /osh:false /pc:true /res:true /slowclean:true /ss:true /art:\".;.\\Mods_EP1\\{5}\\Art;.\\Mods_EP1;.\\Art\" /audio:\".;.\\Mods_EP1\\{5}\\Audio;.\\Mods_EP1;.\\Audio\" /data:\".;.\\Mods_EP1\\{5}\\Data;.\\Mods_EP1;.\\SageXml_EP1\")",
						SDKDirectory, BuiltModDataPath, BinaryAssetBuilder, ModXml, BuiltModsPath, Mod);

					if (((bool)currentGUIData["step12"] == false) && ((bool)currentGUIData["step13"] == false))
					{
						argsF.Arguments = argsF.Arguments + String.Format(" & (\"{0}\" \"{1}\\mod.manifest\")",
							LoDStreamBuilder, BuiltModDataPath);
					}
					BuildHelper.DisplayLine(argsF.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsF);

					break;
				case 12:
					BuildHelper.DisplayLine(String.Format("Step {0}: [合并Assets资产](Merging assets)",
						stepId));
					RunExecutableArguments argsMerge = new RunExecutableArguments(Cmd);
					argsMerge.Arguments = String.Format("/V:ON /C (@echo off) & (cd /D \"{0}\")"
						+ " & (for /R \"{1}\" %I in (\"\") do ("
							+ "(set assets=%~dpI)"
							+ " & (if exist \"!assets!*.asset\" (\"{2}\" \"{3}\\mod\" \"!assets:~0,-1!\"))"
						+ "))",
						SDKDirectory, ModAssetsPath, AssetMerger, BuiltModDataPath);
						
					if ((bool)currentGUIData["step13"] == false) {
						argsMerge.Arguments = argsMerge.Arguments + String.Format(" & (\"{0}\" \"{1}\\mod.manifest\")",
							LoDStreamBuilder, BuiltModDataPath);
					}
					BuildHelper.DisplayLine(argsMerge.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsMerge);
					break;
					
                case 13:
					BuildHelper.DisplayLine(String.Format("Step {0}: [修复中立资产](Fixing assets)",
						stepId));
					RunExecutableArguments argsFix = new RunExecutableArguments(Cmd);
					argsFix.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (\"{2}\" \"{1}\\mod.manifest\")"
						+ " & (\"{3}\" \"{1}\\mod.manifest\")"
						+ " & (\"{4}\" \"{1}\\mod.manifest\")",
						SDKDirectory, BuiltModDataPath, HashFix, AssetResolver, LoDStreamBuilder);
					BuildHelper.DisplayLine(argsFix.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsFix);
					break;
					
                case 14:
					BuildHelper.DisplayLine(String.Format("Step {0}: [复制额外文件](Copying additional files)",
						stepId));
					RunExecutableArguments argsCopyFiles = new RunExecutableArguments(Cmd);
					argsCopyFiles.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")",
						SDKDirectory);
					if (Directory.Exists(SDKDirectory + "\\additional")) CopyFolders(SDKDirectory + "\\additional", BuiltModPath);
					if (Directory.Exists(ModAdditionalFilesPath)) CopyFolders(ModAdditionalFilesPath, BuiltModPath);
					BuildHelper.DisplayLine(argsCopyFiles.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsCopyFiles);
                    break;
				case 15:

					BuildHelper.DisplayLine(String.Format("Step {0}: [修复Manifest](Fix Manifest Header)",
	stepId));

					RunExecutableArguments argsM = new RunExecutableArguments(Cmd);
					argsM.Arguments = String.Format("/C (@echo off) & (\"{0}\" \"{1}\\data\") ", EP1ManifestFixerPath, BuiltModPath);
					BuildHelper.DisplayLine(argsM.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsM);
					break;

				case 16:

					BuildHelper.DisplayLine(String.Format("Step {0}: [修复BIR](Fix BIR Header)",
stepId));

					RunExecutableArguments argsBIR = new RunExecutableArguments(Cmd);
					argsBIR.Arguments = String.Format("/C (@echo off) & (\"{0}\" \"{1}\\data\") ", EP1BIRFixerPath, BuiltModPath);
					BuildHelper.DisplayLine(argsBIR.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsBIR);
					break;
					
				case 17:

					BuildHelper.DisplayLine(String.Format("Step {0}: [Manifest修复](Fix Manifest TypeHash)",
stepId));

					RunExecutableArguments argsMA = new RunExecutableArguments(Cmd);
					argsMA.Arguments = String.Format("/C (@echo off) & (\"{0}\" \"{1}\\data\") ", EP1ManifestAssetFixerPath, BuiltModPath);
					BuildHelper.DisplayLine(argsMA.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsMA);
					break;					
					
				case 18:

					BuildHelper.DisplayLine(String.Format("Step {0}: [拷贝起义CSF](Copy CSF)",
stepId));

					RunExecutableArguments argsCSF = new RunExecutableArguments(Cmd);
					argsCSF.Arguments = String.Format("/C (@echo off) & (copy \"{0}\" \"{1}\\data\") ", EP1CSFPath, BuiltModPath);
					BuildHelper.DisplayLine(argsCSF.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsCSF);
					break;							

				case 19:

					BuildHelper.DisplayLine(String.Format("Step {0}: [建立Big文件](Creating Big file)",
	stepId));
					RunExecutableArguments argsBig = new RunExecutableArguments(Cmd);
					argsBig.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (if exist \"{2}\" (\"{1}\" -f \"{2}\" -x:*.asset -o:\"{3}\\{4}\"))",
						SDKDirectory, MakeBig, BuiltModPath, ModInstallPath, ModBig);
					BuildHelper.DisplayLine(argsBig.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsBig);
                    break;
                    
                case 20:
					BuildHelper.DisplayLine(String.Format("Step {0}: [建立Skudef文件](Creating Skudef file)",
						stepId));
					RunExecutableArguments argsSkudef = new RunExecutableArguments(Cmd);
					argsSkudef.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (if exist \"{0}\\{2}\" (del \"{0}\\{2}\" /F /Q))"
						+ " & (echo mod-game {3}>\"{2}\")"
						+ " & (if exist \"{0}\\{1}\" (echo add-big {1}>>\"{2}\"))",
						ModInstallPath, ModBig, ModSkudef, DEFAULT_GAME_VERSION);
					BuildHelper.DisplayLine(argsSkudef.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsSkudef);
                    break;
                    
                case 21:
					BuildHelper.DisplayLine(String.Format("Step {0}: [建立全屏INI文件](Creating full screen INI file)",
						stepId));
					RunExecutableArguments argsFullScreenINI = new RunExecutableArguments(Cmd);
					argsFullScreenINI.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (if exist \"{0}\\CurrentMOD.ini\" (del \"{0}\\CurrentMOD.ini\" /F /Q))"
                        + " & (echo \"{1}\\{2}\">CurrentMOD.ini)",
						SDKDirectory, ModInstallPath, ModSkudef);
					BuildHelper.DisplayLine(argsFullScreenINI.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsFullScreenINI);
                    break;

				case 22:
					BuildHelper.DisplayLine(String.Format("Step {0}: [建立窗口INI文件](Creating window mode INI file)",
						stepId));
					RunExecutableArguments argsWinModeINI = new RunExecutableArguments(Cmd);
					argsWinModeINI.Arguments = String.Format("/C (@echo off) & (cd /D \"{0}\")"
						+ " & (if exist \"{0}\\CurrentMOD.ini\" (del \"{0}\\CurrentMOD.ini\" /F /Q))"
                        + " & (echo \"{1}\\{2}\" -win>CurrentMOD.ini)",
						SDKDirectory, ModInstallPath, ModSkudef);
					BuildHelper.DisplayLine(argsWinModeINI.Arguments);
					BuildHelper.RunStep(StepType.RunExecutable, argsWinModeINI);
                    break;
                    
                default:
                    // unknown build step, this is probably the end
					onStepSuccess(-1);
                    break;
            }
        }
    }
}

//===================================================//
//===================================================//
//===================================================//