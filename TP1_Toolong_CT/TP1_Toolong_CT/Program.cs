// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.Json;

namespace HelloWorld;

internal class Program
{
    private static void Main(string[] args)
    {
        // Variables
        var engineConfig = new EngineConfig();
        const string batPath = ".\\Engine\\Build\\BatchFiles\\RunUAT.bat"; //"D:\\UnrealEngine\\Engine\\Build\\BatchFiles\\RunUAT.bat";
        var projectPath = args[0];
        var projectCommand = args[1];
        var projectPackagePath = "";
        var parsingCommandLine = "";
        string gameName;

        using (var r = new StreamReader(projectPath))
        {
            var json = r.ReadToEnd();
            engineConfig = JsonSerializer.Deserialize<EngineConfig>(json)!;
            gameName = engineConfig.Modules[0].Name;
        }

        switch (projectCommand)
        {
            // Affichage (nom de jeu, version de Unreal utilisee, plugins utilises)
            case "show-infos":

                Console.WriteLine("GameName = " + gameName);
                Console.WriteLine("Unreal used Version = " + engineConfig.FileVersion);
                if (engineConfig.EngineAssociation[0] == '{')
                {
                    Console.WriteLine("\tFrom Source");
                }
                Console.Write("Plugins :");
                foreach (var plugin in engineConfig.Plugins)
                {
                    Console.Write("\t { Name = " + plugin.Name);
                    Console.Write(", Enabled = " + plugin.Enabled);
                    Console.Write(", Target Allow List = { ");
                    foreach (var item in plugin.TargetAllowList) Console.Write(item + ", ");

                    Console.WriteLine(" } }");
                }

                break;
            case "build":

                var startInfo = new ProcessStartInfo
                {
                    FileName = batPath,
                    Arguments = $"{gameName} {"Win64"} {"Development"} {"\"" + projectPath + "\""} -waitmutex",
                    //UseShellExecute = true,
                    //RedirectStandardOutput = false,
                    //CreateNoWindow = true
                };

                try
                {
                    var process = Process.Start(startInfo);
                    process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                    process.ErrorDataReceived += (sender, e) => Console.WriteLine($"ERROR: {e.Data}");
                    process.WaitForExit();

                    Console.WriteLine("Process completed successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Building Process Error");
                }

                break;
            case "package":

                projectPackagePath = args[2];
                
                parsingCommandLine = "-ScriptsForProject=\"" + projectPath + "\" BuildCookRun -project=\"" + projectPath + "\" -noP4 -clientconfig=Development -serverconfig=Development -nocompile -nocompileeditor" +
                                     "-installed -unrealexe=\".\\Engine\\Binaries\\Win64\\UnrealEditor-Cmd.exe\" -utf8output -platform=Win64 -build -cook -map=level_TP1+menu_TP1 -CookCultures=en" +
                                     "-unversionedcookedcontent -stage -package -stagingdirectory=\"" + projectPackagePath + "\" -cmdline=\" -Messaging\" -addcmdline=\"-SessionId=30E39E4344ADEB1D070C1CBA6845C04A" +
                                     "-SessionOwner='Coralie' -SessionName='UAT_SuperProfil'";
                
                var starInfo = new ProcessStartInfo
                {
                    FileName = batPath,
                    Arguments = parsingCommandLine,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                try
                {
                    var process = new Process { StartInfo = starInfo };
                    process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                    process.ErrorDataReceived += (sender, e) => Console.WriteLine($"ERROR: {e.Data}");

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Packaging Process Error");
                }

                break;
        }
    }
}

//Parsing command line: -ScriptsForProject="C:/../../../Users/Coralie/Documents/2024_Automne/Intelligence artificielle pour le jeu video/TP1/TP1_3/TP1_3.uproject" BuildCookRun -project="C:/../../../Users/Coralie/Documents/2024_Automne/Intelligence artificielle pour le jeu video/TP1/TP1_3/TP1_3.uproject" -noP4 -clientconfig=Development -serverconfig=Development -nocompile -nocompileeditor -installed -unrealexe="C:\Program Files\Epic Games\UE_5.4\Engine\Binaries\Win64\UnrealEditor-Cmd.exe" -utf8output -platform=Win64 -build -cook -map=level_TP1+menu_TP1 -CookCultures=en -unversionedcookedcontent -stage -package -cmdline=" -Messaging" -addcmdline="-SessionId=30E39E4344ADEB1D070C1CBA6845C04A -SessionOwner='Coralie' -SessionName='UAT_SuperProfil' "