using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] public float WalkSpeed;
    [SerializeField] public float Deceleration;

    private FramewiseAnimator _animator;
    private Rigidbody2D _rigidbody;
    private InputAction _moveAction;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<FramewiseAnimator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _moveAction = InputSystem.actions.FindAction("move");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        if (moveValue.magnitude > 0) {
            Walk(moveValue);
            UpdateFacing(moveValue);
        } else {
            StopWalking();
        }
        Decelerate();
    }

    void UpdateFacing(Vector2 moveValue) {
        if (Math.Abs(moveValue.x) < Math.Abs(moveValue.y)) {
            if (moveValue.y > 0) {
                _animator.FacingDirection = Direction2D.Up;
            } else {
                _animator.FacingDirection = Direction2D.Down;
            }
        } else {
            if (moveValue.x < 0) {
                _animator.FacingDirection = Direction2D.Left;
            } else {
                _animator.FacingDirection = Direction2D.Right;
            }
        }
    }

    void Walk(Vector2 moveValue) {
        _animator.CurrentAnimation = "walk";
        _rigidbody.AddForce(moveValue*WalkSpeed);
    }

    void StopWalking() {
        _animator.CurrentAnimation = "stand";
    }

    void Decelerate() {
        _rigidbody.AddForce(-_rigidbody.velocity*Deceleration);
    }
}
