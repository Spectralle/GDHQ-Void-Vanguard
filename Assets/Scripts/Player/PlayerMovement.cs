using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition = new Vector3(0, -3.5f, 0);
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private GameObject _thruster;

    private Vector3 _keyboardInput = Vector3.zero;
    private bool _isSpeedBoosted;
    private float _speedMultiplier = 1;
    private Vector3 _thrusterOriginalScale = Vector3.one;


    private void Awake()
    {
        transform.position = _startPosition;
        _thrusterOriginalScale = _thruster.transform.localScale;
    }

    private void Update()
    {
        _keyboardInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(_keyboardInput * (_movementSpeed * _speedMultiplier) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isSpeedBoosted)
        {
            _speedMultiplier = 1.6f;
            _thruster.transform.localScale = new Vector3(_thrusterOriginalScale.x * 1.1f, _thrusterOriginalScale.y * 1.3f, _thrusterOriginalScale.z);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !_isSpeedBoosted)
        {
            _speedMultiplier = 1f;
            _thruster.transform.localScale = _thrusterOriginalScale;
        }

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
                _speedMultiplier = 2.5f;
                _thruster.transform.localScale = new Vector3(_thrusterOriginalScale.x * 1.2f, _thrusterOriginalScale.y * 1.6f, _thrusterOriginalScale.z);
                yield return new WaitForSeconds(duration);
                _thruster.transform.localScale = _thrusterOriginalScale;
                _speedMultiplier = 1;
                _isSpeedBoosted = false;
                break;
        }
    }
}
