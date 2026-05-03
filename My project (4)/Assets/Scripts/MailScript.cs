using UnityEngine;
using UnityEngine.SceneManagement;

public class MailScript : MonoBehaviour
{void Update()
    {
        if(Input.anyKeyDown)
            SceneManager.LoadScene("SceneWorkPlace");
    }
}
