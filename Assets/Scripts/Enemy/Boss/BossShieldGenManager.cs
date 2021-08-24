using System.Collections.Generic;
using UnityEngine;

public class BossShieldGenManager : MonoBehaviour
{
    public bool allGeneratorsDestroyed() => _childGenerators.Count == 0;

    [SerializeField] private GameObject _generatorPrefab;
    [SerializeField] private Transform _generatorRotator;
    [SerializeField, Range(1, 20)] private int _generatorsToSpawn = 3;
    [SerializeField, Range(0.5f, 5)] private float _spawnRadius = 2f;
    [SerializeField, Range(20f, 70f)] private float _moveSpeed = 40f;

    private List<Transform> _childGenerators = new List<Transform>();
    private List<float> _childGeneratorAngles = new List<float>();
    private float _angleStep;
    private float _worldUpAngle;
    private bool complete;
    private BossFightManager _bfm;


    private void Awake() => _bfm = GetComponent<BossFightManager>();

    void Start()
    {
        BossDefenceShieldManager.i.ActivateShield();

        float _angle = 0;
        _angleStep = 360 / _generatorsToSpawn;
        _worldUpAngle = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int g = 0; g < _generatorsToSpawn; g++)
        {
            float calculation = ((_angle * Mathf.PI) / 180) + _worldUpAngle;
            Vector2 _startPosition = new Vector2(
                Mathf.Sin(calculation),
                Mathf.Cos(calculation)
            );

            _startPosition *= _spawnRadius;

            GameObject generator = Instantiate(
                _generatorPrefab,
                (Vector2)_generatorRotator.position + _startPosition,
                Quaternion.Euler(0, 0, 180),
                _generatorRotator
            );

            _childGenerators.Add(generator.transform);
            _childGeneratorAngles.Add(_angle);

            _angle += _angleStep;
        }

        _bfm.LoadGenGunList(_childGenerators);
    }

    void Update()
    {
        if (complete)
            return;

        _generatorRotator.Rotate(new Vector3(0, 0, _moveSpeed * Time.deltaTime));

        if (allGeneratorsDestroyed())
        {
            complete = true;
            BossDefenceShieldManager.i.GeneratorsDestroyed();
            BossFightManager.MoveToSentryPhase();
        }
    }

    public void GeneratorDestroyed(Transform gen)
    {
        if (_childGenerators.Contains(gen))
            _childGenerators.Remove(gen);

        _bfm.RemoveFromGenGunList(gen);
    }

    private void OnDrawGizmos()
    {
        float angleStep = 360 / _generatorsToSpawn;
        float angle = 0;
        float _anchorUpAngle = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int g = 0; g < _generatorsToSpawn; g++)
        {
            Vector2 _startPosition = new Vector2(
                Mathf.Sin(((angle * Mathf.PI) / 180) + _anchorUpAngle),
                Mathf.Cos(((angle * Mathf.PI) / 180) + _anchorUpAngle)
            );

            _startPosition *= _spawnRadius;

            Color c1 = new Color(1f, 0.5f, 0.2f);
            c1.a = 0.5f;
            Gizmos.color = c1;
            Gizmos.DrawWireSphere((Vector2)transform.position + _startPosition, 0.45f);

            angle += angleStep;
        }

        Color c2 = Color.green;
        c2.a = 0.2f;
        Gizmos.color = c2;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
}
