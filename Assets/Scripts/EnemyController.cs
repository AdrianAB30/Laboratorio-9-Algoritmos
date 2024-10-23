using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ListaSimple<GameObject> pathNodes;
    public float energy;
    public float maxEnergy;
    public float currentWeight;
    public float restTime;
    public float moveSpeed;
    public float restTimer;
    public GameObject visionCone;
    public GameObject objectiveNode;
    public GameObject player;

    private Vector2 fovReferenceToPlayer;
    private bool isResting;
    private int currentIndex;
    private bool isChasingPlayer;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        InitializeEnemy();
        energy = maxEnergy;
        visionCone = Instantiate(visionCone, transform.position, transform.rotation);
        visionCone.transform.parent = transform;
        visionCone.transform.localPosition = new Vector3(-0.6f, 0, 0);
    }
    void Update()
    {
        if (isResting)
        {
            RestingTimer();
        }
        else
        {
            if (isChasingPlayer)
            {
                MoveEnemyToPlayer(player.transform.position);
                visionCone.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
                DrainEnergy();
            }
            else
            {
                MoveEnemyToPlayer(objectiveNode.transform.position);

                if (Vector2.Distance(transform.position, objectiveNode.transform.position) < 0.1f)
                {
                    UpdateCurrentIndex();
                }
            }

            ChangeDirectionSprite();
        }
    }
    private void ChangeDirectionSprite()
    {
        Vector2 targetPosition = isChasingPlayer ? (Vector2)player.transform.position : (Vector2)objectiveNode.transform.position;
        fovReferenceToPlayer = targetPosition - (Vector2)transform.position;

        if (Mathf.Abs(fovReferenceToPlayer.x) > Mathf.Abs(fovReferenceToPlayer.y))
        {
            if (fovReferenceToPlayer.x > 0)
            {
                spriteRenderer.flipX = true;
                visionCone.transform.localPosition = new Vector3(0.5f, 0, 0);
                visionCone.transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else if (fovReferenceToPlayer.x < 0)
            {
                spriteRenderer.flipX = false;
                visionCone.transform.localPosition = new Vector3(-0.5f, 0, 0);
                visionCone.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else
        {
            if (fovReferenceToPlayer.y > 0)
            {
                visionCone.transform.localPosition = new Vector3(0, 0.5f, 0);
                visionCone.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (fovReferenceToPlayer.y < 0)
            {
                visionCone.transform.localPosition = new Vector3(0, -0.5f, 0);
                visionCone.transform.localRotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isChasingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasingPlayer = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == objectiveNode)
        {
            HandleNodeCollision(collision);
        }
    }

    public void InitializePatrolEnemy(ListaSimple<GameObject> nodes)
    {
        pathNodes = nodes;
        currentIndex = 0;
        SetObjective(pathNodes.GetNodeAtPosition(currentIndex));
        currentWeight = 0;
    }

    private void InitializeEnemy()
    {
        energy = maxEnergy;
        isResting = false;
        restTimer = 0;
        currentIndex = 0;

        if (pathNodes != null && pathNodes.Length > 0)
        {
            SetObjective(pathNodes.GetNodeAtPosition(currentIndex));
        }
        else
        {
            Debug.LogError("pathNodes está vacío o no inicializado correctamente.");
        }
    }

    private void MoveEnemyToPlayer(Vector2 objective)
    {
        transform.position = Vector2.MoveTowards(transform.position, objective, moveSpeed * Time.deltaTime);
    }

    private void HandleNodeCollision(Collision2D collision)
    {

        if (collision.gameObject == objectiveNode)
        {
            NodeControl currentNode = collision.gameObject.GetComponent<NodeControl>();

            if (currentNode.connections.Length > 0)
            {
                Debug.Log("Colisión con el nodo: " + currentNode.gameObject.name);
                (NodeControl nextNode, float weight) = currentNode.SelectRandomConnection();

                if (nextNode != null)
                {
                    SetObjective(nextNode.gameObject);
                    Debug.Log("Nuevo objetivo: " + nextNode.gameObject.name);
                }
                else
                {
                    Debug.LogWarning("No se encontro un nodo papeto");
                    UpdateCurrentIndex();
                }

                currentWeight = weight;
                energy = energy - currentWeight;
                Debug.Log("Peso del Nodo:" + currentWeight + " Energía restante: " + energy);

                if (energy <= 0)
                {
                    isResting = true;
                }
            }
            else
            {
                Debug.LogWarning("El nodo no tiene conexiones.");
            }
        }
    }

    private void UpdateCurrentIndex()
    {
        currentIndex++;
        if (currentIndex >= pathNodes.Length)
        {
            currentIndex = 0;
        }
        SetObjective(pathNodes.GetNodeAtPosition(currentIndex));
    }

    private void SetObjective(GameObject newObjective)
    {
        objectiveNode = newObjective;
    }

    private void DrainEnergy(float weight = 0)
    {
        if (weight > 0)
        {
            energy = energy - weight;
        }
        else
        {
            energy = energy - Time.deltaTime;
        }

        if (energy <= 0)
        {
            StartResting();
        }
    }

    private void StartResting()
    {
        isResting = true;
        restTimer = 0;
    }

    private void RestingTimer()
    {
        restTimer += Time.deltaTime;
        if (restTimer >= restTime)
        {
            energy = maxEnergy;
            isResting = false;
        }
    }
}