
namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда для смены владельца у объекта
    /// </summary>
    public partial class TakeOwnCommand {
        /// <summary>
        ///     Id объекта, у которого меняется владелец
        /// </summary>
        public int objectId;
        /// <summary>
        ///     Id нового владельца объекта
        /// </summary>
        public int owner;

        /// <summary>
        ///     Применяет изменения владельца
        /// </summary>
        public void Run() {
            int curOwner;
            if (!ObjectID.TryGetOwner(objectId, out curOwner)) return;

            if (curOwner != 0) {
                return;
            }
            
            ObjectID.SetOwner(objectId, owner);
            foreach (var component in ObjectID.GetObject(objectId).GetComponents<IOwnedEventHandler>()) {
                component.HandleOwnTaken(owner);
            }
        }
    }
}
