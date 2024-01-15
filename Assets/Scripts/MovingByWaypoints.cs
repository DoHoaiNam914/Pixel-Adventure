using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingByWaypoints : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _waypoints = new GameObject[2];

    private int _currentWaypointIndex = 0;

    private float _moveSpeed = 2f;

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
        }

        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * _moveSpeed);
    }
}
