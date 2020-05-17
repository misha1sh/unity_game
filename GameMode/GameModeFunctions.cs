using CommandsSystem.Commands;
using Networking;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameMode {
    /// <summary>
    ///     Класс с функциями для игровых режимов
    /// </summary>
    public static class GameModeFunctions {
        /// <summary>
        ///     Создает персонажа в случайном месте
        /// </summary>
        /// <param name="playerId">ID игрока</param>
        public static void SpawnPlayer(int playerId) {
            var pos = FindPlaceForSpawn(1, 0.5f);
            var rot = new Quaternion();
            var id = ObjectID.RandomID;
            var owner = sClient.ID;
            
            CommandsHandler.gameModeRoom.RunSimpleCommand(new SpawnPlayerCommand(new SpawnPrefabCommand("Robot", pos, rot, id, owner, id), 
                playerId), MessageFlags.IMPORTANT);
        }

        /// <summary>
        ///     Создаёт персонажа для каждого из игроков
        /// </summary>
        public static void SpawnPlayers() {
            foreach (var player in PlayersManager.players) {
                if (player.owner == sClient.ID) {
                    SpawnPlayer(player.id);
                }
            }
        }

        /// <summary>
        ///     RaycastHit для внутреннего использования (нужен, чтобы уменьшить нагрузку на сборщик мусора)
        /// </summary>
        private static RaycastHit _raycastHitInfo;
        
        /// <summary>
        ///     Ищет место для создания объекта заданного размера
        /// </summary>
        /// <param name="height">Высота, на которой нужно создать объект</param>
        /// <param name="radius">Радиус объекта</param>
        /// <returns></returns>
        public static Vector3 FindPlaceForSpawn(float height, float radius) {
            int layerMask = 1 << 9;
            layerMask = ~layerMask;
            // (possible) TODO: reserve space on network before spawn 
            Vector3 pos1, pos2;
            for (int iterCount = 0; ; iterCount++) {
                pos1 = pos2 = Client.client.spawnPolygon.RandomPoint();
                pos1.y = -3;
                pos2.y = height;
                if (iterCount++ >= 100) {
                    Debug.LogError($"Unable to find free place for object with height: {height:F2}, radius: {radius:F2}");
                    return new Vector3(0, height  + Random.value * 10, 0);
                }
               // Assert.IsTrue(iterCount++ < 100, $"Unable to find free place for object with height: {height:F2}, radius: {radius:F2}");
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