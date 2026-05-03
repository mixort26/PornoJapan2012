using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private float leftBoundary = -15f; // X-координата, за которой объект считается "ушедшим"

    void Update()
    {
        // Если объект улетел левее границы
        if (transform.position.x < leftBoundary)
        {
            Destroy(gameObject);
        }
    }
}