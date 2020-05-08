using Character.HP;
using UnityEngine;

public class DownBorder : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        var hp = other.GetComponent<HPController>();
        if (hp != null) {
            hp.TakeDamage(1e9f, DamageSource.InstaKill(), true);
        } else {
            Client.client.RemoveObject(other.gameObject);
        }
    }
}
