using UnityEngine;

namespace Game
{
    public class BoardBuilder : MonoBehaviour
    {
        [SerializeField] private BoardCell _boardCellObject;
        [SerializeField] private GameObject _boardParent;
        [SerializeField][Min(3)] private int _rows;
        [SerializeField][Min(3)] private int _columns;
        [SerializeField] private Transform _corner1;
        [SerializeField] private Transform _corner2;
        [SerializeField] private float _cellScale;

        [SerializeField] private GameInstanceData _instanceData;

        public void BuildBoard()
        {
            _instanceData.GenerateNewBoard(_rows, _columns);

            Vector2 corner1Pos = _corner1.position;
            Vector2 corner2Pos = _corner2.position;

            float dx = (corner2Pos.x - corner1Pos.x) / (_columns - 1);
            float dy = (corner2Pos.y - corner1Pos.y) / (_rows - 1);

            for (int i = 0; i < _rows * _columns; i++)
            {
                int currentRow = i / _rows;
                int currentCol = i % _columns;

                float xPos = corner1Pos.x + currentCol * dx;
                float yPos = corner1Pos.y + currentRow * dy;

                Vector2 spawnPoint = new(xPos, yPos);
                BoardCell newCell = Instantiate(_boardCellObject, spawnPoint, Quaternion.identity, _boardParent.transform);
                newCell.transform.localScale *= _cellScale;

                newCell.SetRowAndColumn(currentRow, currentCol);

                newCell.SetGameData(_instanceData);
                _instanceData.InitializeBoardPosition(currentRow, currentCol, newCell);
            }
        }
    }
}