using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용안함

public class FadeinoutAni : MonoBehaviour
{
    Animator ani;
    public GameManager manager;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ani.SetBool("Fade", true);
        }
    }

    void Fade() {
        ani.SetBool("Fade", false);
    }
}