using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� �ڵ� �ۼ�

public class Title2 : MonoBehaviour
{
    public Animator ani;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            ani.SetBool("Faed", true);
            Invoke("GoTitle", 1f);
        }

     }

    void GoTitle() {

        SceneManager.LoadScene("Title");
    }
}
