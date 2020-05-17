namespace Character {
    /// <summary>
    ///     Структура, хранящая состояние анимации персонажа
    /// </summary>
    public struct PlayerAnimationState {
        /// <summary>
        ///     Должна ли сейчас быть анимация стояния на месте
        /// </summary>
        public bool idle;
        /// <summary>
        ///     Скорость персонажа
        /// </summary>
        public float speed;
        /// <summary>
        ///     Скорость поворота персонажа
        /// </summary>
        public float rotationSpeed;
    }
}