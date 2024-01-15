using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    private AudioSource _endSoundFx;

    private bool _isCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        _endSoundFx = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isCompleted && collision.gameObject.name == "Player")
        {
            _endSoundFx.Play();
            _isCompleted = true;
            Gameplay.Instance.CompleteLevel();
        }
    }
}
