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

        public List<Vector2> GetPossibleMoves(int x, int y, Vector2 size) => _move.Get(x, y, size);

        public MoveContext GetMoveCostAndType(int movesLeft, int currentPlayerID, int targetPlayerID) => _move.GetMoveContext(movesLeft, currentPlayerID, targetPlayerID);
    }
}