using System;

namespace Game
{
    [Serializable]
    public class PlayerGameplayData
    {
        private int _score;
        private PiecePosition[] _piecePositions;

        public int Score => _score;
        public PiecePosition[] PiecePositions => _piecePositions;

        public PlayerGameplayData(int initialScore, int positions)
        {
            _score = initialScore;
            _piecePositions = new PiecePosition[positions];
        }

        public void UpdatePiecePosition(int index, Coordinate newPosition)
        {
            var pieceID = _piecePositions[index].PieceID;
            _piecePositions[index] = new PiecePosition(pieceID, newPosition);
        }

        public void SetPiecePositionAtIndex(int pieceID, Coordinate position, int index) => _piecePositions[index] = new PiecePosition(pieceID, position);

        public void SetScore(int score) => _score = score;

        public void AddPoint(int points) => _score += points;
    }

    public struct PiecePosition
    {
        public int PieceID;
        public Coordinate Position;

        public PiecePosition(int pieceID, Coordinate position)
        {
            PieceID = pieceID;
            Position = position;
        }
    }
}