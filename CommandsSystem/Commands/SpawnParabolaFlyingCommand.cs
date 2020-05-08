using GameMechanics;
using UnityEngine;

namespace CommandsSystem.Commands {
    public partial class SpawnParabolaFlyingCommand {
        public SpawnPrefabCommand command;
        public Vector3 medium;
        public Vector3 target;
        public float totalTime;

        public void Run()  {
            var go = Client.client.SpawnObject(command);
            var flying = go.AddComponent<ParabolaFlyingObject>();
            flying.start = command.position;
            flying.medium = medium;
            flying.stop = target;
            flying.totalTime = totalTime;
        }
    }
}