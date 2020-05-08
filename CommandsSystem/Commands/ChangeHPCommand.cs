using Character.HP;

namespace CommandsSystem.Commands {
    public partial class ChangeHPCommand {
        public int id;
        public HPChange HpChange;

        public void Run() {
            HPSystem.ApplyHPChange(ObjectID.GetObject(id), HpChange);
        }
    }
}