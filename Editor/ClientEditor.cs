using UnityEditor;
using UnityEngine;

namespace Editor {
    /// <summary>
    ///     Класс для показа отладочной информации в инспекторе Unity
    /// </summary>
    [CustomEditor(typeof(Client))]
    public class ClientEditor : UnityEditor.Editor {
        /// <summary>
        ///     Выводит отладочную информацию в инспектор Unity
        /// </summary>
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (Application.isPlaying) {
                EditorGUILayout.TextArea("ID: " + sClient.ID);
                EditorGUILayout.TextArea(ObjectID.ToString());
            }
        }
    }
}