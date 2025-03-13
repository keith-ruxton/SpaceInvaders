using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D), typeof(AudioSource))] 
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;
    private bool _laserActive;
    private bool isDestroyed = false;

    private SpriteRenderer spriteRenderer;
    public Sprite destructionFrame1;
    public Sprite destructionFrame2;
    public Sprite destructionFrame3;
    public Sprite Sdefault;

    public Sprite shootFrame1;
    public Sprite shootFrame2;
    public Sprite shootFrame3;

    [Header("Sound Effects")]
    public AudioClip shootSound;  
    public AudioClip deathSound;  
    private AudioSource audioSource;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); 
    }

    private void Update()
    {
        if (isDestroyed) return;

        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += speed * Time.deltaTime;
        }

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            DestroyPlayer();
        }
    }

    public void Shoot()
    {
        PlaySound(shootSound); 
        StartCoroutine(PlayShootAnimation());

        if (!_laserActive)
        {
            Projectile projectile = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestroyed;
            _laserActive = true;
        }
    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    public void DestroyPlayer()
    {
        if (isDestroyed) return;

        isDestroyed = true;
        PlaySound(deathSound); 
        StartCoroutine(PlayDestructionAnimation());
    }

    private IEnumerator PlayDestructionAnimation()
    {
        spriteRenderer.sprite = destructionFrame1;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = destructionFrame2;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = destructionFrame3;
        yield return new WaitForSeconds(0.1f);

        LoadGameScene();
        gameObject.SetActive(false);
    }

    private IEnumerator PlayShootAnimation()
    {
        spriteRenderer.sprite = shootFrame1;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = shootFrame2;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = shootFrame3;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = Sdefault;
        yield return new WaitForSeconds(0.1f);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void LoadGameScene()
    {
        StartCoroutine(_LoadGameScene());

        IEnumerator _LoadGameScene()
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync("Credits");
            while (!loadOp!.isDone) yield return null;
            GameObject Player = GameObject.Find("Player");
            Debug.Log(Player.name + "yay");
        }
    }
}


