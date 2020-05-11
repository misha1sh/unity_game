using UnityEngine;

namespace Events {
    public delegate void PlayerObjParameterChanged<T>(GameObject player, T parameter);

    
    public class EventsManager : MonoBehaviour {

//    public GameObject ui;

        public static EventsHandler handler;
        //private EventsHandler m_handler;
    
        private void Awake() {
            //  m_handler = new EventsHandler();
            handler = new EventsHandler();
            MainUIController.mainui.SetupHandlers();
        }
    }
}
