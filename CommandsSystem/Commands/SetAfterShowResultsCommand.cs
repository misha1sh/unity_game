using GameMode;

namespace CommandsSystem.Commands {
    public partial class SetAfterShowResultsCommand {
        public int kek;
        
        public void Run() {
            GameManager.SetAfterShowResults();
        }
    }
}