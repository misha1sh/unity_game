using UnityEngine;

namespace Character.HP {
    public static class HPSystem {
        public static void ApplyHPChange(GameObject target, HPChange hpChange) {
            if (hpChange.source == DamageSource.None()) return;
            target.GetComponent<HPController>().currentHp += hpChange.delta;
        }
    }
}