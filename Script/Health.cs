using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 오픈소스 사용

public class Health : MonoBehaviour
{
    Image healthBar;
    float maxHealth = 100f;
    public static float health;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / maxHealth;
        isDead();
    }

    void isDead() {
        if (maxHealth == 0)
            Destroy(healthBar);
    }
}
