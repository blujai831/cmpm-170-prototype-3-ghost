using UnityEngine;

public static class Direction2DMethods
{
    public static Direction2D CW(this Direction2D dir) {
        switch (dir) {
            case Direction2D.Up: return Direction2D.Right;
            case Direction2D.Down: return Direction2D.Left;
            case Direction2D.Left: return Direction2D.Up;
            case Direction2D.Right: return Direction2D.Down;
            default: return dir;
        }
    }
    public static Direction2D CCW(this Direction2D dir) {
        switch (dir) {
            case Direction2D.Up: return Direction2D.Left;
            case Direction2D.Down: return Direction2D.Right;
            case Direction2D.Left: return Direction2D.Down;
            case Direction2D.Right: return Direction2D.Up;
            default: return dir;
        }
    }
    public static Direction2D Opposite(this Direction2D dir) {
        switch (dir) {
            case Direction2D.Up: return Direction2D.Down;
            case Direction2D.Down: return Direction2D.Up;
            case Direction2D.Left: return Direction2D.Right;
            case Direction2D.Right: return Direction2D.Left;
            default: return dir;
        }
    }
    public static Vector2Int ToVector2Int(this Direction2D dir) {
        switch (dir) {
            case Direction2D.Up: return Vector2Int.up;
            case Direction2D.Down: return Vector2Int.down;
            case Direction2D.Left: return Vector2Int.left;
            case Direction2D.Right: return Vector2Int.right;
            default: return Vector2Int.zero;
        }
    }
    public static Vector2 ToVector2(this Direction2D dir) {
        switch (dir) {
            case Direction2D.Up: return Vector2.up;
            case Direction2D.Down: return Vector2.down;
            case Direction2D.Left: return Vector2.left;
            case Direction2D.Right: return Vector2.right;
            default: return Vector2.zero;
        }
    }
    public static Direction2D ToDirection2D(this Vector2 vec) {
        var upness = Vector2.Dot(vec, Vector2.up);
        var downness = Vector2.Dot(vec, Vector2.down);
        var leftness = Vector2.Dot(vec, Vector2.left);
        var rightness = Vector2.Dot(vec, Vector2.right);
        if (upness >= downness && upness >= leftness && upness >= rightness) {
            return Direction2D.Up;
        } else if (downness >= upness && downness >= leftness && downness >= rightness) {
            return Direction2D.Down;
        } else if (leftness >= upness && leftness >= downness && leftness >= rightness) {
            return Direction2D.Left;
        } else {
            return Direction2D.Right;
        }
    }
    public static Direction2D ToDirection2D(this Vector2Int vec) {
        return ((Vector2) vec).ToDirection2D();
    }
}
