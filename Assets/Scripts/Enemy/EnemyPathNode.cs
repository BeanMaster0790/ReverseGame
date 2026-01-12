using UnityEngine;

[System.Serializable]
public class EnemyPathNode
{
    [HideInInspector] public int Id;
    public float WaitTime;
    public float RotationSpeed;
    [HideInInspector] public Vector3 Position;
    [HideInInspector] public int Index = -1;
}
