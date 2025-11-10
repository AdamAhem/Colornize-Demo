using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Board Status", menuName = "SO/Board Status")]
    public class BoardStatus : ScriptableObject
    {
        [Header("Visualisation")]
        [SerializeField][ReadOnly] private CellStatus[][] _boardStatus;
        [SerializeField][ReadOnly] private Coordinate _boardDimensions;

        [Header("Data")]
        [SerializeField] private PieceCatalog _catalog;

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

        public bool IsValidMove(int pieceID, Coordinate clickedPosition, Coordinate centerPosition)
        {
            PieceInfo info = _catalog.Get(pieceID);
            var possibleMoves = info.GetPossibleMoves(centerPosition, _boardDimensions);
            return possibleMoves.Contains(clickedPosition);
        }

        public void HighlightPieceMove(int playerID, int pieceID, Coordinate position)
        {
            // get all possible moves and only set the highlight icon of moves of UNOCCUPIED CELLS OR SAME-COLORED CELLS.
            var possibleMoves = _catalog.Get(pieceID).GetPossibleMoves(position, _boardDimensions);

            for (int i = 0; i < possibleMoves.Count; i++)
            {
                Coordinate potentialMove = possibleMoves[i];

                CellStatus status = GetCellStatusAtPosition(potentialMove);

                int playerIDAtMovePosition = status.PlayerID;
                bool canPlace = !status.IsColored || (playerIDAtMovePosition == playerID && !status.IsOccupied);
                if (canPlace) status.Cell.SetHighlightIconAsPossibleMove(playerID);
            }
        }

        public void UnhighlightPieceMove(int playerID, int pieceID, Coordinate position)
        {
            // get all possible moves and only set the highlight icon of moves of UNOCCUPIED CELLS OR SAME-COLORED CELLS.
            var possibleMoves = _catalog.Get(pieceID).GetPossibleMoves(position, _boardDimensions);

            for (int i = 0; i < possibleMoves.Count; i++)
            {
                Coordinate potentialMove = possibleMoves[i];

                CellStatus status = GetCellStatusAtPosition(potentialMove);

                int playerIDAtMovePosition = status.PlayerID;
                bool canPlace = !status.IsColored || (playerIDAtMovePosition == playerID && !status.IsOccupied);
                if (canPlace) status.Cell.ClearHighlightIcon();
            }
        }

        public void ClearBoard()
        {
            for (int col = 0; col < _boardStatus.Length; col++)
            {
                var cellColumn = _boardStatus[col];
                for (int row = 0; row < cellColumn.Length; row++)
                {
                    CellStatus status = cellColumn[row];
                    status.ResetIDs();

                    BoardCell cell = status.Cell;
                    cell.ClearCell();
                }
            }
        }
    }
}