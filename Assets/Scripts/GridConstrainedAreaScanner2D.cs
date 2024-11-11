using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridConstrainedAreaScanner2D : MonoBehaviour
{
    [SerializeField] private GameObject _gridObject;
    private Grid _grid;
    private HashSet<Vector2Int> _occupiable;
    private List<Vector2Int> _occupiableAsList;
    private bool _initialized = false;
    public Grid Grid {get => _grid;}
    public bool Initialized {get => _initialized;}

    // Start is called before the first frame update
    void Start()
    {
        _grid = _gridObject.GetComponent<Grid>();
        _occupiable = new HashSet<Vector2Int>();
        _occupiableAsList = new List<Vector2Int>();
    }

    void FixedUpdate() {
        if (!_initialized) {
            FindOccupiableCells();
            _initialized = true;
        }
    }

    public bool Occupiable(Vector2Int where) {
        return _occupiable.Contains(where);
    }

    private void FindOccupiableCells() {
        var openSet = new SortedSet<Vector2Int>(new NaiveVector2IntComparer());
        var notOccupiable = new HashSet<Vector2Int>();
        var start =
            GridConstrainedLogic2D.GetRealGridPosition(_grid, gameObject);
        openSet.Add(start);
        if (!GridConstrainedLogic2D.Occupiable(_grid, start)) {
            _initialized = true;
            throw new ArgumentException(
                "GridConstrainedAreaScanner2D placed on solid tile"
            );
        } else {
            _occupiable.Add(start);
            _occupiableAsList.Add(start);
            while (openSet.Count > 0) {
                var current = openSet.Min;
                openSet.Remove(current);
                foreach (
                    Direction2D direction in
                    System.Enum.GetValues(typeof(Direction2D))
                ) {
                    var neighbor = current + direction.ToVector2Int();
                    if (
                        !notOccupiable.Contains(neighbor) &&
                        !_occupiable.Contains(neighbor)
                    ) {
                        if (GridConstrainedLogic2D.Occupiable(
                            _grid, neighbor
                        )) {
                            openSet.Add(neighbor);
                            _occupiable.Add(neighbor);
                            _occupiableAsList.Add(neighbor);
                        } else {
                            notOccupiable.Add(neighbor);
                        }
                    }
                }
            }
        }
    }

    public Vector2Int GetRandomOccupiableCell() {
        return _occupiableAsList[
            (int) (UnityEngine.Random.value*_occupiableAsList.Count)
        ];
    }
}
