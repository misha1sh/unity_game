using System;
using UnityEngine;

namespace CommandsSystem.Commands {
    [Serializable]
    public class SpawnPrefabCommand: Command<SpawnPrefabCommand> {
        public override CommandType type() => CommandType.SpawnPrefabCommand;
        
        
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
        public int id;
        
        

        private static System.Random random = new System.Random();

        public SpawnPrefabCommand() {}
        
        public SpawnPrefabCommand(string prefabName, Vector3 position, Quaternion rotation) {
            this.prefabName = prefabName;
            this.position = position;
            this.rotation = rotation;
            this.id = random.Next();
        }
        
        public override void Run() {
            Client.client.SpawnObject(this);
        }
        
    }
}