using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Audio
{
    public string name;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    private static Dictionary<string, Audio> audioClips = new Dictionary<string, Audio>();
    
    public List<Audio> AudioList = new List<Audio>();
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
            //add the audio clips to the dictionary
            for (int i = 0; i < AudioList.Count; i++)
            {
                //check if the key is already in the dictionary
                audioClips.TryAdd(AudioList[i].name, AudioList[i]);
            }
    }

    public static void PlayAudioClip2D(string name, float volume)
    {
        if (audioClips.TryGetValue(name, out var clip))
        {
            AudioSource.PlayClipAtPoint(clip.clip, Camera.main.transform.position, volume);//temp change
        }
    }   
    
    public static void PlayAudioClip(string name, Vector3 position, float volume)
    {
        if (audioClips.TryGetValue(name, out var clip))
        {
            AudioSource.PlayClipAtPoint(clip.clip, position,volume);
        }
    }


}
