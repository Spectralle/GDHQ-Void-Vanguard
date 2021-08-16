using System.Collections;
using UnityEngine;


public class AttackLibraryInitializer : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_Default;
    [SerializeField] private GameObject Prefab_Ricochet;
    [SerializeField] private GameObject Prefab_HomingMissile;
    [SerializeField] private AudioClip Audio_Default;
    [SerializeField] private AudioClip Audio_DefaultFailed;
    [SerializeField] private AudioClip Audio_HomingMissile;
    [SerializeField] private AudioClip Audio_HomingMissileFailed;


    void Awake() => AttackLibrary.Initialize(
        Prefab_Default, Prefab_Ricochet, Prefab_HomingMissile,
        Audio_Default, Audio_DefaultFailed,
        Audio_HomingMissile, Audio_HomingMissileFailed);
}