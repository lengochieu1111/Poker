using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MatchUI_Canvas : RyoMonoBehaviour, IGameData
{
    [Header("Component")]
    [SerializeField] Sprite _cardBackside_Sprite;
    [SerializeField] List<Sprite> _cards_Sprite;
    [SerializeField] Transform _cardsAbove;
    [SerializeField] Transform _cardsBelow;
    [SerializeField] Transform _victoryCard;
    public Sprite CardBackside_Sprite => _cardBackside_Sprite;
    public List<Sprite> Cards_Sprite => _cards_Sprite;
    public Transform CardsAbove => _cardsAbove;
    public Transform CardsBelow => _cardsBelow;
    public Transform VictoryCard => _victoryCard;

    [Header("Data")]
    [SerializeField] private bool _isInProgress = true;
    [SerializeField] private int _level;
    [SerializeField] private int _balance;
    [SerializeField] private int _match;
    [SerializeField] private int _turns;
    [SerializeField] private int _matchTime;
    [SerializeField] private List<int> _cardNumberOfEachLevel;
    [SerializeField] private List<int> _timeOfEachLevel;
    private float _timerCounter;
    public bool IsInProgress
    {
        get { return _isInProgress; }
        private set { _isInProgress = value; }
    }
    
    public int Level
    {
        get { return _level; }
        private set { _level = value; }
    }
    public int Balance
    {
        get { return _balance; }
        private set 
        {
            this.UpdateBalance_Text(value);

            _balance = value; 
        }
    }
    public int Match
    {
        get { return _match; }
        private set 
        {
            if (value == this.CardNumberPerRow)
            {
                this.Win();
            }

            this.UpdateMatch_Text(value);

            _match = value; 
        }
    }
    public int Turns
    {
        get { return _turns; }
        private set 
        {
            this.UpdateTurns_Text(value);

            _turns = value; 
        }
    }
    public int MatchTime
    {
        get { return _matchTime; }
        private set 
        {
            if (value == 0)
            {
                this.Loser();
            }

            this.UpdateTime_Text(value);

            _matchTime = value; 
        }
    }
    public List<int> CardNumberOfEachLevel
    {
        get { return this._cardNumberOfEachLevel; }
        private set { this._cardNumberOfEachLevel = value; }
    }
    public List<int> TimeOfEachLevel
    {
        get { return this._timeOfEachLevel; }
        private set { this._timeOfEachLevel = value; }
    }

    [Header("Card")]
    [SerializeField] private List<int> _randomCardIndex = new List<int>();
    [SerializeField] private float _distacePerCard = 125.0f;
    [SerializeField] private int _cardNumberPerRow;
    [SerializeField] private float _firstCardPosition;

    [SerializeField] private bool _canOpenCard = true;
    [SerializeField] private int _openCardNumberCounter;
    [SerializeField] private Card_Button _firstOpenCard, _secondOpenCard;
    [SerializeField] private float _comparisonTime = 1.5f;
    [SerializeField] private Vector3 _victoryPositionForCar;
    private Coroutine _comparisonCoroutine;

    public bool CanOpenCard
    {
        get { return this._canOpenCard; }
        private set { this._canOpenCard = value; }
    }
    public int OpenCardNumberCounter
    {
        get { return this._openCardNumberCounter; }
        private set { this._openCardNumberCounter = value; }
    }
    public Card_Button FirstOpenCard
    {
        get { return this._firstOpenCard; }
        private set { this._firstOpenCard = value; }
    }
    
    public Card_Button SecondOpenCard
    {
        get { return this._secondOpenCard; }
        private set { this._secondOpenCard = value; }
    }
    
    public Vector3 VictoryPositionForCar
    {
        get { return this._victoryPositionForCar; }
        private set { this._victoryPositionForCar = value; }
    }
    
    public List<int> RandomCardIndex
    {
        get { return this._randomCardIndex; }
        private set { this._randomCardIndex = value; }
    }

    public int CardNumberPerRow
    {
        get { return this._cardNumberPerRow; }
        private set { this._cardNumberPerRow = value; }
    }
    
    public float FirstCardPosition
    {
        get { return this._firstCardPosition; }
        private set { this._firstCardPosition = value; }
    }

    public float DistacePerCard => _distacePerCard;
    public float ComparisonTime => _comparisonTime;

    [Header("Infortion")]
    [SerializeField] private TextMeshProUGUI _turns_Text;
    [SerializeField] private TextMeshProUGUI _match_Text;
    [SerializeField] private TextMeshProUGUI _balance_Text;
    [SerializeField] private TextMeshProUGUI _time_Text;
    public TextMeshProUGUI Turns_Text => _turns_Text;
    public TextMeshProUGUI Match_Text => _match_Text;
    public TextMeshProUGUI Balance_Text => _balance_Text;
    public TextMeshProUGUI Time_Text => _time_Text;

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCards();
        this.LoadCardBackside();
        this.LoadCardsAbove();
        this.LoadCardsBelow();
        this.LoadVictoryCard();

        this.LoadTurnsText();
        this.LoadMatchText();
        this.LoadBalanceText();
        this.LoadTimeText();

    }

    private void LoadCards()
    {
        if (this._cards_Sprite.Count > 0) return;

        Sprite[] cards_Sprite = Resources.LoadAll<Sprite>("Cards/");
        this._cards_Sprite.AddRange(cards_Sprite);
    }
    
    private void LoadCardBackside()
    {
        if (this._cardBackside_Sprite != null) return;

        Sprite[] cardBackside_Sprite = Resources.LoadAll<Sprite>("CardBackside/");
        this._cardBackside_Sprite = cardBackside_Sprite[0];
    }
    
    private void LoadCardsAbove()
    {
        if (this._cardsAbove != null) return;
        this._cardsAbove = this.FindChildByName(this.transform, "CardsAbove");
    }
    
    private void LoadCardsBelow()
    {
        if (this._cardsBelow != null) return;
        this._cardsBelow = this.FindChildByName(this.transform, "CardsBelow");
    }
    
    private void LoadVictoryCard()
    {
        if (this._victoryCard != null) return;
        this._victoryCard = this.FindChildByName(this.transform, "VictoryCard");
    }

    private void LoadTurnsText()
    {
        if (this._turns_Text != null) return;
        this._turns_Text = this.FindChildByName(this.transform, "Turns_Text")?.GetComponent<TextMeshProUGUI>();
    }

    private void LoadMatchText()
    {
        if (this._match_Text != null) return;
        this._match_Text = this.FindChildByName(this.transform, "Match_Text")?.GetComponent<TextMeshProUGUI>();
    }
    
    private void LoadBalanceText()
    {
        if (this._balance_Text != null) return;
        this._balance_Text = this.FindChildByName(this.transform, "Balance_Text")?.GetComponent<TextMeshProUGUI>();
    }
    
    private void LoadTimeText()
    {
        if (this._time_Text != null) return;
        this._time_Text = this.FindChildByName(this.transform, "Time_Text")?.GetComponent<TextMeshProUGUI>();
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

    protected override void SetupValues()
    {
        base.SetupValues();

        this._timerCounter = 0;
        this._match = 0;
        this._turns = 0;
        this._canOpenCard = true;
        this._isInProgress = true;
        this._victoryPositionForCar = Vector3.zero;
        this._openCardNumberCounter = 0;
        this._randomCardIndex.Clear();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        foreach (Transform child in this.CardsAbove)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in this.CardsBelow)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in this.VictoryCard)
        {
            Destroy(child.gameObject);
        }

        GameDataManager.Instance.SendGameData(this);

        this.UpdateMatch_Text(0);
        this.UpdateTurns_Text(0);

    }

    protected override void OnDisable()
    {
        base.OnDisable();


    }

    private void Update()
    {
        if (this.IsInProgress)
        {
            this._timerCounter += Time.deltaTime;

            if (this._timerCounter > 1)
            {
                this._timerCounter = 0;
                this.MatchTime -= 1;
            }
        }
    }

    /*
     * 
     */

    public void RequestOpenCard(Card_Button card)
    {
        if (this.CanOpenCard == false) return;

        card.Open();
        this.OpenCardNumberCounter++;
        this.Balance -= 10;

        AudioManager.Instance.PlayAudio_Pressed();

        if (this.OpenCardNumberCounter == 1)
        {
            this.FirstOpenCard = card;
        }
        else if (this.OpenCardNumberCounter == 2)
        {
            this.SecondOpenCard = card;
            this._comparisonCoroutine = StartCoroutine(this.ComparativeDelay());
        }
    }

    private IEnumerator ComparativeDelay()
    {
        this.CanOpenCard = false;

        yield return new WaitForSecondsRealtime(this.ComparisonTime);

        if (this.FirstOpenCard.CardIndex == this.SecondOpenCard.CardIndex)
        {
            this.ProcessingTheSameTwoCards();
            this.Match++;
            this.Balance += 100;

        }
        else
        {
            this.FirstOpenCard.Close();
            this.SecondOpenCard.Close();
        }

        this.Turns++;
        this.CanOpenCard = true;
        this.OpenCardNumberCounter = 0;
    }

    private void ProcessingTheSameTwoCards()
    {
        this.FirstOpenCard.transform.SetParent(this.VictoryCard);
        this.SecondOpenCard.transform.SetParent(this.VictoryCard);

        this.VictoryPositionForCar = new Vector3(this.VictoryPositionForCar.x + 8, 0, 0);
        this.FirstOpenCard.Victory(this.VictoryPositionForCar);

        this.VictoryPositionForCar = new Vector3(this.VictoryPositionForCar.x + 8, 0, 0);
        this.SecondOpenCard.Victory(this.VictoryPositionForCar);
    }

    public void RequestCloseCard(Card_Button card)
    {
        card.Close();
    }

    /*
     * 
     */

    public void LoadData(int balacce, int level, List<int> cardNumberOfEachLevel, List<int> timeOfEachLevel)
    {
        this.Level = level;
        this.Balance = balacce;
        this.CardNumberOfEachLevel = cardNumberOfEachLevel;
        this.TimeOfEachLevel = timeOfEachLevel;

        /*
         * 
         */

        this.MatchTime = this.TimeOfEachLevel[this.Level];
        this.CardNumberPerRow = this.CardNumberOfEachLevel[this.Level] / 2;

        this.RandomCard();
        this.SpawnCard();

    }

    public void SaveData(ref int balacce, ref int level)
    {
        level = this.Level;
        balacce = this.Balance;
    }

    /*
     * 
     */

    private void RandomCard()
    {
        List<int> cardIndex_old = new List<int>();

        for (int i = 0; i < this.CardNumberPerRow; i++)
        {
            bool isLoop = true;
            while (isLoop)
            {
                int cardIndex = Random.Range(0, this.Cards_Sprite.Count);

                if (cardIndex_old.Contains(cardIndex) == false)
                {
                    this.RandomCardIndex.Add(cardIndex);
                    cardIndex_old.Add(cardIndex);
                    isLoop = false;
                }

            }

        }
    }

    private void SpawnCard()
    {
        if (this.CardNumberPerRow % 2 == 0)
        {
            this.FirstCardPosition = this.DistacePerCard / 2 + this.DistacePerCard * (this.CardNumberPerRow / 2 - 1);
        }
        else
        {
            this.FirstCardPosition = this.DistacePerCard * (this.CardNumberPerRow - 1) / 2;
        }

        this.FirstCardPosition *= -1;

        List<int> cardIndexs = new List<int>(); 

        for (int i = 0; i < this.CardNumberPerRow; i++)
        {
            this.SpawnCard_Above(ref cardIndexs, i);
            this.SpawnCard_Below(ref cardIndexs, i);
        }
    }

    private void SpawnCard_Above(ref List<int> cardIndexs, int index)
    {
        int cardIndex = RandomIndex(ref cardIndexs);
        this.SpawnCard_(this.CardsAbove, cardIndex, index);
    }
    
    private void SpawnCard_Below(ref List<int> cardIndexs, int index)
    {
        int cardIndex = RandomIndex(ref cardIndexs);
        this.SpawnCard_(this.CardsBelow, cardIndex, index);
    }

    private int RandomIndex(ref List<int> cardIndexs)
    {
        bool isLoop = true;
        int cardIndex = 0;
        while (isLoop)
        {
            cardIndex = Random.Range(0, this.RandomCardIndex.Count);
            int numberOfAppearances = cardIndexs.Count(x => x == cardIndex);
            isLoop = numberOfAppearances >= 2;
            if (isLoop == false)
            {
                cardIndexs.Add(cardIndex);
            }
        }

        return this.RandomCardIndex[cardIndex];
    }

    private void SpawnCard_(Transform parrent, int cardIndex, int index)
    {
        Transform cardBelow = CardSpawner.Instance.Spawn(CardSpawner.Card, this.transform.position, this.transform.rotation);
        cardBelow.SetParent(parrent);
        cardBelow.localPosition = new Vector3(this.FirstCardPosition + index * this.DistacePerCard, 0, 0);
        cardBelow.GetComponentInChildren<Card_Button>()?.SetupCard(this.Cards_Sprite[cardIndex], this.CardBackside_Sprite, cardIndex);
        cardBelow.gameObject.SetActive(true);
    }

    /*
     * 
     */

    private void UpdateTurns_Text(int value)
    {
        if (this.Turns_Text == null) return;
        string newText = "TURNS : " + value;
        this.Turns_Text.text = newText;
    }

    private void UpdateMatch_Text(int value)
    {
        if (this.Match_Text == null) return;
        string newText = "MATCH : " + value;
        this.Match_Text.text = newText;
    }

    private void UpdateBalance_Text(int value)
    {
        if (this.Balance_Text == null) return;
        string newText = "$ " + value;
        this.Balance_Text.text = newText;
    }

    private void UpdateTime_Text(int value)
    {
        if (this.Time_Text == null) return;
        string minute = (value / 60).ToString();
        string second = (value % 60).ToString();
        this.Time_Text.text = "TIME  " + minute + " : " + second;
    }

    /*
     * 
     */

    private void Loser()
    {
        this.EndMatch();
    }

    private void Win()
    {
        this.Level = (this.Level + 1) % this.CardNumberOfEachLevel.Count;
        this.EndMatch();
    }
    
    private void EndMatch()
    {
        this.CanOpenCard = false;
        this.IsInProgress = false;
        GameDataManager.Instance.ReceiveGameData(this);
        UIManager.Instance.ResultMatch();
    }

    /*
     * 
     */

    public void PressPauseButton()
    {
        GameDataManager.Instance.ReceiveGameData(this);

        UIManager.Instance.PauseMatch();
    }

}
