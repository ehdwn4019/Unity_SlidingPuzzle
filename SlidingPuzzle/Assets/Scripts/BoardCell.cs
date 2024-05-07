using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
    [SerializeField] private Image _imgCell;
    [SerializeField] private RectTransform _rtCell;
    [SerializeField] private TextMeshProUGUI _txtNum;

    private int _num;
    private bool _isLastCell = false;

    public Vector2 CellPos 
    { 
        get 
        { 
            return _rtCell.anchoredPosition; 
        } 
        set 
        { 
            _rtCell.anchoredPosition = value; 
        } 
    }

    public bool IsOriginPos { get; set; }

    private Vector2 _startPos;
    private Func<Vector2, Vector2> _callback;

    public void RefreshCell(int num, bool isLastCell, Func<Vector2, Vector2> callback)
    {
        _num = num;
        _txtNum.text = _num.ToString();
        _callback = callback;
        _startPos = CellPos;
        _isLastCell = isLastCell;


        if (_isLastCell)
        {
            _imgCell.enabled = false;
            _txtNum.enabled = false;
        }
    }

    private IEnumerator MoveCell(Vector2 endPos)
    {
        if (endPos == Vector2.one)
            yield break;

        float time = 0.0f;
        float duration = 0.1f;

        var startPos = CellPos;

        while(time <= duration)
        {
            time += Time.deltaTime;

            CellPos = Vector2.Lerp(startPos, endPos, time / duration);

            yield return null;
        }

        IsOriginPos = _startPos == CellPos;

        GameManager.Instance.CheckGameOver();
    }

    public void Clear()
    {
        StopCoroutine("MoveCell");
    }

    public void OnClickCell()
    {
        if (_isLastCell)
        {
#if UNITY_EDITOR
            Debug.Log($"This Cell is Last Cell, Cell Number is : {_num}");
#endif
            return;
        }

        if (!GameManager.Instance.CanPlay)
        {
#if UNITY_EDITOR
            Debug.Log("$Board Shuffle Now, Cannot Touch");
#endif
            return;
        }
            

        StartCoroutine(MoveCell(_callback.Invoke(CellPos)));
    }
}