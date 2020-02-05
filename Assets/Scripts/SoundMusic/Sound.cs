using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "new sound", menuName = "Sound/new sound")]
public class Sound : ScriptableObject{

    public string name;

    public AudioClip audioClip;

    //Audio manager settings variables and range.
    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3)]
    public float pitch;

    [Range(0f, 1f)]
    public float blend;

    [Range(0f, 1000f)]
    public float minDistance;

    [Range(0f, 1000f)]
    public float maxDistance;

    [Range(0f, 256f)]
    public int priority;

    public float length;

    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
