using System.Collections;
using UnityEngine;

public class PlayerCurrencies : SingletonMonobehaviour<PlayerCurrencies>
{
    [SerializeField] private DisplayPlayerCurrency displayPlayerCurrency;
    [SerializeField] private Animator displayPlayerCurrencyAnimator;

    [SerializeField] private float displayTime = 5F;

    private int tempCurrencyAmount = 1500;
    private int permCurrencyAmount = 50;

    private bool displayLock = false;

    public int TempCurrencyAmount => tempCurrencyAmount;
    public int PermCurrencyAmount => tempCurrencyAmount;

    //===========================================================================
    private void OnEnable()
    {
        UpdatePermCurrencyAmount();
        UpdateTempCurrencyAmount();
    }

    //===========================================================================
    public void UpdateTempCurrencyAmount(int amount = 0)
    {
        if (displayLock == false)
        {
            StartCoroutine(DisplayOpenandClose(displayTime));
        }

        tempCurrencyAmount += amount;
        tempCurrencyAmount = Mathf.Clamp(tempCurrencyAmount, 0, 10000);

        displayPlayerCurrency.UpdateTempCurrencyText(tempCurrencyAmount);
    }

    public void UpdatePermCurrencyAmount(int amount = 0)
    {
        permCurrencyAmount += amount;
        permCurrencyAmount = Mathf.Clamp(permCurrencyAmount, 0, 10000);

        displayPlayerCurrency.UpdatePermCurrencyText(permCurrencyAmount);
    }

    private IEnumerator DisplayOpenandClose(float delay)
    {
        displayLock = true;
        displayPlayerCurrencyAnimator.SetTrigger("Open");

        yield return new WaitForSeconds(delay);

        displayPlayerCurrencyAnimator.SetTrigger("Close");

        yield return new WaitForSeconds(0.66F);
        displayLock = false;
    }

    public void Open()
    {
        if (displayLock == false)
        {
            displayLock = true;
            displayPlayerCurrencyAnimator.SetTrigger("Open");
        }
    }

    public void Close()
    {
        if (displayLock == true)
        {
            displayLock = false;
            displayPlayerCurrencyAnimator.SetTrigger("Close");
        }
    }

}
