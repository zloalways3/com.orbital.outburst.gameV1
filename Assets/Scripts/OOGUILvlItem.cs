using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OOG
{
    public class OOGUILvlItem : MonoBehaviour
    {
        [SerializeField] private Button _OOGBtn;
        [SerializeField] private TMP_Text _OOGLvlText;
        [SerializeField] private Image[] _OOGStars;

        private int _oogLvl;
        
        private void Awake()
        {
            _OOGBtn.onClick.AddListener(OOGClick);
        }

        public void OOGInit(int oogLvl, bool oogpass)
        {
            foreach (var t in _OOGStars)
                t.enabled = false;
            
            _oogLvl = oogLvl;
            
            _OOGLvlText.text = oogLvl.ToString();

            _OOGBtn.interactable = oogpass;
            var newColor = _OOGLvlText.color;
            newColor.a = oogpass ? 1f : 0.5f;
            _OOGLvlText.color = newColor;
            
            var stars = PlayerPrefs.GetInt(OGGULTIMA.OOGStarsKey + oogLvl, 0);

            for (var i = 0; i < stars; i++)
                _OOGStars[i].enabled = true;
        }
        
        private void OOGClick()
        {
            PlayerPrefs.SetInt(OGGULTIMA.OOGCurLvlKey, _oogLvl);
            SceneManager.LoadScene(OGGULTIMA.OOGGamePlayer);
        }
    }
}