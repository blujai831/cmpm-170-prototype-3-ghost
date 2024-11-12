using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridConstrainedMotion2D))]
public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _powerupToSpawn;
    [SerializeField] private int _interval;
    [SerializeField] private int _powerupSortingOrder;

    private GridConstrainedMotion2D _mover;
    private GameObject _powerupInstance = null;
    private int _countdown;

    void Start() {
        _mover = GetComponent<GridConstrainedMotion2D>();
        _countdown = _interval;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_countdown <= 0) {
            SpawnPowerup();
            _countdown = _interval;
        } else {
            _countdown--;
        }
    }

    public void SpawnPowerup() {
        if (_powerupInstance != null) {
            Destroy(_powerupInstance);
        }
        _powerupInstance = Instantiate(
            _powerupToSpawn,
            transform.position,
            Quaternion.identity
        );
        var powerupGridMover =
            _powerupInstance.GetComponent<GridConstrainedMotion2D>();
        if (powerupGridMover != null) {
            _mover.TransferSceneContext(powerupGridMover);
        }
        var powerupSpriteRenderer =
            _powerupInstance.GetComponent<SpriteRenderer>();
        if (powerupSpriteRenderer != null) {
            powerupSpriteRenderer.sortingOrder = _powerupSortingOrder;
        }
    }
}
