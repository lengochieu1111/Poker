using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameDataManager : RyoMonoBehaviour
{
    private static GameDataManager instance;
    public static GameDataManager Instance => instance;

    [Header("Property")]
    [SerializeField] private List<IGameData> _gameDatas;
    [SerializeField] private int _balance = 500;
    [SerializeField] private int _level = 0;
    [SerializeField] private List<int> _cardNumberOfEachLevel = new List<int> { 6, 8, 10, 12 };
    [SerializeField] private List<int> _timeOfEachLevel = new List<int> { 30, 50, 80, 100 };

    public int Balance
    {
        get { return this._balance; }
        private set { this._balance = value; }
    }

    public int Level
    {
        get { return this._level; }
        private set { this._level = value; }
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

    protected override void Awake()
    {
        base.Awake();

        if (GameDataManager.instance == null)
        {
            GameDataManager.instance = this;
        }

        this._gameDatas = this.LoadAllGameDataObjects();

        this.LoadGame();
    }

    private List<IGameData> LoadAllGameDataObjects()
    {
        IEnumerable<IGameData> gameDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IGameData>();

        return new List<IGameData>(gameDataObjects);
    }

    public void LoadGame()
    {
        foreach (IGameData gameData in this._gameDatas)
        {
            gameData.LoadData(this._balance, this._level, this._cardNumberOfEachLevel, this._timeOfEachLevel);
        }

    }

    public void SaveGame()
    {
        foreach (IGameData gameData in this._gameDatas)
        {
            gameData.SaveData(ref this._balance, ref this._level) ;
        }
    }


    public void SendGameData(IGameData gameData)
    {
        gameData.LoadData(this._balance, this._level, this._cardNumberOfEachLevel, this._timeOfEachLevel);

    }

    public void ReceiveGameData(IGameData gameData)
    {
        gameData.SaveData(ref this._balance, ref this._level);
    }


}
