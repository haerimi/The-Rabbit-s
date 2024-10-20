using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 流立 内靛 累己

public class Title : MonoBehaviour
{
    public Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    { 

        if (Input.GetKeyDown(KeyCode.Return)){
            ani.SetBool("Faed", true);
            Invoke("GoTutorial", 1f);
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        else if (Input.GetKeyDown(KeyCode.Space)) {
            ani.SetBool("Faed", true);
            Invoke("GoHow", 1f);
        }
    }

    void GoTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    void GoHow() {
        SceneManager.LoadScene("How");
    }
}
