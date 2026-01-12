using System.Collections.Generic;
using UnityEngine;

public class EnemyPathDrawer : BoundingBoxDrawer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        this.CreateLineRenderer();
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> points = new List<Vector3>();

        foreach (Transform node in this.transform.GetComponentInChildren<Transform>())
        {
            if(node.tag != "EnemyPathNode")
                continue;

            points.Add(new Vector3(node.position.x, node.position.y + 0.25f, node.position.z));
        }

        points.Add(new Vector3(points[0].x, points[0].y, points[0].z));

        this._lineRenderer.positionCount = points.Count;
        this._lineRenderer.SetPositions(points.ToArray());
    }
}
