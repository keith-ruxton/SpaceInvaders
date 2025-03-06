using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;
    public System.Action killed;
    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }
    public void AnimateSprite()
    {
        _animationFrame++;
        if(_animationFrame >= this.animationSprites.Length) {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("CLOSE");

        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Debug.Log("AAHHAHAAH");

            int points = 0;

            switch (this.gameObject.tag)
            {
                case "Invader1":
                    points = 10;
                    break;
                case "Invader2":
                    points = 20;
                    break;
                case "Invader3":
                    points = 30;
                    break;
                case "Invader4":
                    points = 40;
                    break;
            }

            if (points > 0)
            {
                GameManager.Instance.AddScore(points);
            }

        this.killed?.Invoke();
        Destroy(other.gameObject);
        this.gameObject.SetActive(false);
    }
}

}
