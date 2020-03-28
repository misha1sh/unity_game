using System.Collections;
using UnityEngine;

namespace Character.Actions {
    public interface IAction {
        IEnumerator OnStartDoing();
        IEnumerator OnStopDoing();
    }
}