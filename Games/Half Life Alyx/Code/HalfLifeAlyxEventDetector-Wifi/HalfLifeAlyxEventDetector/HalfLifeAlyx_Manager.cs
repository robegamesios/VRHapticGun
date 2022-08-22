using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Management;

namespace HalfLifeAlyxEventDetector
{
    class HalfLifeAlyx_Manager : SteamAPIHelper
    {
        const int HalfLifeAlyxID = 546560;
        const uint ExecuteProcessWaitTime_Sec = 10;
        const string HalfLifeAlyxFileName = "hlvr";
        const string HalfLifeAlyxArgs = "-console -vconsole -autoexec";
        string _HalfLifeAlyxPath;
        public string HalfLifeAlyxPath
        {
            get
            {
                return _HalfLifeAlyxPath;
            }
        }
        string HalfLifeAlyxAutoexecPath
        {
            get
            {
                return ($"{_HalfLifeAlyxPath}\\game\\hlvr\\cfg\\autoexec.cfg");
            }
        }
        private bool IsHalfLifeAlyxRunning()
        {
            var HL2_List = Process.GetProcessesByName(HalfLifeAlyxFileName);
            return HL2_List.Length != 0;
        }
        public static void KillHalfLifeAlyx()
        {
            var HL2_List = Process.GetProcessesByName(HalfLifeAlyxFileName);
            foreach (var p in HL2_List)
            {
                try
                {
                    p.Kill();
                }
                catch
                {

                }
            }
        }
        private bool CheckHalfLifeAlyxAutoExec(HalfLifeAlyx_Autoexec autoexec)
        {
            if (File.Exists(HalfLifeAlyxAutoexecPath))
            {
                if (File.ReadAllText(HalfLifeAlyxAutoexecPath).Equals(autoexec.ToString()))
                    return true;
            }
            return false;
        }

        private void CreateAutoexec(HalfLifeAlyx_Autoexec autoexec)
        {
            if (File.Exists(HalfLifeAlyxAutoexecPath))
                File.Delete(HalfLifeAlyxAutoexecPath);
            File.WriteAllText(HalfLifeAlyxAutoexecPath, autoexec.ToString());
        }
        public HalfLifeAlyx_Manager()
        {
            _HalfLifeAlyxPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Half-Life Alyx";
            return;

            //if (!Steam_GameID_Path_Table.Keys.Contains(HalfLifeAlyxID))
            //    throw new FileNotFoundException("Cannot Find Half-Life Alyx!");
            var HalfLifeAlyx_SteamPath = Steam_GameID_Path_Table[HalfLifeAlyxID];
            if (HalfLifeAlyx_SteamPath.EndsWith("\\"))
                _HalfLifeAlyxPath = Steam_GameID_Path_Table[HalfLifeAlyxID] + "common\\Half-Life Alyx";
            else
                _HalfLifeAlyxPath = Steam_GameID_Path_Table[HalfLifeAlyxID] + "\\common\\Half-Life Alyx";
        }
        static string GetCommandLine(Process process)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            using (ManagementObjectCollection objects = searcher.Get())
            {
                return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
            }
        }
        public bool RestartHalfLifeAlyx(HalfLifeAlyx_Autoexec autoexec = null)
        {
            while (IsHalfLifeAlyxRunning())
                KillHalfLifeAlyx();

            if (autoexec == null)
                autoexec = new HalfLifeAlyx_Autoexec();

            if (!CheckHalfLifeAlyxAutoExec(autoexec))
                CreateAutoexec(autoexec);

            StartGame(HalfLifeAlyxID, HalfLifeAlyxArgs);
            for (int i = 0; i < ExecuteProcessWaitTime_Sec * 2; ++i)
            {
                if (!IsHalfLifeAlyxRunning())
                    Thread.Sleep(500);
                else if (CheckHalfLifeAlyxCommandLine())
                    return true;
                else
                    return false;
            }
            return false;
        }
        public static bool CheckHalfLifeAlyxCommandLine()
        {
            var HL2_List = Process.GetProcessesByName(HalfLifeAlyxFileName);
            Console.WriteLine(GetCommandLine(HL2_List[0]));
            return (GetCommandLine(HL2_List[0]).Contains(HalfLifeAlyxArgs));
        }

    }
}
