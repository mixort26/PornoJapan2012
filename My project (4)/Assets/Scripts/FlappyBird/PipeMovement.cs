using System.Linq.Expressions;
using UnityEngine;
public class PipeMovemnt : MonoBehaviour
{
     [SerializeField]private float speed = 5f;
    private void Update()
    {
    transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
    }
}

