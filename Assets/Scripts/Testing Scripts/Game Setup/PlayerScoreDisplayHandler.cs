using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerScoreDisplayHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _playerText;
        [SerializeField] private TMP_Text _scoreText;

        [SerializeField] private Image _backgroundImage;

        [Header("Data")]
        [SerializeField] private int _playerID;
        [SerializeField] private Colors _colors;

        [Header("Coloring")]
        [SerializeField] private Color _defaultTextColor;
        [SerializeField] private Color _defaultBgColor;
        [SerializeField][Range(0, 1)] private float _backgroundDim;

        private int _score;

        public void ResetScore()
        {
            _score = 0;
            _scoreText.text = "00";
        }

        public void Enable()
        {
            Color activeColor = _colors.List[_playerID];
            _playerText.color = activeColor;
            _scoreText.color = activeColor;

            _backgroundImage.color = activeColor * (1 - _backgroundDim) + (Color.black * _backgroundDim);
        }

        public void Disable()
        {
            _playerText.color = _defaultTextColor;
            _scoreText.color = _defaultTextColor;

            _backgroundImage.color = _defaultBgColor;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void AddPoint()
        {
            _score++;

            int tens = _score < 10 ? 0 : _score / 10;
            int ones = _score % 10;
            _scoreText.text = $"{tens}{ones}";
        }
    }
}