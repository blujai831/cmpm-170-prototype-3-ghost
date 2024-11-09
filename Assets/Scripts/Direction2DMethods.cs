public static class Direction2DMethods
{
    public static Direction2D cw(this Direction2D dir) {
        switch (dir) {
            case Direction2D.Up: return Direction2D.Right;
            case Direction2D.Down: return Direction2D.Left;
            case Direction2D.Left: return Direction2D.Up;
            case Direction2D.Right: return Direction2D.Down;
            default: return dir;
        }
    }
    public static Direction2D ccw(this Direction2D dir) {
        switch (dir) {
            case Direction2D.Up: return Direction2D.Left;
            case Direction2D.Down: return Direction2D.Right;
            case Direction2D.Left: return Direction2D.Down;
            case Direction2D.Right: return Direction2D.Up;
            default: return dir;
        }
    }
    public static Direction2D opposite(this Direction2D dir) {
        switch (dir) {
            case Direction2D.Up: return Direction2D.Down;
            case Direction2D.Down: return Direction2D.Up;
            case Direction2D.Left: return Direction2D.Right;
            case Direction2D.Right: return Direction2D.Left;
            default: return dir;
        }
    }
}
