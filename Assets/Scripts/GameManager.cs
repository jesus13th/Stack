using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance  => instance ?? (instance = FindObjectOfType<GameManager>());
    

    [SerializeField] private MovingCube currentCube;
    public MovingCube CurrentCube { get { return currentCube; } set { currentCube = value; } }

    [SerializeField] private MovingCube lastCube;
    public MovingCube LastCube { get { return lastCube; } set { lastCube = value; } }

    [SerializeField] private CubeSpawner[] spawners;
    private int spawnerIndex = 0;
    public static Action OnCubeSpawned = delegate { };
    public bool isGameOver = false;
    [SerializeField] private GameObject pScoreText;
    [SerializeField] private PanelGame pPanel;

    [SerializeField] internal AudioSource audioBackground;

    [SerializeField] private Gradient gradient;

    private float porcent;
    public Color ColorRandom => gradient.Evaluate(porcent++ / 100.0f);

    private void Start() {
        instance = this;
        CurrentCube = LastCube = FindObjectOfType<MovingCube>();
    }
    void Update() {
        if(Input.GetButtonDown("Fire1") && !isGameOver)
            StartCoroutine(_Update());
    }
    private IEnumerator _Update() {
        if (CurrentCube == LastCube) {
            ScoreText.Instance.InitializeScore();
            audioBackground.Play();
        }
        CurrentCube?.Stop();
        yield return new WaitForSeconds(0.25f);
        spawnerIndex = spawnerIndex == 0 ? 1 : 0;
        if (!isGameOver) {
            spawners[spawnerIndex].SpawnCube();
        }
    }
    public void ShowPanelGame() {
        pScoreText.SetActive(false);
        int score, highScore;
        InsertScore(out score, out highScore);
        PanelGame panelGame = Instantiate(pPanel, FindObjectOfType<Canvas>().transform, false);
        panelGame.score = score;
        panelGame.highScore = highScore;
    }
    public void InsertScore(out int score, out int highScore) {
        score = ScoreText.Instance.score;
        if (PlayerPrefs.HasKey(ScoreText.Score_Key)) {
            if (PlayerPrefs.GetInt(ScoreText.Score_Key) < score) {
                PlayerPrefs.SetInt(ScoreText.Score_Key, score);
                Debug.Log("Se registra un nuevo score");
            }
        } else {
            PlayerPrefs.SetInt(ScoreText.Score_Key, score);
        }
        highScore = PlayerPrefs.GetInt(ScoreText.Score_Key);
        PlayerPrefs.Save();
    }
}