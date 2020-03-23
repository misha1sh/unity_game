namespace CommandsSystem {
    public interface ICommand {
        CommandType type();

        void Run();
    }
}