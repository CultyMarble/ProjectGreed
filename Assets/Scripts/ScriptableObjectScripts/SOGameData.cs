using UnityEngine;

[CreateAssetMenu(menuName = "SO Data/New Game Data", fileName = "Game Data Name")]
public class SOGameData : ScriptableObject
{
    // Overworld Data

    // Story Data

    // Player Meta-Progression Data

    // Player Inventory Data

    // Player Currency
    private int playerTempCurrencyAmount = default;
    private int playerPermCurrencyAmount = default;

    // Player Run Data
    private SceneName checkpointScene = default;
    private Vector3 checkpointSpawnPosition = default;

    //===========================================================================
    public void SaveOverworldData() { }
    public void SaveStoryData() { }
    public void SaveMetaProgressionData() { }
    public void SaveInventoryData() { }
    public void SaveCurrencyData(CurrencyType currencyType, int newAmount)
    {
        switch (currencyType)
        {
            case CurrencyType.TempCurrencyAmount:
                playerTempCurrencyAmount = newAmount;
                break;
            case CurrencyType.PermCurrencyAmount:
                playerPermCurrencyAmount = newAmount;
                break;
            default:
                break;
        }
    }
    public void SaveCheckPointData(SceneName sceneName, Vector3 spawnPosition)
    {
        checkpointScene = sceneName;
        checkpointSpawnPosition = spawnPosition;
    }

    public void RetrieveOverworldData() { }
    public void RetrieveStoryData() { }
    public void RetrieveMetaProgressionData() { }
    public void RetrieveInventoryData() { }
    public int RetrieveCurrencyData(CurrencyType currencyType)
    {
        switch (currencyType)
        {
            case CurrencyType.TempCurrencyAmount:
                return playerTempCurrencyAmount;
            case CurrencyType.PermCurrencyAmount:
                return playerPermCurrencyAmount;
            default:
                return 0;
        }
    }
    public SceneName RetrieveCheckPointSceneData()
    {
        return checkpointScene;
    }
    public Vector3 RetrieveCheckPointSpawnLocation()
    {
        return checkpointSpawnPosition;
    }
}