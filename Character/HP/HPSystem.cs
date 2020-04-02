using UnityEngine;

namespace Character.HP {
    public static class HPSystem {
        public static void ApplyHPChange(GameObject target, HPChange hpChange) {
            target.GetComponent<HPController>().currentHp += hpChange.delta;
        }
    }
}