using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitEntity : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Gameplay.Instance.PickupItem(gameObject);
        }
    }
}
