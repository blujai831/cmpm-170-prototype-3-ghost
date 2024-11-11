using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaiveVector2IntComparer : IComparer<Vector2Int>
{
    int IComparer<Vector2Int>.Compare(Vector2Int a, Vector2Int b) {
        if (a.y < b.y) {
            return -1;
        } else if (a.y > b.y) {
            return 1;
        } else if (a.x < b.x) {
            return -1;
        } else if (a.x > b.x) {
            return 1;
        } else {
            return 0;
        }
    }
}
