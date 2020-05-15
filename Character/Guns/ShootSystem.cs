using Character.HP;
using CommandsSystem;
using CommandsSystem.Commands;
using Networking;
using UnityEngine;
using Util2;
using Physics = UnityEngine.Physics;

namespace Character.Guns {
    public static class ShootSystem {

        public static Vector3 GetGunPosition(Vector3 characterPosition) {
            return characterPosition + Vector3.up * 1.5f;
        }
        
        public static void DrawTracer(Vector3 start, Vector3 stop, float width = 0.1f) {
            Client.client.TrailRenderer.MoveFromTo(start, stop);
           // DebugExtension.DrawArrow(start, stop - start);
          /*  RotaryHeart.Lib.PhysicsExtension.DebugExtensions.DebugCapsule(start, stop, Color.magenta,
                drawDuration: 100f, preview: PreviewCondition.Both);*/
        }
        
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

        private static RaycastHit _raycastHit;
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

        public static Vector3 RandomDelta(double sigma) {
            float x = (float) sClient.random.NextGaussian(0, sigma);
            float y = (float) sClient.random.NextGaussian(0, sigma);
            return new Vector3(x, y, 0);
        }

   /*     // y = (kx - b)**2 - c
        private static float calcb(float k, float x1, float y1, float x2, float y2) {
            if (Mathf.Approximately(x1, x2)) return 0;
            
            return ((y1 - y2) + k * k * (x2 * x2 - x1 * x1)) /  
                   (2 * k * (x2 - x1));
        }

        private static float calcc(float k, float b, float x1, float y1) {
            float tmp = (k * x1 - b);
            return tmp * tmp - y1;
        }

        private static void calcbc(float k, float x1, float y1, float x2, float y2, out float b, out float c) {
            b = calcb(k, x1, y1, x2, y2);
            c = calcc(k, b, x1, y1);
        }
        
        private static void calcInterpolation(float )*/

        
        
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
        /*public static bool ShootRandom)WithDamage(GameObject gameObject, float randomWidth, float randomDeep,
            float damage) {
            float randompoi
        }*/
    }
}