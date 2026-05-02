using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private void Update()
    {
        // Летит влево точно так же как трубы
        transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
    }
}