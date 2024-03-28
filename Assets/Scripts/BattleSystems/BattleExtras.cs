using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleExtras : MonoBehaviour
{
    private int killCount;
    [SerializeField] AudioSource audioSource;
    public AudioClip[] audioClips; //0 = first blood, 1 = double kill, 2 = tripple kill, 

    private void Start()
    {
        killCount = 0;
    }

    private void OnEnable()
    {
        Actions.OnKill += KillAnnounce;
    }

    private void OnDisable()
    {
        Actions.OnKill -= KillAnnounce;
    }

    private void KillAnnounce(UnitStateMachine killer)
    {
        killCount++;
        killer.killStreak++;
        if (killCount == 1)
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }
        if (killer.killStreak == 2)
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();
        }
        if (killer.killStreak == 3)
        {
            audioSource.clip = audioClips[2];
            audioSource.Play();
        }
        if (killer.killStreak == 4)
        {
            audioSource.clip = audioClips[3];
            audioSource.Play();
        }
        if (killer.killStreak == 5)
        {
            audioSource.clip = audioClips[4];
            audioSource.Play();
        }
        if (killer.killStreak == 6)
        {
            audioSource.clip = audioClips[5];
            audioSource.Play();
        }
    }
}
