using System.Collections;
using UnityEngine;

public class NodeControl : MonoBehaviour
{
    public ListaSimple<(NodeControl node, float weight)> connections;

    void Awake()
    {
        connections = CreateListConnections();
    }

    public void AddConnection(NodeControl node, float weight)
    {
        var connection = CreateConnection(node, weight);
        connections.InsertAtEnd(connection);
    }

    public (NodeControl node, float weight) SelectRandomConnection()    
    {
        return connections.GetNodeAtPosition(GetRandomIndex());
    }

    private ListaSimple<(NodeControl, float)> CreateListConnections()
    {
        return new ListaSimple<(NodeControl, float)>();
    }

    private (NodeControl, float) CreateConnection(NodeControl node, float weight)
    {
        return (node, weight);
    }

    public int GetRandomIndex()
    {
        return Random.Range(0, connections.Length);
    }
}
