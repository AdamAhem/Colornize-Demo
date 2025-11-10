using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameInstanceData", menuName = "SO/GameInstanceData")]
    public class GameplayStateData : ScriptableObject
    {
        [Header("FIXED CONSTANTS")]
        [SerializeField] private int _piecesPerPlayer;

        [SerializeField][ReadOnly] private Vector2 _boardDimensions;

        private List<Vector2[]> _playerPiecePositions;
        private CellStatus[][] _boardStatus;

        [Obsolete]
        public Vector2 LastCellPositionClicked => default;

        public Vector2 BoardDimensions => _boardDimensions;

        public List<Vector2[]> PlayerPiecePositions => _playerPiecePositions;

        public void ResetData()
        {

            _boardStatus = null;
            _playerPiecePositions = null;
        }

        public void SetPlayerPiecePosition(int playerID, int pieceID, int row, int column)
        {
            SetPlayerPiecePosition(playerID, pieceID, new Vector2(row, column));
        }

        public void SetPlayerPiecePosition(int playerID, int pieceID, Vector2 position)
        {
            _playerPiecePositions[playerID][pieceID] = position;
        }

        public CellStatus LastClickedCellStatus => _boardStatus[(int)LastCellPositionClicked.x][(int)LastCellPositionClicked.y];

        public CellStatus GetCellStatusAtPosition(int row, int column)
        {
            return _boardStatus[row][column];
        }

        public CellStatus GetCellStatusAtPosition(Vector2 coordinate)
        {
            return GetCellStatusAtPosition((int)coordinate.x, (int)coordinate.y);
        }

        public int NumberOfPlayers => 10;
    }
}