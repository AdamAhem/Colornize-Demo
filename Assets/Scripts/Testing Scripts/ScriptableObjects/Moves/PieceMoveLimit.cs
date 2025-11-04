using UnityEngine;

namespace Game
{
    public abstract class PieceMoveLimit : ScriptableObject
    {
        public abstract MoveContext GetMoveLimitContext(int movesLeft, int currentPlayerID, int targetPlayerID);
    }
}