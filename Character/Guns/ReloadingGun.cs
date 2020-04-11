using System;
using CommandsSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.Guns {
    [Serializable]
    public abstract class ReloadingGun : IGun {
        public int _state = (int)GunState.READY;
        public GunState state {
            get => (GunState) _state;
            set => _state = (int) value;
        }

        public Vector3 position;
        public int id;

        public abstract float GetFullReloadTime();
        public abstract float GetReloadTime();
        public abstract int GetBulletsInMagazine();

        // includes current bullet
        public int _bulletsCount;
        public int bulletsCount { 
            get => _bulletsCount;
            private set {
                _bulletsCount = value;
                if (player != null) {
                    EventsManager.handler.OnPlayerBulletsCountChanged(player, _bulletsCount);
                }
            } 
        }

        // not includes current magazine
        public int _magazinesCount = 3;
        public int magazinesCount {
            get => _magazinesCount;
            private set {
                _magazinesCount = value;
                if (player != null) {
                    EventsManager.handler.OnPlayerMagazinesCountChanged(player, _magazinesCount);
                }
            }
        }
        


        

        public ReloadingGun() {
            bulletsCount = GetBulletsInMagazine();
            state = GunState.READY;
            id = ObjectID.RandomID;
        }


        protected GameObject player;
        public void OnPickedUp(GameObject player) {
            this.player = player;
            if (bulletsCount == 0) {
                SetReloadMagazine();
            }            
        }

        public bool IsEmpty() => bulletsCount == 0 && magazinesCount == 0;

        public void OnDropped() {
            if (!IsEmpty()) // just destroy it
            {
                var motionController = player.GetComponent<MotionController>();
                Vector3 dir;
               /* var dir = motionController.TargetDirection;
                if (dir.sqrMagnitude < 0.01f) {*/
                    var rig = player.GetComponent<Rigidbody>();
                    dir = rig.velocity;
                    if (dir.sqrMagnitude < 0.01f)
                        dir = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);//player.transform.forward;
                //}

                id = ObjectID.RandomID;
                this.position = player.transform.position - dir.normalized * 2  + Vector3.up;
                Client.client.commandsHandler.RunSimpleCommand(this as ICommand);
            }
 
            this.player = null;
          /*  
            // drop reloading state
            if (state == GunState.RELOADING_MAGAZINE)
                state = GunState.READY;*/
        }

        private float needTime = 0;
        public void Update(float dt) {
            if (state == GunState.RELOADING_BULLET) {
                needTime -= dt;
                if (needTime <= 0)
                    state = GunState.READY;
            }
            if (state == GunState.RELOADING_MAGAZINE) {
                needTime -= dt;
                if (needTime <= 0) {
                    state = GunState.READY;
                    bulletsCount = GetBulletsInMagazine();
                    magazinesCount--;
                }
            }
        }

        protected abstract void DoShoot();
        
        public void Shoot() {
            if (state == GunState.READY) {
                DoShoot();
                bulletsCount--;
                SetReloadBullet();
          
                if (bulletsCount > 0) {
                    SetReloadBullet();
                } else if (magazinesCount > 0) {
                    SetReloadMagazine();
                } else {
                    //throw  new Exception("werwerwererw");
                    player.GetComponent<ActionController>().SetNothing();
                }
            } else {
                throw new InvalidOperationException("Cannot shoot without bullet");
            }
        }

        private void SetReloadBullet() {
            state = GunState.RELOADING_BULLET;
            needTime = GetReloadTime();
        }

        public void SetReloadMagazine() {
            if (magazinesCount == 0) return;
            bulletsCount = 0;
            state = GunState.RELOADING_MAGAZINE;
            needTime = GetFullReloadTime();
        }
    }
}