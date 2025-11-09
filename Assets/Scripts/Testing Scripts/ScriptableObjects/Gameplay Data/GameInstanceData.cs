using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameInstanceData", menuName = "SO/GameInstanceData")]
    public class GameInstanceData : ScriptableObject
    {
        [Header("FIXED CONSTANTS")]
        [SerializeField] private int _piecesPerPlayer;

        [Header("DO NOT CHANGE UNLESS TESTING")]
        //[SerializeField] private int _numberOfPlayers;

        [Header("Most Recent Clicked Cell")]
        [SerializeField][ReadOnly] private int _lastClickedRow;
        [SerializeField][ReadOnly] private int _lastClickedColumn;
        [SerializeField][ReadOnly] private Vector2 _boardDimensions;

        private List<Vector2[]> _playerPiecePositions;
        private CellStatus[][] _boardStatus;

        public void SetLastClickedCellInfo(int row, int column)
        {
            _lastClickedRow = row;
            _lastClickedColumn = column;
        }

        public Vector2 LastCellPositionClicked => new Vector2(_lastClickedRow, _lastClickedColumn);

        public Vector2 BoardDimensions => _boardDimensions;

        public List<Vector2[]> PlayerPiecePositions => _playerPiecePositions;

        public void ResetData()
        {
            //_numberOfPlayers = 0;

            _lastClickedRow = 0;
            _lastClickedColumn = 0;

            _boardStatus = null;
            _playerPiecePositions = null;
        }

        public void SetPlayerPiecePosition(int playerID, int pieceID, int row, int column)
        {
            SetPlayerPiecePosition(playerID, pieceID, new Vector2(row, column));
        }

        public void SetPlayerPiecePosition(int playerID, int pieceID, Vector2 position)
        {
            _playerPiecePositions[playerID][pieceID] = position;
        }

        public void GenerateNewBoard(int rows, int columns)
        {
            _boardStatus = new CellStatus[rows][];
            _boardDimensions = new Vector2(rows, columns);

            for (int row = 0; row < rows; row++)
            {
                _boardStatus[row] = new CellStatus[columns];
            }
        }

        public void InitializeBoardPosition(int row, int column, BoardCell cell)
        {
            _boardStatus[row][column] = new CellStatus(cell);
        }

        public CellStatus LastClickedCellStatus => _boardStatus[(int)LastCellPositionClicked.x][(int)LastCellPositionClicked.y];

        public CellStatus GetCellStatusAtPosition(int row, int column)
        {
            return _boardStatus[row][column];
        }

        public CellStatus GetCellStatusAtPosition(Vector2 coordinate)
        {
            return GetCellStatusAtPosition((int)coordinate.x, (int)coordinate.y);
        }

        public int PiecesPerPlayer => _piecesPerPlayer;

        public int NumberOfPlayers => 10;
    }
}