using UnityEngine;

namespace Game
{
    public class PieceChoiceHandler : MonoBehaviour
    {
        [Header("ID config")]
        [SerializeField] private int _pieceID;

        [Header("Data")]
        [SerializeField] private SelectionStateData _selectionStateData;

        [Header("Events")]
        [SerializeField] private GameEvent _choosePieceEvent;

        public void OnPressChoosePiece_UI_BUTTON()
        {
            _selectionStateData.SetPieceID(_pieceID);
            _choosePieceEvent.Raise();
        }
    }
}