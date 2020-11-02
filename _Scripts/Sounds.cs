using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] 
    private AudioClip[] clips;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string clipName)
    {
        for(int i = 0; i < clips.Length; i++)
        {
            if(clips[i].name == clipName)
            {
                source.clip = clips[i];
                break;
            }
            continue;
        }
        source.Play();
        Debug.Log("played");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.tag == "TRex" || this.tag == "Triceratops")
        {
            if(other.tag == "Floor")
            {
                PlaySound("LandingSound");
            }
        }
    }
}