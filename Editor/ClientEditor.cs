#if UNITY_EDITOR

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Client))]
public class ClientEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Build commands"))
        {
            var dir = Application.dataPath + "/Scripts/CommandsSystem/";
            Debug.Log("building commands " + dir);
            string[] commands = Directory.GetFiles(dir + "Commands/")
                .Where(file => file.EndsWith(".cs"))
                .Select(command => command.Split('.').First().Split('/').Last())
                .ToArray();

            
                
            var commandsEnum = 
@"namespace CommandsSystem {
    public enum CommandType { 
";

            for (int i = 0; i < commands.Length; i++)
            {
                var divider =  i < commands.Length - 1 ? "," : "";
                commandsEnum += $"        {commands[i]}{divider}\n";

            }


            commandsEnum += 
@"  }
}";

            File.WriteAllText(dir + "CommandType.cs", commandsEnum);


            string part1;
            string part2;
            string part3;
            {
                var text = File.ReadAllText(dir + "CommandsSystem.cs");
                var begin = new string[] { "BEGIN", "END" };

                var p123 = text.Split(begin, StringSplitOptions.None);
                part1 = p123[0];
                part2 = p123[1];
                part3 = p123[2];
            }

            part2 = "";
            foreach (var command in commands)
            {
                part2 +=

$@"             case CommandType.{command}:
                     return MsgPack.Deserialize<{command}>(stream);
";

            }

            part2 = "BEGIN\n" + part2 + "\n//  END";

            File.WriteAllText(dir + "CommandsSystem.cs", $"{part1}{part2}{part3}");
            

            /*
            Vector3 pos = Client.client.FindPlaceForSpawn(10, 1);
            Client.client.commandsHandler.RunSimpleCommand(new SpawnPrefabCommand("coin", pos, new Quaternion()));
            Debug.Log("Building fbe");*/
        }
    }
}

#endif