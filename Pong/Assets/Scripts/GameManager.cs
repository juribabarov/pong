using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Text countDownText;
    [SerializeField] Text player1Goals;
    [SerializeField] Text player2Goals;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject newGameMenu;
    [SerializeField] Toggle increaseSpeedToggle;
    [SerializeField] Toggle multipleBallsToggle;
    [SerializeField] Toggle shrinkingToggle;
    [SerializeField] Text playerType1;
    [SerializeField] Text playerType2;
    [SerializeField] GameObject playerPaddle1;
    [SerializeField] GameObject playerPaddle2;
    [SerializeField] AudioEvent audioSpawnBall;

    private float paddleSize = 7;
    public List<Transform> activeBalls;
    private ShakeGameObject shakeGameObject;
    private bool increaseSpeed;
    private bool multipleBalls;
    private bool shrinking;
    private AudioSource audioSource;
    private Vector2 playerStartPosition1;
    private Vector2 playerStartPosition2;

    private void Awake()
    {
        Instance = this;

        shakeGameObject = GetComponent<ShakeGameObject>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        player1Goals.text = "0";
        player2Goals.text = "0";

        playerStartPosition1 = playerPaddle1.transform.position;
        playerStartPosition2 = playerPaddle2.transform.position;

        activeBalls = new List<Transform>();
    }

    private void Update()
    {
        ManageInput();
        ManageGameModes();
    }

    private void ManageGameModes()
    {
        //wenn option aktiv, beide spieler über zeit kleiner machen
        if (shrinking)
        {
            Vector3 size1 = playerPaddle1.transform.localScale;
            Vector3 size2 = playerPaddle2.transform.localScale;
            if (size1.y > 1)
            {
                size1.y -= 0.1f * Time.deltaTime;
                playerPaddle1.transform.localScale = size1;
            }
            if (size2.y > 1)
            {
                size2.y -= 0.1f * Time.deltaTime;
                playerPaddle2.transform.localScale = size2;
            }
        }
    }

    private void ManageInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(!pauseMenu.activeSelf);
        }
    }

    public void Goal(int _playerGoal, GameObject _ball)
    {
        activeBalls.Remove(_ball.transform);
        Destroy(_ball);

        if (_playerGoal == 1) //Ball bei Spieler 1 reingegangen. Punkt für Spieler 2
        {
            int goals = int.Parse(player2Goals.text);
            player2Goals.text = (goals + 1).ToString();
        }
        else if (_playerGoal == 2) //Andersherum
        {
            int goals = int.Parse(player1Goals.text);
            player1Goals.text = (goals + 1).ToString();
        }

        shakeGameObject.Shake();


        Vector3 size = playerPaddle1.transform.localScale;
        size.y = paddleSize;

        if (!multipleBalls)
        {
            playerPaddle1.transform.localScale = size;
            playerPaddle2.transform.localScale = size;

            StartCoroutine(CountDown());
        }
        else
        {
            if (_playerGoal == 1)
                playerPaddle1.transform.localScale = size;
            else if (_playerGoal == 2)
                playerPaddle2.transform.localScale = size;
        }
    }

    public void StartGame()
    {
        Enum.TryParse(playerType1.text, out PlayerType player1);
        Enum.TryParse(playerType2.text, out PlayerType player2);

        NewGame(increaseSpeedToggle.isOn, multipleBallsToggle.isOn, shrinkingToggle.isOn, player1, player2);
    }

    public void NewGame(bool _increaseSpeed, bool _multipleBalls, bool _shrinking, PlayerType player1, PlayerType player2)
    {
        StopAllCoroutines();

        for (int i = activeBalls.Count - 1; i >= 0; i--)
        {
            Destroy(activeBalls[i].gameObject);
        }
        activeBalls.Clear();

        player1Goals.text = "0";
        player2Goals.text = "0";

        increaseSpeed = _increaseSpeed;
        multipleBalls = _multipleBalls;
        shrinking = _shrinking;

        playerPaddle1.GetComponent<Player>().AI = (player1 == PlayerType.PC);
        playerPaddle2.GetComponent<Player>().AI = (player2 == PlayerType.PC);

        Vector3 size = playerPaddle1.transform.localScale;
        size.y = paddleSize;

        playerPaddle1.transform.localScale = size;
        playerPaddle2.transform.localScale = size;

        playerPaddle1.transform.position = playerStartPosition1;
        playerPaddle2.transform.position = playerStartPosition2;

        PauseGame(false);

        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        float time = 3;
        countDownText.gameObject.SetActive(true);

        while (true)
        {
            while (time > 0)
            {
                countDownText.text = time.ToString("0.0");

                time -= Time.deltaTime;

                yield return null;
            }

            SpawnBall();
            audioSpawnBall.Play(audioSource);

            if (!multipleBalls)
            {
                countDownText.gameObject.SetActive(false);
                break;
            }

            time += 3;
        }
    }

    private void SpawnBall()
    {
        GameObject go = Instantiate(ballPrefab);
        go.GetComponent<Ball>().IncreaseSpeedOverTime = increaseSpeed;
        activeBalls.Add(go.transform);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
