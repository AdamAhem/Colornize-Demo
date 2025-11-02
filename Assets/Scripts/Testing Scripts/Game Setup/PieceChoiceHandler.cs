using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PieceChoiceHandler : MonoBehaviour
    {
        [Header("ID config")]
        [SerializeField] private int _pieceID;

        [Header("Sprite")]
        [SerializeField] private Image _pieceImage;

        [Header("Data")]
        [SerializeField] private SelectionStateData _selectionStateData;

        public void OnPressChoosePiece_UI_BUTTON()
        {
            _selectionStateData.ChoosePieceForSelectedPlayer(_pieceID, _pieceImage.sprite);
        }
    }
}