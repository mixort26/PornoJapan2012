using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartSceneWorkPlace : MonoBehaviour
{
    public AudioClip musicTheme;
    //Y = -6.5 -- Y = 0.15 (from to)
    public GameObject caterpillar1;
    public GameObject caterpillar2;
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject count;
    public GameObject time;
    
    private int _prevMinutes;
    private bool _endDay = false;
    private float _duration = 150f;
    private int[] _order1 = new int[2];
    private int[] _order2 = new int[2];

    private void Start() {
        count.GetComponent<TextMeshProUGUI>().text = "" + GameData.Money;
        MusicManager.Instance.PlayMusic(musicTheme);
        //Прячем всех
        canvas1.SetActive(false);
        canvas2.SetActive(false);
        caterpillar1.transform.position = new Vector2(-5.4f, -6.5f);
        caterpillar2.transform.position = new Vector2(5.4f, -6.5f);
        caterpillar1.SetActive(false);
        caterpillar2.SetActive(false);
    }

    private void Update() {
        if (_prevMinutes != GameData.Minutes) {
            if (_prevMinutes >= 1019) _endDay = true;
            time.GetComponent<TextMeshProUGUI>().text =
                (GameData.Minutes >= 600 ? 
                    "" + GameData.Minutes / 60 : "0" + GameData.Minutes / 60) + ':' +
                                                (GameData.Minutes % 60 >= 10 ?
                                                    GameData.Minutes % 60 : "0" + GameData.Minutes % 60);
            _prevMinutes = GameData.Minutes;
        }
        //Проверка на наличие посетителей, если кого-то нет то начинаем движ
        if (!caterpillar1.activeSelf || caterpillar1.transform.position.y <= -6.5) {
            CaterpillarUp(caterpillar1, canvas1, 0.15f, 1);
        }

        if (!caterpillar2.activeSelf || caterpillar2.transform.position.y <= -6.5) {
            CaterpillarUp(caterpillar2, canvas2, 0.15f, 2);
        }
    }

    private void CaterpillarUp(GameObject caterpillar, GameObject canvas, float to, int number) {
        if (!caterpillar.activeSelf) caterpillar.SetActive(true);
        StartCoroutine(MoveToRoutine(caterpillar, 0.15f));
        if (number == 1)
            StartCoroutine(ShowDialog(caterpillar, canvas, (i, i1) => _order1 = new[] { i, i1 }));
        else 
            StartCoroutine(ShowDialog(caterpillar, canvas, (i, i1) => _order2 = new[] { i, i1 }));
    }

    private void CaterpillarDown(GameObject caterpillar, GameObject canvas, float to) {
        canvas.SetActive(false);
        StartCoroutine(MoveToRoutine(caterpillar, to));
    }
    
    private IEnumerator MoveToRoutine(GameObject caterpillar, float to) {
        float elapsed = 0;
        while (caterpillar.transform.position.y < to-0.002 ||  caterpillar.transform.position.y > to+0.002) {
            float t  = elapsed / _duration;
            caterpillar.transform.position = Vector3.Lerp(caterpillar.transform.position, new Vector2(caterpillar.transform.position.x, to), t);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        caterpillar.transform.position = new Vector3(caterpillar.transform.position.x, to, 0);
    }

    private IEnumerator ShowDialog(GameObject caterpillar, GameObject canvas, System.Action<int, int> onComplete) {
        while (caterpillar.transform.position.y < 0.15f) { yield return null; }
        int typeOfCoffee = Random.Range(1, GameData.SizeOfMenu + 1);
        canvas.transform.Find("SpriteCoffee").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Coffee" + typeOfCoffee);
        int amount = Random.Range(1, 4);
        canvas.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "X " + amount;
        canvas.SetActive(true);
        onComplete?.Invoke(typeOfCoffee, amount);
    }
    public void gave1() {
        if (GameData.Coffees[_order1[0]] >= _order1[1]) {
            GameData.Money += 5*_order1[1];
            CaterpillarDown(caterpillar1, canvas1, -6.5f);
            GameData.Coffees[_order1[0]] -= _order1[1];
            count.GetComponent<TextMeshProUGUI>().text = "" + GameData.Money;
        }
        else Debug.Log($"Нужно {_order1[1]}, у вас есть {GameData.Coffees[_order1[0]]}");
    }

    public void gave2() {
        if (GameData.Coffees[_order2[0]] >= _order2[1]) {
            GameData.Money += 5*_order2[1];
            CaterpillarDown(caterpillar2, canvas2, -6.5f);
            GameData.Coffees[_order2[0]] -= _order2[1];
            count.GetComponent<TextMeshProUGUI>().text = "" + GameData.Money;
        }
        else Debug.Log($"Нужно {_order2[1]}, у вас есть {GameData.Coffees[_order2[0]]}");
    }

    public void CHEAT() {
        for (int i = 0;  i < GameData.Coffees.Length; i++)
            GameData.Coffees[i] += 5;
    }

    public void GoCook() {
        SceneManager.LoadScene("SceneCooking");
    }
}
