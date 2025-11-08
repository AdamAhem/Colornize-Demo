using System;

namespace Game
{
    [Serializable]
    public class PlacementData
    {
        private readonly int _pieceID;
        private readonly int _slotID;

        private Coordinate _position;
        private bool _isPlaced;

        public Coordinate Position => _position;
        public int PieceID => _pieceID;
        public int SlotID => _slotID;
        public bool IsPlaced => _isPlaced;

        public PlacementData(int pieceID, int slotID)
        {
            _position = default;
            _pieceID = pieceID;
            _slotID = slotID;
            _isPlaced = false;
        }

        public void SetPosition(int row, int column) => _position = new Coordinate(column, row);

        public void SetAsPlaced(bool isPlaced) => _isPlaced = isPlaced;
    }
}
