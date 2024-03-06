using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  
    public static SoundManager Instance { get; set; }
    //sound FX
    public AudioSource itemDrop;

    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickItemSound;
    public AudioSource grassWalkSound;
    public AudioSource eatingSound;
    public AudioSource pickAxeHit;
    public AudioSource falingTree;
    public AudioSource breakingRock;

    //Music
    public AudioSource StartingZoneBGMuzic;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }





}
