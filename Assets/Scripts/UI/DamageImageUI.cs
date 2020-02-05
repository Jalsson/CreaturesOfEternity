using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageImageUI : MonoBehaviour {


    private Image damageImage;

    PlayerStats playerStats;

    [SerializeField]
    private float damageUIFlashSpeed = 5f;
    [SerializeField]
    private Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    private void Start()
    {
        if (PlayerManager.S_INSTANCE.player)
        {
            damageImage = GetComponent<Image>();
            playerStats = PlayerManager.S_INSTANCE.player.GetComponent<PlayerStats>();
            playerStats.TookDamage += FlashScreen;
        }
    }

    public void FlashScreen()
    {
        StartCoroutine(FlashDamageScreen());
    }

    IEnumerator FlashDamageScreen()
    {
        damageImage.color = flashColor;
        yield return new WaitForSeconds(0.2f);

        while (damageImage.color != Color.clear)
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, damageUIFlashSpeed * Time.deltaTime);
            yield return null;
        }
        
    }
}
