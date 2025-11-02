using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PieceMoveDisplayer : MonoBehaviour
    {
        [SerializeField] private Image _displayImage;

        [SerializeField] private Sprite[] _moveSprites;

        public void DisplayMoves(int pieceID)
        {
            _displayImage.sprite = _moveSprites[pieceID];
        }

        public void HideMoves()
        {
            _displayImage.sprite = null;
        }
    }
}