using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    public RectTransform[] textObjects;
    public float scrollSpeed = 50f; 
    public float disappearYPosition = 500f; 
    public float sceneDuration = 5f; 

    private void Start()
    {
        StartCoroutine(ScrollText());
        StartCoroutine(LogAfterTenSeconds());
    }

    private IEnumerator ScrollText()
    {
        while (true)
        {
            for (int i = 0; i < textObjects.Length; i++)
            {
                if (textObjects[i] != null)
                {
                    textObjects[i].anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

                    if (textObjects[i].anchoredPosition.y > disappearYPosition)
                    {
                        textObjects[i].gameObject.SetActive(false); 
                    }
                }
            }
            yield return null; 
        }
    }

    private IEnumerator LogAfterTenSeconds()
    {
        yield return new WaitForSeconds(sceneDuration);
        LoadGameScene();
    }

    public void LoadGameScene()
    {
        StartCoroutine(_LoadGameScene());

        IEnumerator _LoadGameScene()
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync("SampleScene");
            while (!loadOp!.isDone) yield return null;
            GameObject Player = GameObject.Find("Player");
            Debug.Log(Player.name + "yay");
        }
        

    }
}

