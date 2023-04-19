using System.Collections.Generic;
using UnityEngine;

namespace LIT.Beaver.Map
{
    public class MapController : MonoBehaviour
    {
        private Dictionary<Vector2, MapPosition> _positionEntries;

        public MapPosition GetMapPosition(Vector2 position)
        {
            if (_positionEntries.TryGetValue(position, out MapPosition mapPos))
            {
                return mapPos;
            }

            return null;
        }

        private void Awake()
        {
            _positionEntries = new Dictionary<Vector2, MapPosition>();

            foreach (MapPosition mapPosition in transform.GetComponentsInChildren<MapPosition>())
            {
                _positionEntries.Add(new Vector2(mapPosition.Sector, mapPosition.Ring), mapPosition);
            }
        }
    }
}