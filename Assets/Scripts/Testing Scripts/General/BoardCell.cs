using System.Collections.Generic;
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
        [SerializeField] private CoordinateEvent _clickCellEvent;

        [Header("Game Data")]
        [SerializeField] private GameplayStateData _gameData;
        [SerializeField] private PieceCatalog _pieceCatalog;
        [SerializeField] private Colors _colors;

        public Coordinate Position => new(_row, _column);

        public void SetGameData(GameplayStateData gameData) => _gameData = gameData;

        public void SetRowAndColumn(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickCellEvent.Raise(new Coordinate(_row, _column));
        }

        public void SetHighlightAsActive(bool active)
        {
            if (active) SetActiveHighlight();
            else SetInActiveHighlight();
        }

        public void SetCellColorAsPlayerColor(int playerID)
        {
            _cellRenderer.color = _colors.List[playerID];
        }

        public void ResetCellColor()
        {
            _cellRenderer.color = _defaultCellColor;
        }

        public void SetHighlightIconAsPossibleMove(int playerID)
        {
            _highlightRenderer.sprite = _possibleMoveSprite;
            _highlightRenderer.color = _colors.List[playerID];
        }

        public void ClearHighlightIcon()
        {
            _highlightRenderer.sprite = null;
        }

        public void ClearCell()
        {
            ClearHighlightIcon();
            ResetCellColor();
        }

        public void SetHighlightIconAsPiece(int pieceID)
        {
            _highlightRenderer.sprite = _pieceCatalog.Get(pieceID).Icon;
        }

        private void SetActiveHighlight()
        {
            var color = _highlightRenderer.color;
            _highlightRenderer.color = new Color(color.r, color.g, color.b, _activeHighlightAlpha);
        }

        private void SetInActiveHighlight()
        {
            var color = _highlightRenderer.color;
            _highlightRenderer.color = new Color(color.r, color.g, color.b, _inactiveHighlightAlpha);
        }
    }
}