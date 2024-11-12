using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EMFDetectorController : MonoBehaviour
{
    private CircleCollider2D _collider;
    private List<GameObject> _enemiesInRange;
    private float _shortestDistance;

    void Start() {
        _collider = GetComponent<CircleCollider2D>();
        _enemiesInRange = new List<GameObject>();
        _shortestDistance = Single.PositiveInfinity;
    }

    void FixedUpdate() {
        RecalcShortestDistance();
    }

    void OnTriggerEnter2D(Collider2D other) {
        var gobj = other.gameObject;
        if (
            !_enemiesInRange.Contains(gobj) &&
            gobj.GetComponent<EnemyController>() != null
        ) {
            _enemiesInRange.Add(gobj);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        _enemiesInRange.Remove(other.gameObject);
    }

    private void RecalcShortestDistance() {
        _shortestDistance = Single.PositiveInfinity;
        foreach (var enemy in _enemiesInRange) {
            float candidate =
                Vector3.Distance(transform.position, enemy.transform.position);
            if (candidate < _shortestDistance) {
                _shortestDistance = candidate;
            }
        }
    }

    public int Count {get => _enemiesInRange.Count;}
    public float ShortestDistance {get => _shortestDistance;}
    public float Radius {get => _collider.radius;}
    public float DangerLevel {
        get => (Count > 0 && ShortestDistance < Radius) ?
            (Radius - ShortestDistance)/Radius :
            0.0f;
    }
}
