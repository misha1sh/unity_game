using CommandsSystem.Commands;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameMode {
    public static class GameModeFunctions {
        public static void SpawnPlayer(int playerId) {
            var pos = FindPlaceForSpawn(0, 0.5f);
            var rot = new Quaternion();
            var id = ObjectID.RandomID;
            var owner = Client.client.ID;
            
            Client.client.commandsHandler.RunSimpleCommand(new SpawnPlayerCommand(new SpawnPrefabCommand("Robot", pos, rot, id, owner), 
                playerId));
        }

        public static void SpawnPlayers() {
            foreach (var player in PlayersManager.players) {
                if (player.owner == Client.client.ID) {
                    SpawnPlayer(player.id);
                }
            }
        }

        private static RaycastHit _raycastHitInfo;
        // (possible) TODO: reserve space on network before spawn 
        public static Vector3 FindPlaceForSpawn(float height, float radius) {
            int layerMask = 1 << 9;
            layerMask = ~layerMask;
            
            Vector3 pos1, pos2;
            for (int iterCount = 0; ; iterCount++) {
                pos1 = pos2 = Client.client.spawnPolygon.RandomPoint();
                pos1.y = -3;
                pos2.y = height;
                Assert.IsTrue(iterCount++ < 100, $"Unable to find free place for object with height: {height:F2}, radius: {radius:F2}");
                var intersections = Physics.OverlapCapsule(pos1, pos2, radius, layerMask, QueryTriggerInteraction.Ignore);
                if (intersections.Length != 1) continue;
                var b = intersections[0];
                Ray ray = new Ray();
                ray.direction = Vector3.down;

                bool flag = true;
                for (int x = -1; x <= 1; x += 2) {
                    for (int z = -1; z <= 1; z += 2) {
                        ray.origin = new Vector3(pos2.x + x * radius, pos2.y + 10f, pos2.z + z * radius);
                        if (!b.Raycast(ray, out _raycastHitInfo, 100f)) {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag) break;
            }
            //      capsules.Add(new CapsuleGizmos(pos1, pos2, radius));

            return pos2;
        }
    }
}