using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���¼ҽ� ���

public class Platform : MonoBehaviour
{
    public Collider2D platformCollider;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platformCollider, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platformCollider, false);
    }
}
