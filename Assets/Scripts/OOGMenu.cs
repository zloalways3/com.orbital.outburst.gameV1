using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OOG
{
    public class OOGMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _oogMenu;
        [SerializeField] private CanvasGroup _oogLvls;
        [SerializeField] private CanvasGroup _oogExit;

        [SerializeField] private Button _oogPlayBtn;
        [SerializeField] private Button _oogLvlBtn;
        [SerializeField] private Button _oogExitBtn;

        [SerializeField] private Button _oogQuetBtn;
        [SerializeField] private Button[] _oogBackBtn;

        private void Awake()
        {
            _oogMenu.OOGCAN(true);
            _oogLvls.OOGCAN(false);
            _oogExit.OOGCAN(false);

            _oogPlayBtn.onClick.AddListener(() => SceneManager.LoadScene(OGGULTIMA.OOGGamePlayer));
            _oogQuetBtn.onClick.AddListener(Application.Quit);

            _oogLvlBtn.onClick.AddListener(() =>
            {
                _oogMenu.OOGCAN(false);
                _oogLvls.OOGCAN(true);
            });
            
            _oogExitBtn.onClick.AddListener(() =>
            {
                _oogMenu.OOGCAN(false);
                _oogExit.OOGCAN(true);
            });

            foreach (var oogBack in _oogBackBtn)
            {
                oogBack.onClick.AddListener(() =>
                {
                    _oogMenu.OOGCAN(true);
                    _oogLvls.OOGCAN(false);
                    _oogExit.OOGCAN(false);
                });
            }
        }
    }
}