using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CellStatus
    {
        [SerializeField][ReadOnly] private BoardCell _cell;
        [SerializeField][ReadOnly] private int _playerID;
        [SerializeField][ReadOnly] private int _pieceID;

        public BoardCell Cell => _cell;
        public int PlayerID => _playerID;
        public int PieceID => _pieceID;

        public CellStatus(BoardCell cell)
        {
            _cell = cell;
            _playerID = Defaults.PLAYER_ID;
            _pieceID = Defaults.PIECE_ID;
        }

        public void SetPlayerID(int playerID) => _playerID = playerID;
        public void SetPieceID(int pieceID) => _pieceID = pieceID;
    }
}