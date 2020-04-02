#if UNITY_EDITOR

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CodeGenEditorScript {
    [MenuItem("Tools/GenerateMessagesCode")]
    public static void GenerateMessagesCode() {
        var dir = Application.dataPath + "/Scripts/CommandsSystem/";

        // Use ProcessStartInfo class
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = false;
        startInfo.FileName = "python.exe";
        startInfo.WindowStyle = ProcessWindowStyle.Normal;
        startInfo.Arguments = dir + "codegeg.py";

        Debug.Log($"Executing {startInfo.FileName} {startInfo.Arguments}");
        using (Process exeProcess = Process.Start(startInfo)) {
            exeProcess.WaitForExit();
        }
        Debug.Log("Success");
  
    }

/*    private static void ReplaceBeginEnd(string filename, string begin, string end, string output) {

        var input = File.ReadAllText(filename);

        string part1;
        string part2;
        string part3;
        {
            var spl = new string[] {begin, end};
            var p123 = input.Split(spl, StringSplitOptions.None);
            part1 = p123[0];
            part2 = p123[1];
            part3 = p123[2];
        }

        part2 = begin + output + end;

        File.WriteAllText(filename, $"{part1}{part2}{part3}");
    }

    private string GeneratePartialCommand(string input) {
        
        var spl = new string[] { $"{input}"};
        input = input.Split(spl, 2, StringSplitOptions.None)[1];

        foreach (var line in input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)) {
            if (line.Contains("Run")) break;
            Debug.Log(line);
        }
        
        var result = new StringWriter();
        result.Write($@"




");

        return result.ToString();
    }
    
    [MenuItem("Tools/GenerateMessagesCode")]
    public static void GenerateMessagesCode() {
        var dir = Application.dataPath + "/Scripts/CommandsSystem/";
        Debug.Log("building fcommands " + dir);

        string[] codegen_commands = File.ReadAllLines(dir + "codegen.txt");

        string[] commands = Directory.GetFiles(dir + "Commands/")
            .Where(file => file.EndsWith(".cs"))
            .Select(command => command.Split('.').First().Split('/').Last())
            .Concat(codegen_commands)
            .ToArray();




        var decodePart = "";
        for (int i = 0; i < commands.Length; i++) {
            decodePart +=
                $@"             case {i}:
                     return MsgPack.Deserialize<{commands[i]}>(stream);
";
        }

        var encodePart = "";
        for (int i = 0; i < commands.Length; i++) {
            encodePart +=
                $@"            if (command is {commands[i]}) {{
                    type = {i};
               }} else ";
        }


        ReplaceBeginEnd(dir + "CommandsSystem.cs", @"/*BEGIN1*//*", @"*//*END1*//*", decodePart);
       // ReplaceBeginEnd(dir + "CommandsSystem.cs", @"/*BEGIN2*//*", @"/*END2*//*", encodePart);


        /*
        Vector3 pos = Client.client.FindPlaceForSpawn(10, 1);
        Client.client.commandsHandler.RunSimpleCommand(new SpawnPrefabCommand("coin", pos, new Quaternion()));
        Debug.Log("Building fbe");*/
   // }
}

#endif