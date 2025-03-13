using UnityEngine;
using System.Collections;

public class Invader : MonoBehaviour
{
    public delegate void InvaderKilledEvent();
    public Sprite Sdefault;

    public Sprite shootFrame1;
    public Sprite shootFrame2;
    public Sprite shootFrame3;

    public Sprite deathFrame1;
    public Sprite deathFrame2;
    public Sprite deathFrame3;
    public Sprite[] animationSprites; 
    public float animationTime = 1.0f;
    public System.Action killed;

    private SpriteRenderer spriteRenderer;
    private int _animationFrame;
    private bool isAnimating = true;

    [Header("Sound Effects")]
    public AudioClip shootSound; 
    public AudioClip deathSound;  
    private AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); 
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    public void Die()
    {
        if (!isAnimating) return; 
        isAnimating = false;
        StopCoroutine(PlayShootAnimation());

        PlaySound(deathSound);

        StartCoroutine(PlayDeathAnimation());
    }

    public void PlayShoot()
    {
        if (!isAnimating) return; 
        StopCoroutine(AnimateSpriteCoroutine());

        PlaySound(shootSound);

        StartCoroutine(PlayShootAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        spriteRenderer.sprite = deathFrame1;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = deathFrame2;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = deathFrame3;
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
        killed?.Invoke();
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
        StartCoroutine(AnimateSpriteCoroutine()); 
    }

    private void AnimateSprite()
    {
        if (!isAnimating) return; 

        _animationFrame++;
        if (_animationFrame >= animationSprites.Length)
        {
            _animationFrame = 0;
        }

        spriteRenderer.sprite = animationSprites[_animationFrame];
    }

    private IEnumerator AnimateSpriteCoroutine()
    {
        while (isAnimating)
        {
            AnimateSprite();
            yield return new WaitForSeconds(animationTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            int points = GetPointsForTag(this.tag);
            if (points > 0)
            {
                GameManager.Instance.AddScore(points);
            }

            killed?.Invoke();
            Destroy(other.gameObject);
            Die(); 
        }
    }

    private int GetPointsForTag(string tag)
    {
        switch (tag)
        {
            case "Invader1": return 10;
            case "Invader2": return 20;
            case "Invader3": return 30;
            case "Invader4": return 40;
            default: return 0;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}


