using UnityEngine;


public static class LevelBoundary
{
    private static Bounds _bounds;


    static LevelBoundary() => _bounds = new Bounds(Vector3.zero, Vector3.one);

    #region Sets
    public static void SetBounds(Vector2 radius) => _bounds.extents = radius;

    public static void SetCenter(Vector2 center) => _bounds.center = center;

    public static void SetBounds(Bounds bounds) => _bounds = bounds;
    #endregion

    #region Gets
    public static Vector2 GetRadius() => _bounds.extents;

    public static Vector2 GetCenter() => _bounds.center;

    public static Bounds GetBounds() => _bounds;
    #endregion

    #region Get Points
    public static float L() => _bounds.center.x - _bounds.extents.x;
    public static float L(float offset) => _bounds.center.x - _bounds.extents.x + offset;
    public static float R() => _bounds.center.x + _bounds.extents.x;
    public static float R(float offset) => _bounds.center.x + _bounds.extents.x + offset;
    public static float U() => _bounds.center.y + _bounds.extents.y;
    public static float U(float offset) => _bounds.center.y + _bounds.extents.y + offset;
    public static float D() => _bounds.center.y - _bounds.extents.y;
    public static float D(float offset) => _bounds.center.y - _bounds.extents.y + offset;
    #endregion
}