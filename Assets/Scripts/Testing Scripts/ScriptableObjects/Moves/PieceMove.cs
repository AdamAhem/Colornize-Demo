using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class PieceMove : ScriptableObject
    {
        public PieceMoveLimit Limit;

        public abstract List<Coordinate> Get(int x, int y, Coordinate size);

        public MoveContext GetMoveContext(int movesLeft, int currentPlayerID, int targetPlayerID) => Limit.GetMoveLimitContext(movesLeft, currentPlayerID, targetPlayerID);

        protected bool IsValid(int x, int y, Vector2 size)
        {
            return (x >= 0 && x <= size.x - 1) && (y >= 0 && y <= size.y - 1);
        }
    }
}