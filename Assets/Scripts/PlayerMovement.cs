using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3 playerStartPosition;
    [SerializeField] private float playerMovementSpeed = 2f;
    [SerializeField] private Bounds levelBounds;

    private Vector3 _keyboardInput;


    // The position of this GameObject is set to the variable "playerStartPosition"
    private void Awake() => transform.position = playerStartPosition;

    private void Update()
    {
        #region Player Movement
        // Get the current input from the project-defined buttons, "Horizontal" and "Vertical", using WASD (and arrow keys) by default
        _keyboardInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        // Move this GameObject in the scene, based on the current _keyboardInput values, player move speed, and delta time
        transform.Translate(_keyboardInput * playerMovementSpeed * Time.deltaTime);
        #endregion

        #region Restrict player to within level bounds
        float x = transform.position.x;
        float y = transform.position.y;
        // If player position X is under levelBounds -X, set to -X.
        if (transform.position.x <= -levelBounds.extents.x)
            x = levelBounds.extents.x;
        // If player position X is over levelBounds X, set to X.
        else if (transform.position.x >= levelBounds.extents.x)
            x = -levelBounds.extents.x;
        // If player position Y is under levelBounds -Y, set to -Y.
        if (transform.position.y <= -levelBounds.extents.y)
            y = levelBounds.extents.y;
        // If player position Y is under levelBounds Y, set to Y.
        else if (transform.position.y >= levelBounds.extents.y)
            y = -levelBounds.extents.y;

        transform.position = new Vector3(x, y, transform.position.z);
        #endregion
    }


    // Draw the bounds in the scene view, for ease-of-understanding
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
