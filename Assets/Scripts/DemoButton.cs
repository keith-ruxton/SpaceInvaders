using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DemoButton : MonoBehaviour
{
    public GameObject Player;
    void Start()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGameScene()
    {
        StartCoroutine(_LoadGameScene());

        IEnumerator _LoadGameScene()
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync("DemoScene");
            while (!loadOp!.isDone) yield return null;
            GameObject Player = GameObject.Find("Player");
            Debug.Log(Player.name + "yay");
        }
        

    }
}
