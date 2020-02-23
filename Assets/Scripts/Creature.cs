using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
    Creature.cs

    The superclass of all living creatures. This is not meant to be used
    as is, but to be inherited from.
*/

// [RequireComponent(typeof(Health))]
public abstract class Creature : MonoBehaviour
{
    // Component References
    // public Health stats;
    Renderer rend;
    

    // Stats
    [SerializeField]
    private int MAX_HEALTH = 100;

    public int hp;

    protected bool hitting;
    public int attackDamage = 10;

    public Animator attackAnimator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask attackLayers; // The layers this creature can deal damage to
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    // Other Stats
    private float hitTime = 1f;
    private Material defMat;
    public static Material hitMat;

    protected virtual void Start()
    {
        // stats = gameObject.GetComponent<Health>();
        rend = GetComponent<Renderer> ();
        defMat = rend.material;

        if (hitMat == null)
            hitMat = Resources.Load<Material>("HitMat");
    }

    // Delegates
    public static event Action<Creature> OnHealthAdded = delegate {};
    public static event Action<Creature> OnHealthRemoved = delegate {};
    public event Action<float> OnHealthChanged = delegate {};


    private void OnEnable()
    {
        hp = MAX_HEALTH;
        OnHealthAdded(this);
    }
    
    
    protected virtual void Update()
    {
        // Attacking
        if (Time.time >= nextAttackTime)
        {
            if (hitting)
            {
                // Debug.Log("Hitting");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        // Hit feedback
        if (hitTime < 1)
        {
            hitTime += Time.deltaTime;
        }
        else
        {
            if (rend.material != defMat)
            {
                rend.material = defMat;
            }
        }
    }


    void Attack()
    {
        hitting = false;

        if (attackAnimator != null)
        {
            attackAnimator.SetTrigger("swing");
        }

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, attackLayers);
        
        if(hits.Length == 0) return;
        // Debug.Log("There are " + hits.Length);

        foreach (Collider enemy in hits)
        {
            // Debug.Log(enemy.name);
            enemy.GetComponent<Creature>().TakeDamage(attackDamage);
        }
    }


    public void TakeDamage(int damage)
    {
        Debug.Log(name + " took " + damage + " damage!");
        ModifyHealth(damage * -1);
        
        hitTime = 0;
        rend.material = hitMat;
    }

    
    public void ModifyHealth(int amount)
    {
        hp += amount;
        UpdateHealthBar();
    }


    protected virtual void OnDeath()
    {
        Debug.Log("I Died!");
        Destroy(gameObject);
    }


    private void UpdateHealthBar()
    {
        float hpPercent = (float)hp / MAX_HEALTH;
        OnHealthChanged(hpPercent); // update health bar

        if (hp <= 0) // check if dead
        {
            OnDeath();
        }
    }

    
    protected virtual void OnDisable()
    {
        OnHealthRemoved(this);
    }

    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public int health 
    {
        get { return hp; }
        set { hp = value; }
    }
}
