using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition = new Vector3(0, -3.5f, 0);
    [SerializeField] private float _movementSpeed = 3f;

    private Vector3 _keyboardInput;
    private bool _isSpeedBoosted;


    private void Awake() => transform.position = _startPosition;

    private void Update()
    {
        #region Player Movement
        _keyboardInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(_keyboardInput * _movementSpeed * Time.deltaTime);
        #endregion

        #region Restrict player to within level bounds
        float x = transform.position.x;
        float y = transform.position.y;

        if (transform.position.x <= LevelBoundary.L())
            x = LevelBoundary.R();  // X wraps
        else if (transform.position.x >= LevelBoundary.R())
            x = LevelBoundary.L(); // X wraps
        if (transform.position.y <= LevelBoundary.D())
            y = LevelBoundary.D(); // Y doesn't wrap
        else if (transform.position.y >= LevelBoundary.U())
            y = LevelBoundary.U();  // Y doesn't wrap

        transform.position = new Vector3(x, y, transform.position.z);
        #endregion
    }

    public void ActivatePowerup(PowerupType type, int duration)
    {
        if (_isSpeedBoosted)
            StopAllCoroutines();
        StartCoroutine(ManagePowerup(type, duration));
    }

    private IEnumerator ManagePowerup(PowerupType type, int duration)
    {
        switch (type)
        {
            case PowerupType.SpeedBoost:
                _isSpeedBoosted = true;
                float originalSpeed = _movementSpeed;
                _movementSpeed *= 3;
                yield return new WaitForSeconds(duration);
                _movementSpeed = originalSpeed;
                _isSpeedBoosted = false;
                break;
        }
    }
}
