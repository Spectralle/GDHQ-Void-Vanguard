using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldGenLineRend : MonoBehaviour
{
    [SerializeField, Range(0.1f, 4)] private float _powerLineLength = 1.8f;

    private static Transform _lineTarget;
    private LineRenderer _lineRenderer;


    void Awake()
    {
        if (_lineTarget == null)
            _lineTarget = transform.parent.parent;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() =>
        _lineRenderer.SetPosition(1, (transform.position - _lineTarget.position).normalized * _powerLineLength);
}
