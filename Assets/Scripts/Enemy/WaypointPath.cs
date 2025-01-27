using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

public class WaypointPath : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints = new List<Transform>();

    [Header("Debug Settings")]
    [SerializeField] private bool alwaysDrawPath = true;
    [SerializeField] private bool drawAsLoop = false;
    [SerializeField] private bool drawNumbers = true;
    [SerializeField] private Color pathColor = Color.white;
    [SerializeField] private float waypointSize = 0.5f;
    [SerializeField] private float labelSize = 30f;

#if UNITY_EDITOR
    private void OnValidate()
    {
        waypoints.RemoveAll(wp => wp == null);
        waypointSize = Mathf.Max(0.1f, waypointSize);
        labelSize = Mathf.Max(10f, labelSize);
    }

    private void OnDrawGizmos()
    {
        if (alwaysDrawPath)
        {
            DrawPath();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!alwaysDrawPath)
        {
            DrawPath();
        }
    }

    private void DrawPath()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null) continue;

            Gizmos.color = pathColor;
            Gizmos.DrawWireSphere(waypoints[i].position, waypointSize);

            if (drawNumbers)
            {
                GUIStyle labelStyle = new GUIStyle
                {
                    fontSize = (int)labelSize,
                    normal = { textColor = pathColor },
                    alignment = TextAnchor.MiddleCenter
                };

                Handles.Label(waypoints[i].position, i.ToString(), labelStyle);
            }

            if (i > 0 && waypoints[i - 1] != null)
            {
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }
        }

        if (drawAsLoop && waypoints.Count > 1 &&
            waypoints[0] != null && waypoints[waypoints.Count - 1] != null)
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }
    }

    public Vector3 GetWaypointPosition(int index)
    {
        if (IsValidWaypointIndex(index))
        {
            return waypoints[index].position;
        }
        Debug.LogWarning($"Invalid waypoint index: {index}");
        return Vector3.zero;
    }

    public int GetNextWaypointIndex(int currentIndex)
    {
        if (!IsValidWaypointIndex(currentIndex)) return 0;

        int nextIndex = currentIndex + 1;
        return drawAsLoop ? nextIndex % waypoints.Count :
                          Mathf.Min(nextIndex, waypoints.Count - 1);
    }

    public int GetWaypointCount()
    {
        return waypoints?.Count ?? 0;
    }

    private bool IsValidWaypointIndex(int index)
    {
        return waypoints != null && index >= 0 && index < waypoints.Count;
    }
#endif
}