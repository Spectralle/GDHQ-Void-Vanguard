using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    public static bool IsMagnetized;
    public static float MagnetStrength;

    [SerializeField] private float _magnetStrength = 1.6f;
    [SerializeField] private float _usageSpeed = 50f;
    [SerializeField] private float _rechargeSpeed = 5f;

    private float _magnetRemaining = 0;


    private void Awake() => MagnetStrength = _magnetStrength;

    private void Start() => UIManager.i.ChangeMagnet(_magnetRemaining);

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.E)))
        {
            if (_magnetRemaining >= 100)
                IsMagnetized = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.E))
            IsMagnetized = false;

        if (IsMagnetized)
            UseMagnet();
        else
            RechargeMagnet();
    }

    private void UseMagnet()
    {
        _magnetRemaining -= Time.deltaTime * _usageSpeed;

        if (_magnetRemaining < 0)
        {
            _magnetRemaining = 0;
            IsMagnetized = false;
        }

        UIManager.i.ChangeMagnet(_magnetRemaining);
    }

    private void RechargeMagnet()
    {
        if (_magnetRemaining == 100)
            return;

        _magnetRemaining += Time.deltaTime * _rechargeSpeed;

        if (_magnetRemaining > 100)
            _magnetRemaining = 100;

        UIManager.i.ChangeMagnet(_magnetRemaining);
    }
}