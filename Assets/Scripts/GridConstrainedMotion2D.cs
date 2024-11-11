using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridConstrainedMotion2D : MonoBehaviour
{
    [SerializeField] private GameObject _gridObject;
    [SerializeField] private int _ticksPerMove;
    [SerializeField] private GameObject _scannerObject;

    [SerializeField] public Direction2D FacingDirection = Direction2D.Down;

    private Grid _grid;
    private GridConstrainedAreaScanner2D _scanner;
    private Vector2Int _gridPosition;
    private Vector2Int _targetGridPosition;
    private int _moveCountdown;
    private Direction2D _queuedMove;
    private Direction2D _currentMove;
    private bool _anyMoveQueued;

    public bool DirectionFix;

    // Start is called before the first frame update
    void Start()
    {
        _grid = _gridObject.GetComponent<Grid>();
        if (_scannerObject == null) {
            _scanner = null;
        } else {
            _scanner = _scannerObject.GetComponent<
                GridConstrainedAreaScanner2D
            >();
        }
        _gridPosition = GetRealGridPosition();
        _targetGridPosition = _gridPosition;
        SnapToGrid();
        _moveCountdown = -1;
        _anyMoveQueued = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Moving()) {
            UpdateMove();
        } else if (_anyMoveQueued) {
            TryMove(_queuedMove);
            _anyMoveQueued = false;
        }
    }

    public Vector2Int GetRealGridPosition() {
        return GridConstrainedLogic2D.GetRealGridPosition(_grid, gameObject);
    }

    public void SnapToGrid() {
        GridConstrainedLogic2D.SnapToGrid(_grid, gameObject, _gridPosition);
    }

    public void Move(Direction2D direction) {
        if (Moving()) {
            if (_currentMove == direction) {
                _anyMoveQueued = false;
            } else {
                _anyMoveQueued = true;
                _queuedMove = direction;
            }
        } else {
            if (!DirectionFix) {
                FacingDirection = direction;
            }
            _currentMove = direction;
            if (_ticksPerMove <= 0) {
                _gridPosition += direction.ToVector2Int();
                SnapToGrid();
                _targetGridPosition = _gridPosition;
            } else {
                _targetGridPosition += direction.ToVector2Int();
                _moveCountdown = _ticksPerMove;
            }
        }
    }

    public bool TryMove(Direction2D direction) {
        if (!DirectionFix && !Moving()) {
            FacingDirection = direction;
        }
        if (CanMove(direction)) {
            Move(direction);
            return true;
        } else {
            return false;
        }
    }

    public bool CanMoveFrom(Vector2Int fromWhichCell, Direction2D direction) {
        return GridConstrainedLogic2D.CanMove(
            _grid,
            fromWhichCell,
            direction,
            _scanner
        );
    }

    public bool CanMove(Direction2D direction) {
        return CanMoveFrom(_targetGridPosition, direction);
    }

    public bool Moving() {
        return _targetGridPosition != _gridPosition;
    }

    public void Teleport(Vector2Int where) {
        _gridPosition = where;
        _targetGridPosition = where;
        SnapToGrid();
        _moveCountdown = -1;
    }

    private void UpdateMove() {
        if (_moveCountdown <= 0) {
            _gridPosition = _targetGridPosition;
            SnapToGrid();
            _moveCountdown = -1;
        } else {
            var fromWhere = _grid.GetCellCenterWorld(
                new Vector3Int(_gridPosition.x, _gridPosition.y, 0)
            );
            var toWhere = _grid.GetCellCenterWorld(
                new Vector3Int(_targetGridPosition.x, _targetGridPosition.y, 0)
            );
            var progress =
                1.0f - ((float) _moveCountdown)/((float) _ticksPerMove + 1.0f);
            transform.position = new Vector3(
                fromWhere.x + (toWhere.x - fromWhere.x)*progress,
                fromWhere.y + (toWhere.y - fromWhere.y)*progress,
                transform.position.z
            );
            _moveCountdown--;
        }
    }

    public Vector2Int GridPosition {get => _targetGridPosition;}

    public Grid Grid {get => _grid;}
}
