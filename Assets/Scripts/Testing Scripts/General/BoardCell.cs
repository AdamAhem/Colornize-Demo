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

        private bool _active = true;

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

        public void ResetHighlightIcon()
        {
            _highlightRenderer.sprite = null;
        }

        public void ClearHighlight()
        {
            _highlightRenderer.sprite = null;
        }

        public void ClearCell()
        {
            ClearHighlight();
            ResetCellColor();
        }

        public void SetHighlightIconAsPiece(int pieceID)
        {
            _highlightRenderer.sprite = _pieceCatalog.Get(pieceID).Icon;
        }

        public Vector2[] GetAndHighlightPossibleMoves(int playerID, int pieceID)
        {
            // player id for color, piece id for highlight positions.
            // need to get the board cells of all valid places to click.

            // THIS PART IS ONLY UP TO BOARD DIMENSIONS
            List<Vector2> potentialMoves = _pieceCatalog.Get(pieceID).GetPossibleMoves(_row, _column, _gameData.BoardDimensions);

            for (int i = potentialMoves.Count - 1; i >= 0; i--)
            {
                Vector2 potentialMove = potentialMoves[i];
                CellStatus potentialCell = _gameData.GetCellStatusAtPosition(potentialMove);

                int cellPlayerID = potentialCell.PlayerID;

                bool uncoloredCell = cellPlayerID == Defaults.PLAYER_ID;
                bool sameColoredCell = cellPlayerID == playerID;
                bool cellOccupied = potentialCell.PieceID != Defaults.PIECE_ID;

                // move is only possible if the cell is unoccupied and if the cell is colored none or same color as the player.
                bool movePossible = !cellOccupied && (uncoloredCell || sameColoredCell);

                if (movePossible)
                {
                    potentialCell.Cell.SetHiglhightIconAsPossibleMove(playerID);
                }
                else
                {
                    potentialMoves.Remove(potentialMove);
                }
            }

            return potentialMoves.ToArray();
        }

        public void UnhighlightPossibleMoves(int playerID, int pieceID)
        {
            List<Vector2> possibleMovesWithinBoard = _pieceCatalog.Get(pieceID).GetPossibleMoves(_row, _column, _gameData.BoardDimensions);

            for (int i = possibleMovesWithinBoard.Count - 1; i >= 0; i--)
            {
                CellStatus potentialCell = _gameData.GetCellStatusAtPosition(possibleMovesWithinBoard[i]);

                int cellPlayerID = potentialCell.PlayerID;

                bool uncoloredCell = cellPlayerID == Defaults.PLAYER_ID;
                bool sameColoredCell = cellPlayerID == playerID;
                bool cellOccupied = potentialCell.PieceID != Defaults.PIECE_ID;

                // move is only possible if the cell is unoccupied and if the cell is colored none or same color as the player.

                if (!cellOccupied && (uncoloredCell || sameColoredCell)) potentialCell.Cell.ResetHighlightIcon();
            }
        }
    }
}