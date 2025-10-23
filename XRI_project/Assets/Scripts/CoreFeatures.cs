using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using System;

public enum FeatureUsage
{
    Once, //use once
    Toggle, //use on or off

}
public class CoreFeatures : MonoBehaviour
{
    /*
     * Property - Common way to access code that exists outside of class
     * Can create public variable and access them that way, or you can use properties
     * Properties ENCAPSULATES variables as fields
     * GET Accessor (READ) - returns encapsulated variable values
     * SET Accessor (WRITE) - allocates new values to property fields
     * PROPERTY values use PascalCase
     * SERIALIZEFIELD - makes private variables visible in inspector
     * Awake() - called before Start(), preloads any variables or game state before game starts
     * == is comparing, = is setting value, === is ALWAYS equal to
     */

    //Components
    public bool AudioSFXSourceCreated { get; set; }

    [field: Header("Core Audio")]
    [field: SerializeField] //field is for properties
    public AudioClip AudioClipOnStart { get; set; }

    [field: SerializeField]
    public  AudioClip AudioClipOnEnd { get; set; }

    private AudioSource audioSource;

    [Header("Core Features")]
    public FeatureUsage featureUsage = FeatureUsage.Once;

    protected virtual void Awake()
    {
        MakeSFXAudioSource(); //calls method to create audio source
    }

    public void MakeSFXAudioSource()
    { 
        audioSource = GetComponent<AudioSource>();

        //if equal to null, create it here
        if (audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        //whether null or not, still need to make sure that its true
        //on awake, create audioscource
        AudioSFXSourceCreated = true;
    }


    //WHEN TO CALL SOUNDS
    protected void PlayOnStart()
    {
        if(AudioSFXSourceCreated && AudioClipOnStart != null) //if audiosource created and clip exists (not equal to null)...
        {
            audioSource.clip = AudioClipOnStart;  //...set clip to audiosource...
            audioSource.Play(); //...and play the clip
        }
    }

    protected void PlayOnEnd()
    {
        if (AudioSFXSourceCreated && AudioClipOnEnd != null) //if audiosource created and clip exists (not equal to null)...
        {
            audioSource.clip = AudioClipOnEnd; //...set clip to audiosource...
            audioSource.Play(); //...and play the clip
        }
    }
}
