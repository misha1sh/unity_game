using System.Collections.Generic;

namespace GameMode {
    public class InstanceManager {
        public static List<Instance> instances = new List<Instance>();
        public static Instance currentInstance;
        public static int ID => currentInstance.id;

        public static void Init() {
            currentInstance = new Instance(ObjectID.RandomID);
            instances.Add(currentInstance);
        }

        public static void Reset() {
            instances.Clear();
            if (currentInstance != null) {
                instances.Add(currentInstance);
                currentInstance.currentLoadedGamemodeNum = -1;
            }
        }
    }
}