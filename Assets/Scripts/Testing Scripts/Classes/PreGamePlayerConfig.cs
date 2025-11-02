using System;

namespace Game
{
    [Serializable]
    public class PreGamePlayerConfig
    {
        public PreGamePlayerConfig(int ID, int pieceCount)
        {
            PlayerID = ID;
            PieceIDs = new int[pieceCount];
        }

        public void SetPieceID(int index, int ID)
        {
            PieceIDs[index] = ID;
        }

        public int PlayerID;
        public int[] PieceIDs;
    }
}