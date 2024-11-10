using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestTriangleController : MonoBehaviour
{

    private GridConstrainedMotion2D _mover;
    private SpriteRenderer _spriteRenderer;
    private InputAction _moveAction;

    // Start is called before the first frame update
    void Start()
    {
        _mover = GetComponent<GridConstrainedMotion2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _moveAction = InputSystem.actions.FindAction("move");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessMoveInput();
        UpdateSpriteFacing();
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
            if (!_mover.TryMove(direction) && !_mover.Moving()) {
                Debug.Log("can't move that way");
            }
        }
    }

    private void UpdateSpriteFacing() {
        transform.rotation = Quaternion.identity;
        switch (_mover.FacingDirection) {
            case Direction2D.Up: transform.Rotate(0.0f, 0.0f, 180.0f); break;
            case Direction2D.Down: break;
            case Direction2D.Left: transform.Rotate(0.0f, 0.0f, 270.0f); break;
            case Direction2D.Right: transform.Rotate(0.0f, 0.0f, 90.0f); break;
        }
    }
}
