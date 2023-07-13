using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuAnimation : MonoBehaviour
{

    [SerializeField] private TMP_Text jokeText;

    [Space]

    [TextArea(2, 2)]
    [SerializeField] private string[] jokes;

    private void OnEnable()
    {
        if (jokes.Length > 0)
        {
            int randomIndex = Random.Range(0, jokes.Length);
            string randomJoke = jokes[randomIndex];
            jokeText.text = $"{randomJoke}";
        }
    }

    public void CloseGameObject()
    {
        gameObject.SetActive(false);
    }

}
