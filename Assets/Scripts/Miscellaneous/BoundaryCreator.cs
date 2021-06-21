using UnityEngine;


public class BoundaryCreator : MonoBehaviour
{
    [SerializeField] private Bounds levelBounds;


    void Awake() => LevelBoundary.SetBounds(levelBounds);


    private void OnDrawGizmos()
    {
        Vector3 topLeft = new Vector3(levelBounds.center.x - levelBounds.extents.x, levelBounds.center.y + levelBounds.extents.y, 0);
        Vector3 topRight = new Vector3(levelBounds.center.x + levelBounds.extents.x, levelBounds.center.y + levelBounds.extents.y, 0);
        Vector3 bottomLeft = new Vector3(levelBounds.center.x - levelBounds.extents.x, levelBounds.center.y - levelBounds.extents.y, 0);
        Vector3 bottomRight = new Vector3(levelBounds.center.x + levelBounds.extents.x, levelBounds.center.y - levelBounds.extents.y, 0);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(topRight, bottomRight);
    }
}