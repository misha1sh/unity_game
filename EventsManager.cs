using UnityEngine;

public class EventsManager : MonoBehaviour {

//    public GameObject ui;

    public static EventsHandler handler;
    private EventsHandler m_handler;
    
    private void OnEnable() {
        m_handler = new EventsHandler();
        handler = m_handler;
    }
}
