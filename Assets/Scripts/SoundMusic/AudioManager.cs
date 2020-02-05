using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Venus.Utilities;

public class AudioManager : Singleton<AudioManager>
{

    public Sound[] sounds;
    public AudioSource currentAudio;
    bool themebool = false;
    public UnityEngine.Object[] allSounds { private set; get; }


    protected override void Awake () {
        base.Awake();
        allSounds = Resources.LoadAll("Sounds", typeof(Sound));
        List<Sound> tempSounds = new List<Sound>();
        for (int i = 0; i < allSounds.Length; i++)
        {
            tempSounds.Add((Sound)allSounds[i]);
        }
        sounds = tempSounds.ToArray();
        foreach (Sound s in sounds)
        {
            //gets audioSource

           s.source =  gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            //adding wanted settings to audiomanager
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.blend;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;
            s.source.priority = s.priority;
            s.source.loop = s.loop;
            

        }
	}
	
	// Plays sound if found by name
	public void Play (string name)
    {

        //Sound theme = Array.Find(sounds, sound => sound.name == "theme");
        Sound s = Array.Find(sounds, sound => sound.name == name);

       /* if (themebool == true)
        {
            float soundlength;
            
            s.source.Play();
            soundlength = s.source.clip.length;
            Debug.Log(soundlength);
            StartCoroutine(waitTheme(soundlength));
            

        }*/
        
        if (s == null)
        {
            return;
        }
        s.source.Play();
        
        

    }

   /* IEnumerator waitTheme(float soundlength)
    {
        Sound theme = Array.Find(sounds, sound => sound.name == "theme");

        Debug.Log("waiting");
        theme.source.volume = 0.05f;
        yield return new WaitForSeconds(soundlength);
        theme.source.volume = 0.5f;
        Debug.Log("stopped waiting");
    }*/
    //plays theme right away
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Hub")
        {
            Play("theme2");
        }
        else
        Play("theme");

        themebool = true;
    }

    
}
