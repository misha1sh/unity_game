
namespace CommandsSystem.Commands {
    public partial class TakeOwnCommand {
        public int objectId;
        public int owner;

        public void Run() {
            int curOwner;
            if (!ObjectID.TryGetOwner(objectId, out curOwner)) return;

            if (curOwner != 0) {
                return;
                //Debug.LogWarning("Changed owner of " + ObjectID.GetObject(objectId).name);
            }
            
            ObjectID.SetOwner(objectId, owner);
            foreach (var component in ObjectID.GetObject(objectId).GetComponents<IOwnedEventHandler>()) {
                component.HandleOwnTaken(owner);
            }
        }
    }
}
