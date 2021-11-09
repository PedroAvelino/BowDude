using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] int SceneIndex = 1;

    public void LoadScene( )
    {
        SceneManager.LoadScene( SceneIndex );
    }
}
