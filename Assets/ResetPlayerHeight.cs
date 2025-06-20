using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPlayerHeight : MonoBehaviour
{
    public float height = -10;
    void Update()
    {
        if(transform.position.y < height)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
