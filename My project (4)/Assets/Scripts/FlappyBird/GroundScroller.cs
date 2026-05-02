using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float resetPositionX = -17.8f; // Когда уходит за экран
    [SerializeField] private float startPositionX = 17.8f;  // Откуда начинать заново
    
    private void Update()
    {
        // Движение влево
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        
        // Если ушла за экран - переставляем вправо
        if (transform.position.x <= resetPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}