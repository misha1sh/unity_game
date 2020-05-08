using System.Collections.Generic;
using CommandsSystem.Commands;
using Networking;
using UnityEngine;

namespace GameMode {
    public class Instance {
        public int id;

        public string name;
        // need by gamemanager to detect that all players loaded gamemode
        public int currentLoadedGamemodeNum;
        public Instance() {}

        public Instance(int id) {
            this.id = id;
            this.currentLoadedGamemodeNum = -1;
            this.name = "Player#" + id;
        }


        public void Send() {
            CommandsHandler.gameRoom.RunSimpleCommand(new AddOrChangeInstance(this), MessageFlags.IMPORTANT);
        }
      //  private List<int> ownedObjects;
    }
}