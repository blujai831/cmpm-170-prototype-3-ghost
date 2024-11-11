using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from Wikipedia pseudocode

public class AStarForGridConstrainedMotion2D : MonoBehaviour
{
    private class PathNodeComparer : IComparer<Vector2Int> {
        private Dictionary<Vector2Int, float> _fScore;
        public PathNodeComparer(Dictionary<Vector2Int, float> fScore) {
            _fScore = fScore;
        }
        int IComparer<Vector2Int>.Compare(Vector2Int a, Vector2Int b) {
            bool fScoreAExists = _fScore.ContainsKey(a);
            bool fScoreBExists = _fScore.ContainsKey(b);
            if (fScoreAExists && !fScoreBExists) {
                return -1;
            } else if (!fScoreAExists && !fScoreBExists) {
                return 0;
            } else if (!fScoreAExists && fScoreBExists) {
                return 1;
            } else {
                var fScoreA = _fScore[a];
                var fScoreB = _fScore[b];
                if (fScoreA < fScoreB) {
                    return -1;
                } else if (fScoreA > fScoreB) {
                    return 1;
                } else {
                    return 0;
                }
            }
        }
    }

    private GridConstrainedMotion2D _mover;
    private List<Direction2D> _assignedPath;

    void Start() {
        _mover = GetComponent<GridConstrainedMotion2D>();
        _assignedPath = new List<Direction2D>();
    }

    void FixedUpdate() {
        if (!_mover.Moving() && _assignedPath.Count > 0) {
            if (_mover.TryMove(_assignedPath[0])) {
                _assignedPath.RemoveAt(0);
            }
        }
    }

    private List<Vector2Int> ReconstructPath(
        Dictionary<Vector2Int, Vector2Int> cameFrom,
        Vector2Int current
    ) {
        var totalPath = new List<Vector2Int>();
        totalPath.Insert(0, current);
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    private List<Vector2Int> AStar(Vector2Int start, Vector2Int goal) {
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, float>();
        gScore[start] = 0.0f;
        var fScore = new Dictionary<Vector2Int, float>();
        fScore[start] = Vector2Int.Distance(start, goal);
        var openSet = new SortedSet<Vector2Int>(new PathNodeComparer(fScore));
        openSet.Add(start);
        while (openSet.Count > 0) {
            Vector2Int current = Vector2Int.zero;
                // Will be overwritten but compiler doesn't think so
            foreach (var first in openSet) {
                current = first;
                break;
            }
            openSet.Remove(current);
            if (current == goal) {
                return ReconstructPath(cameFrom, current);
            }
            foreach (
                Direction2D direction in
                System.Enum.GetValues(typeof(Direction2D))
            ) {
                if (_mover.CanMoveFrom(current, direction)) {
                    var tentativeGScore = gScore[current] + 1;
                    var neighbor = current + direction.ToVector2Int();
                    if (
                        !gScore.ContainsKey(neighbor) ||
                        tentativeGScore < gScore[neighbor]
                    ) {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        openSet.Remove(neighbor);
                        fScore[neighbor] =
                            tentativeGScore +
                            Vector2Int.Distance(neighbor, goal);
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    private List<Direction2D> VecPathToDirPath(List<Vector2Int> path) {
        if (path == null) {
            return null;
        }
        Vector2Int current = Vector2Int.zero;
        bool anyYet = false;
        var result = new List<Direction2D>();
        foreach (var vec in path) {
            if (!anyYet) {
                anyYet = true;
            } else {
                result.Add((vec - current).ToDirection2D());
            }
            current = vec;
        }
        return result;
    }

    public void MoveToward(Vector2Int where) {
        _assignedPath = VecPathToDirPath(AStar(_mover.GridPosition, where));
        if (_assignedPath == null) {
            _assignedPath = new List<Direction2D>();
        }
    }

    public bool AnyPathAssigned() {
        return _assignedPath.Count > 0;
    }
}
