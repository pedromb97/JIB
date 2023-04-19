using LIT.Beaver.Map;
using LIT.Beaver.Settings;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _st;

    public MapController mapController;
    public GameSettings gameSettings;

    private void Awake()
    {
        _st = this;
    }
}
