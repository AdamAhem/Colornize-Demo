namespace Game
{
    public struct MoveContext
    {
        public int Cost;
        public MoveType Type;

        public MoveContext(int cost, MoveType type)
        {
            Cost = cost;
            Type = type;
        }
    }
}