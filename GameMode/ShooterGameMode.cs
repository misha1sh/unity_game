namespace GameMode {
    public class ShooterGameMode : IGameMode {

        private enum STATE {
            INIT,
            
        }
        
        public bool Update() {
            
            
            return true;
        }

        public bool Stop() {
            return false;
        }


        public float TimeLength() {
            return 60f;
        }
    }
}