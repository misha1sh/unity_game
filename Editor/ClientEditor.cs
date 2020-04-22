using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(Client))]
    public class ClientEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();

            if (Application.isPlaying) {
                EditorGUILayout.TextArea("ID: " + sClient.ID);
                EditorGUILayout.TextArea(ObjectID.ToString());
            }
        }
    }
}