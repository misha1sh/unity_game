using CommandsSystem.Commands;

namespace Interpolation.Managers {
    /// <summary>
    ///     Компонента для персонажа, управляемого из другого клиента
    /// </summary>
    public class PlayerUnmanagedGameObject : UnmanagedGameObject<PlayerProperty> {
        /// <summary>
        ///     Обрабатывает событие конца анимации.
        ///     Нужен, чтобы unity не кидал warning, что событие не было обработано
        /// </summary>
        public void pushEnd() {}
    }
}