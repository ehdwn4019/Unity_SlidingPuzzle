using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject _boardCell;

    private List<BoardCell> _boardCellList = new List<BoardCell>();

    public Vector2 EmptyCellPos { get; set; }

    private const int BOARD_WIDTH = 4;
    private const int BOARD_HEIGHT = 4;
    private const int BOARD_SIZE = BOARD_WIDTH * BOARD_HEIGHT;

    private const int ARROUND_SPACING = 102;

    public void RefreshBoard()
    {
        _boardCell.SetActive(true);

        BoardCell cell = null;

        for(int i=0; i<BOARD_SIZE; i++)
        {
            cell = Instantiate(_boardCell, this.transform).GetComponent<BoardCell>();

            cell.RefreshCell(i + 1, (i + 1 == BOARD_SIZE), 
            (pos) => 
            {
                return SwapCell(pos);
            });

            _boardCellList.Add(cell);
        }

        _boardCell.SetActive(false);

        StartCoroutine(ShuffleCell());
    }

    private IEnumerator ShuffleCell()
    {
        float deltaTime = 0.0f;
        float time = 1.0f;

        while(deltaTime < time)
        {
            deltaTime += Time.deltaTime;

            int firstIndex = Random.Range(0, BOARD_SIZE / 2);
            int secondInex = Random.Range(BOARD_SIZE / 2, BOARD_SIZE);

            var tempPos = _boardCellList[firstIndex].CellPos;
            _boardCellList[firstIndex].CellPos = _boardCellList[secondInex].CellPos;
            _boardCellList[secondInex].CellPos = tempPos;

            yield return null;
        }

        EmptyCellPos = _boardCellList[_boardCellList.Count - 1].CellPos;

        GameManager.Instance.CanPlay = true;
    }

    private Vector2 SwapCell(Vector2 pos)
    {
        if(Vector2.Distance(EmptyCellPos, pos) == ARROUND_SPACING)
        {
            var targetPos = EmptyCellPos;

            EmptyCellPos = pos;

            GameManager.Instance.MoveCount++;

            return targetPos;
        }

        return Vector2.one;
    }

    public bool IsGameOver()
    {
        // 빈 공간 제외하기 위해 하나 빼준다  
        return _boardCellList.Count(x => x.IsOriginPos) == ( BOARD_SIZE - 1 );
    }

    public void Clear()
    {
        StopCoroutine(ShuffleCell());
        _boardCellList.ForEach(x => x.Clear());
    }
}
