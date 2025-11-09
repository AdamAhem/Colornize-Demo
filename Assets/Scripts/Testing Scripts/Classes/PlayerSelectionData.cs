using System;
using System.Linq;

namespace Game
{
    [Serializable]
    public class PlayerSelectionData
    {
        public int PlayerID;
        public int[] PieceIDs;
        private bool[] _piecesSetStaus;

        private readonly int _pieceCount;

        public PlayerSelectionData(int playerID, int pieceCount)
        {
            PlayerID = playerID;
            PieceIDs = new int[pieceCount];
            _piecesSetStaus = new bool[pieceCount];

            _pieceCount = pieceCount;
        }

        public void ResetPieces()
        {
            PieceIDs = new int[_pieceCount];
            _piecesSetStaus = new bool[_pieceCount];
        }

        public void UpdateSlotWithPiece(int slotID, int pieceID)
        {
            PieceIDs[slotID] = pieceID;
            _piecesSetStaus[slotID] = true;
        }

        public bool IsReady()
        {
            return _piecesSetStaus.All(status => status is true);
        }

        public int GetPieceID(int slotID) => PieceIDs[slotID];
    }
}