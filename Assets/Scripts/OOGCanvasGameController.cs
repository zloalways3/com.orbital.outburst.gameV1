using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OOG
{
    public class OOGCanvasGameController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _oogGame;
        [SerializeField] private CanvasGroup _oogPause;
        [SerializeField] private CanvasGroup _oogVictory;
        [SerializeField] private CanvasGroup _oogLose;

        [SerializeField] private Button _oogPauseBtn;
        [SerializeField] private Button _oogUnPauseBtn;

        [SerializeField] private Button[] _oogHome;
        [SerializeField] private Button[] _oogRestart;
        [SerializeField] private Button _oogNext;

        [SerializeField] private TMP_Text[] _oogLvlText;
        [SerializeField] private TMP_Text[] _oogScoreText;
        
        [SerializeField] private Image[] _oogStars;

        [SerializeField] private AudioClip _oogLoseSnd;
        [SerializeField] private AudioClip _oogWinSnd;
        [SerializeField] private AudioClip _oogStarSnd;
        
        public AudioSource OOGSource { get; set; }

        private void Awake()
        {
            _oogGame.OOGCAN(true);
            _oogPause.OOGCAN(false);
            _oogVictory.OOGCAN(false);
            _oogLose.OOGCAN(false);
            
            _oogPauseBtn.onClick.AddListener(()=>OogPause(true));
            _oogUnPauseBtn.onClick.AddListener(()=>OogPause(false));

            foreach (var ooghome in _oogHome)
                ooghome.onClick.AddListener(()=>SceneManager.LoadScene(OGGULTIMA.OOGMENU));
            
            foreach (var oogRestart in _oogRestart)
                oogRestart.onClick.AddListener(()=>SceneManager.LoadScene(OGGULTIMA.OOGGamePlayer));
            
            _oogNext.onClick.AddListener(OOGNextLvl);
        }

        private void OOGNextLvl()
        {
            var lvl = PlayerPrefs.GetInt(OGGULTIMA.OOGCurLvlKey, 1);
            PlayerPrefs.SetInt(OGGULTIMA.OOGCurLvlKey, lvl + 1);

            SceneManager.LoadScene(OGGULTIMA.OOGGamePlayer);
        }

        private void OogPause(bool oog)
        {
            OGGULTIMA.OOGPAUSE = oog;
            
            _oogPause.OOGCAN(oog);
        }

        private Sequence _oogSeq;
        
        public void OOGVictory(int oogScore, int oogLvl, int oogStars)
        {
            OGGULTIMA.OOGPAUSE = true;

            OOGSource.PlayOneShot(_oogWinSnd);
            
            var pass = PlayerPrefs.GetInt(OGGULTIMA.OOGPassLvlKey, 1);
            
            if (pass == oogLvl)
                PlayerPrefs.SetInt(OGGULTIMA.OOGPassLvlKey, pass + 1);
            
            var stars = PlayerPrefs.GetInt(OGGULTIMA.OOGStarsKey + oogLvl, 0);
            
            if (stars < oogStars)
                PlayerPrefs.SetInt(OGGULTIMA.OOGStarsKey + oogLvl, oogStars);
            
            _oogGame.OOGCAN(false);
            _oogVictory.OOGCAN(true);

            foreach (var text in _oogLvlText)
                text.text = $"LEVEL {oogLvl} COMPLETED!";
            
            foreach (var text in _oogScoreText)
                text.text = $"SCORE {oogScore}";
            
            _oogSeq?.Kill();
            _oogSeq = DOTween.Sequence();

            foreach (var oogStar in _oogStars)
                oogStar.rectTransform.DOScale(Vector3.zero, 0f);
            
            _oogSeq.AppendInterval(0.24f);

            for (var i = 0; i < oogStars; i++)
            {
                var star = _oogStars[i];

                var endRotation = star.rectTransform.rotation.eulerAngles;
                endRotation.z += 360f;

                _oogSeq.Append(star.rectTransform.DOScale(Vector3.one, 0.25f))
                    .AppendCallback(()=> OOGSource.PlayOneShot(_oogStarSnd))
                    .Append(star.rectTransform.DOPunchScale(Vector3.one * 1.05f, 0.1f).SetEase(Ease.Flash));
            }
            
        }
        
        public void OogLose(int oogScore, int oogLvl)
        {
            OGGULTIMA.OOGPAUSE = true;
            
            OOGSource.PlayOneShot(_oogLoseSnd);
            
            _oogGame.OOGCAN(false);
            _oogLose.OOGCAN(true);
            
            foreach (var text in _oogLvlText)
                text.text = $"OOPS... LEVEL {oogLvl} FAILED!";
            
            foreach (var text in _oogScoreText)
                text.text = $"SCORE {oogScore}";
        }
    }
}