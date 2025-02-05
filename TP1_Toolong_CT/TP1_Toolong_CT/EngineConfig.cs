using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelloWorld;

public class EngineConfig
{
    public int FileVersion { get; set; }
    public string EngineAssociation { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public IList<Module>? Modules { get; set; }
    public IList<Plugin> Plugins { get; set; }
}

public class Module
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string LoadingPhase { get; set; }
    public IList<string> AdditionalDependencies { get; set; }
}

public class Plugin
{
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public IList<string> TargetAllowList { get; set; }
}

/*
 * {
        "FileVersion": 3,
        "EngineAssociation": "5.4",
        "Category": "",
        "Description": "",
        "Modules": [
                {
                        "Name": "TP1_3",
                        "Type": "Runtime",
                        "LoadingPhase": "Default",
                        "AdditionalDependencies": [
                                "Engine"
                        ]
                }
        ],
        "Plugins": [
                {
                        "Name": "ModelingToolsEditorMode",
                        "Enabled": true,
                        "TargetAllowList": [
                                "Editor"
                        ]
                }
        ]
}
*/