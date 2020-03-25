using UnityEditor;

namespace Editor {
    [CustomEditor(typeof(Client))]
    public class ClientEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            
            
            EditorGUILayout.HelpBox(ObjectID.ToString(), UnityEditor.MessageType.Info);
        }
    }
}