﻿using Gameloop.Vdf;
using System;
using System.IO;
using Windows.Management.Deployment;

namespace modmanager
{
    
    public class InstallPath
    {

        public static string settingsFilePath = "C:\\Users\\" + Environment.UserName + "\\Documents\\ForzaModManager\\settings.ini";

        public static string GetInstallPath(string packageName)
        {
            PackageManager packageManager = new PackageManager();
            var packages = packageManager.FindPackages();
            foreach (var package in packageManager.FindPackages())
            {
                if (package.Id.FamilyName == packageName)
                {
                    return package.InstalledLocation.Path;
                }
            }

            return null;
        }

        public static void installPath()
        {
            string fileContent = File.ReadAllText(path: settingsFilePath);

            if (fileContent.Contains("Game Install Path = Not Found"))
            {

                dynamic libraryfolders = VdfConvert.Deserialize(File.ReadAllText(@"C:\Program Files (x86)\Steam\steamapps\libraryfolders.vdf"));
                var installpath = "";
                foreach (var folder in libraryfolders.Value)
                {
                    if (folder.ToString().Contains("\"1551360\""))
                    {
                        installpath = folder.Value.path.ToString() + @"\steamapps\common\ForzaHorizon5";
                        
                        string[] lines = File.ReadAllLines(settingsFilePath);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].Contains("Game Install Path"))
                            {
                                lines[i] = "Game Install Path = " + installpath;
                                break;
                            }
                        }

                        File.WriteAllLines(settingsFilePath, lines);
                    }
                }
                if (installpath == "")
                {
                    string packageName = "Microsoft.624F8B84B80_8wekyb3d8bbwe";
                    string installpathMS = GetInstallPath(packageName);

                    string[] lines = File.ReadAllLines(settingsFilePath);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("Game Install Path"))
                        {
                            lines[i] = "Game Install Path = " + installpathMS;
                            break;
                        }
                    }

                    File.WriteAllLines(settingsFilePath, lines);
                }
            }
        }
    }
}
