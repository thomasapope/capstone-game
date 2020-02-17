using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBarPrefab;

    private Camera mainCamera;

    private Dictionary<Health, HealthBar> healthBars = new Dictionary<Health, HealthBar>();


    private void Awake()
    {
        mainCamera = Camera.main;

        Health.OnHealthAdded += AddHealthBar;
        Health.OnHealthRemoved += RemoveHealthBar;
    }


    private void AddHealthBar(Health health) 
    {
        if (healthBars.ContainsKey(health) == false)
        {
            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBars.Add(health, healthBar);
            healthBar.SetHealth(health);
            healthBar.SetCamera(mainCamera);
        }
    }


    private void RemoveHealthBar(Health health) 
    {
        if (healthBars.ContainsKey(health))
        {
            Destroy(healthBars[health].gameObject);
            healthBars.Remove(health);
        }
    }
}
