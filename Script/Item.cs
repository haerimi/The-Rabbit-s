using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 직접작성

public class Item : MonoBehaviour
{
    public string type;
    Rigidbody2D rigid; 


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    
}
