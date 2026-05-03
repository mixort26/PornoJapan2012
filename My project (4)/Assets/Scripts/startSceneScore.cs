using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startSceneScore : MonoBehaviour
{
    public GameObject ScoreList;
    public TextMeshProUGUI[] ScoreText = new TextMeshProUGUI[4];
    public Image[] ScoreImage = new Image[4];
    public Sprite[] ScoreSprites = new Sprite[4];

    private int[] prevIng = GameData.Ingredients.Clone() as int[];
    
    void Start() {
        if (SceneManager.GetSceneByName("SceneFlappyBird").isLoaded) SceneManager.UnloadSceneAsync("SceneFlappyBird");
        if (!SceneManager.GetSceneByName("SceneWorkPlace").isLoaded) SceneManager.LoadScene("SceneWorkPlace", LoadSceneMode.Additive);
        StartCoroutine(StartCor());
        StartCoroutine(MoveToRoutine(ScoreList, 0, 0, 1f));
    }

    void Update()
    {
        // if (Input.anyKeyDown) StartCoroutine(GoWork());
        GameData.Minutes = 475;
    }

    private IEnumerator StartCor() {
        ScoreList.transform.position = new Vector2(0, 9);
        for (int i = 0; i < 4; i++) {
            ScoreImage[i].color = new Color(ScoreImage[i].color.r, ScoreImage[i].color.g, ScoreImage[i].color.b, 0f);
            ScoreText[i].text = null;
        }
        while (GameData.Score > 0) {
            for (int i = 0; i < GameData.SizeOfMenuIngredients; i++) {
                var rand = Random.Range(0, GameData.Score+1);
                Debug.Log($"Рандом сказал {rand}");
                GameData.Ingredients[i] += rand;
                Debug.Log($"Теперь {GameData.Ingredients[i]}, а было {prevIng[i]}");
                GameData.Score -= rand;
            }
        }
        
        for (int i = 0; i < GameData.SizeOfMenuIngredients; i++) {
            Debug.Log($"Должно быть {GameData.Ingredients[i] - prevIng[i]}");
            prevIng[i] = GameData.Ingredients[i] - prevIng[i];
            Debug.Log($"Получается {prevIng[i]}");
        }
        
        int j = 0;
        for (int i = 0; i < GameData.SizeOfMenuIngredients; i++) {
            if (prevIng[i] > 0) {
                ScoreImage[i].color = new Color(ScoreImage[i].color.r, ScoreImage[i].color.g, ScoreImage[i].color.b, 255f);
                ScoreImage[j].sprite = ScoreSprites[i];
                ScoreText[j].text = prevIng[i].ToString();
                j++;
            }
        }
        yield return null;
    }
    
    private IEnumerator MoveToRoutine(GameObject obj, float toY, float toX, float duration) {
        RectTransform rect = obj.GetComponent<RectTransform>();
        float elapsed = 0;
    
        Vector2 startPos = (rect != null) ? rect.anchoredPosition : (Vector2)obj.transform.position;
        Vector2 endPos = new Vector2(toX, toY);

        while (elapsed < duration) {
            float t = elapsed / duration;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            if (rect != null)
                rect.anchoredPosition = Vector2.Lerp(startPos, endPos, smoothT);
            else
                obj.transform.position = Vector2.Lerp(startPos, endPos, smoothT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (obj != null) {
            if (rect != null) rect.anchoredPosition = endPos;
            else obj.transform.position = endPos;
        }
    }

    public void GoWorkButton() {
        StartCoroutine(GoWork());
    }

    private IEnumerator GoWork() {
        StartCoroutine(MoveToRoutine(ScoreList, 0, 15, 1f));
        yield return new WaitForSeconds(1.1f);
        SceneManager.UnloadSceneAsync("SceneScore");
    }
}
