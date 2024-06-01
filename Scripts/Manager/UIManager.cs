using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : RyoMonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    protected override void Awake()
    {
        base.Awake();

        if (UIManager.instance == null)
        {
            UIManager.instance = this;
        }
    }

    [Header("Component")]
    [SerializeField] private LoadUI_Canvas _loadUI;
    [SerializeField] private GameUI_Canvas _gameUI;
    [SerializeField] private MatchUI_Canvas _matchUI;
    [SerializeField] private PauseUI_Canvas _pauseUI;
    [SerializeField] private ResultUI_Canvas _resultUI;
    public LoadUI_Canvas LoadUI => _loadUI;
    public GameUI_Canvas GameUI => _gameUI;
    public MatchUI_Canvas MatchUI => _matchUI;
    public PauseUI_Canvas PauseUI => _pauseUI;
    public ResultUI_Canvas ResultUI => _resultUI;

    #region Load

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.Load_LoadUI();
        this.Load_MainUI();
        this.Load_MatchUI();
        this.Load_PauseUI();
        this.Load_ResultUI();
    }

    private void Load_LoadUI()
    {
        if (this._loadUI != null) return;
        this._loadUI = GetComponentInChildren<LoadUI_Canvas>(true);
    }

    private void Load_MainUI()
    {
        if (this._gameUI != null) return;
        this._gameUI = GetComponentInChildren<GameUI_Canvas>(true);
    }
    
    private void Load_MatchUI()
    {
        if (this._matchUI != null) return;
        this._matchUI = GetComponentInChildren<MatchUI_Canvas>(true);
    }

    private void Load_PauseUI()
    {
        if (this._pauseUI != null) return;
        this._pauseUI = GetComponentInChildren<PauseUI_Canvas>(true);
    }
    
    private void Load_ResultUI()
    {
        if (this._resultUI != null) return;
        this._resultUI = GetComponentInChildren<ResultUI_Canvas>(true);
    }

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.LoadUI?.gameObject.SetActive(true);
        this.GameUI?.gameObject.SetActive(false);
        this.MatchUI?.gameObject.SetActive(false);
        this.PauseUI?.gameObject.SetActive(false);
        this.ResultUI?.gameObject.SetActive(false);

    }

    /*
     * 
     */

    public void StartGame()
    {
        this.LoadUI?.gameObject.SetActive(false);
        this.GameUI?.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        this.LoadUI?.gameObject.SetActive(true);
        this.GameUI?.gameObject.SetActive(false);
    }

    public void StartMatch()
    {
        this.GameUI?.gameObject.SetActive(false);
        this.MatchUI?.gameObject.SetActive(true);
    }

    public void PauseMatch()
    {
        Time.timeScale = 0.0f;
        this.PauseUI?.gameObject.SetActive(true);
    }

    public void RetryMatch()
    {
        Time.timeScale = 1.0f;
        this.MatchUI?.gameObject.SetActive(false);
        this.PauseUI?.gameObject.SetActive(false);
        this.MatchUI?.gameObject.SetActive(true);
    }

    public void ResumeMatch()
    {
        Time.timeScale = 1.0f;
        this.PauseUI?.gameObject.SetActive(false);
    }

    public void ExitMatch()
    {
        Time.timeScale = 1.0f;
        this.GameUI?.gameObject.SetActive(true);
        this.MatchUI?.gameObject.SetActive(false);
        this.PauseUI?.gameObject.SetActive(false);
    }

    /*
     * 
     */

    public void ResultMatch()
    {
        this.MatchUI?.gameObject.SetActive(false);
        this.ResultUI?.gameObject.SetActive(true);
    }

    public void NextMatch()
    {
        this.MatchUI?.gameObject.SetActive(true);
        this.ResultUI?.gameObject.SetActive(false);
    }

    public void ExitResult()
    {
        this.GameUI?.gameObject.SetActive(true);
        this.ResultUI?.gameObject.SetActive(false);
    }


}
