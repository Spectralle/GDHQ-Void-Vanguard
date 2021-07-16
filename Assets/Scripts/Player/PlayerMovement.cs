using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition = new Vector3(0, -3.5f, 0);
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private GameObject _thruster;

    private bool _canControlMovement;
    private Vector3 _keyboardInput = Vector3.zero;
    private bool _isSpeedBoosted;
    public bool IsSpeedBoosted => _isSpeedBoosted;
    private float _speedMultiplier = 1;
    private Vector3 _thrusterOriginalScale = Vector3.one;
    private Animator _anim;
    private PlayerThruster _playerThruster;
    private bool _restrictInBoundary;


    private void Awake()
    {
        TryGetComponent(out _anim);
        TryGetComponent(out _playerThruster);
        transform.position = _startPosition;
        _thrusterOriginalScale = _thruster.transform.localScale;

        StartCoroutine(PlayerEntersScene());
    }

    private void Update()
    {
        if (_canControlMovement)
        {
            _keyboardInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.Translate(_keyboardInput * (_movementSpeed * _speedMultiplier) * Time.deltaTime);
        }

        HandleShipAnimation();

        #region Restrict player to within level bounds
        if (_restrictInBoundary)
        {
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
        }
        #endregion
    }

    private void HandleShipAnimation()
    {
        if (!_anim)
            return;

        _anim.SetFloat("VelocityX", _keyboardInput.x);
        if (Input.GetKey(KeyCode.A))
            _anim.SetBool("AHeldDown", true);
        else
        {
            if (!Input.GetKey(KeyCode.A))
                _anim.SetBool("AHeldDown", false);
        }

        if (Input.GetKey(KeyCode.D))
            _anim.SetBool("DHeldDown", true);
        else
        {
            if (!Input.GetKey(KeyCode.D))
                _anim.SetBool("DHeldDown", false);
        }
    }

    public void ActivateBoost(float speedMultiplier, float thrusterScaleUpX, float thrusterScaleUpY)
    {
        _speedMultiplier = speedMultiplier;
        _thruster.transform.localScale = new Vector3(_thrusterOriginalScale.x * thrusterScaleUpX, _thrusterOriginalScale.y * thrusterScaleUpY, _thrusterOriginalScale.z);
    }

    public void DeactivateBoost()
    {
        _speedMultiplier = 1;
        _thruster.transform.localScale = _thrusterOriginalScale;
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
                ActivateBoost(1.6f, 1.2f, 1.7f);
                yield return new WaitForSeconds(duration);
                DeactivateBoost();
                _isSpeedBoosted = false;
                break;
        }
    }

    private IEnumerator PlayerEntersScene()
    {
        transform.position = new Vector3(_startPosition.x, _startPosition.y - 6f, _startPosition.z);

        while (Vector3.Distance(transform.position, _startPosition) > 0.1f)
        {
            transform.Translate(new Vector3(0, _movementSpeed * 1.3f * Mathf.Clamp01(Vector3.Distance(transform.position, _startPosition)) * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }

        _restrictInBoundary = true;
        _canControlMovement = true;
    }
}
