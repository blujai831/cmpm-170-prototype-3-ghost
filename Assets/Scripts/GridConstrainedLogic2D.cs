using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridConstrainedLogic2D
{
    public static Vector2Int GetRealGridPosition(Grid grid, GameObject gobj) {
        var realGridPosition3D = grid.WorldToCell(gobj.transform.position);
        return new Vector2Int(realGridPosition3D.x, realGridPosition3D.y);
    }

    public static void SnapToGrid(
        Grid grid,
        GameObject gobj,
        Vector2Int gridPosition
    ) {
        var gridCellCenter = grid.GetCellCenterWorld(
            new Vector3Int(gridPosition.x, gridPosition.y, 0)
        );
        gobj.transform.position = new Vector3(
            gridCellCenter.x,
            gridCellCenter.y,
            gobj.transform.position.z
        );
    }

    public static bool Occupiable(
        Grid grid,
        Vector2Int toWhichCell,
        GridConstrainedAreaScanner2D scanner = null
    ) {
        if (scanner == null || !scanner.Initialized || scanner.Grid != grid) {
            bool anyOk = false;
            foreach (
                Direction2D direction in
                System.Enum.GetValues(typeof(Direction2D))
            ) {
                var fromWhichCell = toWhichCell - direction.ToVector2Int();
                var fromWhere = grid.GetCellCenterWorld(
                    new Vector3Int(fromWhichCell.x, fromWhichCell.y, 0)
                );
                var toWhere = grid.GetCellCenterWorld(
                    new Vector3Int(toWhichCell.x, toWhichCell.y, 0)
                );
                var castResults = Physics2D.BoxCastAll(
                    new Vector2(fromWhere.x, fromWhere.y),
                    new Vector2(grid.cellSize.x*0.9f, grid.cellSize.y*0.9f),
                    0.0f,
                    direction.ToVector2(),
                    Vector3.Distance(fromWhere, toWhere),
                    LayerMask.GetMask("Map")
                );
                bool anyNonTriggers = false;
                foreach (var castResult in castResults) {
                    if (!castResult.collider.isTrigger) {
                        anyNonTriggers = true;
                        break;
                    }
                }
                if (!anyNonTriggers) {
                    anyOk = true;
                    break;
                }
            }
            return anyOk;
        } else {
            return scanner.Occupiable(toWhichCell);
        }
    }

    public static bool CanMove(
        Grid grid,
        Vector2Int fromWhichCell,
        Direction2D direction,
        GridConstrainedAreaScanner2D scanner = null
    ) {
        return Occupiable(
            grid,
            fromWhichCell + direction.ToVector2Int(),
            scanner
        );
    }
}
