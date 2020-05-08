using GameMode;

namespace CommandsSystem.Commands {
    public partial class AddOrChangeInstance {
        public Instance instance;

        public void Run() {
            if (instance.id == InstanceManager.currentInstance.id) return;
            for (int i = 0; i < InstanceManager.instances.Count; i++) {
                if (InstanceManager.instances[i].id == instance.id) {
                    InstanceManager.instances[i] = instance;
                    return;
                }
            }
            InstanceManager.instances.Add(instance);
        }
    }
}