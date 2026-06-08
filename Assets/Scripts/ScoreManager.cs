using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private Transform player;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text scoreText;

    private int coins;
    private int score;
    private float startZ;

    public int Coins => coins;
    public int Score => score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            player = playerObject != null ? playerObject.transform : null;
        }

        if (player != null)
        {
            startZ = player.position.z;
        }

        CreateDefaultUIIfNeeded();
        UpdateUI();
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        score = Mathf.Max(0, Mathf.FloorToInt(player.position.z - startZ));
        UpdateUI();
    }

    public void AddCoin()
    {
        coins++;
        UpdateUI();
    }

    public void ResetScore()
    {
        coins = 0;
        score = 0;

        if (player != null)
        {
            startZ = player.position.z;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinsText != null)
        {
            coinsText.text = "Monedas: " + coins;
        }

        if (scoreText != null)
        {
            scoreText.text = "Puntaje: " + score;
        }
    }

    private void CreateDefaultUIIfNeeded()
    {
        if (coinsText != null && scoreText != null)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Score Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        if (coinsText == null)
        {
            coinsText = CreateCounterText("Coins Text", canvas.transform, font, new Vector2(20f, -20f));
        }

        if (scoreText == null)
        {
            scoreText = CreateCounterText("Score Text", canvas.transform, font, new Vector2(20f, -55f));
        }
    }

    private Text CreateCounterText(string objectName, Transform parent, Font font, Vector2 anchoredPosition)
    {
        GameObject textObject = new GameObject(objectName);
        textObject.transform.SetParent(parent, false);

        Text text = textObject.AddComponent<Text>();
        text.font = font;
        text.fontSize = 28;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleLeft;

        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(280f, 32f);

        return text;
    }
}
