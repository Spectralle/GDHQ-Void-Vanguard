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

    private void Start() => UIManager.i.ChangeThruster(_thrustRemaining);

    void Update()
    {
        if (_playerMovement.IsSpeedBoosted)
            _isThrusting = false;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartManualThruster();
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            StopManualThruster();

        if (_isThrusting)
            DrainManualThruster();
        else
            RechargeManualThruster();
    }

    private void StartManualThruster()
    {
        if (_playerMovement.IsSpeedBoosted || _thrustRemaining < 40)
            return;

        _playerMovement.ActivateBoost(1.6f, 1.1f, 1.5f);
        _isThrusting = true;
    }

    private void DrainManualThruster()
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
        if (_playerMovement.IsSpeedBoosted)
            return;

        _playerMovement.DeactivateBoost();
        _isThrusting = false;
    }

    private void RechargeManualThruster()
    {
        if (_thrustRemaining == 100)
            return;

        _thrustRemaining += Time.deltaTime * _rechargeSpeed;

        if (_thrustRemaining > 100)
            _thrustRemaining = 100;

        UIManager.i.ChangeThruster(_thrustRemaining);
    }
}