using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static event Action<Health> OnHealthAdded = delegate {};
    public static event Action<Health> OnHealthRemoved = delegate {};
    
    [SerializeField]
    private int MAX_HEALTH = 100;

    public int hp { get; private set; }

    public event Action<float> OnHealthChanged = delegate {};


    private void OnEnable()
    {
        hp = MAX_HEALTH;
        OnHealthAdded(this);
    }


    public void ModifyHealth(int amount)
    {
        hp += amount;
        // Debug.Log("I took " + amount + " damage!");

        float hpPercent = (float)hp / MAX_HEALTH;
        OnHealthChanged(hpPercent); // update health bar

        if (hp <= 0) // check if dead
        {
            OnDeath();
        }
    }


    protected virtual void OnDeath()
    {
        Debug.Log("I Died!");
        Destroy(gameObject);
    }


    private void OnDisable()
    {
        OnHealthRemoved(this);
    }
}
