using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [SerializeField] private Board _board;

    [Header("Result Info")]
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _txtPlayTime;
    [SerializeField] private TextMeshProUGUI _txtMoveCount;

    public int MoveCount { get; set; }
    public bool CanPlay { get; set; }

    private float _playTime;

    private void Awake ()
    {
        Instance = this;

        MoveCount = 0;
        CanPlay = false;
    }

    private void Start ()
    {
        StartCoroutine(CoPlayTimer());

        _board.RefreshBoard();
    }

#if UNITY_EDITOR
    private void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopPlayTimer();
            ShowResultPanel();
        }
    }
#endif

    private IEnumerator CoPlayTimer()
    {
        var waitForSeconds = new WaitForSeconds(1.0f);

        while(true)
        {
            _playTime++;

            yield return waitForSeconds;
        }
    }

    public void CheckGameOver()
    {
        if(_board.IsGameOver())
        {
            StopPlayTimer();
            ShowResultPanel();
        }
    }

    private void StopPlayTimer()
    {
        StopCoroutine(CoPlayTimer());
    }

    private void ShowResultPanel()
    {
        _resultPanel.SetActive(true);

        _txtMoveCount.text = $"MOVE COUNT : {MoveCount}";

        var timeSpan = TimeSpan.FromSeconds(_playTime);
        _txtPlayTime.text = $"PLAY TIME : {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
