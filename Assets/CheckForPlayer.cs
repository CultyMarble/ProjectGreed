using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForPlayer : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("playerIsNear", true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("playerIsNear", false);

    }
}
