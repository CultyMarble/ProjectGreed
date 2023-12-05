using UnityEngine;

public enum UpgradeChoice { Left, Middle, Right, None, }

[CreateAssetMenu(menuName = "SOData/NewSOPlayerUpgradeData")]
public class SOPlayerUpgradeData : ScriptableObject
{
    [SerializeField] private UpgradeChoice tier1Choice = UpgradeChoice.None;
    [SerializeField] private UpgradeChoice tier2Choice = UpgradeChoice.None;

    public UpgradeChoice Tier1Choice => tier1Choice;
    public UpgradeChoice Tier2Choice => tier2Choice;

    [SerializeField] private int permCurrencyAmount = default;
    public int PermCurrencyAmount => permCurrencyAmount;

    //===========================================================================
    public void SetTier1Choice(UpgradeChoice choice) { tier1Choice = choice; }
    public void SetTier2Choice(UpgradeChoice choice) { tier2Choice = choice; }

    public void UpdateCurrencyAmount() { permCurrencyAmount = PlayerCurrencies.Instance.PermCurrencyAmount; }
}