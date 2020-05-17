using Character;
using Character.Actions;
using Character.Guns;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     
    /// </summary>
    public partial class PickUpGunCommand {
        public int player;
        public int gun;

        

        public void Run()
        {
            var player = ObjectID.GetObject(this.player);
            var gunObject = ObjectID.GetObject(this.gun);
            if (player == null || gunObject == null)
            {
                Debug.LogWarning("Player or gun null.");
                return;
            }

            var managed = player.GetComponent<ActionController>();
            if (managed == null) {
                Client.client.RemoveObject(gunObject);
                return;
            };

            ReloadingGun gun = gunObject.GetComponent<PistolController>()?.gun ?? 
                               gunObject.GetComponent<SemiautoController>()?.gun ??
                               gunObject.GetComponent<ShotgunController>()?.gun ??
                               gunObject.GetComponent<BombGunController>()?.gun as ReloadingGun;/*??
                  managed.GetComponent<SemiautoController>()?.gun as ReloadingGun;*/

            Assert.IsNotNull(gun);
            if (gun is Pistol || gun is ShotGun || gun is BombGun) {
                managed.SetAction<ShootPistolAction>(action => action.gun = gun);
            } else {
                managed.SetAction<ShootSemiautoAction>(action => action.gun = gun);
            }
            
            Client.client.RemoveObject(gunObject);
            // managed.SetAction<();
        }
    }
}