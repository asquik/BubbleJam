using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region Variables
    private float maxPlatformSpeed;
    private float decelerationDistance;
    private float accelerationDistance;
    private float waitTime;

    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float currentSpeed = 1f;

    bool checkDecelerationRate = true;
    float decelerationRate = 0;

    private Rigidbody2D rb;
    private MovingPlatformInspector movingPlatformInspector;

    private Vector2 positionLastFrame;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        movingPlatformInspector = transform.parent.GetComponent<MovingPlatformInspector>();

        positionLastFrame = transform.position;
        
    }
    void Update()
    {
        maxPlatformSpeed = movingPlatformInspector.getMaxPlatformSpeed();
        decelerationDistance = movingPlatformInspector.getDecelerationDistance();
        accelerationDistance = movingPlatformInspector.getAccelerationDistance();
        waitTime = movingPlatformInspector.getWaitTime();
    }
    #endregion

    void FixedUpdate()
    {
        #region Velocity Logic

        int nextWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

        Vector3 directionToNextWaypoint = waypoints[nextWaypointIndex].position - transform.position;
        float distanceToNextWaypoint = Vector2.Distance(transform.position, waypoints[nextWaypointIndex].position);
        float distanceToLastWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);
        float accelerationRate = maxPlatformSpeed / accelerationDistance;

        if (distanceToNextWaypoint <= decelerationDistance && checkDecelerationRate)
        {
            decelerationRate = currentSpeed / distanceToNextWaypoint;
            checkDecelerationRate = false;

        }


        if (distanceToNextWaypoint <= decelerationDistance)
        {
            checkDecelerationRate = false;
            currentSpeed -= decelerationRate * Vector2.Distance(transform.position, positionLastFrame);
            currentSpeed = Mathf.Max(currentSpeed, 0);
        }
        else if (distanceToLastWaypoint <= accelerationDistance)
        {
            checkDecelerationRate = true;
            currentSpeed += accelerationRate * Vector2.Distance(transform.position, positionLastFrame);
            currentSpeed = Mathf.Min(currentSpeed, maxPlatformSpeed);
        }
        else
        {
            currentSpeed = maxPlatformSpeed;
        }

        rb.linearVelocity = directionToNextWaypoint.normalized * currentSpeed;
        #endregion

        positionLastFrame = transform.position;


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Waypoint" && !(collision.gameObject.transform == waypoints[currentWaypointIndex]))
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }


    IEnumerator WaitAtWaypoint()
    {
        yield return new WaitForSeconds(waitTime);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}