using UnityEngine;

public class IntroInvaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public float missileAttackRate = 1.00f;
    private Vector3 _direction = Vector2.right;
    public int amountKilled { get; private set;}
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;
    private void Awake()
    {
        for(int row = 0; row<this.rows; row++)
        {
            float width = 2.0f * (this.columns-1);
            float height = 2.0f * (this.rows-1);
            Vector2 centering = new Vector2(-width/2, -height/2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);
            for(int col = 0; col<this.columns; col++)
            {
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }
}