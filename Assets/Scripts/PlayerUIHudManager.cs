using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] public UI_StatBar healthBar;
    [SerializeField] public UI_StatBar staminaBar;

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(true);
    }

    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }  

    public void SetNewStaminaValue(int oldValue, int newValue)
    {
        staminaBar.SetStat(newValue);
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }


}
