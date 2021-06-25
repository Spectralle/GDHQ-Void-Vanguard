using UnityEngine;


public class BoundaryCreator : MonoBehaviour
{
    [SerializeField] private Bounds _levelBounds = new Bounds(Vector3.zero, new Vector3(11.4f, 4.5f, 0));


    void Awake() => LevelBoundary.SetBounds(_levelBounds);


    private void OnDrawGizmos()
    {
        Vector3 topLeft = new Vector3(_levelBounds.center.x - _levelBounds.extents.x, _levelBounds.center.y + _levelBounds.extents.y, 0);
        Vector3 topRight = new Vector3(_levelBounds.center.x + _levelBounds.extents.x, _levelBounds.center.y + _levelBounds.extents.y, 0);
        Vector3 bottomLeft = new Vector3(_levelBounds.center.x - _levelBounds.extents.x, _levelBounds.center.y - _levelBounds.extents.y, 0);
        Vector3 bottomRight = new Vector3(_levelBounds.center.x + _levelBounds.extents.x, _levelBounds.center.y - _levelBounds.extents.y, 0);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(topRight, bottomRight);
    }
}