using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (_instance == null) {
            GameObject go = new GameObject("GlobalTimeManager");
            _instance = go.AddComponent<TimeManager>();
            DontDestroyOnLoad(go);
        }
    }

    private void Start() {
        StartCoroutine(Aaa());
    }
    
    private IEnumerator Aaa() {
        while (true) {
            yield return new WaitForSeconds(0.1f);
            GameData.Minutes++;
        }
    }
}
