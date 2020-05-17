/// <summary>
///     Интерфейс для обработки событий, когда у объекта меняется владелец
/// </summary>
public interface IOwnedEventHandler {
    /// <summary>
    ///     Обработчик события, когда у объекта меняется владелец
    /// </summary>
    /// <param name="owner">Новый владелец объекта</param>
    void HandleOwnTaken(int owner);
}
