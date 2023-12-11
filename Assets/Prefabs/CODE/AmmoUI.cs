using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public PlayerAim playerAim;
    private TMP_Text ammoText;

    private void Start()
    {
        ammoText = GetComponent<TMP_Text>();
        playerAim = GameObject.FindWithTag("Player").GetComponent<PlayerAim>();
        if (playerAim == null)
        {
            Debug.LogError("PlayerAim script not assigned!");
        }
    }

    private void Update()
    {
        if (playerAim != null)
        {
            int currentAmmo = 0;
            if (playerAim.currentWeapon == 1) // Pistol
            {
                currentAmmo = playerAim.pistolAmmo;
            }
            else if (playerAim.currentWeapon == 2) // Rifle
            {
                currentAmmo = playerAim.rifleAmmo;
            }

            ammoText.text = currentAmmo.ToString();
        }
    }
}
