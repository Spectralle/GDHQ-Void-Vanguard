using System.Collections.Generic;
using UnityEngine;

public class BossSentryManager : MonoBehaviour
{
    public bool allSentriesDestroyed() => _childSentries.Count == 0;

    [SerializeField] private GameObject _sentryPrefab;
    [SerializeField, Range(1, 20)] private int _sentriesToSpawn = 2;
    [SerializeField, Range(0.5f, 5)] private float _spawnRadius = 2f;
    private List<Transform> _childSentries = new List<Transform>();
    private List<float> _childSentryAngles = new List<float>();

    private float _angleStep;
    private float _worldUpAngle;


    void Start()
    {
        _angleStep = 360 / (_sentriesToSpawn + 1);
        float _angle = _angleStep;
        _worldUpAngle = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int s = 0; s < _sentriesToSpawn; s++)
        {
            float calculation = ((_angle * Mathf.PI) / 180) + _worldUpAngle;
            Vector2 _startPosition = new Vector2(
                Mathf.Sin(calculation),
                Mathf.Cos(calculation)
            );

            _startPosition *= _spawnRadius;

            GameObject generator = Instantiate(_sentryPrefab, (Vector2)transform.position + _startPosition, Quaternion.Euler(0, 0, 180), transform);

            _childSentries.Add(generator.transform);
            _childSentryAngles.Add(_angle);

            _angle += _angleStep;
        }
    }

    public void SentryDestroyed(Transform gen)
    {
        if (_childSentries.Contains(gen))
            _childSentries.Remove(gen);
    }

    private void OnDrawGizmos()
    {
        _angleStep = 360 / (_sentriesToSpawn + 1);
        float angle = _angleStep;
        float _anchorUpAngle = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int s = 0; s < _sentriesToSpawn; s++)
        {
            Vector2 _startPosition = new Vector2(
                Mathf.Sin(((angle * Mathf.PI) / 180) + _anchorUpAngle),
                Mathf.Cos(((angle * Mathf.PI) / 180) + _anchorUpAngle)
            );

            _startPosition *= _spawnRadius;

            Color c1 = Color.blue;
            c1.a = 0.5f;
            Gizmos.color = c1;
            Gizmos.DrawWireSphere((Vector2)transform.position + _startPosition, 0.6f);

            angle += _angleStep;
        }

        Color c2 = Color.white;
        c2.a = 0.2f;
        Gizmos.color = c2;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
}