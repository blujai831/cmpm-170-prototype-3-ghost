using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarForGridConstrainedMotion2D))]
public class GridConstrainedRandomWalker2D : MonoBehaviour
{
    private GridConstrainedAreaScanner2D _scanner;
    private AStarForGridConstrainedMotion2D _aStar;

    // Start is called before the first frame update
    void Start()
    {
        _scanner = null;
        _aStar = GetComponent<AStarForGridConstrainedMotion2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_scanner == null) {
            _scanner = GetComponent<GridConstrainedMotion2D>().Scanner;
        }
        if (_scanner.Initialized && !_aStar.AnyPathAssigned()) {
            _aStar.MoveToward(_scanner.GetRandomOccupiableCell());
        }
    }
}
