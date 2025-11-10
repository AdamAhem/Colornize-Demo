using UnityEngine;

namespace Game
{
    public class Quitter : MonoBehaviour
    {
        //enable obj
        [SerializeField] private GameObject _quitterObject;

        public void OnPressQuit_UI_BUTTON()
        {
            _quitterObject.SetActive(true);
        }

        public void OnPressYes_UI_BUTTON()
        {
            Application.Quit();
        }

        public void OnPressNoUI_BUTTON()
        {
            _quitterObject.SetActive(false);
        }
    }
}