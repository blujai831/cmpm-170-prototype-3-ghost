using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollowerController : MonoBehaviour
{
    [SerializeField] public GameObject Target;

    private AStarForGridConstrainedMotion2D _aStar;
    private GridConstrainedMotion2D _mover;
    private GridConstrainedMotion2D _targetMover;
    private Vector2Int _targetLastKnownLocation;
    private bool _everSetTargetLastKnownLocation;

    // Start is called before the first frame update
    void Start()
    {
        _aStar = GetComponent<AStarForGridConstrainedMotion2D>();
        _mover = GetComponent<GridConstrainedMotion2D>();
        _targetMover = Target.GetComponent<GridConstrainedMotion2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_mover.Moving()) {
            if ((
                !_everSetTargetLastKnownLocation ||
                _targetLastKnownLocation != _targetMover.GridPosition
            ) && _mover.GridPosition != _targetMover.GridPosition) {
                _targetLastKnownLocation = _targetMover.GridPosition;
                _everSetTargetLastKnownLocation = true;
                _aStar.MoveToward(_targetMover.GridPosition);
            }
            if (!_aStar.AnyPathAssigned()) {
                _mover.TryMove((Direction2D) (
                    System.Enum.GetValues(typeof(Direction2D)).GetValue(
                        (int) (Random.value*4.0f)
                    )
                ));
            }
        }
    }
}
