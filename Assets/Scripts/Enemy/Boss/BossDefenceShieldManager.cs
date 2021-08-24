using System.Collections;
using UnityEngine;


public class BossDefenceShieldManager : MonoBehaviour
{
    public static BossDefenceShieldManager i;

    [SerializeField] private Vector2 _sentryPhaseScale;
    [SerializeField, Range(0.5f, 4f)] private float _scaleSpeed = 2f;

    private SpriteRenderer _sprite;
    private Collider2D _collider;


    void Awake()
    {
        i = this;
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public void ActivateShield()
    {
        _sprite.enabled = true;
        _collider.enabled = true;
    }

    public void GeneratorsDestroyed() => StartCoroutine(Shrink());

    private IEnumerator Shrink()
    {
        Vector2 scale = transform.localScale;
        while (scale.x > _sentryPhaseScale.x || scale.y > _sentryPhaseScale.y)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, _sentryPhaseScale, Time.deltaTime * _scaleSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    public void SentriesDestroyed() => StartCoroutine(Disable());

    public IEnumerator Disable()
    {
        while (_sprite.color.a > 0f)
        {
            Color c = _sprite.color;
            c.a -= Time.deltaTime;
            _sprite.color = c;
            yield return new WaitForEndOfFrame();
        }
        _sprite.enabled = false;
        _collider.enabled = false;
    }
}