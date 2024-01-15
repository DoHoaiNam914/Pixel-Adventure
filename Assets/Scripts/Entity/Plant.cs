using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Plant : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private GameObject _bullet;

    private Vector3 _playerPosition;
    private float _nextFire;

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject newBullet = Instantiate(_bullet, new Vector3(transform.position.x + (_spriteRenderer.flipX ? 1.5f : -1.5f), transform.position.y + 0.15f), Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = (_spriteRenderer.flipX ? Vector2.right : Vector2.left) * 20f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _nextFire = Time.time;
    }

    void FixedUpdate()
    {
        _playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _playerPosition) <= 10f && Time.time > _nextFire)
        {
            _animator.SetTrigger("Fire");
            StartCoroutine(Fire());
            _nextFire = Time.time + 2f;
        }
        else
        {
            if (_playerPosition.x < transform.position.x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }
        }
    }
}
