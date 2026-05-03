using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartSceneWorkPlace : MonoBehaviour
{
    //Y = -6.5 -- Y = 0.15 (from to)

    [Header("Main")]
    public AudioClip musicTheme;
    public GameObject sceneContent;
    public Canvas worldCanvas;
    public Sprite[] buttons = new Sprite[4];
    public Sprite[] coffeeSprites = new Sprite[5];
    public Sprite[] ingSprites = new Sprite[4];
    public Image blackscreen;
    public GameObject Blackscreen;
    [Header("Caterpillars")]
    public GameObject caterpillar1;
    public GameObject caterpillar2;
    public GameObject canvas1;
    public GameObject canvas2;
    [Header("Я НЕ ПОМНЮ ЧТО ЭТО ТАКОЕ((((")]
    public GameObject count;
    public GameObject time;
    public GameObject[] cups = new GameObject[3];
    public GameObject error;
    public GameObject[] notice = new GameObject[2];
    public GameObject[] check;
    [Header("LevelUpList")]
    public GameObject errorLvlUp;
    public GameObject buttonLevelUp;
    public GameObject levelUpList;
    public GameObject levelUpCost;
    [Header("ScoreList")]
    public GameObject ScoreList;
    public TextMeshProUGUI[] ScoreText = new TextMeshProUGUI[4];
    public Image[] ScoreImage = new Image[4];
    //private ТУТ Я ТОЛКОМ ТО ТОЖЕ ХЗ(((
    private int _miss;
    private int _prevMinutes;
    private bool _endDay = false;
    private int[] _order1 = new int[2];
    private int[] _order2 = new int[2];
    private bool _order1Active = false;
    private bool _order2Active = false;
    private int _timeAccept = 15000;
    private int[] prevIng = GameData.Ingredients.Clone() as int[];
    private bool _scoreListActive = false;
    private Queue<int[]> _tasks = new Queue<int[]>();
    private bool _isProcessing = false;
    public static int Final = 0;
    private bool _isProcess = false;
    public static bool OrderBeRemove = false;

    private void Start() {
        if (check != null) foreach (GameObject t in check) t.SetActive(false);
        count.GetComponent<TextMeshProUGUI>().text = "" + GameData.Money;
        MusicManager.Instance.PlayMusic(musicTheme);
        //Прячем всех
        Blackscreen.SetActive(false);
        canvas1.SetActive(false);
        canvas2.SetActive(false);
        levelUpList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1000);
        notice[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-1100f, 160f);
        notice[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-1100f, 60f);
        error.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 600f);
        errorLvlUp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 600f);
        ScoreList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1000);
        caterpillar1.transform.position = new Vector2(-5.4f, -7f);
        caterpillar2.transform.position = new Vector2(5.4f, -7f);
        caterpillar1.SetActive(false);
        caterpillar2.SetActive(false);
        foreach (var t in cups)
         t.SetActive(false);
        buttonLevelUp.GetComponent<Image>().sprite = buttons[2];
        _miss = 0;
        GameData.Coffees = new int[5];
        worldCanvas.GetComponent<GraphicRaycaster>().enabled = true;
        if (SceneManager.GetSceneByName("ToDay").isLoaded) {
            SceneManager.UnloadSceneAsync("ToDay");
            _scoreListActive = true;
            worldCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            Debug.Log("Канвас стал неактивен!");
            StartCoroutine(ShowScoreList());
            StartCoroutine(MoveToRoutine(ScoreList, 0, 0, 1));
        }
    }
    
    private void Update() {
         if (Input.GetKeyUp(KeyCode.Space)) Cheat();
        
         if (_scoreListActive) GameData.Minutes = 475;
        
         if (_prevMinutes != GameData.Minutes) {
             if (_prevMinutes == 1199) _endDay = true;
             time.GetComponent<TextMeshProUGUI>().text =
                 (GameData.Minutes >= 600 ? "" + GameData.Minutes / 60 : "0" + GameData.Minutes / 60) + ':' +
                 (GameData.Minutes % 60 >= 10 ? GameData.Minutes % 60 : "0" + GameData.Minutes % 60);
             _prevMinutes = GameData.Minutes;
         }
    
        // Проверка на наличие посетителей, если кого-то нет то начинаем движ
         if ((!caterpillar1.activeSelf || caterpillar1.transform.position.y <= -7) && GameData.Minutes >= 480) {
             CaterpillarUp(caterpillar1, canvas1, -1f, 1);
         }
        
         if ((!caterpillar2.activeSelf || caterpillar2.transform.position.y <= -7) && GameData.Minutes >= 480) {
             CaterpillarUp(caterpillar2, canvas2, -1f, 2);
         }
        
         if (_endDay || _miss >= 3) {
             if (_miss >= 3) GameData.Money -= 20;
             _endDay = false;
             SceneManager.LoadScene("ToNight");
         }
        
         if (GameData.Coffees[4] < Final) {
             if (worldCanvas.GetComponent<GraphicRaycaster>().enabled) {
                 Blackscreen.SetActive(true);
                 StartCoroutine(FadeRoutine(blackscreen, 5f));
             }
             worldCanvas.GetComponent<GraphicRaycaster>().enabled = false;
             if (!_isProcess) {
                 SceneManager.LoadScene("Final");
             }
         }
    }

    private void CaterpillarUp(GameObject caterpillar, GameObject canvas, float toY, int number) {
        if (!caterpillar.activeSelf) caterpillar.SetActive(true);
        StartCoroutine(MoveToRoutine(caterpillar, toY, caterpillar.transform.position.x, 1.5f));
        if (number == 1)
            StartCoroutine(ShowDialog(caterpillar, canvas, (i, i1) => _order1 = new[] { i, i1 }));
        else
            StartCoroutine(ShowDialog(caterpillar, canvas, (i, i1) => _order2 = new[] { i, i1 }));
    }

    private void CaterpillarDown(GameObject caterpillar, GameObject canvas) {
        canvas.SetActive(false);
        StartCoroutine(MoveToRoutine(caterpillar, -7f, caterpillar.transform.position.x, 1.5f));
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


    private IEnumerator ShowDialog(GameObject caterpillar, GameObject canvas, System.Action<int, int> onComplete) {
        while (caterpillar.transform.position.y < -1f) {
            yield return null;
        }

        int typeOfCoffee = Random.Range(0, GameData.SizeOfMenu);
        canvas.transform.Find("SpriteCoffee").GetComponent<Image>().sprite = coffeeSprites[typeOfCoffee];
        int amount = Random.Range(1, 4);
        canvas.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "X " + amount;
        canvas.transform.Find("Button").GetComponent<Image>().sprite = buttons[0];
        canvas.SetActive(true);
        onComplete?.Invoke(typeOfCoffee, amount);
    }

    private IEnumerator FadeRoutine(Image sprite, float duration) {
        _isProcess = true;
        float elapsed = 0;
        Color color = sprite.color;

        color.a = 0;
        sprite.color = color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            
            color.a = Mathf.Clamp01(elapsed / duration);
            sprite.color = color;
            
            yield return null;
        }

        color.a = 1f;
        sprite.color = color;
        _isProcess = false;
    }
    
    public void gave1() {
        if (_order1Active) {
            if (GameData.Coffees[_order1[0]] >= _order1[1]) {
                GameData.Money += GameData.CostCoffees[_order1[0]] * _order1[1];
                CaterpillarDown(caterpillar1, canvas1);
                GameData.Coffees[_order1[0]] -= _order1[1];
                StartSceneCooking.CurrentOrder = null;
                count.GetComponent<TextMeshProUGUI>().text = "" + GameData.Money;
                if (GameData.Level < GameData.CostLevelUp.Length)
                    buttonLevelUp.GetComponent<Image>().sprite = GameData.Money >= GameData.CostLevelUp[GameData.Level]
                        ? buttons[3]
                        : buttons[2];
                else buttonLevelUp.GetComponent<Image>().sprite = buttons[2];
                _order1Active = false;
                TakeCup();
                var ingredient = Random.Range(1, 3);
                var typeIng = Random.Range(0, GameData.SizeOfMenuIngredients);
                GameData.Ingredients[typeIng] += ingredient;
                AddTaskToQueue(new int[] {ingredient, typeIng, GameData.CostCoffees[_order1[0]] * _order1[1]});
            }

            Debug.Log($"Нужно {_order1[1]}, у вас есть {GameData.Coffees[_order1[0]]}");
        }
        else if (!_order2Active) {
            _order1Active = true;
            canvas1.transform.Find("Button").GetComponent<Image>().sprite = buttons[1];
            StartSceneCooking.CurrentOrder = new[] { _order1[0], _order1[1] };
            _timeAccept = GameData.Minutes;
            StartCoroutine(Miss(caterpillar1, canvas1, _timeAccept));
        }
        else StartCoroutine(Error(error));
    }

    public void gave2() {
        if (_order2Active) {
            if (GameData.Coffees[_order2[0]] >= _order2[1]) {
                GameData.Money += GameData.CostCoffees[_order2[0]] * _order2[1];
                CaterpillarDown(caterpillar2, canvas2);
                GameData.Coffees[_order2[0]] -= _order2[1];
                StartSceneCooking.CurrentOrder = null;
                count.GetComponent<TextMeshProUGUI>().text = "" + GameData.Money;
                if (GameData.Level < GameData.CostLevelUp.Length)
                    buttonLevelUp.GetComponent<Image>().sprite = GameData.Money >= GameData.CostLevelUp[GameData.Level]
                        ? buttons[3]
                        : buttons[2];
                else buttonLevelUp.GetComponent<Image>().sprite = buttons[2];
                _order2Active = false;
                TakeCup();
                var ingredient = Random.Range(1, 3);
                var typeIng = Random.Range(0, GameData.SizeOfMenuIngredients);
                GameData.Ingredients[typeIng] += ingredient;
                AddTaskToQueue(new int[] {ingredient, typeIng, GameData.CostCoffees[_order2[0]] * _order2[1]});
            }

            Debug.Log($"Нужно {_order2[1]}, у вас есть {GameData.Coffees[_order2[0]]}");
        }
        else if (!_order1Active) {
            _order2Active = true;
            canvas2.transform.Find("Button").GetComponent<Image>().sprite = buttons[1];
            StartSceneCooking.CurrentOrder = new[] { _order2[0], _order2[1] };
            _timeAccept = GameData.Minutes;
            StartCoroutine(Miss(caterpillar2, canvas2, _timeAccept));
        }
        else StartCoroutine(Error(error));
    }

    public void AddTaskToQueue(int[] taskData)
    {
        _tasks.Enqueue(taskData);
        
        if (!_isProcessing)
        {
            StartCoroutine(ProcessQueue());
        }
    }
    
    IEnumerator Miss(GameObject caterpillar, GameObject canvas, int timeA) {
        Debug.Log("Заказ принят");
        while (GameData.Minutes - timeA <= 55 && (_order1Active || _order2Active)) {
            if (GameData.Minutes - timeA >= 45) {
                Debug.Log("Время вышло");
                CaterpillarDown(caterpillar, canvas);
                _miss++;
                StartSceneCooking.CurrentOrder = null;
                _order1Active = false;
                _order2Active = false;
            }
            // Debug.Log($"Времени прошло: {GameData.Minutes - timeA}");
            yield return new WaitForSeconds(1f);
        }
    }

    private void RemoveOrder(GameObject caterpillar, GameObject canvas) {
        CaterpillarDown(caterpillar, canvas);
        _miss++;
        StartSceneCooking.CurrentOrder = null;
        _order1Active = false;
        _order2Active = false;
    }
    
    private IEnumerator ProcessQueue()
    {
        _isProcessing = true;

        while (_tasks.Count > 0)
        {
            int[] taskData = _tasks.Dequeue();
            yield return StartCoroutine(Notice(taskData[0],  taskData[1], taskData[2]));
        }

        _isProcessing = false;
    }
    private IEnumerator Notice(int count, int typeIng, int money) {
        Debug.Log($"Уведомление запущено: {GameData.CostCoffees[_order1[0]]} X {_order1[1]} денег и {count} ингредиентов.");
        notice[0].transform.Find("Coins").GetComponent<TextMeshProUGUI>().text = money.ToString();
        notice[1].transform.Find("IngCount").GetComponent<TextMeshProUGUI>().text = count.ToString();
        notice[1].transform.Find("IngType").GetComponent<Image>().sprite = ingSprites[typeIng];
        StartCoroutine(MoveToRoutine(notice[0], 160, -830, 2f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveToRoutine(notice[1], 60, -830, 2f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(MoveToRoutine(notice[0], 160, -1100, 2f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveToRoutine(notice[1], 60, -1100, 2f));
    }
    
    private IEnumerator ShowScoreList() {

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
                ScoreImage[i].color = new Color(ScoreImage[i].color.r, ScoreImage[i].color.g, ScoreImage[i].color.b, 1f);
                ScoreImage[j].sprite = ingSprites[i];
                ScoreText[j].text = prevIng[i].ToString();
                j++;
            }
        }
        yield return null;
    }

    public void CloseScoreList() {
        StartCoroutine(MoveToRoutine(ScoreList, 0, 1500, 1f));
        _scoreListActive = false;
        worldCanvas.GetComponent<GraphicRaycaster>().enabled = true;
    }

    private IEnumerator Error(GameObject obj) {
        StartCoroutine(MoveToRoutine(obj, 500, 0, 1f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(MoveToRoutine(obj, 600, 0, 1f));
    }
    
    public void GoCook() {
        SceneManager.LoadScene("SceneCooking", LoadSceneMode.Additive);
        sceneContent.SetActive(false);
    }
    
    private void OnEnable() {
        StartSceneCooking.OnBookClosed += MyMethodToRun;
    }
    private void OnDisable() {
        StartSceneCooking.OnBookClosed -= MyMethodToRun;
    }
    private void MyMethodToRun() {
        sceneContent.SetActive(true);
        TakeCup();
        if (!OrderBeRemove) return; if (_order1Active) RemoveOrder(caterpillar1, canvas1); else RemoveOrder(caterpillar2, canvas2);
    }

    public void ShowLevelUpList() {
        StartCoroutine(MoveToRoutine(levelUpList, 0, 0, 1f));
        if (GameData.Level < GameData.CostLevelUp.Length)
            levelUpCost.GetComponent<TextMeshProUGUI>().text = GameData.CostLevelUp[GameData.Level].ToString();
        else levelUpCost.GetComponent<TextMeshProUGUI>().text = "MaxLvl";
        worldCanvas.GetComponent<GraphicRaycaster>().enabled = false;
    }

    public void CloseLevelUpList() {
        StartCoroutine(MoveToRoutine(levelUpList, 1000, 0f, 1f));
        worldCanvas.GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void BuyLevelUp() {
        
        if (GameData.Level < GameData.CostLevelUp.Length && GameData.Money >= GameData.CostLevelUp[GameData.Level]) {
            GameData.LevelUp();
            ShowLevelUpList();
            if (GameData.Level < GameData.CostLevelUp.Length)
                buttonLevelUp.GetComponent<Image>().sprite = GameData.Money >= GameData.CostLevelUp[GameData.Level]
                    ? buttons[3]
                    : buttons[2];
            else buttonLevelUp.GetComponent<Image>().sprite = buttons[2];
        }
        else StartCoroutine(Error(errorLvlUp));
    }

    private void Cheat() {
        GameData.Money += 1000;
        count.GetComponent<TextMeshProUGUI>().text = "" + GameData.Money;
    }

    public void TakeCup() {
        int index = 0;
        foreach (var i in cups)
            i.SetActive(false);
        for (int i = 0; i < (GameData.Coffees.Length > 3 ? 3 : GameData.Coffees.Length); i++) {
            int j = GameData.Coffees[i];
            while (j > 0) {
                cups[index].GetComponent<Image>().sprite = coffeeSprites[i];
                cups[index].SetActive(true);
                j--;
                index++;
            }
        }
    }
}