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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ModifyHealth(-10);
    }


    public void ModifyHealth(int amount)
    {
        hp += amount;

        float hpPercent = (float)hp / MAX_HEALTH;
        OnHealthChanged(hpPercent); // update health bar

        if (hp <= 0) // check if dead
        {
            OnDeath();
        }
    }


    protected virtual void OnDeath()
    {
        // What happens when something dies
        // if(gameObject.name == "Enemy")
        // {
        //     GameStats.score++;
        //     // Debug.Log("Score!");
        // }
        Destroy(gameObject);
    }


    private void OnDisable()
    {
        OnHealthRemoved(this);
    }
}
