using Character.HP;
using CommandsSystem;
using CommandsSystem.Commands;
using Networking;
using UnityEngine;
using Util2;
using Physics = UnityEngine.Physics;

namespace Character.Guns {
    /// <summary>
    ///     Класс с функциями для стрельбы
    /// </summary>
    public static class ShootSystem {
        
        /// <summary>
        ///     Находит раположение оружия у игрока
        /// </summary>
        /// <param name="characterPosition">Координаты игрока</param>
        /// <returns>Расположение оружия</returns>
        public static Vector3 GetGunPosition(Vector3 characterPosition) {
            return characterPosition + Vector3.up * 1.5f;
        }
        
        /// <summary>
        ///     Рисует трассер от выстрела
        /// </summary>
        /// <param name="start">Координата, откуда был произведен выстрел</param>
        /// <param name="stop">Координата, куда попал выстрел</param>
        public static void DrawTracer(Vector3 start, Vector3 stop) {
            Client.client.bulletTrailRenderer.MoveFromTo(start, stop);
           // DebugExtension.DrawArrow(start, stop - start);
          /*  RotaryHeart.Lib.PhysicsExtension.DebugExtensions.DebugCapsule(start, stop, Color.magenta,
                drawDuration: 100f, preview: PreviewCondition.Both);*/
        }
        
        /// <summary>
        ///     Выпускает пулю из заданной позиции и проверяет в какой объект она попала
        ///     Отрисовывает выстрел.
        /// </summary>
        /// <param name="transform">Положение игрока, выпустившего пулю</param>
        /// <param name="rotation">Направление, в котором дожна лететь пуля</param>
        /// <param name="directionDelta">Насколько нужно отклонить пулю от правильного направления</param>
        /// <param name="raycastRes">Информация о попаданнии пули<param>
        /// <param name="command">Команда отрисовки трассера от выстрела.</param>
        /// <returns>true если пуля попала в какой-то объект, иначе - false</returns>
        public static bool SimpleRaycast(Transform transform, Quaternion rotation, Vector3 directionDelta, out RaycastHit raycastRes, out ICommand command) {
            var position = GetGunPosition(transform.position);
            var direction = rotation * (Vector3.forward + directionDelta);

            /*RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(position, direction, drawDuration: 0.1f,
                hitColor: Color.red, noHitColor: Color.white, preview: PreviewCondition.Both);*/
            bool rres = Physics.Raycast(position, direction, out raycastRes);
            if (rres && ObjectID.TryGetID(raycastRes.collider.gameObject, out int targetID)) {
                var t = ObjectID.GetID(raycastRes.collider.gameObject);
                DrawTracer(position, raycastRes.point);
                command = new DrawTargetedTracerCommand(ObjectID.GetID(transform.gameObject), targetID, new HPChange());
            } else if (rres) {
                Vector3 target = raycastRes.point;
                DrawTracer(position, target);
                command = new DrawPositionTracerCommand(ObjectID.GetID(transform.gameObject), target);
            } else {
                Vector3 target = position + direction.normalized * 100;
                DrawTracer(position, target);
                command = new DrawPositionTracerCommand(ObjectID.GetID(transform.gameObject), target);
            }
            return rres;
        }

        /// <summary>
        ///     Переменнаая для хранения результата от попадания пули
        /// </summary>
        private static RaycastHit _raycastHit;
        /// <summary>
        ///     Производит выстрел с уроном
        /// </summary>
        /// <param name="gameObject">Игрок, производящий выстрел</param>
        /// <param name="rotation">Направление, в котором нужно произвести выстрел</param>
        /// <param name="directionDelta">Отклонение от направления выстрела</param>
        /// <param name="damage">Урон от выстрела</param>
        /// <returns>true, если в результате выстрела был нанесен урон. Иначе false</returns>
        public static bool ShootWithDamage(GameObject gameObject, Quaternion rotation, Vector3 directionDelta, float damage) {
            ICommand command;
            var raycastRes = SimpleRaycast(gameObject.transform, rotation, directionDelta,  out _raycastHit, out command);
            
            if (raycastRes != false) {
                var other = _raycastHit.collider.gameObject;
                var hp = other.GetComponent<HPController>();

                if (hp != null) {
                    float realDamage = hp.TakeDamage(damage, DamageSource.Player(gameObject), false);
                    if (command is DrawTargetedTracerCommand c) {
                        c.HpChange.delta = -realDamage;
                        c.HpChange.source = DamageSource.Player(c.player);
                    }
                    
                    CommandsHandler.gameModeRoom.RunSimpleCommand(command, MessageFlags.NONE);
                    return true;
                }
            }
            
            CommandsHandler.gameModeRoom.RunSimpleCommand(command, MessageFlags.NONE);
            return false;
        }

        /// <summary>
        ///     Создает случайное отклонение на основе распределения Гаусса
        /// </summary>
        /// <param name="sigma">Коэффициент в распределении Гаусса</param>
        /// <returns>Случайное отклонение</returns>
        public static Vector3 RandomDelta(double sigma) {
            float x = (float) sClient.random.NextGaussian(0, sigma);
            float y = (float) sClient.random.NextGaussian(0, sigma);
            return new Vector3(x, y, 0);
        }

        /// <summary>
        ///     Производит выстрел бомбой
        /// </summary>
        /// <param name="gameObject">Игрок, который произвел выстрел</param>
        /// <param name="target">Координата, в которую игрок производит выстрел</param>
        /// <param name="bombPrefab">Название префаба бомбы</param>
        public static void ShootWithBomb(GameObject gameObject, Vector3 target, string bombPrefab) {
            Vector3 start = GetGunPosition(gameObject.transform.position);
            start += (target - start).normalized * 0.7f;
            
            float len = (target - start).magnitude;
            Vector3 medium = (start + target) / 2 + Vector3.up * len;


            float totalTime = len / 10f;
            
            CommandsHandler.gameModeRoom.RunSimpleCommand(new SpawnParabolaFlyingCommand(
                new SpawnPrefabCommand(bombPrefab, start, Quaternion.identity, ObjectID.RandomID, sClient.ID, ObjectID.GetID(gameObject)),
                medium, target, totalTime), MessageFlags.IMPORTANT);
        }
    }
}