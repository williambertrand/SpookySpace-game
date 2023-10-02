using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class GamePlayManager : MonoBehaviour
{

    [Header("Game play setup")]
    [SerializeField] private int startLevel;
    [SerializeField] private List<Level> gameLevels;
    private int currentLevel;
    public int currentPlayerMoves = 0;
    public bool isDebug = false;

    public delegate void OnLevelSpawned(Level level);
    private OnLevelSpawned onLevelSpawned;

    // UI fields
    [Header("UI Fields")]
    [SerializeField] TMP_Text moveText;
    [SerializeField] private EnemyManager enemyManager;

    [SerializeField] private TransitionEffectManager transitions;
    [SerializeField] private InfoPanelConroller infoPanel;

    [SerializeField] private Transform focus;

    #region Singleton
    public static GamePlayManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    #endregion

    void Start()
    {

        currentLevel = startLevel;

        // TODO: Spawn level stuff
        SpawnLevel();
        enemyManager.onEnemiesEmpty += OnNextLevel;
        onLevelSpawned += infoPanel.HandleLevelSpawn;


        StartCoroutine(NotifyFirstLevelSpawned());
    }

    private void OnDestroy()
    {
        onLevelSpawned -= infoPanel.HandleLevelSpawn;
    }

    private void SpawnLevel()
    {

        Debug.Log("------------ Level: " + currentLevel + " ------------");

        Level level = gameLevels[currentLevel];

        // Grid
        GridManager.Instance.SpawnGrid(level.gridWidth, level.gridHeight);

        // Player
        Player.Instance.Movement.SetPosition(level.playerSpawn);

        // Enemies
        enemyManager.SpawnEnemies(level.enemySpawns);

        // Obstacles / misc....
        GridManager.Instance.SpawnObstacles(level.objectSpawns);

        // TODO: Update camera position based on grid layout
        // Camera.main.transform.position
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        if (level.useFocus)
        {
            focus.position = GridManager.Instance.PositionFromPoint(new Point(level.gridWidth / 2, level.gridHeight / 2));
            cam.Follow = focus;
        } else
        {
            cam.Follow = Player.Instance.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        moveText.text = "Level " + (currentLevel + 1);
    }

    public void OnPlayerDidMove()
    {
        currentPlayerMoves += 1;
        StartCoroutine(MoveAllEnemiesAfterDelay());
        
    }

    public IEnumerator MoveAllEnemiesAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        enemyManager.MoveAllEnemies();
    }

    public void OnNotifyPlayerHit()
    {
        if (isDebug) return;
        GameConstants.playerDidDie = true;
        GameConstants.score = currentPlayerMoves;
        Player.Instance.IsDead = true;
        StartCoroutine(LoadGameOverAfter());
    }

    private IEnumerator LoadGameOverAfter()
    {
        yield return new WaitForSeconds(1.25f);
        OnResetLevel();
        //SceneManager.LoadScene(GameConstants.Scenes.GameOver);
    }

    private void OnNextLevel()
    {

        Debug.Log("On next level called");

        Player.Instance.Movement.enabled = false;

        currentLevel += 1;
        UpdateUI();

        if (currentLevel >= gameLevels.Count)
        {
            Debug.Log("GAME WIN!!!");
            SceneManager.LoadScene(GameConstants.Scenes.GameWon);
            return;
        }
        // TODO: Transitions, etc..
        transitions.CloudsTranstion();
        StartCoroutine(SpawnLevelAfter());
    }

    private IEnumerator SpawnLevelAfter()
    {
        yield return new WaitForSeconds(1.5f);
        SpawnLevel();

        yield return new WaitForSeconds(0.5f);
        onLevelSpawned?.Invoke(gameLevels[currentLevel]);

        Player.Instance.Movement.enabled = true;
    }

    public void OnResetLevel()
    {
        enemyManager.ClearEnemies();
        SpawnLevel();
        Player.Instance.IsDead = false;
    }

    private IEnumerator NotifyFirstLevelSpawned()
    {
        yield return new WaitForSeconds(0.25f);
        onLevelSpawned?.Invoke(gameLevels[0]);
    }
}
