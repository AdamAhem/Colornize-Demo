using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class BoardCell : MonoBehaviour, IPointerClickHandler
    {
        [Header("Components")]
        [SerializeField] private SpriteRenderer _cellRenderer;
        [SerializeField] private SpriteRenderer _highlightRenderer;

        [Header("Visuals")]
        [SerializeField] private float _activeHighlightAlpha;
        [SerializeField] private float _inactiveHighlightAlpha;
        [SerializeField] private Sprite _possibleMoveSprite;
        [SerializeField] private Color _defaultCellColor;

        [Header("Assigned Position")]
        [SerializeField][ReadOnly] private int _row;
        [SerializeField][ReadOnly] private int _column;

        [Header("Events")]
        [SerializeField] private GameEvent _clickCellEvent;

        [Header("Game Data")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private PieceCatalog _pieceCatalog;
        [SerializeField] private Colors _colors;


        private bool _active = true;

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

        public void SetActiveHighlight()
        {
            if (_active) return;
            _active = true;
            var color = _highlightRenderer.color;
            _highlightRenderer.color = new Color(color.r, color.g, color.b, _activeHighlightAlpha);
        }

        public void SetInActiveHighlight()
        {
            if (!_active) return;
            _active = false;
            var color = _highlightRenderer.color;
            _highlightRenderer.color = new Color(color.r, color.g, color.b, _inactiveHighlightAlpha);
        }

        public void SetCellColorAsPlayerColor(int playerID)
        {
            _cellRenderer.color = _colors.List[playerID];
        }

        public void ResetCellColor()
        {
            _cellRenderer.color = _defaultCellColor;
        }

        public void SetHiglhightIconAsPossibleMove(int playerID)
        {
            _highlightRenderer.sprite = _possibleMoveSprite;
            _highlightRenderer.color = _colors.List[playerID];
        }

        public void ClearHighlight()
        {
            _highlightRenderer.sprite = null;
        }

        public void SetHighlightIconAsPiece(int pieceID)
        {
            _highlightRenderer.sprite = _pieceCatalog.Get(pieceID).Icon;
        }
    }
}