using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Card_Button : RyoMonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private MatchUI_Canvas _matchUI;
    [SerializeField] private Sprite _open_Sprite, _close_Sprite;
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _canPress = true;
    [SerializeField] private int _cardIndex;
    [SerializeField] private float _statusChangeTime = 1f;
    [SerializeField] private float _timeToVictoryPosition = 10f;
    private Coroutine _openCoroutine;
    private Coroutine _closeCoroutine;
    private Coroutine _victoryCoroutine;
    public Image Image => _image;
    public MatchUI_Canvas MatchUI => _matchUI;
    public Sprite Open_Sprite
    {
        get { return _open_Sprite; }
        private set { _open_Sprite = value; }
    }
    
    public Sprite Close_Sprite
    {
        get { return _close_Sprite; }
        private set { _close_Sprite = value; }
    }

    public bool IsOpen
    {
        get { return  _isOpen; }
        private set 
        {
            if (value)
            {
                this._openCoroutine = StartCoroutine(this.StatusChangeDelay(this.Open_Sprite));
            }
            else
            {
                this._closeCoroutine = StartCoroutine(this.StatusChangeDelay(this.Close_Sprite));
            }

            _isOpen = value; 
        }
    }

    public float StatusChangeTime => _statusChangeTime;
    public float TimeToVictoryPosition => _timeToVictoryPosition;
    public int CardIndex
    {
        get { return _cardIndex; }
        private set { _cardIndex = value; }
    }
    
    public bool CanPress
    {
        get { return _canPress; }
        private set { _canPress = value; }
    }

    #region Load
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCard();
        this.LoadMatachUI();

    }

    private void LoadCard()
    {
        if (this._image != null) return;

        this._image = GetComponent<Image>();
    }
    
    private void LoadMatachUI()
    {
        if (this._matchUI != null) return;

        this._matchUI = GetComponentInParent<MatchUI_Canvas>();
    }

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this._isOpen = false;
        this._canPress = true;
        this._image.sprite = this._close_Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.CanPress == false) return;

        this.MatchUI.RequestOpenCard(this);
    }

    public void SetupCard(Sprite openSprite, Sprite closeSprite, int cardIndex)
    {
        this.Open_Sprite = openSprite;
        this.Close_Sprite = closeSprite;
        this.CardIndex = cardIndex;
    }

    public void Open()
    {
        this.IsOpen = true;
    }

    public void Close()
    {
        this.IsOpen = false;
    }

    private IEnumerator StatusChangeDelay(Sprite sprite)
    {
        this.CanPress = false;

        float closeCounter = 0f;
        float rotationY = 0;
        while (closeCounter < this.StatusChangeTime)
        {
            closeCounter += Time.deltaTime;

            float newRotationY = Mathf.Lerp(rotationY, -90f, closeCounter / this.StatusChangeTime);
            this.transform.localRotation = Quaternion.Euler(0f, newRotationY, 0f);

            yield return null;
        }

        this.Image.sprite = sprite;

        float openCounter = 0f;
        rotationY = -90;
        while (openCounter < this.StatusChangeTime)
        {
            openCounter += Time.deltaTime;

            float newRotationY = Mathf.Lerp(rotationY, 0f, openCounter / this.StatusChangeTime);
            this.transform.localRotation = Quaternion.Euler(0f, newRotationY, 0f);

            yield return null;
        }

        this.CanPress = true;

    }

    public void Victory(Vector3 newLocalPosition)
    {
        this.CanPress = false;
        this._victoryCoroutine = StartCoroutine(this.Victory_Coroutine(newLocalPosition));
    }

    public IEnumerator Victory_Coroutine(Vector3 newLocalPosition)
    {
        float timeCounter = 0;

        while (timeCounter <= this.TimeToVictoryPosition)
        {
            timeCounter += Time.deltaTime;
            Vector3 localPosition = this.transform.localPosition;

            this.transform.localPosition = Vector3.Lerp(localPosition, newLocalPosition, timeCounter / this.TimeToVictoryPosition);
            
            float distanceForVictoryPosition = Vector3.Distance(this.transform.localPosition, newLocalPosition);
            if (distanceForVictoryPosition <= 0.005f)
            {
                this.Image.sprite = this.Close_Sprite;
            }

            yield return null;
        }

    }



}
