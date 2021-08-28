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
    public static void MoveToSentryPhase()
    {
        i._moveToSentryPhase = true;
        i._anim.SetBool("ShieldGensDestroyed", true);
    }

    public bool _moveToMainPhase;
    public static void MoveToMainPhase()
    {
        i._moveToMainPhase = true;
        i._anim.SetBool("SentriesDestroyed", true);
    }

    [Space]
    [SerializeField, Range(1f, 30f)] private float _gPTimeOnSide = 7f;
    [Space]
    [SerializeField, Range(1f, 4f)] private float _sPFirstBeamTime = 2f;
    [SerializeField, Range(1f, 4f)] private float _sPSecondBeamTime = 2f;
    [SerializeField, Range(0.6f, 4f)] private float _sPBeamPause = 1f;

    private Animator _anim;
    private List<BossGeneratorGun> _genGuns = new List<BossGeneratorGun>(); 
    private List<BossSentryGun> _sentryGuns = new List<BossSentryGun>();
    private BossMainGun _mainGun;
    private List<Vector3> _sentryOriginalPositions = new List<Vector3>();
    private List<Vector2> _vertBeamPositions = new List<Vector2>();
    private List<Vector2> _horBeamPositions = new List<Vector2>();
    private List<Transform> leftSide = new List<Transform>();
    private List<Transform> rightSide = new List<Transform>();
    private int _mainFightLoopNumber = 1;
    private float _difficultyMultiplier = 1f;
    public bool _isBossDead { get; private set; }
    

    private void Awake()
    {
        i = this;
        _anim = GetComponent<Animator>();
    }

    #region Generator Phase
    private IEnumerator GenPhaseBehaviourWhileLeft()
    {
        float timer = _gPTimeOnSide;
        while (timer >= 0.0f)
        {
            #region Behaviour
            _canGensShoot = true;
            #endregion

            if (_moveToSentryPhase)
                timer = -1f;

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
        float timer = _gPTimeOnSide;
        while (timer >= 0.0f)
        {
            #region Behaviour
            _canGensShoot = true;
            #endregion

            if (_moveToSentryPhase)
                timer = -1f;

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
    private void StartVerticalSentryBeams()
    {
        StopAllCoroutines();
        StartCoroutine(SentryPhaseBehaviour_VerticalBeams());
    }

    private IEnumerator SentryPhaseBehaviour_VerticalBeams()
    {
        #region Setup
        _vertBeamPositions.Clear();
        float XStep = 23.6f / _sentryGuns.Count;

        for (int s = 0; s < _sentryGuns.Count; s++)
        {
            _sentryOriginalPositions.Add(_sentryGuns[s].transform.position);

            _vertBeamPositions.Add(new Vector2(
                s < (_sentryGuns.Count / 2) ? XStep / 2 : -(XStep / 2),
                5.7f
            ));

            _sentryGuns[s].transform.position = new Vector2(_vertBeamPositions[s].x, _vertBeamPositions[s].y + 2f);
            _sentryGuns[s].transform.rotation = Quaternion.Euler(0, 0, 180);

            StartCoroutine(MoveTransform(_sentryGuns[s].transform, _vertBeamPositions[s]));
        }
        #endregion

        #region Behaviour
        float timer = _sPFirstBeamTime;
        while (timer >= 0.0f)
        {
            if (_moveToMainPhase)
                yield break;

            _canSentriesShoot = true;

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timer = _sPBeamPause;
        while (timer >= 0.0f)
        {
            if (_moveToMainPhase)
                yield break;

            _canSentriesShoot = false;

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timer = _sPSecondBeamTime;
        while (timer >= 0.0f)
        {
            if (_moveToMainPhase)
                yield break;

            _canSentriesShoot = true;

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _canSentriesShoot = false;
        #endregion

        #region Conclusion
        if (!_moveToMainPhase)
        {
            for (int s = 0; s < _sentryGuns.Count; s++)
                StartCoroutine(MoveTransform(_sentryGuns[s].transform, _vertBeamPositions[s] + new Vector2(0, 2f)));

            yield return new WaitForSeconds(1.8f);
            StartHorizontalSentryBeams();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            _moveToMainPhase = false;
        }
        #endregion
    }

    private void StartHorizontalSentryBeams()
    {
        StopAllCoroutines();
        StartCoroutine(SentryPhaseBehaviour_HorizontalBeams());
    }

    private IEnumerator SentryPhaseBehaviour_HorizontalBeams()
    {
        #region Setup
        float maxDiffBetweenFirstAndLast = 8f;
        float negativeOffset;
        float YStep;

        leftSide.Clear();
        rightSide.Clear();
        _horBeamPositions.Clear();

        for (int s = 0; s < _sentryGuns.Count; s++)
        {
            _sentryOriginalPositions.Add(_sentryGuns[s].transform.position);

            bool isLeft = s < (int)(_sentryGuns.Count / 2f);

            if (isLeft)
                leftSide.Add(_sentryGuns[s].transform);
            else
                rightSide.Add(_sentryGuns[s].transform);
        }

        for (int s = 0; s < leftSide.Count; s++)
        {
            YStep = maxDiffBetweenFirstAndLast / leftSide.Count;
            negativeOffset = -(maxDiffBetweenFirstAndLast / 2f);

            if (_horBeamPositions.Count - 1 < s)
            {
                _horBeamPositions.Add(new Vector2(
                    10.5f,
                    (leftSide.Count == 1) ? 0f : negativeOffset + (YStep / 2) + (s * YStep)
                ));
            }

            leftSide[s].transform.position = _horBeamPositions[s] + new Vector2(2f, 0);
            leftSide[s].transform.rotation = Quaternion.Euler(0, 0, 90);

            StartCoroutine(MoveTransform(leftSide[s].transform, _horBeamPositions[s]));
        }

        for (int s = 0; s < rightSide.Count; s++)
        {
            YStep = maxDiffBetweenFirstAndLast / rightSide.Count;
            negativeOffset = -(maxDiffBetweenFirstAndLast / 2f);

            if (_horBeamPositions.Count - leftSide.Count < s + 1)
            {
                _horBeamPositions.Add(new Vector2(
                    -10.5f,
                    (rightSide.Count == 1) ? 0f : negativeOffset + (YStep / 2) + (s * YStep)
                ));
            }

            rightSide[s].transform.position = _horBeamPositions[leftSide.Count + s] - new Vector2(2f, 0);
            rightSide[s].transform.rotation = Quaternion.Euler(0, 0, -90);

            StartCoroutine(MoveTransform(rightSide[s].transform, _horBeamPositions[leftSide.Count + s]));
        }
        #endregion

        #region Behaviour
        float timer = _sPFirstBeamTime;
        while (timer >= 0.0f)
        {
            if (_moveToMainPhase)
                yield break;

            _canSentriesShoot = true;

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timer = _sPBeamPause;
        while (timer >= 0.0f)
        {
            if (_moveToMainPhase)
                yield break;

            _canSentriesShoot = false;

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timer = _sPSecondBeamTime;
        while (timer >= 0.0f)
        {
            if (_moveToMainPhase)
                yield break;

            _canSentriesShoot = true;

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _canSentriesShoot = false;
        #endregion

        #region Conclusion
        if (!_moveToMainPhase)
        {
            for (int s = 0; s < leftSide.Count; s++)
                StartCoroutine(MoveTransform(leftSide[s].transform, _horBeamPositions[s] + new Vector2(2f, 0)));

            for (int s = 0; s < rightSide.Count; s++)
                StartCoroutine(MoveTransform(rightSide[s].transform, _horBeamPositions[leftSide.Count + s] - new Vector2(2f, 0)));

            yield return new WaitForSeconds(1.8f);
            StartVerticalSentryBeams();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            _moveToMainPhase = false;
        }
        #endregion
    }

    private IEnumerator MoveTransform(Transform sentry, Vector2 destination)
    {
        while (sentry && Vector2.Distance((Vector2)sentry.transform.position, destination) > 0.01f)
        {
            sentry.transform.position = Vector3.Lerp(
                sentry.transform.position,
                destination,
                Time.deltaTime * 2f
            );
            yield return new WaitForEndOfFrame();
        }
    }

    private void DisableShield() => StartCoroutine(BossDefenceShieldManager.i.Disable());
    #endregion

    #region Main Phase
    private void StartMainPhase()
    {
        StopCoroutine(MainPhaseBehaviour());
        StartCoroutine(MainPhaseBehaviour());
    }

    private IEnumerator MainPhaseBehaviour()
    {
        yield return new WaitForSeconds(0.5f * _difficultyMultiplier);

        _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.Ten360());

        yield return new WaitForSeconds(1.5f * _difficultyMultiplier);

        _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.Ten360());

        if (_mainFightLoopNumber > 2)
        {
            yield return new WaitForSeconds(0.3f * _difficultyMultiplier);

            _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.Ten360());

            if (_mainFightLoopNumber > 5)
            {
                if (_mainFightLoopNumber <= 9)
                {
                    yield return new WaitForSeconds(1.4f * _difficultyMultiplier);

                    _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.Twenty360());
                }

                if (_mainFightLoopNumber > 7)
                {
                    yield return new WaitForSeconds(2f * _difficultyMultiplier);

                    _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.ThreeForward100());

                    yield return new WaitForSeconds(0.4f * _difficultyMultiplier);

                    _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.ThreeForward100());

                    if (_mainFightLoopNumber > 9)
                    {
                        yield return new WaitForSeconds(0.7f * _difficultyMultiplier);

                        _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.FiveForward100());

                        if (_mainFightLoopNumber > 11)
                        {
                            yield return new WaitForSeconds(0.6f * _difficultyMultiplier);

                            _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.FiveForward100());

                            if (_mainFightLoopNumber > 14)
                            {
                                if (_mainFightLoopNumber <= 16)
                                {
                                    yield return new WaitForSeconds(0.6f * _difficultyMultiplier);

                                    _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.EightForward100());
                                }

                                if (_mainFightLoopNumber > 17)
                                {
                                    if (_mainFightLoopNumber <= 20)
                                    {
                                        yield return new WaitForSeconds(0.4f * _difficultyMultiplier);

                                        _mainGun.CallRemoteAttack(AttackLibrary.Laser.Boss.EightForward100());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(1.5f * _difficultyMultiplier);

        _difficultyMultiplier = Mathf.Clamp(_difficultyMultiplier - 0.02f, 0.3f, 1f);
        _mainFightLoopNumber++;
        
        //Debug.Log($"Main Phase, loop {_mainFightLoopNumber}. Difficulty: {(1f - _difficultyMultiplier + 0.3f).ToString("f2")}");

        StartCoroutine(MainPhaseBehaviour());
    }
    #endregion

    #region Misc
    public void Die()
    {
        StopAllCoroutines();
        _isBossDead = true;
    }

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

        if (leftSide.Contains(sg.transform))
            leftSide.Remove(sg.transform);

        if (rightSide.Contains(sg.transform))
            rightSide.Remove(sg.transform);
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Vector2 s in _vertBeamPositions)
            Gizmos.DrawSphere(s, 0.15f);

        Gizmos.color = Color.red;
        foreach (Vector2 s in _horBeamPositions)
            Gizmos.DrawSphere(s, 0.15f);
    }
#endif
}
