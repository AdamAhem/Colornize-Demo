using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class PieceMoveHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PieceMoveDisplayer _moveDisplayer;
        [SerializeField] private int _pieceID;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _moveDisplayer.DisplayMoves(_pieceID);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _moveDisplayer.HideMoves();
        }
    }
}