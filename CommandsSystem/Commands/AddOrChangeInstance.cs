using GameMode;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда, сообщаяющая о изменении одного из Instance в текущей игре
    /// </summary>
    public partial class AddOrChangeInstance {
        /// <summary>
        ///     Изменившийся Instance
        /// </summary>
        public Instance instance;

        /// <summary>
        ///     Применяет изменения
        /// </summary>
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