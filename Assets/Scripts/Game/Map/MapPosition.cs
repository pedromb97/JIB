using UnityEngine;

namespace LIT.Beaver.Map
{
    public enum eMapPositionState { NORMAL, BLOCKED }
    public class MapPosition : MonoBehaviour
    {
        [SerializeField, HideInInspector] private int _sector;
        [SerializeField, HideInInspector] private int _ring;

        [field : SerializeField] public eMapPositionState State { get; private set; }
        public int Sector => _sector;
        public int Ring => _ring;

        public void SetData(int sector, int ring)
        {
            _sector = sector;
            _ring = ring;
        }

        public void SetMapPositionState(eMapPositionState state)
        {
            State = state;
        }

        [ContextMenu("Show Sector-Ring")]
        private void ShowSectorAndRing()
        {
            Debug.Log($"{Sector}-{Ring}");
        }

        private void OnDrawGizmos()
        {
            Color gizmosColor = Color.green;
            switch (State)
            {
                case eMapPositionState.BLOCKED:
                    gizmosColor = Color.red;
                    break;
                default:
                    gizmosColor = Color.green;
                    break;
            }

            Gizmos.color = gizmosColor;
            Gizmos.DrawSphere(transform.position, .2f);
        }
    }
}