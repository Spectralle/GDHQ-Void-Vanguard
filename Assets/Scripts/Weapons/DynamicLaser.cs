using UnityEngine;


public class DynamicLaser : MonoBehaviour
{
    private LineRenderer _lineRendererLeft;
    private LineRenderer _lineRendererRight;
    private Animator _anim;
    private int _laserLength = 30;
    private bool _isActive;
    private Vector2 _localHitPoint;


    private void Awake()
    {
        _lineRendererLeft = transform.Find("Laser Rotator Left/Laser Left").GetComponent<LineRenderer>();
        _lineRendererRight = transform.Find("Laser Rotator Right/Laser Right").GetComponent<LineRenderer>();
        _lineRendererLeft.enabled = false;
        _lineRendererRight.enabled = false;
        _anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (_isActive)
        {
            _lineRendererLeft.SetPosition(1, GetLaserEndPoint(_lineRendererLeft.transform));
            _lineRendererRight.SetPosition(1, GetLaserEndPoint(_lineRendererRight.transform));
        }
    }

    public void ActivateLaser()
    {
        _lineRendererLeft.SetPosition(1, GetLaserEndPoint(_lineRendererLeft.transform));
        _lineRendererRight.SetPosition(1, GetLaserEndPoint(_lineRendererRight.transform));
        _isActive = true;
        _anim.SetTrigger("Activate");
    }

    public void DeactivateLaser() => _isActive = false;

    private Vector2 GetLaserEndPoint(Transform origin)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin.position, origin.up, _laserLength);

        if (hit.transform == null)
            return origin.localPosition + new Vector3(0, _laserLength, 0);
        else
        {
            CheckForDestruction(hit.transform);
            _localHitPoint = new Vector3(0, hit.distance * 2, 0);
            return _localHitPoint;
        }
    }

    private void CheckForDestruction(Transform hitObj)
    {
        if (hitObj.CompareTag("Enemy"))
        {
            hitObj.TryGetComponent(out EnemyMovement e);
            e.MakeExplode();
        }
    }
}
