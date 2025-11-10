using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CellStatus
    {
        [Header("Fixed on initialization")]
        [SerializeField][ReadOnly] private BoardCell _cell;

        [Header("Variables")]
        [SerializeField][ReadOnly] private int _playerID;
        [SerializeField][ReadOnly] private int _pieceID;

        public BoardCell Cell => _cell;
        public int PlayerID => _playerID;
        public int PieceID => _pieceID;

        public bool IsOccupied => _playerID != Defaults.PLAYER_ID;

        public CellStatus(BoardCell cell)
        {
            _cell = cell;
            _playerID = Defaults.PLAYER_ID;
            _pieceID = Defaults.PIECE_ID;
        }

        public void SetPlayerID(int playerID) => _playerID = playerID;
        public void SetPieceID(int pieceID) => _pieceID = pieceID;

        public void SetPlayerAndPieceID(int playerID, int pieceID)
        {
            SetPlayerID(playerID);
            SetPieceID(pieceID);
        }

        public void ResetIDs()
        {
            _playerID = Defaults.PLAYER_ID;
            _pieceID = Defaults.PIECE_ID;
        }

        public Vector2[] GetAndHighlightPossibleMoves()
        {
            return _cell.GetAndHighlightPossibleMoves(_playerID, _pieceID);
        }
    }
}