using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Foreground Layer:")]
    [SerializeField] private Transform _fgOriginalTransform;
    [SerializeField] private SpriteRenderer _fgImageSizeReference;
    [SerializeField, Range(0, 5)] private int _fgVerticalCopies;
    [SerializeField, Range(-30, 30)] private float _fgScrollSpeed = 1f;
    [SerializeField, Range(0, 20)] private float _fgXRandomRange = 5f;

    [Header("Background Layer:")]
    [SerializeField] private Transform _bgOriginalTransform;
    [SerializeField] private SpriteRenderer _bgImageSizeReference;
    [SerializeField, Range(0, 8)] private int _bgVerticalCopies;
    [SerializeField, Range(-30, 30)] private float _bgScrollSpeed = 0.4f;
    [SerializeField, Range(0, 20)] private float _bgXRandomRange = 5f;

    private float _fgImageHeight;
    private float _bgImageHeight;
    private List<Transform> _fgTransforms = new List<Transform>();
    private List<Transform> _bgTransforms = new List<Transform>();


    private void Awake()
    {
        _fgImageHeight = _fgImageSizeReference.sprite.bounds.size.y * _fgImageSizeReference.transform.localScale.y;
        _bgImageHeight = _bgImageSizeReference.sprite.bounds.size.y * _bgImageSizeReference.transform.localScale.y;

        InstantiateForegroundLayerDuplicates();
        InstantiateBackgroundLayerDuplicates();
    }

    void Update()
    {
        UpdateForegroundLayer();
        UpdateBackgroundLayer();
    }

    private void InstantiateForegroundLayerDuplicates()
    {
        _fgTransforms.Add(_fgOriginalTransform);
        Vector3 _fgCopyPosition = new Vector3(Random.Range(-_fgXRandomRange, _fgXRandomRange), _fgImageHeight, 0);
        for (int copyIndex = 0; copyIndex < _fgVerticalCopies; copyIndex++)
        {
            _fgTransforms.Add(Instantiate(
                _fgTransforms[0],
                _fgTransforms[0].position + _fgCopyPosition,
                Quaternion.identity,
                _fgTransforms[0].parent)
            );
            _fgCopyPosition.x = Random.Range(-_fgXRandomRange, _fgXRandomRange);
            _fgCopyPosition.y += _fgImageHeight;
        }
    }
    
    private void InstantiateBackgroundLayerDuplicates()
    {
        _bgTransforms.Add(_bgOriginalTransform);
        Vector3 _bgCopyPosition = new Vector3(Random.Range(-_bgXRandomRange, _bgXRandomRange), _bgImageHeight, 0);
        for (int copyIndex = 0; copyIndex < _bgVerticalCopies; copyIndex++)
        {
            _bgTransforms.Add(Instantiate(
                _bgTransforms[0],
                _bgTransforms[0].position + _bgCopyPosition,
                Quaternion.identity,
                _bgTransforms[0].parent)
            );
            _bgCopyPosition.x = Random.Range(-_bgXRandomRange, _bgXRandomRange);
            _bgCopyPosition.y += _bgImageHeight;
        }
    }

    private void UpdateForegroundLayer()
    {
        foreach (Transform child in _fgTransforms)
        {
            child.Translate(Vector2.down * _fgScrollSpeed * Time.deltaTime);

            if (child.position.y <= -Camera.main.orthographicSize - (_fgImageHeight / 2))
            {
                float X = Random.Range(-_fgXRandomRange, _fgXRandomRange);
                float Y = child.position.y + (_fgTransforms.Count * _fgImageHeight);
                child.position = new Vector3(X, Y);
            }
        }
    }

    private void UpdateBackgroundLayer()
    {
        foreach (Transform child in _bgTransforms)
        {
            child.Translate(Vector2.down * _bgScrollSpeed * Time.deltaTime);

            if (child.position.y <= -Camera.main.orthographicSize - (_bgImageHeight / 2))
            {
                float X = (Random.Range(-_bgXRandomRange, _bgXRandomRange));
                float Y = child.position.y + (_bgTransforms.Count * _bgImageHeight);
                child.position = new Vector3(X, Y);
            }
        }
    }
}
