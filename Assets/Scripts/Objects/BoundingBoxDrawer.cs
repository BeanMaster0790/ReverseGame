using UnityEngine;

public class BoundingBoxDrawer : MonoBehaviour
{
    private BoxCollider _collider;

    protected LineRenderer _lineRenderer;

    private LineRenderer _duplicatedRenderer;

    protected float _lineWidth = 0.02f;

    public Color _lineColour = Color.green;

    private bool _isObjectSelected;


    private SelectionObject _selectionObject;

    private void Start()
    {
        this._collider = this.GetComponent<BoxCollider>();

        CreateLineRenderer();

        this._selectionObject = this.GetComponent<SelectionObject>();

        if(this._selectionObject.RecordTimeSeconds > 0)
        {
            this._duplicatedRenderer = new GameObject(this.name + "RewindLine").AddComponent<LineRenderer>();
            this._duplicatedRenderer.transform.SetParent(this.transform, false);

            this._duplicatedRenderer.material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));

            this._duplicatedRenderer.startWidth = this._lineWidth;
            this._duplicatedRenderer.positionCount = 16;
            this._duplicatedRenderer.loop = false;

            this._duplicatedRenderer.useWorldSpace = true;
            this._duplicatedRenderer.startColor = new Color(this._lineColour.r, this._lineColour.g, this._lineColour.b, 100);
            this._duplicatedRenderer.endColor = new Color(this._lineColour.r, this._lineColour.g, this._lineColour.b, 100);

            this._duplicatedRenderer.enabled = false;

            this._duplicatedRenderer.gameObject.layer = LayerMask.NameToLayer("lineRenderers");

            ObjectController.S_ObjectSelect += ToggleObjectSelect;
        }
    }

    public void CreateLineRenderer()
    {
        this._lineRenderer = new GameObject(this.name + "Line").AddComponent<LineRenderer>();
        this._lineRenderer.transform.SetParent(this.transform, false);

        this._lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));

        this._lineRenderer.startWidth = this._lineWidth;
        this._lineRenderer.positionCount = 16;
        this._lineRenderer.loop = false;

        this._lineRenderer.useWorldSpace = true;
        this._lineRenderer.startColor = this._lineColour;
        this._lineRenderer.endColor = this._lineColour;

        this._lineRenderer.enabled = false;

        this._lineRenderer.gameObject.layer = LayerMask.NameToLayer("lineRenderers");

        ObjectController.S_ControlModeToggle += ToggleControlMode;
    }

    void ToggleControlMode(object sender, ControlModeEvent e)
    {
        this._lineRenderer.enabled = e.State;

        if(e.State == false && this._duplicatedRenderer != null)
            this._duplicatedRenderer.enabled = false;
    }

    void ToggleObjectSelect(object sender, ObjectSelectEvent e)
    {
        if(e.SelectionObject!= null && e.SelectionObject.gameObject == this.gameObject)
        {
            this._isObjectSelected = true;
        }
        else
        {
            this._isObjectSelected = false;
        }
    }

    void OnDestroy()
    {
        ObjectController.S_ControlModeToggle -= ToggleControlMode;

        if(this._duplicatedRenderer)
        {
            ObjectController.S_ObjectSelect -= ToggleObjectSelect;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(this._collider == null || !this._lineRenderer.enabled)
            return;

        Vector3 center = this._collider.center;
        Vector3 edgeDistance = this._collider.size * 0.5f;

        Vector3[] local =
        {
            center + new Vector3(-edgeDistance.x, -edgeDistance.y, -edgeDistance.z),
            center + new Vector3( edgeDistance.x, -edgeDistance.y, -edgeDistance.z),
            center + new Vector3( edgeDistance.x, -edgeDistance.y,  edgeDistance.z),
            center + new Vector3(-edgeDistance.x, -edgeDistance.y,  edgeDistance.z),
            center + new Vector3(-edgeDistance.x,  edgeDistance.y, -edgeDistance.z),
            center + new Vector3( edgeDistance.x,  edgeDistance.y, -edgeDistance.z),
            center + new Vector3( edgeDistance.x,  edgeDistance.y,  edgeDistance.z),
            center + new Vector3(-edgeDistance.x,  edgeDistance.y,  edgeDistance.z),
        };

        for (int i = 0; i < local.Length; i++)
            local[i] = this._collider.transform.TransformPoint(local[i]);

        Vector3[] edges =
        {
            local[0], local[1], local[2], local[3], local[0],
            local[4], local[5], local[1], local[5], local[6],
            local[2], local[6], local[7], local[3], local[7], local[4]
        };

        this._lineRenderer.positionCount = edges.Length;
        this._lineRenderer.SetPositions(edges);

        if(!this._duplicatedRenderer)
            return;

        if(this._isObjectSelected)
        {
            this._duplicatedRenderer.enabled = true;

            DrawDuplicatedLine();
        }
        else
        {
            this._duplicatedRenderer.enabled = false;
        }
    }

    void DrawDuplicatedLine()
    {
        if (_selectionObject.FirstRecordedPosition == null ||
            _selectionObject.FirstRecordedRotation == null ||
            !_lineRenderer.enabled)
            return;

        Vector3 oldPos = this._selectionObject.FirstRecordedPosition.Value;
        Quaternion oldRot = this._selectionObject.FirstRecordedRotation.Value;

        Vector3 center = this._collider.center;
        Vector3 edgeDistance = this._collider.size * 0.5f;

        Vector3 scaleDelta = transform.lossyScale;

        Vector3[] local =
        {
            Vector3.Scale(center + new Vector3(-edgeDistance.x, -edgeDistance.y, -edgeDistance.z), scaleDelta),
            Vector3.Scale(center + new Vector3( edgeDistance.x, -edgeDistance.y, -edgeDistance.z), scaleDelta),
            Vector3.Scale(center + new Vector3( edgeDistance.x, -edgeDistance.y,  edgeDistance.z), scaleDelta),
            Vector3.Scale(center + new Vector3(-edgeDistance.x, -edgeDistance.y,  edgeDistance.z), scaleDelta),
            Vector3.Scale(center + new Vector3(-edgeDistance.x,  edgeDistance.y, -edgeDistance.z), scaleDelta),
            Vector3.Scale(center + new Vector3( edgeDistance.x,  edgeDistance.y, -edgeDistance.z), scaleDelta),
            Vector3.Scale(center + new Vector3( edgeDistance.x,  edgeDistance.y,  edgeDistance.z), scaleDelta),
            Vector3.Scale(center + new Vector3(-edgeDistance.x,  edgeDistance.y,  edgeDistance.z), scaleDelta),
        };

        Vector3[] world = new Vector3[8];

        for (int i = 0; i < 8; i++)
            world[i] = oldPos + oldRot * local[i];

        Vector3[] edges =
        {
            world[0], world[1], world[2], world[3], world[0],
            world[4], world[5], world[1], world[5], world[6],
            world[2], world[6], world[7], world[3], world[7], world[4]
        };

        this._duplicatedRenderer.positionCount = edges.Length;
        this._duplicatedRenderer.SetPositions(edges);
    }
}
