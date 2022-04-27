using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int shield;
    public int damageTaken;
    public bool alive;
    public Animator animator;
    public GunSystem gunSystem;

	// Start is called before the first frame update
	void Start()
    {
        animator = animator.GetComponent<Animator>();
        health = 100;
        maxHealth = 100;
        gunSystem = GameObject.Find("WeaponHolder").GetComponentInChildren<GunSystem>();
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
	public void OnCollisionEnter(Collision other)
	{
        if (other.collider.gameObject.tag == "Bullet")
        {
            damageTaken = gunSystem.bulletDamage;
        }
	}

}
