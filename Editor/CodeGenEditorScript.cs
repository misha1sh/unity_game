#if UNITY_EDITOR

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CodeGenEditorScript {

    private static void ReplaceBeginEnd(string filename, string begin, string end, string output) {

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

    [MenuItem("Tools/GenerateMessagesCode")]
    public static void GenerateMessagesCode() {
        var dir = Application.dataPath + "/Scripts/CommandsSystem/";
        Debug.Log("building commands " + dir);

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


        ReplaceBeginEnd(dir + "CommandsSystem.cs", @"/*BEGIN1*/", @"/*END1*/", decodePart);
        ReplaceBeginEnd(dir + "CommandsSystem.cs", @"/*BEGIN2*/", @"/*END2*/", encodePart);


        /*
        Vector3 pos = Client.client.FindPlaceForSpawn(10, 1);
        Client.client.commandsHandler.RunSimpleCommand(new SpawnPrefabCommand("coin", pos, new Quaternion()));
        Debug.Log("Building fbe");*/
    }
}

#endif