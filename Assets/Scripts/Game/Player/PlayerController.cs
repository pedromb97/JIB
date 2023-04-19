using LIT.Beaver.Map;
using UnityEngine;

namespace LIT.Beaver.Player
{
    public enum eMovementDirection { FORWARDS, BACKWARDS, RIGHT, LEFT }
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector2 _startPosition = new Vector2(0, 1);
        [SerializeField] private Transform _playerContainer;

        private Vector2 _currentPosition;

        void Start()
        {
            _currentPosition = _startPosition;
            TryPositionPlayer(_currentPosition, true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                TryMovePlayer(eMovementDirection.LEFT);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                TryMovePlayer(eMovementDirection.RIGHT);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                TryMovePlayer(eMovementDirection.FORWARDS);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                TryMovePlayer(eMovementDirection.BACKWARDS);
            }
        }

        private void TryMovePlayer(eMovementDirection direction)
        {
            int rings = GameManager._st.gameSettings.mapSettings.rings - 1;
            int sectors = GameManager._st.gameSettings.mapSettings.sectors - 1;

            float sector = _currentPosition.x;
            float ring = _currentPosition.y;

            switch (direction)
            {
                case eMovementDirection.FORWARDS:
                    ring -= 1;                    
                    break;
                case eMovementDirection.BACKWARDS:
                    ring += 1;
                    break;
                case eMovementDirection.RIGHT:
                    sector -= 1;                    
                    break;
                case eMovementDirection.LEFT:
                    sector += 1;                    
                    break;
                default:
                    break;
            }

            if (sector < 0) sector = sectors;
            if (sector > sectors) sector = 0;
            ring = Mathf.Clamp(ring, 0, rings);
            Vector2 desiredPosition = new Vector2(sector, ring);

            TryPositionPlayer(desiredPosition);
        }

        private void TryPositionPlayer(Vector2 position, bool instant = false)
        {
            MapPosition desiredPosition = GameManager._st.mapController.GetMapPosition(position);
            if (!CanMoveToPosition(desiredPosition.State)) return;

            transform.rotation = desiredPosition.transform.rotation;
            _currentPosition = position;

            float ringsDistance = GameManager._st.gameSettings.mapSettings.ringsDistance;
            float bossToFirstRingDistance = GameManager._st.gameSettings.mapSettings.bossToFirstRingDistance;
            Vector3 playerPosition = transform.forward * (ringsDistance * position.y + bossToFirstRingDistance);
            _playerContainer.position = playerPosition;
        }

        private void MovePlayer()
        {

        }

        private bool CanMoveToPosition(eMapPositionState positionState)
        {
            switch (positionState)
            {
                case eMapPositionState.NORMAL:
                    return true;
                case eMapPositionState.BLOCKED:
                    return false;
                default:
                    return false;
            }
        }
    }
}