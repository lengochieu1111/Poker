using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI_Canvas : RyoMonoBehaviour
{
    [SerializeField] private Image _music_Image, _sound_Image;
    [SerializeField] private Sprite[] _music_Sprite, _sound_Sprite;
    [SerializeField] private bool _isActive_Music, _isActive_Sound;
    public Image Music_Image => _music_Image;
    public Image Sound_Image => _sound_Image;
    public Sprite[] Music_Sprite => _music_Sprite;
    public Sprite[] Sound_Sprite => _sound_Sprite;
    public bool IsActive_Music
    {
        get { return this._isActive_Music; }
        private set
        {
            if (value)
            {
                this.Music_Image.sprite = Music_Sprite[0];
            }
            else
            {
                this.Music_Image.sprite = Music_Sprite[1];
            }

            this._isActive_Music = value;
        }
    }
    
    public bool IsActive_Sound
    {
        get { return this._isActive_Sound; }
        private set
        {
            if (value)
            {
                this.Sound_Image.sprite = Sound_Sprite[0];
            }
            else
            {
                this.Sound_Image.sprite = Sound_Sprite[1];
            }

            this._isActive_Sound = value;
        }
    }

    #region Load
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadImage_Music();
        this.LoadImage_Sound();

    }

    private void LoadImage_Music()
    {
        if (this._music_Image != null) return;
        this._music_Image = this.FindChildByName(this.transform, "Music_Button")?.GetComponent<Image>();

    }
    
    private void LoadImage_Sound()
    {
        if (this._sound_Image != null) return;
        this._sound_Image = this.FindChildByName(this.transform, "Sound_Button")?.GetComponent<Image>();
    }

    private Transform FindChildByName(Transform parrent, string childName)
    {
        Transform childObject = parrent.Find(childName);

        if (childObject != null)
            return childObject;
        else
        {
            foreach (Transform child in parrent)
            {
                childObject = this.FindChildByName(child, childName);

                if (childObject != null)
                    return childObject;
            }

            return null;
        }

    }

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.IsActive_Music = AudioManager.Instance.IsActive_Music;
        this.IsActive_Sound = AudioManager.Instance.IsActive_Sound;
    }

    /*
     * BUTTON
     */

    public void PressedResumeButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();

        UIManager.Instance.ResumeMatch();
    }

    public void PressedRetryButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();

        UIManager.Instance.RetryMatch();
    }
    
    public void PressedHomeButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();

        UIManager.Instance.ExitMatch();
    }

    public void PressedMusicButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();
        this.IsActive_Music = !this.IsActive_Music;
        AudioManager.Instance.ChangeActive_Music();

    }

    public void PressedSoundButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();
        this.IsActive_Sound = !this.IsActive_Sound;
        AudioManager.Instance.ChangeActive_Sound();
    }

    /*
     * 
     */
}
