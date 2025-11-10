using System;

namespace Game
{
    [Serializable]
    public class PlacementData
    {
        private readonly int _pieceID;

        private Coordinate _position;
        private bool _isPlaced;

        public Coordinate Position => _position;
        public int PieceID => _pieceID;
        public bool IsPlaced => _isPlaced;

        public PlacementData(int pieceID)
        {
            _position = default;
            _pieceID = pieceID;
            _isPlaced = false;
        }

        public void ResetData()
        {
            _isPlaced = false;
            _position = default;
        }

        public void ResetPosition() => _position = default;

        public void SetPosition(Coordinate position) => _position = position;

        public void SetAsPlaced(bool isPlaced) => _isPlaced = isPlaced;
    }
}