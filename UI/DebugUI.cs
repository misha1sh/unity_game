using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DebugUI : MonoBehaviour {
        public Text textPanel;
        
        
        private static bool debugTextDirty = true;
        private static string[] _debugText = new string[10];

        
        
        public static string[] debugText {
            get {
                debugTextDirty = true;
                return _debugText;
            }
        }

        private void Start() {
            debugText[0] = "DEBUG MODE";
        }

        private void Update() {
            if (debugTextDirty) {
                textPanel.text = String.Join("\n", debugText);
                debugTextDirty = false;
            }
        }
    }
}