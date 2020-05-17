using Character.HP;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда, сообщающая что нужно изменить здоровье объекта
    /// </summary>
    public partial class ChangeHPCommand {
        /// <summary>
        ///     id объекта, у которого нужно изменить здоровье
        /// </summary>
        public int id;
        /// <summary>
        ///     Изменение здоровья
        /// </summary>
        public HPChange HpChange;

        /// <summary>
        ///     Применяет изменение здоровья
        /// </summary>
        public void Run() {
            HPController.ApplyHPChange(ObjectID.GetObject(id), HpChange);
        }
    }
}