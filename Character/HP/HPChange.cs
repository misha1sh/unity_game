namespace Character.HP {
    /// <summary>
    ///     Структура, представляющая собой изменение здоровья игрового объекта
    /// </summary>
    public struct HPChange {
        /// <summary>
        ///     Дельта, на которую изменилось здоровье
        /// </summary>
        public float delta;
        /// <summary>
        ///     Источник изменения здоровья
        /// </summary>
        public int source;

        /// <summary>
        ///     Конструктор изменения здоровья
        /// </summary>
        /// <param name="delta">Дельта, на которую изменилось здоровье</param>
        /// <param name="source">Источник изменения здоровья</param>
        public HPChange(float delta, int source) {
            this.delta = delta;
            this.source = source;
        }
        
    }
}