namespace Util2 {
    /// <summary>
    ///     Класс с дополнительными методами
    /// </summary>
    public static class gUtil {
        /// <summary>
        ///     Меняет переменные местами
        /// </summary>
        /// <param name="lhs">Первая переменная</param>
        /// <param name="rhs">Вторая переменная</param>
        /// <typeparam name="T">Тип переменных</typeparam>
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}