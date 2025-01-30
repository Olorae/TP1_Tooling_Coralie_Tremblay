// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Diagnostics;
using System.Diagnostics;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables
            EngineConfig engineConfig = new EngineConfig();
            string batPath = "C:\\Program Files\\Epic Games\\UE_5.4\\Engine\\Build\\BatchFiles\\Build.bat";
            string projectPath = args[0];
            string projectCommand = args[1];
            string projectPackagePath = "";
            string gameName;

            using (StreamReader r = new StreamReader(projectPath))
            {
                string json = r.ReadToEnd();
                engineConfig = JsonSerializer.Deserialize<EngineConfig>(json)!;
                gameName = (engineConfig.Modules[0]).Name;
            }

            switch (projectCommand)
            {
                // Affichage (nom de jeu, version de Unreal utilisee, plugins utilises)
                // TODO : ( Marquer « From Source » si cela est le cas )
                case "show-infos":
                    
                    Console.WriteLine("GameName = " + gameName);
                    Console.WriteLine("Unreal used Version = " + engineConfig.FileVersion.ToString());
                    Console.Write("Plugins :");
                    foreach (Plugin plugin in engineConfig.Plugins)
                    {
                        Console.Write("\t { Name = " + plugin.Name);
                        Console.Write(", Enabled = " + plugin.Enabled);
                        Console.Write(", Target Allow List = { ");
                        foreach (string item in plugin.TargetAllowList)
                        {
                            Console.Write(item + ", ");
                        }

                        Console.WriteLine(" } }");
                    }

                    break;
                case "build":

                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = batPath,
                        Arguments = $"{gameName} {"Win64"} {"Development"} {"\"" + projectPath + "\""} -waitmutex",
                        //UseShellExecute = true,
                        //RedirectStandardOutput = false,
                    };

                    try
                    {
                        Process process = Process.Start(startInfo);
                        process.WaitForExit();
                        Console.WriteLine("Process completed successfully");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Building Process Error");
                    }

                    break;
                // TODO : Resolve : "No platforms specified for target"
                case "package":

                    //projectPackagePath = args[2];
                    ProcessStartInfo starInfo = new ProcessStartInfo
                    {
                        FileName = batPath,
                        Arguments = "-ScriptsForProject=\"C:/../../../Users/Coralie/Documents/2024_Automne/Intelligence artificielle pour le jeu video/TP1/TP1_3/TP1_3.uproject\" BuildCookRun -project=\"C:/../../../Users/Coralie/Documents/2024_Automne/Intelligence artificielle pour le jeu video/TP1/TP1_3/TP1_3.uproject\" -noP4 -clientconfig=Development -serverconfig=Development -nocompile -nocompileeditor -installed -unrealexe=\"C:\\Program Files\\Epic Games\\UE_5.4\\Engine\\Binaries\\Win64\\UnrealEditor-Cmd.exe\" -utf8output -platform=Win64 -build -cook -map=level_TP1+menu_TP1 -CookCultures=en -unversionedcookedcontent -stage -package -cmdline=\" -Messaging\" -addcmdline=\"-SessionId=30E39E4344ADEB1D070C1CBA6845C04A -SessionOwner='Coralie' -SessionName='UAT_SuperProfil' \"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    try
                    {
                        Process process = new Process { StartInfo = starInfo };
                        process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                        process.ErrorDataReceived += (sender, e) => Console.WriteLine($"ERROR: {e.Data}");

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Building Process Error");
                    }

                    break;
                default:
                    // code block
                    break;
            }
        }
    }
}

//TP1_Toolong_CT.exe "C:\Users\Coralie\Documents\2024_Automne\Intelligence artificielle pour le jeu video\TP1\TP1_3\TP1_3.uproject" build
//TP1_Toolong_CT.exe "C:\Users\Coralie\Documents\2025_Hiver\Prototypage de jeux avec un langage de script\EchoBlade\EchoBlade.uproject" build

//TP1_Toolong_CT.exe "C:\Users\Coralie\Documents\2024_Automne\Intelligence artificielle pour le jeu video\TP1\TP1_3\TP1_3.uproject" package
//"C:\Program Files\Epic Games\UE_5.4\Engine\Binaries\Win64\UnrealEditor-Cmd.exe" "C:/Users/Coralie/Documents/2024_Automne/Intelligence artificielle pour le jeu video/TP1/TP1_3/TP1_3.uproject" -run=Cook -targetplatform=Windows -unattended

//Parsing command line: -ScriptsForProject="C:/../../../Users/Coralie/Documents/2024_Automne/Intelligence artificielle pour le jeu video/TP1/TP1_3/TP1_3.uproject" BuildCookRun -project="C:/../../../Users/Coralie/Documents/2024_Automne/Intelligence artificielle pour le jeu video/TP1/TP1_3/TP1_3.uproject" -noP4 -clientconfig=Development -serverconfig=Development -nocompile -nocompileeditor -installed -unrealexe="C:\Program Files\Epic Games\UE_5.4\Engine\Binaries\Win64\UnrealEditor-Cmd.exe" -utf8output -platform=Win64 -build -cook -map=level_TP1+menu_TP1 -CookCultures=en -unversionedcookedcontent -stage -package -cmdline=" -Messaging" -addcmdline="-SessionId=30E39E4344ADEB1D070C1CBA6845C04A -SessionOwner='Coralie' -SessionName='UAT_SuperProfil' "