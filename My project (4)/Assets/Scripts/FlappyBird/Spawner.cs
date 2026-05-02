using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Префабы для спавна")]
    [SerializeField] private GameObject[] pipes;
    
    [Header("Интервалы спавна")]
    [SerializeField] private float[] spawnDelays = new float[] { 2f, 3f, 4f };
    
    [Header("Настройки позиции")]
    [SerializeField] private float minYOffset = -2f;
    [SerializeField] private float maxYOffset = 2f;
    
    private void Start()
    {
        if (pipes.Length == 0)
        {
            Debug.LogError("Не назначены префабы труб!");
            return;
        }
        
        if (spawnDelays.Length != pipes.Length)
        {
            Debug.LogError("Количество задержек должно совпадать с количеством префабов!");
            return;
        }
        
        // Запускаем корутину для каждого типа труб
        for (int i = 0; i < pipes.Length; i++)
        {
            StartCoroutine(SpawnPipe(i));
        }
    }
    
    private IEnumerator SpawnPipe(int pipeIndex)
    {
        while (true)
        {
            // Используем WaitForSeconds, который автоматически учитывает Time.timeScale
            yield return new WaitForSeconds(spawnDelays[pipeIndex]);
            
            GameObject pipeObj = Instantiate(pipes[pipeIndex]);
            pipeObj.transform.position = transform.position + new Vector3(0, Random.Range(minYOffset, maxYOffset));
        }
    }
}