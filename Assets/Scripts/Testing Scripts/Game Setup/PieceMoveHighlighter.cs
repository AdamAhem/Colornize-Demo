using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class PieceMoveHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PieceMoveDisplayer _moveDisplayer;
        [SerializeField] private TMP_Text _pieceNameText;
        [SerializeField] private int _pieceID;
        [SerializeField] private string _pieceName;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pieceNameText.text = _pieceName;
            _moveDisplayer.DisplayMoves(_pieceID);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pieceNameText.text = "";
            _moveDisplayer.HideMoves();
        }
    }
}