using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{

     public Waypoint[] Waypoints;
    public int waypointIndex;

    private int numPoints;
    private Vector3[] points;
    private float[] distances;
    public float Length { get; private set; }



    public Transform CurrentWaypoint;
    public Transform ActiveWaypoint
    {
        get { return CurrentWaypoint; }
    }


    private void Awake()
    {
        Waypoints = GetComponentsInChildren<Waypoint>();
        CurrentWaypoint = Waypoints[0].transform;
        waypointIndex = 0;


        if (Waypoints.Length > 1)
        {
            CachePositionsAndDistances();
        }
        numPoints = Waypoints.Length;
    }

    public void UpdateWaypoint()
    {
        Waypoints[waypointIndex].enabled = false;
        waypointIndex++;
        int maxWaypointindex = Waypoints.Length - 1;

        //Reach last waypoint return to first point
        if (waypointIndex > maxWaypointindex) { waypointIndex = 0; }
        Waypoints[waypointIndex].enabled = true;
        CurrentWaypoint = Waypoints[waypointIndex].transform;

    }



    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }


    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }


    private void DrawGizmos(bool selected)
    {
        if (Waypoints.Length > 1)
        {
            int numPoints = Waypoints.Length;

            CachePositionsAndDistances();
            Length = distances[distances.Length - 1];

            Gizmos.color = selected ? Color.yellow : new Color(1, 1, 0, 0.5f);
            Vector3 prev = Waypoints[0].transform.position;
                for (int n = 0; n < Waypoints.Length; ++n)
                {
                    Vector3 next = Waypoints[(n + 1) % Waypoints.Length].transform.position;
                    Gizmos.DrawLine(prev, next);
                    prev = next;
                }
        }
    }


    private void CachePositionsAndDistances()
    {
        // transfer the position of each point and distances between points to arrays for
        // speed of lookup at runtime
        points = new Vector3[Waypoints.Length + 1];
        distances = new float[Waypoints.Length + 1];

        float accumulateDistance = 0;
        for (int i = 0; i < points.Length; ++i)
        {
            var t1 = Waypoints[(i) % Waypoints.Length].transform;
            var t2 = Waypoints[(i + 1) % Waypoints.Length].transform;
            if (t1 != null && t2 != null)
            {
                Vector3 p1 = t1.position;
                Vector3 p2 = t2.position;
                points[i] = Waypoints[i % Waypoints.Length].transform.position;
                distances[i] = accumulateDistance;
                accumulateDistance += (p1 - p2).magnitude;
            }
        }
    }
}
