using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class EnemyPath : MonoBehaviour
{
    private List<EnemyPathNode> _nodes = new List<EnemyPathNode>();

    [SerializeField] private bool _drawGizmos;
 
    void Start()
    {
        FindAllNodes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public EnemyPathNode GetNextNode(EnemyPathNode node)
    {
        int nextNode = 0;

        if (node == null)
            nextNode = 0;
        else if (node.Index == this._nodes.Count - 1)
            nextNode = 0;
        else
            nextNode = node.Index + 1;

        return this._nodes[nextNode];
    }

    public bool IsColliderMyNode(Collider collider, EnemyPathNode node)
    {
        Debug.Log(node.Id);

        if (collider.tag != "EnemyPathNode")
            return false;

        Debug.Log("Passed tag");

        bool foundNode = false;

        foreach (EnemyPathNode checkNode in this._nodes)
        {
            Debug.Log(checkNode.Id + "Vs" + node.Id);

            if (checkNode.Id == node.Id)
                foundNode = true;
        }


        if (!foundNode)
            return false;

        Debug.Log("Passed ownership");


        if (collider.GetComponent<EnemyPathNodeCom>().Node.Index == node.Index)
            return true;

        return false;
    }

    private void FindAllNodes()
    {
        this._nodes.Clear();

        int i = 0;

        foreach (Transform node in this.transform.GetComponentInChildren<Transform>())
        {
            node.GetComponent<EnemyPathNodeCom>().Node.Index = i;

            this._nodes.Add(new EnemyPathNode() { Index = i, Position = node.position, WaitTime = node.GetComponent<EnemyPathNodeCom>().Node.WaitTime, Id = Random.Range(0, int.MaxValue) });

            i += 1;
        }
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        if (!this._drawGizmos)
            return;

        if (!Application.isPlaying)
            FindAllNodes();

        for (int i = 0; i < this._nodes.Count; i++)
        {
            int nextLine = i + 1;

            if (i + 1 == this._nodes.Count)
            {
                nextLine = 0;
            }

            Gizmos.DrawLine(this._nodes[i].Position, this._nodes[nextLine].Position);

            if (i == 0)
                Gizmos.color = Color.red;
            else if (this._nodes[i].WaitTime != 0)
                Gizmos.color = Color.magenta;
            else
                Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(this._nodes[i].Position, 0.5f);
            Handles.Label(this._nodes[i].Position, i.ToString());

            Gizmos.color = Color.white;
        }
    }
    
    #endif
}
