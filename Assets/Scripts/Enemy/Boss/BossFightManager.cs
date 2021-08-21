using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public static BossFightManager i;

    public static bool _canMainShoot;
    public static bool _canSentriesShoot;
    public static bool _canGensShoot;

    public bool _moveToSentryPhase;
    public static void MoveToSentryPhase() => i._moveToSentryPhase = true;
    public bool _moveToMainPhase;
    public static void MoveToMainPhase() => i._moveToMainPhase = true;

    [SerializeField, Range(1f, 30f)] private float _genPhaseTimeOnEachSide = 7f;
    [SerializeField, Range(1f, 4f)] private float _sentryBeamTime = 4f;

    private Animator _anim;
    private List<BossGeneratorGun> _genGuns = new List<BossGeneratorGun>(); 
    private List<BossSentryGun> _sentryGuns = new List<BossSentryGun>();
    private BossMainGun _mainGun;
    private List<Vector3> _sentryOriginalPositions = new List<Vector3>();


    private void Awake()
    {
        i = this;
        _anim = GetComponent<Animator>();
    }

    #region Generator Phase
    private IEnumerator GenPhaseBehaviourWhileLeft()
    {
        float timer = _genPhaseTimeOnEachSide;
        while (timer >= 0.0f)
        {
            #region Behaviour
            _canGensShoot = true;
            #endregion

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _canGensShoot = false;
        if (_moveToSentryPhase)
        {
            _moveToSentryPhase = false;
            _anim.SetBool("ShieldGensDestroyed", true);
        }
        _anim.Play("Boss_GeneratorPhase_FromLeft_anim");
    }

    private IEnumerator GenPhaseBehaviourWhileRight()
    {
        float timer = _genPhaseTimeOnEachSide;
        while (timer >= 0.0f)
        {
            #region Behaviour
            _canGensShoot = true;
            #endregion

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _canGensShoot = false;
        if (_moveToSentryPhase)
        {
            _moveToSentryPhase = false;
            _anim.SetBool("ShieldGensDestroyed", true);
        }
        _anim.Play("Boss_GeneratorPhase_FromRight_anim");
    }
    #endregion

    #region Sentry Phase
    private void StartSentryBeams() => StartCoroutine(SentryPhaseBehaviour_VerticalBeams());

    private IEnumerator SentryPhaseBehaviour_VerticalBeams()
    {
        List<Vector2> beamPositions = new List<Vector2>();
        float XStep = 23.6f / _sentryGuns.Count;

        for (int s = 0; s < _sentryGuns.Count; s++)
        {
            _sentryOriginalPositions.Add(_sentryGuns[s].transform.position);

            beamPositions.Add(new Vector2(
                s < (_sentryGuns.Count / 2) ? XStep / 2 : -(XStep / 2),
                5.9f
            ));

            _sentryGuns[s].transform.position = new Vector2(beamPositions[s].x, beamPositions[s].y + 1.5f);

            StartCoroutine(MoveTransform(_sentryGuns[s].transform, beamPositions[s]));
        }

        float timer = _sentryBeamTime;
        while (timer >= 0.0f)
        {
            #region Behaviour
            _canSentriesShoot = true;
            #endregion

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _canSentriesShoot = false;

        for (int s = 0; s < _sentryGuns.Count; s++)
            StartCoroutine(MoveTransform(_sentryGuns[s].transform, beamPositions[s] + new Vector2(0, 1.5f)));

        if (!_moveToMainPhase)
            StartCoroutine(SentryPhaseBehaviour_HorizontalBeams());
        else
        {
            for (int s = 0; s < _sentryGuns.Count; s++)
                _sentryGuns[s].transform.position = _sentryOriginalPositions[s];

            _moveToMainPhase = false;
            _anim.SetBool("SentriesDestroyed", true);
        }
    }

    private IEnumerator SentryPhaseBehaviour_HorizontalBeams()
    {
        List<Vector2> beamPositions = new List<Vector2>();
        float YStep = 22.6f / _sentryGuns.Count;

        for (int s = 0; s < _sentryGuns.Count; s++)
        {
            _sentryOriginalPositions.Add(_sentryGuns[s].transform.position);

            beamPositions.Add(new Vector2(
                s < (_sentryGuns.Count / 2) ? 11 : -11,
                s < (_sentryGuns.Count / 2) ? YStep / 2 : -(YStep / 2)
            ));

            _sentryGuns[s].transform.position = new Vector2(
                beamPositions[s].x + (s < (_sentryGuns.Count / 2) ? 1.5f : -1.5f),
                beamPositions[s].y
            );

            Debug.Log(beamPositions[s], _sentryGuns[s]);


            //StartCoroutine(MoveTransform(_sentryGuns[s].transform, beamPositions[s]));
        }
        yield break;

        //float timer = _sentryBeamTime;
        //while (timer >= 0.0f)
        //{
        //    #region Behaviour
        //    _canSentriesShoot = true;
        //    #endregion

        //    timer -= Time.deltaTime;
        //    yield return new WaitForEndOfFrame();
        //}

        //_canSentriesShoot = false;

        //for (int s = 0; s < _sentryGuns.Count; s++)
        //    StartCoroutine(MoveTransform(_sentryGuns[s].transform, beamPositions[s] + new Vector2(s < (_sentryGuns.Count / 2) ? 1.5f : -1.5f, 0)));

        //if (!_moveToMainPhase)
        //    StartCoroutine(SentryPhaseBehaviour_VerticalBeams());
        //else
        //{
        //    for (int s = 0; s < _sentryGuns.Count; s++)
        //        _sentryGuns[s].transform.position = _sentryOriginalPositions[s];

        //    _moveToMainPhase = false;
        //    _anim.SetBool("SentriesDestroyed", true);
        //}
    }

    private IEnumerator MoveTransform(Transform sentry, Vector2 destination)
    {
        while (Vector2.Distance((Vector2)sentry.transform.position, destination) > 0.01f)
        {
            sentry.transform.position = Vector3.Lerp(
                sentry.transform.position,
                destination,
                Time.deltaTime * 5
            );

            Debug.DrawLine(destination + (Vector2.left * 0.3f), destination + (Vector2.right * 0.3f));
            Debug.DrawLine(destination + (Vector2.up * 0.3f), destination + (Vector2.down * 0.3f));

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    public void IsOnLeft()
    {
        _anim.SetBool("isOnLeft", true);
        StartCoroutine(GenPhaseBehaviourWhileLeft());
    }
    public void IsNotOnLeft() => _anim.SetBool("isOnLeft", false);
    public void IsOnRight()
    {
        _anim.SetBool("isOnRight", true);
        StartCoroutine(GenPhaseBehaviourWhileRight());
    }
    public void IsNotOnRight() => _anim.SetBool("isOnRight", false);

    public void SetMainGun(BossMainGun mainGun) => _mainGun = mainGun;
    public void LoadGenGunList(List<Transform> generators)
    {
        foreach (Transform generator in generators)
            _genGuns.Add(generator.GetComponent<BossGeneratorGun>());
    }
    public void RemoveFromGenGunList(Transform generator)
    {
        BossGeneratorGun gg = generator.GetComponent<BossGeneratorGun>();
        if (_genGuns.Contains(gg))
            _genGuns.Remove(gg);
    }
    public void LoadSentryGunList(List<Transform> sentries)
    {
        foreach (Transform sentry in sentries)
            _sentryGuns.Add(sentry.GetComponent<BossSentryGun>());
    }
    public void RemoveFromSentryGunList(Transform sentry)
    {
        BossSentryGun sg = sentry.GetComponent<BossSentryGun>();
        if (_sentryGuns.Contains(sg))
            _sentryGuns.Remove(sg);
    }
}
