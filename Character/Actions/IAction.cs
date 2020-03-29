using System.Collections;
using UnityEngine;

namespace Character.Actions {
    public interface IAction {
        void OnStartDoing();
        void OnStopDoing();
    }
}