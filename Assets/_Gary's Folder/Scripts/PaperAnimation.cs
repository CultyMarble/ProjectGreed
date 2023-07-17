using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperAnimation : MonoBehaviour
{
    [SerializeField] private OptionMenu optionMenu;

    public void FadeIn()
    {
        //print("TEST");
    }

    public void FadeOut()
    {
        optionMenu.ShowMenu(optionMenu.currentSelected);
        optionMenu.paperAnimator.SetTrigger("Out");
    }

}
