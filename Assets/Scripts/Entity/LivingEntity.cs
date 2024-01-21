using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private GameObject[] _waypoints = new GameObject[8];
    [SerializeField]
    private float _moveSpeed = 2f;

    private int _currentWaypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(_waypoints[_currentWaypointIndex].transform.position, transform.position) < 0.1f)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }

            if (transform.position.x > _waypoints[_currentWaypointIndex].transform.position.x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * _moveSpeed);
    }
}
