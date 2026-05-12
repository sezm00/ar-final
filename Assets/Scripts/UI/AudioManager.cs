using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource voiceoverSource;
    [SerializeField] private AudioSource engineSource;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip engineStartSFX;
    [SerializeField] private AudioClip engineIdleLoop;
    [SerializeField] private AudioClip engineStopSFX;
    [SerializeField] private AudioClip[] voiceoverClips;

    private bool isEngineRunning = false;
    private AudioClip currentVoiceover;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start background music
        if (musicSource != null && musicSource.clip != null)
            musicSource.Play();
    }

    public void PlayButtonClick()
    {
        if (buttonClickSFX != null && sfxSource != null)
            sfxSource.PlayOneShot(buttonClickSFX);
    }

    public void PlayVoiceover(int carIndex)
    {
        if (voiceoverSource == null) return;

        if (carIndex >= 0 && carIndex < voiceoverClips.Length)
        {
            voiceoverSource.Stop();
            voiceoverSource.clip = voiceoverClips[carIndex];
            voiceoverSource.Play();
        }
    }

    public void StopVoiceover()
    {
        if (voiceoverSource != null)
            voiceoverSource.Stop();
    }

    public void ToggleEngine()
    {
        if (engineSource == null) return;

        isEngineRunning = !isEngineRunning;

        if (isEngineRunning)
        {
            // Play engine start sound
            if (engineStartSFX != null)
                engineSource.PlayOneShot(engineStartSFX);

            // Then loop idle sound
            if (engineIdleLoop != null)
            {
                engineSource.clip = engineIdleLoop;
                engineSource.loop = true;
                engineSource.PlayDelayed(engineStartSFX != null ? engineStartSFX.length : 0);
            }
        }
        else
        {
            // Stop idle loop and play stop sound
            engineSource.Stop();
            if (engineStopSFX != null)
                engineSource.PlayOneShot(engineStopSFX);
        }
    }

    public bool IsEngineRunning()
    {
        return isEngineRunning;
    }
}