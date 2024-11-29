using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource efxSource; // For single sound effects
    public AudioSource musicSource; // For background music
    public AudioSource loopedEffectSource; // Dedicated source for looped sound effects

    [Header("Background Music Clips")]
    public AudioClip bgMusic;
    public AudioClip gameMusic;

    [Header("Sound Effects")]
    [SerializeField] private List<AudioClip> soundEffects;

    private readonly Dictionary<SoundEffectType, AudioClip> soundEffectClips = new();

    private bool muteMusic;
    private bool muteEfx;

    private const string MuteMusicKey = "PurrfectCatch_MuteMusic";
    private const string MuteEfxKey = "PurrfectCatch_MuteEfx";

    private void Start()
    {
        InitializeMuteSettings();
        PopulateSoundEffectsDictionary();
        PlayMusic(gameMusic);
    }

    private void InitializeMuteSettings()
    {
        muteMusic = PlayerPrefs.GetInt(MuteMusicKey, 0) == 1;
        muteEfx = PlayerPrefs.GetInt(MuteEfxKey, 0) == 1;
    }

    private void PopulateSoundEffectsDictionary()
    {
        // Populate the dictionary for quick access to sound clips by type
        foreach (var clip in soundEffects)
        {
            if (System.Enum.TryParse(clip.name, out SoundEffectType type))
            {
                soundEffectClips[type] = clip;
            }
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (muteMusic || musicSource.isPlaying && musicSource.clip == clip)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayEffect(SoundEffectType effectType)
    {
        if (muteEfx || !soundEffectClips.ContainsKey(effectType))
            return;

        efxSource.PlayOneShot(soundEffectClips[effectType]);
    }

    public void PlayLoopedEffect(SoundEffectType effectType)
    {
        if (muteEfx || !soundEffectClips.ContainsKey(effectType))
        {
            Debug.Log("SoundEffects does not contain the clip or muted");
            return;
        }

        // Stop if already playing the requested effect to avoid restarts
        if (loopedEffectSource.isPlaying && loopedEffectSource.clip == soundEffectClips[effectType])
            return;

        // Assign the looped clip and play
        loopedEffectSource.clip = soundEffectClips[effectType];
        loopedEffectSource.loop = true; // Enable looping
        loopedEffectSource.Play();
    }

    public void StopLoopedEffect()
    {
        // Stop the looped sound effect
        loopedEffectSource.Stop();
        loopedEffectSource.loop = false; // Reset looping
    }

    public void ToggleMusicMute()
    {
        muteMusic = !muteMusic;
        PlayerPrefs.SetInt(MuteMusicKey, muteMusic ? 1 : 0);
        PlayerPrefs.Save();

        if (muteMusic)
            StopMusic();
        else
            PlayMusic(bgMusic); // Play default music
    }

    public void ToggleEfxMute()
    {
        muteEfx = !muteEfx;
        PlayerPrefs.SetInt(MuteEfxKey, muteEfx ? 1 : 0);
        PlayerPrefs.Save();

        // If EFX is muted, stop any looped sound effect immediately
        if (muteEfx && loopedEffectSource.isPlaying)
            StopLoopedEffect();
    }

    public bool IsMusicMuted => muteMusic;
    public bool IsEfxMuted => muteEfx;
}
