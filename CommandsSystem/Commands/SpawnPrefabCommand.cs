using System;
using UnityEngine;

namespace CommandsSystem.Commands {
    public partial class SpawnPrefabCommand {
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
        public int id;
        public int owner;
        
        private static System.Random random = new System.Random();

        
      /*  public SpawnPrefabCommand(string prefabName, int owner, Vector3 position, Quaternion rotation) {
            this.prefabName = prefabName;
            this.position = position;
            this.rotation = rotation;
            this.owner = owner;
            this.id = random.Next();
        }*/
        
        public void Run() {
            Client.client.SpawnObject(this);
        }
        
    }
}