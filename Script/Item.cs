using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����ۼ�

public class Item : MonoBehaviour
{
    public string type;
    Rigidbody2D rigid; 


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    
}
