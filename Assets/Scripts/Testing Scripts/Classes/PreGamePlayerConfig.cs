using System;
using System.Linq;

namespace Game
{
    [Serializable]
    public class PreGamePlayerConfig
    {
        public PreGamePlayerConfig(int ID, int pieceCount)
        {
            PlayerID = ID;
            PieceIDs = new int[pieceCount];
            _piecesSetStaus = new bool[pieceCount];
        }

        public void SetPieceID(int index, int ID)
        {
            PieceIDs[index] = ID;
            _piecesSetStaus[index] = true;
        }

        public bool IsReady()
        {
            return _piecesSetStaus.All(status => status is true);
        }

        public int GetPieceID(int slotID) => PieceIDs[slotID];

        public int PlayerID;
        public int[] PieceIDs;

        private bool[] _piecesSetStaus;
    }
}