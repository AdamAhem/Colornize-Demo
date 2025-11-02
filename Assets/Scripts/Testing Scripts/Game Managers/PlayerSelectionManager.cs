using UnityEngine;

namespace Game
{
    public class PlayerSelectionManager : MonoBehaviour
    {
        [SerializeField] private PlayerCountSelectorHandler _playerCountHandler;

        public void Initialize()
        {
            _playerCountHandler.Initialize();
        }
    }
}