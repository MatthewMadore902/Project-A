using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int shield;
    public int damageTaken;
    public int damage;
    public bool alive;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        maxHealth = 100;

    }

    // Update is called once per frame
    void Update()
    {
        health = health - damageTaken;
        if (health < 0)
        {
            health = 0;
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health == 0)
        {
            
        }

    }
}
