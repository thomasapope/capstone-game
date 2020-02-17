using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;
    
    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    [SerializeField]
    private float positionOffset = 2;

    private Camera mainCamera;

    private Health health;

    
    public void SetHealth(Health health) 
    {
        this.health = health;
        health.OnHealthChanged += HandleHealthChanged;
    }


    public void SetCamera(Camera mainCamera)
    {
        this.mainCamera = mainCamera;
    }


    private void Start()
    {
        // GetComponentInParent<Health>().OnHealthChanged += HandleHealthChanged;
        // Creature p = GetComponentInParent<Player>().OnHealthChanged += HandleHealthChanged;
        // Creature p = gameObject.GetComponent<Creature>();
        // p.OnHealthChanged += HandleHealthChanged;

        //transform.root.gameObject.GetComponent<Creature>().OnHealthChanged += HandleHealthChanged;
    }


    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }


    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        foregroundImage.fillAmount = pct;
    }


    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        //transform.Rotate(0, 180, 0);
        transform.position = health.transform.position + Vector3.up * positionOffset;
        //transform.position = Camera.main.WorldToScreenPoint(health.transform.position + Vector3.up * positionOffset);
        //transform.position = Camera.main.WorldToScreenPoint(health.transform.position);
        // transform.position = health.transform.position;
    }


    private void OnDestroy() 
    {
        health.OnHealthChanged -= HandleHealthChanged;
    }
}
