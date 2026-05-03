using System.Collections;
using UnityEngine;

[System.Serializable]
public class PipeSpawnSettings
{
    public GameObject pipePrefab;
    public float spawnDelay = 2f;
    public bool useRandomScale = false;
    public float minScale = 1f;  // Минимальный пропорциональный масштаб
    public float maxScale = 1f;  // Максимальный пропорциональный масштаб
}

public class Spawner : MonoBehaviour
{
    [Header("Настройки спавна труб")]
    [SerializeField] private PipeSpawnSettings[] pipeSettings;
    
    [Header("Настройки позиции")]
    [SerializeField] private float minYOffset = -2f;
    [SerializeField] private float maxYOffset = 2f;
    
    private void Start()
    {
        if (pipeSettings.Length == 0)
        {
            Debug.LogError("Не назначены настройки труб!");
            return;
        }
        
        // Проверяем, что все префабы назначены
        foreach (var setting in pipeSettings)
        {
            if (setting.pipePrefab == null)
            {
                Debug.LogError("Обнаружен неназначенный префаб трубы!");
                return;
            }
        }
        
        // Запускаем корутину для каждого типа труб
        for (int i = 0; i < pipeSettings.Length; i++)
        {
            StartCoroutine(SpawnPipe(i));
        }
    }
    
    private IEnumerator SpawnPipe(int index)
    {
        PipeSpawnSettings settings = pipeSettings[index];
        
        while (true)
        {
            yield return new WaitForSeconds(settings.spawnDelay);
            
            GameObject pipeObj = Instantiate(settings.pipePrefab);
            
            // Устанавливаем позицию
            pipeObj.transform.position = transform.position + new Vector3(0, Random.Range(minYOffset, maxYOffset));
            
            // Устанавливаем случайный пропорциональный размер, если включено
            if (settings.useRandomScale)
            {
                float randomScale = Random.Range(settings.minScale, settings.maxScale);
                pipeObj.transform.localScale = new Vector3(randomScale, randomScale, 1f);
            }
        }
    }
}