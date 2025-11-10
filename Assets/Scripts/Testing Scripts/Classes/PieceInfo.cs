using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PieceInfo
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private PieceMove _move;

        public int ID => _id;
        public string Name => _name;
        public Sprite Icon => _icon;

        public List<Coordinate> GetPossibleMoves(Coordinate center, Coordinate size) => _move.Get(center.X, center.Y, size);

        public MoveContext GetMoveContext(int movesLeft, int currentPlayerID, int targetPlayerID) => _move.GetMoveContext(movesLeft, currentPlayerID, targetPlayerID);
    }
}