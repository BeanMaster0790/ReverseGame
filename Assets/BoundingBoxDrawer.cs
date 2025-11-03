using UnityEngine;

public class BoundingBoxDrawer : MonoBehaviour
{
    private Renderer _renderer;

    private LineRenderer _lineRenderer;

    private float _lineWidth = 0.02f;

    public Color _lineColour = Color.green;

    void Start()
    {
        this._renderer = this.GetComponent<Renderer>();

        this._lineRenderer = new GameObject(this.name + "Line").AddComponent<LineRenderer>();
        this._lineRenderer.transform.SetParent(this.transform, false);

        this._lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

        this._lineRenderer.startWidth = this._lineWidth;
        this._lineRenderer.positionCount = 16;
        this._lineRenderer.loop = false;

        this._lineRenderer.useWorldSpace = true;
        this._lineRenderer.startColor = this._lineColour;
        this._lineRenderer.endColor = this._lineColour;

        this._lineRenderer.enabled = false;

        this._lineRenderer.gameObject.layer = LayerMask.NameToLayer("lineRenderers");

        ObjectController.S_ControlModeToggle += (object sender, ControlModeEvent e) =>
        {
            this._lineRenderer.enabled = e.State;
        };
    }

    // Update is called once per frame
    void Update()
    {
        Bounds lineBounds = this._renderer.bounds;

        Vector3[] points =
        {
            new(lineBounds.min.x, lineBounds.min.y, lineBounds.min.z),
            new(lineBounds.max.x, lineBounds.min.y, lineBounds.min.z),
            new(lineBounds.max.x, lineBounds.min.y, lineBounds.max.z),
            new(lineBounds.min.x, lineBounds.min.y, lineBounds.max.z),
            new(lineBounds.min.x, lineBounds.max.y, lineBounds.min.z),
            new(lineBounds.max.x, lineBounds.max.y, lineBounds.min.z),
            new(lineBounds.max.x, lineBounds.max.y, lineBounds.max.z),
            new(lineBounds.min.x, lineBounds.max.y, lineBounds.max.z)
        };

        Vector3[] orderedPoints =
        {
            points[0], points[1], points[2], points[3], points[0], points[4], points[5], points[1], points[5], points[6], points[2],
            points[6], points[7], points[3], points[7], points[4]
        };

        this._lineRenderer.positionCount = orderedPoints.Length;
        this._lineRenderer.SetPositions(orderedPoints);
    }
}
