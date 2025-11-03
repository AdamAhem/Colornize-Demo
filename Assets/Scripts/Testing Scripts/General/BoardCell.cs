using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class BoardCell : MonoBehaviour, IPointerClickHandler
    {
        [Header("Assigned Position")]
        [SerializeField][ReadOnly] private int _row;
        [SerializeField][ReadOnly] private int _column;

        [Header("Events")]
        [SerializeField] private GameEvent _clickCellEvent;

        [Header("Game Data")]
        [SerializeField] private GameInstanceData _gameData;

        public void SetRowAndColumn(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // set data (so that other scripts can read from it)
            _gameData.SetLastClickedCellInfo(_row, _column);

            // THEN raise click event.
            _clickCellEvent.Raise();
        }
    }
}