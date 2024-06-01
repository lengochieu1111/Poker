using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : RyoMonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    protected override void Awake()
    {
        base.Awake();

        if (AudioManager.instance == null)
        {
            AudioManager.instance = this;
        }
    }

    [Header("Property")]
    [SerializeField] private bool _isActive_Music;
    [SerializeField] private bool _isActive_Sound;
    [SerializeField] private AudioSource _audioSource_Music;
    [SerializeField] private AudioSource _audioSource_Sound;
    [SerializeField] private AudioClip _pressedClip;
    public bool IsActive_Music
    {
        get { return this._isActive_Music; }
        private set 
        {
            if (this.AudioSource_Music != null)
            {
                this.AudioSource_Music.enabled = value;
            }

            this._isActive_Music = value; 
        }
    }
    
    public bool IsActive_Sound
    {
        get { return this._isActive_Sound; }
        private set
        {
            if (this.AudioSource_Sound != null)
            {
                this.AudioSource_Sound.enabled = value;

                if (value)
                {
                    this.PlayAudio_Pressed();
                }
            }

            this._isActive_Sound = value; 
        }
    }

    private AudioSource AudioSource_Music => _audioSource_Music;
    private AudioSource AudioSource_Sound => _audioSource_Sound;
    private AudioClip PressedClip => _pressedClip;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadAudioSource_Music();
        this.LoadAudioSource_Sound();
    }

    private void LoadAudioSource_Music()
    {
        if (this._audioSource_Music != null) return;
        this._audioSource_Music = this.transform.GetChild(0)?.GetComponent<AudioSource>();
    }
    
    private void LoadAudioSource_Sound()
    {
        if (this._audioSource_Sound != null) return;
        this._audioSource_Sound = this.transform.GetChild(1)?.GetComponent<AudioSource>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this._isActive_Music = true;
        this._isActive_Sound = true;
    }

    /*
     * 
     */

    public void ChangeActive_Music()
    {
        this.IsActive_Music = !this.IsActive_Music;
    }
    
    public void ChangeActive_Sound()
    {
        this.IsActive_Sound = !this.IsActive_Sound;
    }

    public void PlayAudio_Pressed()
    {
        this.AudioSource_Sound?.PlayOneShot(this.PressedClip);
    }

}
