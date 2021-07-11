using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerThruster : MonoBehaviour
{
    [SerializeField] private float _usageSpeed = 1;
    [SerializeField] private float _rechargeSpeed = 1;

    private bool _isThrusting;
    private PlayerMovement _playerMovement;
    private float _thrustRemaining = 100;


    private void Awake() => _playerMovement = GetComponent<PlayerMovement>();

    void Update()
    {
        if (_playerMovement.IsSpeedBoosted)
            _isThrusting = false;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !_playerMovement.IsSpeedBoosted && _thrustRemaining > 10)
            StartManualThruster();

        if (Input.GetKey(KeyCode.LeftShift) && _isThrusting && _thrustRemaining > 0)
            MaintainManualThruster();

        if (Input.GetKeyUp(KeyCode.LeftShift) && !_playerMovement.IsSpeedBoosted)
            StopManualThruster();

        if (!_isThrusting && _thrustRemaining < 100)
            RechargeManualThruster();
    }

    private void StartManualThruster()
    {
        _playerMovement.ActivateBoost(1.6f, 1.1f, 1.5f);
        _isThrusting = true;
        UIManager.i.ChangeThrusterBarVisibility(1);
    }

    private void MaintainManualThruster()
    {
        _thrustRemaining -= Time.deltaTime * _usageSpeed;

        if (_thrustRemaining < 0)
        {
            _thrustRemaining = 0;
            StopManualThruster();
        }

        UIManager.i.ChangeThruster(_thrustRemaining);
    }

    private void StopManualThruster()
    {
        _playerMovement.DeactivateBoost();
        _isThrusting = false;
    }

    private void RechargeManualThruster()
    {
        _thrustRemaining += Time.deltaTime * _rechargeSpeed;
        UIManager.i.ChangeThrusterBarVisibility(0.4f);

        if (_thrustRemaining > 100)
        {
            _thrustRemaining = 100;
            UIManager.i.ChangeThrusterBarVisibility(0);
        }

        UIManager.i.ChangeThruster(_thrustRemaining);
    }
}