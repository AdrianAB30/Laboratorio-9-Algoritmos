using System.Collections;
using UnityEngine;

public class GraphControl : MonoBehaviour
{
    public GameObject nodePrefabs;
    public TextAsset nodePositionTxt;
    public string[] arrayNodePositions;
    public string[] currentNodePositions;
    public ListaSimple<GameObject> AllNodes;
    public TextAsset nodeConnectionsTxt;
    public string[] arrayNodeConnections;
    public string[] currentNodeConnections;
    public EnemyController enemy;

    void Start()
    {
        AllNodes = new ListaSimple<GameObject>();
        DrawNodes();
        ConnectNodes();
        CreateEnemyPath();
    }

    void DrawNodes()
    {
        if (nodePositionTxt != null)
        {
            GameObject currentNode;
            arrayNodePositions = nodePositionTxt.text.Split('\n');
            for (int i = 0; i < arrayNodePositions.Length; i++)
            {
                currentNodePositions = arrayNodePositions[i].Split(',');
                Vector2 positionToCreate = new Vector2(float.Parse(currentNodePositions[0]), float.Parse(currentNodePositions[1]));
                currentNode = Instantiate(nodePrefabs, positionToCreate, transform.rotation);
                currentNode.name = "Node" + i.ToString();
                currentNode.transform.SetParent(this.transform);
                AllNodes.InsertAtEnd(currentNode);
                Debug.Log("Nodo añadido: " + currentNode.name + " en la posición " + positionToCreate);
            }
        }
    }

    void CreateEnemyPath()
    {
        enemy.InitializePatrolEnemy(AllNodes);
    }

    void ConnectNodes()
    {
        if (nodeConnectionsTxt != null)
        {
            arrayNodeConnections = nodeConnectionsTxt.text.Split('\n');
            for (int i = 0; i < arrayNodeConnections.Length; i++)
            {
                currentNodeConnections = arrayNodeConnections[i].Split(',');
                int fromNodeIndex = int.Parse(currentNodeConnections[0]);
                int toNodeIndex = int.Parse(currentNodeConnections[1]);   
                float weight = float.Parse(currentNodeConnections[2]);   

                GameObject fromNode = AllNodes.GetNodeAtPosition(fromNodeIndex);
                GameObject toNode = AllNodes.GetNodeAtPosition(toNodeIndex);

                if (fromNode != null && toNode != null)
                {
                    fromNode.GetComponent<NodeControl>().AddConnection(toNode.GetComponent<NodeControl>(), weight);
                    Debug.Log("Conexión creada entre " + fromNode.name + " y " + toNode.name + " con un peso de " + weight);
                }
            }
        }
    }
}
