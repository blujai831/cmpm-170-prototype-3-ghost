using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridConstrainedMotion2D))]
[RequireComponent(typeof(FramewiseAnimator))]
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private GameObject _flashlight;
    [SerializeField] private float _flashlightTurnSpeed;
    [SerializeField] private int _expectedPowerupDuration;

    private GridConstrainedMotion2D _mover;
    private FramewiseAnimator _animator;
    private InputAction _moveAction;
    private int _powerupTimer;

    // Start is called before the first frame update
    void Start()
    {
        _mover = GetComponent<GridConstrainedMotion2D>();
        _animator = GetComponent<FramewiseAnimator>();
        _moveAction = InputSystem.actions.FindAction("move");
        _powerupTimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessMoveInput();
        UpdateAnimation();
        UpdateFlashlightAngle();
        UpdatePowerupState();
    }

    private void ProcessMoveInput() {
        var moveValue = _moveAction.ReadValue<Vector2>();
        if (moveValue.magnitude > 0.0f) {
            Direction2D direction;
            if (Math.Abs(moveValue.x) < Math.Abs(moveValue.y)) {
                if (moveValue.y > 0) {
                    direction = Direction2D.Up;
                } else {
                    direction = Direction2D.Down;
                }
            } else {
                if (moveValue.x < 0) {
                    direction = Direction2D.Left;
                } else {
                    direction = Direction2D.Right;
                }
            }
            _mover.TryMove(direction);
        }
    }

    private string GetCorrectAnimation() {
        bool poweredUp = _powerupTimer > 0;
        bool moving = _mover.Moving();
        if (!poweredUp && !moving) return "Stand";
        if (!poweredUp && moving) return "Walk";
        if (poweredUp && !moving) return "Stand Powerup";
        if (poweredUp && moving) return "Walk Powerup";
        Contract.Assert(false); return "";
    }

    private void UpdateAnimation() {
        _animator.CurrentAnimation = GetCorrectAnimation();
        _animator.FacingDirection = _mover.FacingDirection;
    }

    private void UpdateFlashlightAngle() {
        var targetQ = Quaternion.identity;
        switch (_animator.FacingDirection) {
            case Direction2D.Up:
                targetQ = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                break;
            case Direction2D.Left:
                targetQ = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                break;
            case Direction2D.Right:
                targetQ = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                break;
        }
        _flashlight.transform.rotation = Quaternion.Slerp(
            _flashlight.transform.rotation,
            targetQ,
            Time.deltaTime*_flashlightTurnSpeed
        );
    }

    private void UpdatePowerupState() {
        if (_powerupTimer > 0) {
            _powerupTimer--;
        }
    }

    public bool HasPowerup() {
        return _powerupTimer > 0;
    }

    public void GivePowerup(int duration) {
        _powerupTimer += duration;
    }

    public float PowerupAmountRemaining {
        get => _powerupTimer*1.0f/_expectedPowerupDuration;
    }
}
