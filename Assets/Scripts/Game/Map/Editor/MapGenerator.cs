using LIT.Beaver.Map;
using LIT.Beaver.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LIT.Beaver.Editor.Map
{
    public class MapGenerator : EditorWindow
    {
        private MapPosition _prefab;
        private Transform _mapCenter;
        private GameSettings _gameSettings;
        private int _sectors;
        private int _rings;
        private float _bossToFirstRingDistance;
        private float _ringsDistance;

        private float AngleStep => 360f / _sectors;

        [MenuItem("LIT/Map Generator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MapGenerator), false, "Map Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Generate new map", EditorStyles.boldLabel);

            _prefab = EditorGUILayout.ObjectField("MapPosition Prefab", _prefab, typeof(MapPosition), false) as MapPosition;
            _mapCenter = EditorGUILayout.ObjectField("Scene MapController", _mapCenter, typeof(Transform), true) as Transform;
            _gameSettings = EditorGUILayout.ObjectField("Game Settings", _gameSettings, typeof(GameSettings), true) as GameSettings;

            _rings = _gameSettings.mapSettings.rings;
            _sectors = _gameSettings.mapSettings.sectors;
            _bossToFirstRingDistance = _gameSettings.mapSettings.bossToFirstRingDistance;
            _ringsDistance = _gameSettings.mapSettings.ringsDistance;

            if (GUILayout.Button("Generate map"))
            {
                GenerateMap();
            }

            if (GUILayout.Button("Clear map"))
            {
                ClearMap();
            }
        }

        private void GenerateMap()
        {
            ClearMap();
            GenerateMapWithDelay();
        }

        private async void GenerateMapWithDelay()
        {
            GameObject map = new GameObject($"Map");
            map.transform.SetParent(_mapCenter);

            for (int ringIndex = 0; ringIndex < _rings; ringIndex++)
            {
                GameObject sectorParent = new GameObject($"Ring {ringIndex}");
                sectorParent.transform.SetParent(map.transform);
                for (int sectorIndex = 0; sectorIndex < _sectors; sectorIndex++)
                {
                    Vector3 spawnPos = _mapCenter.forward * (_ringsDistance * ringIndex + _bossToFirstRingDistance);
                    MapPosition pos = Instantiate(_prefab, spawnPos, Quaternion.identity, sectorParent.transform);
                    pos.transform.RotateAround(_mapCenter.position, _mapCenter.up, AngleStep * sectorIndex);
                    pos.name = $"Sector {sectorIndex}-{ringIndex}";
                    pos.SetData(sectorIndex, ringIndex);

                    await Task.Delay(1);
                }
            }
        }

        private void ClearMap()
        {
            List<Transform> children = _mapCenter.Cast<Transform>().ToList();
            for (int i = 0; i < children.Count; i++)
            {
                DestroyImmediate(children[i].gameObject);
            }
            children.Clear();
        }

        private void OnEnable()
        {
            _sectors = PlayerPrefs.GetInt("sectors");
            _rings = PlayerPrefs.GetInt("rings");
            _bossToFirstRingDistance = PlayerPrefs.GetFloat("bossToFirstRingDistance");
            _ringsDistance = PlayerPrefs.GetFloat("ringsDistance");
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("sectors", _sectors);
            PlayerPrefs.SetInt("rings", _rings);
            PlayerPrefs.SetFloat("bossToFirstRingDistance", _bossToFirstRingDistance);
            PlayerPrefs.SetFloat("ringsDistance", _ringsDistance);
        }
    }
}