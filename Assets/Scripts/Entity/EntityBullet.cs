using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBullet : MonoBehaviour
{
    private IEnumerator WhenExistTimeIsOut()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WhenExistTimeIsOut());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }

        Destroy(gameObject);
    }
}
