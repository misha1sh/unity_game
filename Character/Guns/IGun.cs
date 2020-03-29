namespace Character.Guns {
    public interface IGun {
        GunState state { get; }
        void OnPickedUp();
        void OnDropped();
    }
}