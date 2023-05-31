using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO Data/New Game Data", fileName = "Game Data Name")]
public class SOGameData : ScriptableObject
{
    // Overworld Data
    private Dictionary<string, bool> SceneClearDataDictionary = new Dictionary<string, bool>();

    // Story Data

    // Player Meta-Progression Data

    // Player Inventory Data

    // Player Currency
    private int playerTempCurrencyAmount = default;
    private int playerPermCurrencyAmount = default;

    //===========================================================================
    public void SaveOverworldData(SceneName sceneName, bool newBool)
    {
        if (SceneClearDataDictionary.ContainsKey(sceneName.ToString()) == false)
        {
            SceneClearDataDictionary.Add(sceneName.ToString(), newBool);
        }
        else
        {
            SceneClearDataDictionary[sceneName.ToString()] = newBool;
        }
    }

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

    public bool? RetrieveOverworldData(SceneName sceneName)
    {
        if (SceneClearDataDictionary.ContainsKey(sceneName.ToString()))
        {
            return SceneClearDataDictionary[sceneName.ToString()];
        }

        return null;
    }
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
}