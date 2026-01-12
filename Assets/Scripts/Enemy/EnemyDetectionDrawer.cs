using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionDrawer : BoundingBoxDrawer
{
    private EnemyDetection _detection;

    [SerializeField] private float _drawAccuracy;

    void Start()
    {
        CreateLineRenderer();

        this._detection = this.gameObject.GetComponent<EnemyDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        float halfFOV = this._detection.Fov * 0.5f;

        Vector3 rightDir = Quaternion.Euler(0, halfFOV, 0) * transform.forward * this._detection.ViewDistance;
        Vector3 leftDir = Quaternion.Euler(0, -halfFOV, 0) * transform.forward * this._detection.ViewDistance;

        List<Vector3> points = new List<Vector3>();

        points.Add(this._detection.transform.position);

        for(float currentFOV = -halfFOV; currentFOV <= halfFOV; currentFOV += this._drawAccuracy)
        {
            Vector3 dir = Quaternion.Euler(0, currentFOV, 0) * transform.forward * this._detection.ViewDistance;

            if(Physics.Raycast(_detection.RayPoint.position, dir, out RaycastHit hitInfo ,this._detection.ViewDistance, this._detection.RaycastLayers))
            {
                points.Add(hitInfo.point);
            }
            else
            {
                points.Add(this._detection.transform.position + dir);
            }
            
        }

        points.Add(this._detection.transform.position);
        
        this._lineRenderer.positionCount = points.Count;
        this._lineRenderer.SetPositions(points.ToArray());
    }
}
