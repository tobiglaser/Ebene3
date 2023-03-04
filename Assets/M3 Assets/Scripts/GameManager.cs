using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum GameStates
    {
        Playing,
        Pausing,
        Ending,
    }
    [SerializeField] GameStates CurrentState = GameStates.Playing;

    [Header("Panels")]
    [SerializeField] GameObject IngamePanel;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject EndPanel;

    [Header("UI")]
    [SerializeField] public int CurrentLives;
    [SerializeField] int Lives;
    [SerializeField] Text LiveText;

    // Start is called before the first frame update
    void Start()
    {
        CurrentLives = Lives;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchStates();
        if(Input.GetKey(KeyCode.Escape) && CurrentState == GameStates.Playing)
        {
            CurrentState = GameStates.Pausing;
        }
        if(CurrentLives <= 0)
        {
            CurrentState = GameStates.Ending;
        }
    }

    void SwitchStates()
    {
        switch (CurrentState)
        {
            case GameStates.Playing:
                IsPlaying();
                break;
            case GameStates.Pausing:
                IsPaused();
                break;
            case GameStates.Ending:
                IsEnding();
                break;
            default:
                break;
        }
    }

    void IsPlaying()
    {
        Time.timeScale = 1;

        IngamePanel.SetActive(true);
        PausePanel.SetActive(false);
        EndPanel.SetActive(false);
        LiveText.text = $"Remaining Lives: {CurrentLives}/{Lives}";
    }
    void IsPaused()
    {
        Time.timeScale = 0;

        IngamePanel.SetActive(false);
        PausePanel.SetActive(true);
        EndPanel.SetActive(false);
    }
    void IsEnding()
    {
        IngamePanel.SetActive(false);
        PausePanel.SetActive(false);
        EndPanel.SetActive(true);
    }

    public void ContinueGame()
    {
        CurrentState = GameStates.Playing;
    }
}
