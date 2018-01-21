using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public void LoadScene( string sceneName )
	{
		SceneManager.LoadScene( sceneName );
	}
}
