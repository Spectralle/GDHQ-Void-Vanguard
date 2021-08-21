using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public enum partType
    {
        MainBody,
        Sentry,
        ShieldGenerator
    }
    [SerializeField] private partType _type;
    [SerializeField] private int _healthPoints = 20;

    private BossSentryManager _sentryManager;
    private BossShieldGenManager _generatorManager;


    void Start()
    {
        _sentryManager = FindObjectOfType<BossSentryManager>();
        _generatorManager = FindObjectOfType<BossShieldGenManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Projectile"))
        {
            switch (_type)
            {
                case partType.MainBody:
                    if (_generatorManager.allGeneratorsDestroyed() && _sentryManager.allSentriesDestroyed())
                        _healthPoints--;
                    if (_healthPoints <= 0)
                    {
                        GameConclusionHandler.i.Victory();
                        Die();
                    }
                    break;
                case partType.Sentry:
                    if (_generatorManager.allGeneratorsDestroyed())
                        _healthPoints--;
                    if (_healthPoints <= 0)
                        Die();
                    break;
                case partType.ShieldGenerator:
                    _healthPoints--;
                    if (_healthPoints <= 0)
                        Die();
                    break;
            }
        }
    }

    private void Die() => Destroy(gameObject);

    private void OnDestroy()
    {
        switch (_type)
        {
            case partType.MainBody:
                break;
            case partType.Sentry:
                _sentryManager.SentryDestroyed(transform);
                break;
            case partType.ShieldGenerator:
                _generatorManager.GeneratorDestroyed(transform);
                break;
        }
    }
}