using UnityEngine;

public class PlayerPositionVector : MonoBehaviour
{
    public Transform aTransform;
    public Transform bTransform;
    public Transform cTransform;
    public Transform dTransform;
    public Transform playerTransform;
    public static PlayerPositionVector Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public Vector2 CalculateMapPosition()
    {
        Vector3 a = aTransform.position;
        Vector3 b = bTransform.position;
        Vector3 c = cTransform.position;
        Vector3 d = dTransform.position;
        Vector3 playerPosition = playerTransform.position;

        // Calcule as dimensões do retângulo
        float rectWidth = Vector3.Distance(a, b);
        float rectHeight = Vector3.Distance(a, c);

        // Encontre a posição do jogador em relação ao ponto 'a'
        Vector3 relativePosition = playerPosition - a;

        // Projeção do vetor relativo no vetor base horizontal e vertical
        float xProjection = Vector3.Dot(relativePosition, (b - a).normalized);
        float yProjection = Vector3.Dot(relativePosition, (c - a).normalized);

        // Calcule as proporções na direção x e y, normalizadas pelo tamanho do retângulo
        float xRatio = xProjection / rectWidth;
        float yRatio = yProjection / rectHeight;

        // Certifique-se de que os valores estão dentro do intervalo [0, 1]
        xRatio = (Mathf.Clamp01(xRatio) - 0.5f) * 0.1485f;
        yRatio = (Mathf.Clamp01(yRatio) - 0.5f) * 0.2295f;
        return new Vector2(xRatio, yRatio);
    }
}
