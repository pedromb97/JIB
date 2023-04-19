using UnityEngine;

namespace LIT.Beaver.Settings
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "GameSettings", menuName = "LIT/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public MapSettings mapSettings;
    }
}