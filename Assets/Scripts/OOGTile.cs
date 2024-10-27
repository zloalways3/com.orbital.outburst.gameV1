using UnityEngine;

namespace OOG
{
    public class OOGTile : MonoBehaviour
    {
        [field: SerializeField] public bool OGGISBAD { get; private set; }
        [field: SerializeField] public int OGGPoints { get; private set; }

        private SpriteRenderer _oggSpriteRenderer;
        public SpriteRenderer OGGSpriteRenderer => _oggSpriteRenderer ??= GetComponent<SpriteRenderer>();

        public float OGGSpeed { get; set; }
        public OOGManager OOGManager { get; set; }

        private bool _isUsed;

        private void Update()
        {
            if (OGGULTIMA.OOGPAUSE)
                return;
            
            if (_isUsed)
                return;

            transform.position += Vector3.down * OGGSpeed * Time.deltaTime;

            if (transform.position.y >= -50f)
                return;

            OGGDEstroy();
        }

        private void OnMouseDown()
        {
            if (OGGULTIMA.OOGPAUSE)
                return;
            
            if (!OGGISBAD)
                OOGManager.OggOnTouchik(OGGSpriteRenderer.sprite, OGGPoints, transform.position);
            else
                OOGManager.OggOnTouchik(OGGSpriteRenderer.sprite, -OGGPoints, transform.position);

            OGGDEstroy();
        }

        private void OGGDEstroy()
        {
            Destroy(gameObject);

            _isUsed = true;
        }
    }
}