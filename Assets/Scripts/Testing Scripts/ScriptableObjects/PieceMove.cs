using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class PieceMove : ScriptableObject
    {
        public abstract List<Vector2> Get(int x, int y, Vector2 size);

        public abstract MoveContext GetMoveContext(int movesLeft, int currentPlayerID, int targetPlayerID);

        protected bool IsValid(int x, int y, Vector2 size)
        {
            return (x >= 0 && x <= size.x - 1) && (y >= 0 && y <= size.y - 1);
        }
    }
}