using UnityEngine;
using System.Collections.Generic; 
public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public Projectile missilePrefab;
    public float missileAttackRate = 1.00f;
    private Vector3 _direction = Vector2.right;
    public AnimationCurve speed;
    public int amountKilled { get; private set; }
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    private List<Invader> activeInvaders = new List<Invader>(); 

    private void Awake()
    {
        for (int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < this.columns; col++)
            {
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
                activeInvaders.Add(invader); 
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void MissileAttack()
    {
        foreach (Transform invaderTransform in this.transform)
        {
            if (!invaderTransform.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1 / (float)this.amountAlive))
            {
                
                Invader invader = invaderTransform.GetComponent<Invader>();

                if (invader != null) 
                {
                    Instantiate(this.missilePrefab, invaderTransform.position, Quaternion.identity);
                    invader.PlayShoot(); 
                }

                break;
            }
        }
    }


    private void Update()
    {
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 5.0f))
            {
                AdvanceRow();
            }
            else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 5.0f))
            {
                AdvanceRow();
            }
        }

        
        if (Input.GetKeyDown(KeyCode.M))
        {
            KillRandomInvader();
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void InvaderKilled()
    {
        this.amountKilled++;
    }

    private void KillRandomInvader()
    {
        if (activeInvaders.Count > 0)
        {
            int randomIndex = Random.Range(0, activeInvaders.Count);
            Invader selectedInvader = activeInvaders[randomIndex];

            if (selectedInvader.gameObject.activeInHierarchy)
            {
                selectedInvader.Die();
                activeInvaders.RemoveAt(randomIndex);
            }
        }
    }
}

