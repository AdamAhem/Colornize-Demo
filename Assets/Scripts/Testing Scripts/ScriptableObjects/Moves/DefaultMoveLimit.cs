using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Move Limits/Default")]
    public class DefaultMoveLimit : PieceMoveLimit
    {
        public override MoveContext GetMoveLimitContext(int movesLeft, int currentPlayerID, int targetPlayerID)
        {
            int cost;
            MoveType type;

            if (movesLeft > 1)
            {
                bool sameCellColor = currentPlayerID == targetPlayerID;

                cost = sameCellColor ? 2 : 1;
                type = sameCellColor ? MoveType.Move : MoveType.Both;
            }
            else
            {
                cost = 1;
                type = MoveType.Color;
            }

            return new MoveContext(cost, type);
        }
    }
}