using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OOG
{
    public class OOGTileTargetUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _oggCG;
        [SerializeField] private Image _oggImage;
        [SerializeField] private TMP_Text _oggText;

        private int _oggCount;
        
        public bool OggIsFull => _oggCount == 0;

        public void OggINIT(Sprite oggIcon)
        {
            _oggCount = Random.Range(3, 11);
            _oggText.text = _oggCount.ToString();
            _oggImage.sprite = oggIcon;
        }

        public bool OggTryDis(Sprite oggIcon)
        {
            if (oggIcon != _oggImage.sprite)
                return false;

            if (_oggCount == 0)
                return true;

            _oggCount--;
            _oggText.text = _oggCount.ToString();

            if (_oggCount == 0)
                _oggCG.alpha /= 2f;
            
            return true;
        }
    }
}