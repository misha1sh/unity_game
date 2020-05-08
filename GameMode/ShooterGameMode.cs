namespace GameMode {
    public class ShooterGameMode : IGameMode {

        private enum STATE {
            INIT,
            
        }
        
        public bool Update() {
            MainUIController.mainui.SetTask( " - Kill enemy = <color=green>+100</color>\n" +
                                               " - Deal <color=red>1</color> damage = <color=green>+1</color>");
            MainUIController.mainui.gunsPanel.SetActive(true);
            
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