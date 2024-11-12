using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridConstrainedMotion2D))]
[RequireComponent(typeof(AStarForGridConstrainedMotion2D))]
[RequireComponent(typeof(FramewiseAnimator))]
public class EnemyController : MonoBehaviour
{
    private static int _s_count = 0;

    public static int Count {get => _s_count;}

    [SerializeField] private GameObject _playerCharacter;
    [SerializeField] private int _pathRecalcInterval;

    private GridConstrainedMotion2D _mover;
    private AStarForGridConstrainedMotion2D _aStar;
    private FramewiseAnimator _animator;
    private PlayerCharacterController _pcController;
    private GridConstrainedMotion2D _pcMover;
    private bool _pcPoweredUpLastChecked;
    private int _pathRecalcCountdown;

    // Start is called before the first frame update
    void Start()
    {
        _s_count++;
        _mover = GetComponent<GridConstrainedMotion2D>();
        _aStar = GetComponent<AStarForGridConstrainedMotion2D>();
        _animator = GetComponent<FramewiseAnimator>();
        _pcController =
            _playerCharacter.GetComponent<PlayerCharacterController>();
        _pcMover =
            _playerCharacter.GetComponent<GridConstrainedMotion2D>();
        _pcPoweredUpLastChecked = false;
        _pathRecalcCountdown = _pathRecalcInterval;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChaseOrFlee();
        UpdateAnimation();
        UpdatePathRecalcCountdown();
    }

    private void ChaseOrFlee() {
        bool pcPoweredUp = _pcController.HasPowerup();
        if (_pcPoweredUpLastChecked != pcPoweredUp) {
            _pcPoweredUpLastChecked = pcPoweredUp;
            _aStar.CancelPath();
            _pathRecalcCountdown = _pathRecalcInterval;
        }
        if (!_aStar.AnyPathAssigned()) {
            if (pcPoweredUp) {
                Flee();
            } else {
                Chase();
            }
        }
    }

    private void Flee() {
        Vector2Int bestCell = _mover.GridPosition;
        float bestCellScore = 0.0f;
        foreach (var cell in _mover.Scanner.OccupiableCells) {
            float score =
                2.0f*Vector2Int.Distance(_pcMover.GridPosition, cell) -
                Vector2Int.Distance(_mover.GridPosition, cell);
            if (score > bestCellScore) {
                bestCell = cell;
                bestCellScore = score;
            }
        }
        _aStar.MoveToward(bestCell);
    }

    private void Chase() {
        _aStar.MoveToward(_pcMover.GridPosition);
    }

    private void UpdateAnimation() {
        _animator.FacingDirection = _mover.FacingDirection;
    }

    private void UpdatePathRecalcCountdown() {
        if (_pathRecalcCountdown <= 0) {
            _aStar.CancelPath();
            _pathRecalcCountdown = _pathRecalcInterval;
        } else {
            _pathRecalcCountdown--;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        var pc = other.GetComponent<PlayerCharacterController>();
        if (pc != null) {
            if (pc.HasPowerup()) {
                Die();
            } else {
                Storyboard.Lose();
            }
        }
    }

    public void Die() {
        Destroy(this.gameObject);
        --_s_count;
        if (_s_count <= 0) {
            Storyboard.Win();
        }
    }
}
