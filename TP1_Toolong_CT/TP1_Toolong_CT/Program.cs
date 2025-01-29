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
            string projectPath = args[0]; // Mettre entre guillements
            string projectCommand = args[1];
            EngineConfig engineConfig = new EngineConfig();
            Process UBTProcess = new Process();
            string gameName;
            
            // Show JSON
            string readText = File.ReadAllText(projectPath);
            //Console.WriteLine(readText);
            //var parsedData = int.Parse(File.ReadAllText(projectPath)).parse(data);

            using (StreamReader r = new StreamReader(projectPath))
            {
                string json = r.ReadToEnd();

                //Console.WriteLine(r);
                engineConfig = JsonSerializer.Deserialize<EngineConfig>(json)!;
                gameName = (engineConfig.Modules[0]).Name;
            }

            switch (projectCommand)
            {
                case "show-infos":
                    /*
                     Afficher le nom du jeu
                     Afficher la version de Unreal utilisée ( Marquer « From Source » si cela est le cas )
                     Afficher les plugins utilisés
                     */
                    /*
                    foreach (Plugin plugin in engineConfig.Plugins)
                    {
                        Console.WriteLine("GameName = " + plugin.Name);
                    }
                    */
                    
                    Console.WriteLine("GameName = " + gameName);
                    Console.WriteLine("Unreal used Version = " + engineConfig.FileVersion.ToString());
                    Console.Write("Plugins :" );
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
                        FileName = "C:\\Program Files\\Epic Games\\UE_5.4\\Engine\\Build\\BatchFiles\\Build.bat",
                        Arguments = $"{gameName} {"Win64"} {"Development"} {"\"" + projectPath + "\""} -waitmutex",
                        UseShellExecute = true,
                        RedirectStandardOutput = false,
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
                case "package":
                    /*
                    Comment bien utiliser UAT ? Les paramètres sont très compliqués
o Vous allez vous servir du Project Launcher de Unreal Engine
o Dans votre éditeur : Tools -> Project Launcher
o Ajoutez un Custom Launch Profile (gros bouton Add en bas)
§ Section Project
• Vérifier que c’est le bon Project
§ Section Build
• Choisissez Build au lieu de Detect Automatically
§ Section Cook
• Choisissez By the book au lieu de On the fly
• Choisissez votre plateforme (Windows ou Mac)
• Choisissez vos Maps
§ Section Package
• Choisissez Package and store locally au lieu de Do Not
Package
§ Section Deploy
• Choisissez Do not deploy au lieu de Copy to device
o Revenez dans le menu précédent (en appuyant sur le bouton Back en haut)
o Clic droit sur votre profile -> Rename
§ Donnez-lui un nom intéressant
o Un bouton « Launch this profile » devrait être là (sinon corrigez votre
configuration)
o Appuyez dessus
o Une nouvelle fenêtre devrait se lancer
§ Dans l’Output Log de cette fenêtre, tout en haut, cherchez Parsing
Command Line :
§ Ce qui suit est la command-line à passer à UAT pour votre packaging
process
                    */
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
