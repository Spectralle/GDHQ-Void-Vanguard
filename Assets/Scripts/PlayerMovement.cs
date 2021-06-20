using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3 playerStartPosition;
    [SerializeField] private float playerMovementSpeed = 2f;
    [SerializeField] private Bounds levelBounds;

    private Vector3 _keyboardInput;


    private void Awake() => transform.position = playerStartPosition;

    private void Update()
    {
        #region Player Movement
        _keyboardInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(_keyboardInput * playerMovementSpeed * Time.deltaTime);
        #endregion

        #region Restrict player to within level bounds
        float x = transform.position.x;
        float y = transform.position.y;

        if (transform.position.x <= -levelBounds.extents.x)
            x = levelBounds.extents.x;  // X wraps
        else if (transform.position.x >= levelBounds.extents.x)
            x = -levelBounds.extents.x; // X wraps
        if (transform.position.y <= -levelBounds.extents.y)
            y = -levelBounds.extents.y; // Y doesn't wrap
        else if (transform.position.y >= levelBounds.extents.y)
            y = levelBounds.extents.y;  // Y doesn't wrap

        transform.position = new Vector3(x, y, transform.position.z);
        #endregion
    }


    private void OnDrawGizmos()
    {
        Vector3 topLeft = new Vector3(-levelBounds.extents.x, levelBounds.extents.y, 0);
        Vector3 topRight = new Vector3(levelBounds.extents.x, levelBounds.extents.y, 0);
        Vector3 bottomLeft = new Vector3(-levelBounds.extents.x, -levelBounds.extents.y, 0);
        Vector3 bottomRight = new Vector3(levelBounds.extents.x, -levelBounds.extents.y, 0);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(topRight, bottomRight);
    }
}
