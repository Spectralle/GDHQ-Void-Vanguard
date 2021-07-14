using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker i;

    [SerializeField] private AnimationCurve _strengthOverTime;

    private static bool _isShaking;
    public static bool IsShaking => _isShaking;


    private void Awake()
    {
        if (i == null)
            i = this;
        else
            Destroy(this);
    }

    public static void StartShaking(float duration, float offsetLimit)
    {
        if (!_isShaking)
            i.StartCoroutine(Shake(duration, new Vector2(offsetLimit, offsetLimit)));
    }

    public static void StartShaking(float duration, Vector2 offsetLimits)
    {
        if (!_isShaking)
            i.StartCoroutine(Shake(duration, offsetLimits));
    }

    private static IEnumerator Shake(float duration, Vector2 offsetLimits)
    {
        _isShaking = true;
        Vector3 origin = i.transform.position;
        float _durationTimer = 0;

        while (_durationTimer < duration)
        {
            Vector2 offset = new Vector2(
                Random.Range(-offsetLimits.x, offsetLimits.x),
                Random.Range(-offsetLimits.y, offsetLimits.y)
            );

            offset *= i._strengthOverTime.Evaluate(RangeToZeroOne(_durationTimer, 0, duration));

            i.transform.position = origin + (Vector3)offset;

            _durationTimer += 0.07f;
            yield return new WaitForSecondsRealtime(0.07f);
        }
        i.transform.position = origin;

        _isShaking = false;
    }

    private static float RangeToZeroOne(float inValue, float inMin, float inMax)
    {
        float diffInputRange = Mathf.Abs(inMax - inMin);
        float convFactor = 1 / diffInputRange;
        return convFactor * (inValue - inMin);
    }
}
