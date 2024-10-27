using UnityEngine;
using UnityEngine.UI;

namespace OOG
{
    public class OOGSettings : MonoBehaviour
    {
        private const string OOGMscKey = "OOGMsc";
        private const string OOGMscVolumeKey = "OOGMscVolume";
        private const string OOGSndKey = "OOGSnd";
        private const string OOGSndVolumeKey = "OOGSndVolume";

        [SerializeField] private CanvasGroup _oggCG;
        [SerializeField] private Button _oogOpen;
        [SerializeField] private Button _oogClose;

        [SerializeField] private Button _oogMscBtn;
        [SerializeField] private Button _oogSndBtn;

        [SerializeField] private Image _oogmscImage;
        [SerializeField] private Image _oogsndImage;

        /// <summary>
        /// 0 - on
        /// 1 - off
        /// </summary>
        [SerializeField] private Sprite[] _oogState;

        [SerializeField] private Slider _oogMscSlider;
        [SerializeField] private Slider _oogSndSlider;

        [SerializeField] private AudioSource _oogMsc;
        [SerializeField] private AudioSource _oogSnd;

        [SerializeField] private AudioClip _oogClick;
        
        private void Awake()
        {
            Application.targetFrameRate = 120;

            _oggCG.OOGCAN(false);

            _oogOpen.onClick.AddListener(() =>
            {
                OGGULTIMA.OOGPAUSE = true;
                _oggCG.OOGCAN(true);
            });

            _oogClose.onClick.AddListener(() =>
                {
                    OGGULTIMA.OOGPAUSE = false;
                    _oggCG.OOGCAN(false);
                }
            );

            OOGMscLogic();
            OOGSndLogic();
        }

        private void OOGMscLogic()
        {
            OOGLoad();

            _oogMscBtn.onClick.AddListener(OOGClick);

            _oogMscSlider.onValueChanged.AddListener(OOGVolume);

            return;

            void OOGClick()
            {
                var mute = PlayerPrefs.GetInt(OOGMscKey, 1);
                mute *= -1;

                _oogMsc.mute = mute < 0;
                _oogmscImage.sprite = _oogState[mute > 0 ? 0 : 1];

                PlayerPrefs.SetInt(OOGMscKey, mute);
            }

            void OOGVolume(float oog)
            {
                PlayerPrefs.SetFloat(OOGMscVolumeKey, oog);
                _oogMsc.volume = oog;
            }

            void OOGLoad()
            {
                var lastSave = PlayerPrefs.GetInt(OOGMscKey, 1);
                var lastSaveVolume = PlayerPrefs.GetFloat(OOGMscVolumeKey, 1f);

                _oogmscImage.sprite = lastSave > 0 ? _oogState[0] : _oogState[1];
                _oogMscSlider.value = lastSaveVolume;

                _oogMsc.mute = lastSave < 0;
                _oogMsc.volume = lastSaveVolume;
            }
        }

        private void OOGSndLogic()
        {
            OOGLoad();

            _oogSndBtn.onClick.AddListener(OOGClick);

            _oogSndSlider.onValueChanged.AddListener(OOGVolume);

            return;

            void OOGClick()
            {
                var mute = PlayerPrefs.GetInt(OOGSndKey, 1);
                mute *= -1;

                _oogSnd.mute = mute < 0;
                _oogsndImage.sprite = _oogState[mute > 0 ? 0 : 1];

                PlayerPrefs.SetInt(OOGSndKey, mute);
            }

            void OOGVolume(float oog)
            {
                PlayerPrefs.SetFloat(OOGSndVolumeKey, oog);
                _oogSnd.volume = oog;
            }

            void OOGLoad()
            {
                var lastSave = PlayerPrefs.GetInt(OOGSndKey, 1);
                var lastSaveVolume = PlayerPrefs.GetFloat(OOGSndVolumeKey, 1f);

                _oogsndImage.sprite = lastSave > 0 ? _oogState[0] : _oogState[1];
                _oogSndSlider.value = lastSaveVolume;

                _oogSnd.mute = lastSave < 0;
                _oogSnd.volume = lastSaveVolume;
            }
        }

        public void OGGSndClick() => _oogSnd.PlayOneShot(_oogClick);
    }
}