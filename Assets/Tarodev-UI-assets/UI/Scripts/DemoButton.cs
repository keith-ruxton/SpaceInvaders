using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DemoButton : MonoBehaviour
{
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGameScene()
    {
        StartCoroutine(_LoadGameScene());

        IEnumerator _LoadGameScene()
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync("Game");
            while (!loadOp!.isDone) yield return null;
            GameObject Player = GameObject.Find("Player");
            Debug.Log(Player.name + "yay");
        }
        

    }
}
