using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Canvas : RyoMonoBehaviour, IGameData
{
    [SerializeField] private Image[] _image;
    [SerializeField] private TextMeshProUGUI _balance_Text;
    [SerializeField] private Sprite[] _offSprite, _onSprite;
    [SerializeField] private bool[] _activeStatus;
    [SerializeField] private int _balance;
    public Image[] Image => _image;
    public TextMeshProUGUI Balance_Text => _balance_Text;
    public Sprite[] OffSprite => _offSprite;
    public Sprite[] OnSprite => _onSprite;
    public bool[] ActiveStatus
    {
        get { return this._activeStatus; }
        private set 
        {
            this._activeStatus = value; 
            this.UpdateActiveStatus_Image();
        }
    }
    public int Balance
    {
        get { return this._balance; }
        private set 
        {
            this.Balance_Text.text = "BALANCE : $ " + value;

            this._balance = value;
        }
    }

    #region Load
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadImage();
        this.LoadBalance_Text();

    }

    private void LoadImage()
    {
        Transform button = this.FindChildByName(this.transform, "BUTTON");
        for (int i = 0; i < 4; i++)
        {
            this._image[i] = button.GetChild(i)?.GetComponent<Image>();
        }

    }
    
    private void LoadBalance_Text()
    {
        if (this._balance_Text != null) return;
        this._balance_Text = this.FindChildByName(this.transform, "Balance")?.GetComponent<TextMeshProUGUI>();
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

        this.ActiveStatus = new bool[] { true, true, true, true };

        this.ActiveStatus[2] = AudioManager.Instance.IsActive_Music;
        this.ActiveStatus = this.ActiveStatus;

        this.ActiveStatus[3] = AudioManager.Instance.IsActive_Sound;
        this.ActiveStatus = this.ActiveStatus;

        GameDataManager.Instance.SendGameData(this);

    }

    /*
     * BUTTON
     */

    public void PressedPlayButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();
        this.ActiveStatus[0] = !this.ActiveStatus[0];
        this.ActiveStatus = this.ActiveStatus;
        UIManager.Instance.StartMatch();
    }

    public void PressedExitButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();
        this.ActiveStatus[1] = !this.ActiveStatus[1];
        this.ActiveStatus = this.ActiveStatus;
        UIManager.Instance.ExitGame();
    }

    public void PressedMusicButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();
        this.ActiveStatus[2] = !this.ActiveStatus[2];
        this.ActiveStatus = this.ActiveStatus;
        AudioManager.Instance.ChangeActive_Music();

    }

    public void PressedSoundButton()
    {
        AudioManager.Instance.PlayAudio_Pressed();
        this.ActiveStatus[3] = !this.ActiveStatus[3];
        this.ActiveStatus = this.ActiveStatus;
        AudioManager.Instance.ChangeActive_Sound();
    }

    /*
     * 
     */

    private void UpdateActiveStatus_Image()
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.ActiveStatus[i])
                this.Image[i].sprite = this.OnSprite[i];
            else
                this.Image[i].sprite = this.OffSprite[i];
        }
    }

    public void LoadData(int balacce, int level, List<int> cardNumberOfEachLevel, List<int> timeOfEachLevel)
    {
        this.Balance = balacce;
    }

    public void SaveData(ref int balacce, ref int level)
    {
        
    }
}
