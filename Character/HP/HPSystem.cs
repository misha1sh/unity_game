using UnityEngine;

namespace Character.HP {
    public static class HPSystem {
        public static void ApplyHPChange(GameObject target, HPChange hpChange) {
            if (hpChange.source == DamageSource.None()) return;
            var hp = target.GetComponent<HPController>();
            hp._applyHpChange(hpChange);
        }
        
    }
}