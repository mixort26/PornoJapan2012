using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneCooking : MonoBehaviour
{
    public static GameObject[] Locks = new GameObject[3];
    public Sprite[] coffeeSprites = new Sprite[5];
    public GameObject spriteCoffee;
    public GameObject countCoffee;
    public GameObject buttonCollect;
    public TextMeshProUGUI[] counts = new TextMeshProUGUI[4];
    public GameObject buttonToRemove;
    public static event Action OnBookClosed;
    private AudioSource audioSource;
    public static int[] CurrentOrder;
    private int _prevBeans = GameData.Ingredients[0]-1;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < 3; i++) {
            Locks[i] = GameObject.Find("Lock" + i);
            Locks[i].SetActive(true);
        } for (int i = 0; i < GameData.Level; i++) 
            Locks[i].SetActive(false);

        spriteCoffee.SetActive(false);
        countCoffee.SetActive(false);
        buttonCollect.SetActive(false);
        buttonToRemove.SetActive(false);
        ShowOrder();
        if (CurrentOrder != null) Debug.Log($"Order {CurrentOrder[0]} X {CurrentOrder[1]}");
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) GoWork();
        if (_prevBeans != GameData.Ingredients[0]) {
            for (int i = 0; i < GameData.SizeOfMenuIngredients; i++) {
                counts[i].text = GameData.Ingredients[i].ToString();
            }
            _prevBeans = GameData.Ingredients[0];
        }
    }

    public void removeOrder() {
        StartSceneWorkPlace.OrderBeRemove = true;
        HideOrder();
    }
    
    private void ShowOrder() {
        if (CurrentOrder != null) {
            spriteCoffee.GetComponent<Image>().sprite = coffeeSprites[CurrentOrder[0]];
            countCoffee.GetComponent<TextMeshProUGUI>().text = "" + CurrentOrder[1];
            spriteCoffee.SetActive(true);
            countCoffee.SetActive(true);
            buttonCollect.SetActive(true);
            buttonToRemove.SetActive(true);
        }
    }
    
    private void HideOrder() {
        spriteCoffee.SetActive(false);
        countCoffee.SetActive(false);
        buttonCollect.SetActive(false);
        buttonToRemove.SetActive(false);
    }

    public void Collect() {
        switch (CurrentOrder[0]) {
            case 0:
                if (GameData.Ingredients[0] - 2 * CurrentOrder[1] >= 0) {
                    GameData.Ingredients[0] -= 2 * CurrentOrder[1];
                    GameData.Coffees[CurrentOrder[0]] += CurrentOrder[1];
                }

                break;
            case 1:
                if (GameData.Ingredients[0] - 2 * CurrentOrder[1] >= 0 &&
                    GameData.Ingredients[1] - 1 * CurrentOrder[1] >= 0) {
                    GameData.Ingredients[0] -= 2 * CurrentOrder[1];
                    GameData.Ingredients[1] -= 1 * CurrentOrder[1];
                    GameData.Coffees[CurrentOrder[0]] += CurrentOrder[1];
                }

                break;
            case 2:
                if (GameData.Ingredients[0] - 1 * CurrentOrder[1] >= 0 &&
                    GameData.Ingredients[1] - 2 * CurrentOrder[1] >= 0) {
                    GameData.Ingredients[0] -= 1 * CurrentOrder[1];
                    GameData.Ingredients[1] -= 2 * CurrentOrder[1];
                    GameData.Coffees[CurrentOrder[0]] += CurrentOrder[1];
                }

                break;
            case 3:
                if (GameData.Ingredients[0] - 1 * CurrentOrder[1] >= 0 &&
                    GameData.Ingredients[1] - 1 * CurrentOrder[1] >= 0 &&
                    GameData.Ingredients[2] - 1 * CurrentOrder[1] >= 0) {
                    GameData.Ingredients[0] -= 1 * CurrentOrder[1];
                    GameData.Ingredients[1] -= 1 * CurrentOrder[1];
                    GameData.Ingredients[2] -= 1 * CurrentOrder[1];
                    GameData.Coffees[CurrentOrder[0]] += CurrentOrder[1];
                }

                break;
            case 4:
                if (GameData.Ingredients[0] - 1 * CurrentOrder[1] >= 0 &&
                    GameData.Ingredients[1] - 1 * CurrentOrder[1] >= 0 &&
                    GameData.Ingredients[3] - 1 * CurrentOrder[1] >= 0) {
                    GameData.Ingredients[0] -= 1 * CurrentOrder[1];
                    GameData.Ingredients[1] -= 1 * CurrentOrder[1];
                    GameData.Ingredients[3] -= 1 * CurrentOrder[1];
                    GameData.Coffees[CurrentOrder[0]] += CurrentOrder[1];
                    StartSceneWorkPlace.Final = CurrentOrder[1];
                }

                break;
        }
        
        if (GameData.Coffees[CurrentOrder[0]] >= CurrentOrder[1]) {
            audioSource.PlayOneShot(audioSource.clip);
            HideOrder();
        }
    }

    public void GoWork() {
        OnBookClosed?.Invoke();
        SceneManager.UnloadSceneAsync("SceneCooking");
    }
}