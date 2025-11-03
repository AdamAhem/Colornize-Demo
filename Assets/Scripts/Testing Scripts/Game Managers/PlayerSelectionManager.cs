using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerSelectionManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerCountSelectorHandler _playerCountHandler;
        [SerializeField] private Button[] _buttonsToDisableWhileChoosingPiece;
        [SerializeField] private PlayerPieceSelectionHandler[] _pieceSelectionHandlers;
        [SerializeField] private TMP_Text _pieceNameText;
        [SerializeField] private Button _playButton;
        [SerializeField] private CanvasGroup _mainCanvasGroup;

        [Header("Events")]
        [SerializeField] private GameEvent _playerAddPieceEvent;
        [SerializeField] private GameEvent _playerChoosePieceEvent;

        [Header("Data")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private SelectionStateData _selectionStateData;

        public void Initialize()
        {
            _playerCountHandler.Initialize();
            _pieceNameText.text = "";
            _playButton.interactable = false;
        }

        public void Enable()
        {
            Debug.Log("Player Selection Manager Enabled");

            _playerAddPieceEvent.AddEvent(OnPressAddPiece);
            _playerChoosePieceEvent.AddEvent(OnPressChoosePiece);
        }

        public void Disable()
        {
            Debug.Log("Player Selection Manager Disabled");

            _playerAddPieceEvent.RemoveEvent(OnPressAddPiece);
            _playerChoosePieceEvent.RemoveEvent(OnPressChoosePiece);
        }

        public void OnIncreasePlayerCount_UI_BUTTON()
        {
            CheckIfCanPlay();
        }

        public void OnDecreasePlayerCount_UI_BUTTON()
        {
            CheckIfCanPlay();
        }

        public void FadeOut(float seconds)
        {
            _mainCanvasGroup.interactable = false;
            StartCoroutine(FadeOutCanvasGroup(seconds));
        }

        private void OnPressAddPiece()
        {
            // disable increment buttons and play button
            for (int i = 0; i < _buttonsToDisableWhileChoosingPiece.Length; i++)
            {
                _buttonsToDisableWhileChoosingPiece[i].interactable = false;
            }
        }

        private void OnPressChoosePiece()
        {
            int playerID = _selectionStateData.PlayerID;
            int slotID = _selectionStateData.SlotID;
            int pieceID = _selectionStateData.PieceID;
            Sprite pieceSprite = _selectionStateData.PieceSprite;

            // here, need to reset the BUTTON COLOR of the player slot that was selected.
            _pieceSelectionHandlers[playerID].OnPieceChosen(slotID, pieceSprite);

            //Debug.Log($"Player {playerID + 1} has added piece no.{pieceID + 1} to slot {slotID + 1}.");

            _gameData.UpdatePlayerConfig(playerID, slotID, pieceID);

            for (int i = 0; i < _buttonsToDisableWhileChoosingPiece.Length; i++)
            {
                _buttonsToDisableWhileChoosingPiece[i].interactable = true;
            }

            CheckIfCanPlay();
        }

        private void CheckIfCanPlay()
        {
            bool playersReady = _gameData.PlayerSlotsAreFilled();
            _playButton.interactable = playersReady;
        }

        private IEnumerator FadeOutCanvasGroup(float duration)
        {
            float elapsedTime = 0;
            float durationReciprocal = 1f / duration;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime * durationReciprocal;
                _mainCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime);
                yield return null;
            }

            _mainCanvasGroup.alpha = 0;
        }
    }
}