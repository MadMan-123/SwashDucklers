using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioChannels audioChannels = new();
    [SerializeField] private int loopTime = 16; // 16 seconds in 120 bpm is 8 bars
    public static MusicManager instance;
    public static int DrumsChannel = 0, ClapChannel = 1, JollyChannel = 2, SpookyBassChannel = 3, SpookyLeadChannel = 4;
    private Dictionary<int, Coroutine> volumeLerpCoroutines = new Dictionary<int, Coroutine>();

    private double nextStartTime;
    private bool isPlaying = false;
    
    private void Start()
    {
        // Singleton boilerplate
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        // Don't destroy on load
        DontDestroyOnLoad(gameObject);
        
        // Initialize audio channels
        InitializeAudioChannels();
        
        // Start the loop
        StartLoop();
    }
    
    private void InitializeAudioChannels()
    {
        audioChannels.audioBufferSize = 5;
        
        // Initialize channel names
        audioChannels.channelNames = new string[audioChannels.channelsCount];
        
        // Initialize channel volumes if null
        if(audioChannels.channelVolumes == null)
            audioChannels.channelVolumes = new float[audioChannels.channelsCount];
            
        // Initialize audio clips buffer if null
        if (audioChannels.audioClips == null)  
            audioChannels.audioClips = new AudioClipsBuffer[audioChannels.channelsCount];
            
        // Initialize each channel
        for (int i = 0; i < audioChannels.channelsCount; i++)
        {
            audioChannels.audioClips[i].currentIndex = i;
            audioChannels.channelNames[i] = "Channel " + (i + 1);
            
            // Initialize the audio clips array if null
            if (audioChannels.audioClips[i].audioClips == null)
                audioChannels.audioClips[i].audioClips = new AudioClip[audioChannels.audioBufferSize];
        }
        
        // Double-buffer: Two audio sources per channel
        audioChannels.primarySources = new AudioSource[audioChannels.channelsCount];
        audioChannels.secondarySources = new AudioSource[audioChannels.channelsCount];
        
        // Set up audio sources
        for (int i = 0; i < audioChannels.channelsCount; i++)
        {
            // Create primary source
            audioChannels.primarySources[i] = gameObject.AddComponent<AudioSource>();
            ConfigureAudioSource(audioChannels.primarySources[i], i);
            
            // Create secondary source
            audioChannels.secondarySources[i] = gameObject.AddComponent<AudioSource>();
            ConfigureAudioSource(audioChannels.secondarySources[i], i);
        }
    }
    
    private void ConfigureAudioSource(AudioSource source, int channelIndex)
    {
        source.playOnAwake = false;
        source.loop = false;
        source.volume = audioChannels.channelVolumes[channelIndex];
        source.spatialBlend = 0; // Make it 2D
    }
    
    public void AddAudioClipToChannel(int channel, AudioClip clip)
    {
        // Add the clip to the channel
        if (channel >= audioChannels.channelsCount || channel < 0) return;
        
        for (int i = 0; i < audioChannels.audioBufferSize; i++)
        {
            if (audioChannels.audioClips[channel].audioClips[i] != null) continue;
            audioChannels.audioClips[channel].audioClips[i] = clip;
            break;
        }
    }
   
    public void SetVolumeOfChannel(int channel, float volume)
    {
        // Set the volume of the channel
        if (channel >= audioChannels.channelsCount || channel < 0) return;
        
        audioChannels.primarySources[channel].volume = volume;
        audioChannels.secondarySources[channel].volume = volume;
        audioChannels.channelVolumes[channel] = volume;
    }
    
    public void StartLoop()
    {
        if (isPlaying) return;
        
        isPlaying = true;
        nextStartTime = AudioSettings.dspTime + 0.1; // Small initial delay
        
        // Initialize by playing the first set of clips
        PlayNextClips(true);
        
        // Start the scheduler that will keep the loops going
        StartCoroutine(ScheduleNextClips());
    }
    
    private IEnumerator ScheduleNextClips()
    {
        while (isPlaying)
        {
            // Calculate how long to wait before scheduling the next clips
            double timeToNextSchedule = (nextStartTime - AudioSettings.dspTime) - 0.2;
            
            if (timeToNextSchedule > 0)
                yield return new WaitForSecondsRealtime((float)timeToNextSchedule);
                
            // Schedule the next clips
            PlayNextClips(false);
        }
    }
    
    private void PlayNextClips(bool isFirstPlay)
    {
        // For each channel, schedule the next clip
        for (int i = 0; i < audioChannels.channelsCount; i++)
        {
            // Only proceed if we have clips for this channel
            if (audioChannels.audioClips[i].audioClips == null || 
                audioChannels.audioClips[i].audioClips.Length == 0)
                continue;
                
            // Get a random clip for this channel
            int clipIndex = UnityEngine.Random.Range(0, audioChannels.audioClips[i].audioClips.Length);
            AudioClip clip = audioChannels.audioClips[i].audioClips[clipIndex];
            
            if (clip == null) continue;
            
            // Alternate between primary and secondary sources for perfect double-buffering
            bool useSecondary = !isFirstPlay || (AudioSettings.dspTime > nextStartTime);
            
            if (useSecondary)
            {
                // Use secondary source for this play
                audioChannels.secondarySources[i].clip = clip;
                audioChannels.secondarySources[i].PlayScheduled(nextStartTime);
                
                // Swap sources for next iteration
                (audioChannels.primarySources[i], audioChannels.secondarySources[i]) = (audioChannels.secondarySources[i], audioChannels.primarySources[i]);
            }
            else
            {
                // Use primary source for first play
                audioChannels.primarySources[i].clip = clip;
                audioChannels.primarySources[i].PlayScheduled(nextStartTime);
            }
        }
        
        // Update the next start time
        nextStartTime += loopTime;
    }
    
    public void StopLoop()
    {
        // Stop scheduling new clips
        isPlaying = false;
        
        // Stop all audio sources
        for (int i = 0; i < audioChannels.channelsCount; i++)
        {
            if (audioChannels.primarySources[i] != null)
                audioChannels.primarySources[i].Stop();
                
            if (audioChannels.secondarySources[i] != null)
                audioChannels.secondarySources[i].Stop();
        }
        
        // Stop all coroutines
        StopAllCoroutines();
    }
    public void LerpChannelVolume(int channel, float targetVolume, float duration)
    {
        // Validate input
        if (channel >= audioChannels.channelsCount || channel < 0) return;
        
        // Clamp target volume between 0 and 1
        targetVolume = Mathf.Clamp01(targetVolume);
        
        // If a lerp is already in progress for this channel, stop it
        if (volumeLerpCoroutines.TryGetValue(channel, out Coroutine existingCoroutine))
        {
            if (existingCoroutine != null)
                StopCoroutine(existingCoroutine);
            
            volumeLerpCoroutines.Remove(channel);
        }
        
        // Start a new lerp coroutine and store its reference
        Coroutine newCoroutine = StartCoroutine(LerpVolumeCoroutine(channel, targetVolume, duration));
        volumeLerpCoroutines[channel] = newCoroutine;
    }
    
    private IEnumerator LerpVolumeCoroutine(int channel, float targetVolume, float duration)
    {
        // Get current volume
        float startVolume = audioChannels.channelVolumes[channel];
        float currentTime = 0f;
        
        // Perform the lerp over time
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration; // Normalized time (0 to 1)
            
            // Calculate new volume
            float newVolume = Mathf.Lerp(startVolume, targetVolume, t);
            
            // Apply the volume
            SetVolumeOfChannel(channel, newVolume);
            
            yield return null;
        }
        
        // Ensure we end exactly at the target volume
        SetVolumeOfChannel(channel, targetVolume);
        
        // Remove the coroutine from the dictionary
        volumeLerpCoroutines.Remove(channel);
    }
    public float GetVolumeOfChannel(int channel)
    {
        // Get the volume of the channel
        if (channel >= audioChannels.channelsCount || channel < 0) return 0;
        
        return audioChannels.channelVolumes[channel];
    }
}

[Serializable]
public struct AudioChannels
{
    public int channelsCount;
    public int audioBufferSize;
    public string[] channelNames;
    [Range(0,1)] public float[] channelVolumes;
    public AudioClipsBuffer[] audioClips;
    // Double buffer - two sets of audio sources
    [NonSerialized] public AudioSource[] primarySources;
    [NonSerialized] public AudioSource[] secondarySources;
}

[Serializable]
public struct AudioClipsBuffer
{
    public AudioClip[] audioClips;
    public int currentIndex;
}