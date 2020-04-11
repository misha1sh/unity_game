namespace GameMode {
    public interface IGameMode {
        // false means need stop
        bool Update();
        bool Stop();
        float TimeLength();
    }
}