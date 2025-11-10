using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Board Status", menuName = "SO/Board Status")]
    public class BoardStatus : ScriptableObject
    {
        [Header("Visualisation")]
        [SerializeField][ReadOnly] private CellStatus[][] _boardStatus;
        [SerializeField][ReadOnly] private Coordinate _boardDimensions;

        public CellStatus[][] Status => _boardStatus;
        public Coordinate Dimensions => _boardDimensions;

        public void InitializeBoardCellsStatus(int rows, int columns)
        {
            _boardStatus = new CellStatus[rows][];
            _boardDimensions = new Coordinate(rows, columns);

            for (int row = 0; row < rows; row++)
            {
                _boardStatus[row] = new CellStatus[columns];
            }
        }

        public void SetCellWithBoardCellObject(int row, int column, BoardCell cell)
        {
            _boardStatus[row][column] = new CellStatus(cell);
        }

        public CellStatus GetCellStatusAtPosition(Coordinate position) => _boardStatus[position.X][position.Y];

        public Coordinate GetNextFreePosition()
        {
            for (int i = 0; i < _boardStatus.Length; i++)
            {
                for (int j = 0; j < _boardStatus[i].Length; j++)
                {
                    if (!_boardStatus[i][j].IsOccupied) return _boardStatus[i][j].Cell.Position;
                }
            }
            Debug.LogWarning("ALL CELLS ARE OCCUPIED. THIS SHOULDN'T HAPPEN.");
            return default;
        }
    }
}