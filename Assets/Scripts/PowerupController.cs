using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [SerializeField] private int _duration;
    void OnTriggerEnter2D(Collider2D other) {
        var pc = other.GetComponent<PlayerCharacterController>();
        if (pc != null) {
            pc.GivePowerup(_duration);
            Destroy(this.gameObject);
        }
    }
}
