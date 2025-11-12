using TMPro;
using UnityEngine;

namespace Game
{
    public class WinnerMessageManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject _screen;
        [SerializeField] private TMP_Text _text;

        [Header("Data")]
        [SerializeField] private Colors _colors;

        public void DisplayWinners(int[] winnersIndices)
        {
            if (winnersIndices == null || winnersIndices.Length == 0)
            {
                Debug.LogWarning("no winners.");
                return;
            }

            _screen.SetActive(true);

            int winnerCount = winnersIndices.Length;

            if (winnerCount == 1)
            {
                int id = winnersIndices[0];
                int playerNumber = id + 1;

                _text.text = $"Player {playerNumber} won!";

                _text.color = _colors.List[id];
                SetColorGradient(Color.white, Color.white);
            }
            else if (winnerCount == 2)
            {
                int player1ID = winnersIndices[0];
                int player2ID = winnersIndices[1];

                _text.text = $"Players {player1ID + 1} and {player2ID + 1} tied!";

                _text.color = Color.white;
                SetColorGradient(_colors.List[player1ID], _colors.List[player2ID]);
            }
            else
            {
                int lastPlayer = winnersIndices[winnersIndices.Length - 1] + 1;
                int[] playerNumbers = new int[lastPlayer - 1];
                for (int i = 0; i < playerNumbers.Length; i++) playerNumbers[i] = winnersIndices[i] + 1;

                _text.text = $"{winnerCount}-way tie between\nplayers {string.Join(", ", playerNumbers)} and {lastPlayer}!";

                _text.color = Color.white;
                SetColorGradient(Color.white, Color.white);
            }
        }

        public void Hide()
        {
            if (_screen == null) return; //sometimes gets destroyed when exiting playmode (WHY DOES THIS KEEP HAPPENING???)
            _screen.SetActive(false);
        }

        private void SetColorGradient(Color top, Color bottom)
        {
            VertexGradient gradient = _text.colorGradient;

            gradient.topLeft = top;
            gradient.topRight = top;
            gradient.bottomLeft = bottom;
            gradient.bottomRight = bottom;

            _text.colorGradient = gradient;
        }
    }
}