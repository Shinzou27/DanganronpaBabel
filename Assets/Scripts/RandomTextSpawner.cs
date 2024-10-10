using UnityEngine;
using UnityEngine.UI;
using TMPro; // Adicione se estiver usando TextMeshPro

public class RandomTextSpawner : MonoBehaviour
{
    public GameObject textPrefab; // Prefab do texto a ser instanciado
    public Canvas canvas; // Canvas onde os textos serão instanciados
    public float spawnInterval = 1f; // Intervalo de tempo entre spawns
    public float moveSpeed = 100f; // Velocidade de movimento dos textos

    private RectTransform canvasRectTransform;

    void Start()
    {
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        InvokeRepeating(nameof(SpawnRandomText), 0f, spawnInterval);
    }

    void SpawnRandomText()
    {
        // Cantos aleatórios do canvas
        Vector2[] corners = new Vector2[]
        {
            new Vector2(0, 0), // Inferior esquerdo
            new Vector2(0, canvasRectTransform.rect.height), // Superior esquerdo
            new Vector2(canvasRectTransform.rect.width, 0), // Inferior direito
            new Vector2(canvasRectTransform.rect.width, canvasRectTransform.rect.height) // Superior direito
        };

        // Escolher um canto aleatório
        Vector2 spawnPosition = corners[Random.Range(0, corners.Length)];

        // Instanciar o texto no canto aleatório
        GameObject newText = Instantiate(textPrefab, canvas.transform);
        RectTransform textRectTransform = newText.GetComponent<RectTransform>();
        textRectTransform.anchoredPosition = spawnPosition;

        // Definir um ângulo aleatório de movimento
        float angle = Random.Range(0f, 360f);
        Vector2 moveDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        // Adicionar script de movimento ao texto instanciado
        newText.AddComponent<MoveText>().Initialize(moveDirection, moveSpeed);
    }
}

public class MoveText : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private RectTransform rectTransform;

    public void Initialize(Vector2 direction, float speed)
    {
        moveDirection = direction;
        moveSpeed = speed;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += moveDirection * moveSpeed * Time.deltaTime;
    }
}
