using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawGizmos(Waypoint waypoint, GizmoType gizmoType)
    {
        // Set gizmo color based on selection
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.blue * 0.5f;
        }

        // Draw the waypoint sphere
        Gizmos.DrawSphere(waypoint.transform.position, 0.1f);

        // Draw a line representing the waypoint's width
        Gizmos.color = Color.white;
        Gizmos.DrawLine(
            waypoint.transform.position + (waypoint.transform.right * waypoint.waypointWidth / 2f),
            waypoint.transform.position - (waypoint.transform.right * waypoint.waypointWidth / 2f)
        );

        // Draw connection to the previous waypoint
        if (waypoint.prevWaypoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.waypointWidth / 2f;
            Vector3 offsetTo = waypoint.prevWaypoint.transform.right * waypoint.prevWaypoint.waypointWidth / 2f;

            Gizmos.DrawLine(
                waypoint.transform.position + offset,
                waypoint.prevWaypoint.transform.position + offsetTo
            );
        }

        // Draw connection to the next waypoint
        if (waypoint.nextWaypoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * -waypoint.waypointWidth / 2f;
            Vector3 offsetTo = waypoint.nextWaypoint.transform.right * -waypoint.nextWaypoint.waypointWidth / 2f;

            Gizmos.DrawLine(
                waypoint.transform.position + offset,
                waypoint.nextWaypoint.transform.position + offsetTo
            );
        }
    }
}

