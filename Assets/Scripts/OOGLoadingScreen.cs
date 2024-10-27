using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace OOG
{
    public class OOGLoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _oogProgress;

        private Sequence _oogSeq;
        private void Awake()
        {
            _oogProgress.fillAmount = 0f;
        }

        private void Start()
        {
            _oogSeq = DOTween.Sequence();
            _oogSeq.AppendInterval(0.1f);
            _oogSeq.Append(_oogProgress.DOFillAmount(1f, Random.Range(0.4f, 1.2f)))
                .OnComplete(() => SceneManager.LoadScene(OGGULTIMA.OOGMENU));
        }
    }
}