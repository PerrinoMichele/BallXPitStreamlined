using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthPoints;

    public void TakeDamage(int damage)
    {
        if (healthPoints > 1)
        {
            healthPoints -= damage;
        }
        else { Destroy(gameObject); }
    }
}
