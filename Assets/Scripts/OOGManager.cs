using System.Collections.Generic;
using System.Linq;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace OOG
{
    public static class OGGULTIMA
    {
        public static readonly string OOGMENU = "Menu";
        public static readonly string OOGGamePlayer = "Gameplay";
        
        public static readonly string OOGStarsKey = "OOGStars";
        public static readonly string OOGCurLvlKey = "OOGLvl";
        public static readonly string OOGPassLvlKey = "OOGLvlPass";
        public static readonly string OOGtargetsLvlKey = "OGGTArgets";
        
        public static bool OOGPAUSE;
#if UNITY_EDITOR
        [MenuItem("Tools/Clear prefs")]
        public static void OOGClearPrefs() =>
            PlayerPrefs.DeleteAll();
#endif

        public static void OOGCAN(this CanvasGroup canvasGroup, bool oog)
        {
            canvasGroup.alpha = oog ? 1f : 0f;
            canvasGroup.interactable = oog;
            canvasGroup.blocksRaycasts = oog;
        }
    }
    
    public class OOGManager : MonoBehaviour
    {
        private const float OGG_TOTAL_TIME = 90f;
        
        [SerializeField] private OOGTile[] _OOGTilesPrefabs;
        [SerializeField] private float[] _OGGspawnChances;
        [SerializeField] private AnimationCurve _OGGCurved;
        [SerializeField] private OGGDepthSettings[] _oggDepthSettings;
        [SerializeField] private OGGPointsManager _oggPointsManager;

        [SerializeField] private OOGTileTargetUI[] _oogTargets;

        [SerializeField] private Image _oggProgress;
        [SerializeField] private Image[] _oggStars;
        
        [SerializeField] private TMP_Text _OGGLvlIndex;
        [SerializeField] private TMP_Text _OGGTimer;
        [SerializeField] private TMP_Text _OGGScore;

        [SerializeField] private AudioSource _oogSndSource;
        [SerializeField] private AudioClip _oogHit;
        [SerializeField] private AudioClip _oogBoom;
        
        private int _OGGPoints;
        private int _OGGLvl;

        private int _OggTargetPointsOnLvl;
        
        private float _OGGTimerValue;
        
        private float _oggElapsedTime = 0f;

        [SerializeField] private OOGCanvasGameController _OOGui;
        [SerializeField] private OOGRipple _oogRippleEffect;

        private void Awake()
        {
            OGGULTIMA.OOGPAUSE = false;

            _OOGui.OOGSource = _oogSndSource;
            
            _OGGLvl = PlayerPrefs.GetInt(OGGULTIMA.OOGCurLvlKey, 1);
            _OGGLvlIndex.text = $"Level\n{_OGGLvl}";
            
            _OggTargetPointsOnLvl = PlayerPrefs.GetInt($"{OGGULTIMA.OOGtargetsLvlKey}{_OGGLvl}", Random.Range(5000, 12000));
            
            _OGGTimerValue = OGG_TOTAL_TIME;
            _OGGTimer.text = $"Time: {(int) _OGGTimerValue}s";
            _OGGScore.text = $"Score: {_OGGPoints}";

            var oggTargetHash = new HashSet<OOGTile>();

            foreach (var oogTileTargetUI in _oogTargets)
            {
                var rand = _OOGTilesPrefabs[Random.Range(0, _OOGTilesPrefabs.Length)];
                while (oggTargetHash.Contains(rand))
                    rand = _OOGTilesPrefabs[Random.Range(0, _OOGTilesPrefabs.Length)];
                
                oggTargetHash.Add(rand);

                if (rand.OGGISBAD)
                    continue;
                
                var inst = Instantiate(rand, Vector3.one * 100f, Quaternion.identity);
                
                oogTileTargetUI.OggINIT(inst.OGGSpriteRenderer.sprite);
                
                Destroy(inst.gameObject);
            }

            OOGCalculateProgress();
        }

        private void Update()
        {
            if (OGGULTIMA.OOGPAUSE)
                return;
            
            _oggElapsedTime += Time.deltaTime;
            _OGGTimerValue -= Time.deltaTime;
            
            _OGGTimer.text = $"Time: {((int) _OGGTimerValue).ToString()}s";

            if (_OGGTimerValue <= 0f)
            {
                _OOGui.OogLose(_OGGPoints, _OGGLvl);
                return;
            }
            
            var spawnInterval = 1f / _OGGCurved.Evaluate(_OGGTimerValue / OGG_TOTAL_TIME);

            if (_oggElapsedTime < spawnInterval) 
                return;
            
            OGGSpawnObject();
            _oggElapsedTime = 0f;
        }

        public void OggOnTouchik(Sprite sprite, int points, Vector3 oogPosition)
        {
            _OGGPoints = Mathf.Clamp(_OGGPoints + points, 0, int.MaxValue);
            _OGGScore.text = $"Score: {_OGGPoints}";
            
            if (points < 0)
                _oogRippleEffect.RippleEffect(Input.mousePosition);
            
            foreach (var oogTileTargetUI in _oogTargets)
            {
                if (oogTileTargetUI.OggTryDis(sprite))
                    break;
            }

            _oogSndSource.PlayOneShot(points > 0 ? _oogHit : _oogBoom);
            
            OOGCalculateProgress();
            OOGChekWin();
        }

        private void OOGCalculateProgress()
        {
            var percent = _OGGPoints / (float) _OggTargetPointsOnLvl;

            _oggProgress.fillAmount = percent;
            
            _oggStars[0].enabled = percent >= 0.2f;
            _oggStars[1].enabled = percent >= 0.63f;
            _oggStars[2].enabled = percent >= 0.95f;
        }
        
        private void OOGChekWin()
        {
            if (_oogTargets.Any(oogTileTargetUI => !oogTileTargetUI.OggIsFull))
                return;

            var strsCount = 0;

            if (_oggProgress.fillAmount >= 0.95f)
                strsCount = 3;
            else if (_oggProgress.fillAmount >= 0.63f)
                strsCount = 2;
            else if (_oggProgress.fillAmount >= 0.2f)
                strsCount = 1;
            
            _OOGui.OOGVictory(_OGGPoints, _OGGLvl, strsCount);
        }

        private void OGGSpawnObject()
        {
            var totalChance = _OGGspawnChances.Sum();

            var randomValue = Random.Range(0f, totalChance);
            var cumulativeChance = 0f;

            for (var i = 0; i < _OOGTilesPrefabs.Length; i++)
            {
                cumulativeChance += _OGGspawnChances[i];
                if (!(randomValue <= cumulativeChance)) 
                    continue;
                
                var oggTile = Instantiate(_OOGTilesPrefabs[i], _oggPointsManager.OGGRandomPoint, Quaternion.identity);

                oggTile.OOGManager = this;
                
                var randDepth = _oggDepthSettings[Random.Range(0, _oggDepthSettings.Length)];
                
                oggTile.transform.localScale = Vector3.one * randDepth.OGGSize;
                oggTile.OGGSpriteRenderer.sortingOrder = randDepth.OGGSortingLayer;
                
                var oggrilePos = oggTile.transform.position;
                oggrilePos.z += randDepth.OGGZoffset;
                oggTile.transform.position = oggrilePos;
                
                oggTile.OGGSpeed = Random.Range(randDepth.OGGSpeedRange.x, randDepth.OGGSpeedRange.y);
                
                break;
            }
        }
    }
}