using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Restricted Move Limit", menuName = "SO/Move Limits/Restricted")]
    public class RestrictedMoveLimit : PieceMoveLimit
    {
        public override MoveContext GetMoveLimitContext(int movesLeft, int currentPlayerID, int targetPlayerID)
        {
            return new MoveContext(movesLeft, MoveType.Both);
        }
    }
}
