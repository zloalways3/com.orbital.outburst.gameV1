using UnityEngine;

namespace OOG
{
    public class OGGPointsManager : MonoBehaviour
    {
        [SerializeField] private Vector2 _OGGoffset = new Vector2(0.5f, 0f);

        private Vector3[] _oggpoints;

        private Vector3 _oggLeft;
        private Vector3 _oggRight;
        public Vector3 OGGRandomPoint => new Vector3(Random.Range(_oggLeft.x, _oggRight.x), _oggLeft.y, 0f);

        private void Awake()
        {
            var oggcam = Camera.main;
            _oggLeft = oggcam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
            _oggLeft.x += _OGGoffset.x;
            _oggLeft.y += _OGGoffset.y;
            _oggRight = oggcam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            _oggRight.x -= _OGGoffset.x;
            _oggRight.y += _OGGoffset.y;
        }
    }
}