using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField]
    public int pointAward;

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.velocity = new Vector2(10f, 12f);
        Gameplay.Instance.AddPoint(pointAward);
        GetComponent<Animator>().SetTrigger("Dead");
        Invoke("Destroy", 2f);
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Vector2.Dot(collision.relativeVelocity, Vector2.down) >= 13f)
        {
            Die();
        }
    }
}
