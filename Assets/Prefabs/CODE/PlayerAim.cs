using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform[] weaponTransforms;
    public Camera mainCamera;
    public GameObject bullet;
    public Transform bulletTransform;
    private float timer;
    public float fireRate = 0.3f;
    public float swordDistance = 1.0f;
    public int currentWeapon = 0;
    private bool isPerformingAction = false;
    public int pistolAmmo = 12;
    public int rifleAmmo = 35;
    private float reloadTimer = 0f;
    private bool isReloading = false;

    private void Start()
    {
        weaponTransforms = new Transform[3]; // Initialize an array to hold up to 3 weapons.
        weaponTransforms[0] = transform.Find("Aim/Sword");
        weaponTransforms[1] = transform.Find("Aim/Pistol");
        weaponTransforms[2] = transform.Find("Aim/Rifle");

        // Ensure the current weapon is within the valid range.
        currentWeapon = Mathf.Clamp(currentWeapon, 0, weaponTransforms.Length - 1);

        // Disable all weapons except the current one.
        for (int i = 0; i < weaponTransforms.Length; i++)
        {
            weaponTransforms[i].gameObject.SetActive(i == currentWeapon);
        }
    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();
        HandleReloading();

        // Switch weapons when the player presses numbers 1-3.
        for (int i = 0; i < weaponTransforms.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchWeapon(i);
            }
        }
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        // Set the position of the weapon based on the mouse direction and distance.
        Vector3 weaponPosition = transform.position + aimDirection * swordDistance;
        weaponTransforms[currentWeapon].position = weaponPosition;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Flip the weapon based on mouse position relative to the player.
        if (mousePosition.x < transform.position.x)
        {
            angle += 180f; // Rotate by 180 degrees if mouse is to the left of the player
            Vector3 currentScale = weaponTransforms[currentWeapon].localScale;
            weaponTransforms[currentWeapon].localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z); // Multiply by -1 on the x-axis
        }
        else
        {
            Vector3 currentScale = weaponTransforms[currentWeapon].localScale;
            weaponTransforms[currentWeapon].localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z); // Reset the scale
        }

        weaponTransforms[currentWeapon].eulerAngles = new Vector3(0, 0, angle);
    }

    // private void HandleShooting()
    // {
    //     if (isPerformingAction)
    //     {
    //         timer += Time.deltaTime;
    //         if (timer >= fireRate)
    //         {
    //             timer = 0;
    //         }
    //     }

    //     if (Input.GetMouseButtonDown(0) && !isPerformingAction)
    //     {
    //         if (currentWeapon == 0) // Sword or Stick
    //         {
    //             StartCoroutine(SwingSword());
    //         }
    //         else // Pistol or Rifle
    //         {
    //             Vector3 gunEndPosition = weaponTransforms[currentWeapon].position;
    //             Vector3 aimDirection = (GetMouseWorldPosition() - transform.position).normalized;
                
    //             // Calculate the side of the player based on the mouse position
    //             float playerSide = Mathf.Sign(GetMouseWorldPosition().x - transform.position.x);
    //             Vector3 offset = playerSide * Vector3.right * swordDistance;

    //             // Calculate the bullet spawn position
    //             gunEndPosition += offset;

    //             // Instantiate the bullet at the calculated spawn position and rotation
    //             Instantiate(bullet, gunEndPosition, bulletTransform.rotation);
    //             StartCoroutine(KickbackWeapon());
    //         }
    //     }
    // }
    private void HandleShooting()
    {
        if (isPerformingAction || isReloading)
        {
            timer += Time.deltaTime;
            if (timer >= fireRate)
            {
                timer = 0;
                isPerformingAction = false;
            }
            return;
        }

        if (currentWeapon == 0 && Input.GetMouseButtonDown(0)) // Sword or Stick
        {
            StartCoroutine(SwingSword());
        }
        else if (currentWeapon == 1 && Input.GetMouseButtonDown(0) && pistolAmmo > 0) // Pistol
        {
            FireBullet();
            pistolAmmo--;
        }
        else if (currentWeapon == 2 && Input.GetMouseButton(0) && rifleAmmo > 0) // Rifle
        {
            FireBullet();
            rifleAmmo--;
        }
    }

    private void HandleReloading()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            reloadTimer = 1f; // Set reload time to 1 second
        }

        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                isReloading = false;
                if (currentWeapon == 1) pistolAmmo = 12; // Reset pistol ammo
                if (currentWeapon == 2) rifleAmmo = 35; // Reset rifle ammo
            }
        }
    }

    private void FireBullet()
    {
        Vector3 gunEndPosition = weaponTransforms[currentWeapon].position;
        Vector3 aimDirection = (GetMouseWorldPosition() - transform.position).normalized;
        
        // Calculate the side of the player based on the mouse position
        float playerSide = Mathf.Sign(GetMouseWorldPosition().x - transform.position.x);
        Vector3 offset = playerSide * Vector3.right * swordDistance;

        // Calculate the bullet spawn position
        gunEndPosition += offset;

        // Instantiate the bullet at the calculated spawn position and rotation
        Instantiate(bullet, gunEndPosition, bulletTransform.rotation);
        StartCoroutine(KickbackWeapon());
        isPerformingAction = true;
        timer = 0;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    public Transform GetCurrentWeaponTransform()
    {
        return weaponTransforms[currentWeapon];
    }

    private void SwitchWeapon(int newWeapon)
    {
        if (newWeapon < 0 || newWeapon >= weaponTransforms.Length)
        {
            return;
        }

        // Disable the current weapon and enable the new one.
        weaponTransforms[currentWeapon].gameObject.SetActive(false);
        currentWeapon = newWeapon;
        weaponTransforms[currentWeapon].gameObject.SetActive(true);
    }

    private IEnumerator SwingSword()
    {
        isPerformingAction = true;

        float swingDuration = 0.15f; // Adjust the duration of the swing
        float elapsedTime = 0.1f;

        while (elapsedTime < swingDuration)
        {
            float t = elapsedTime / swingDuration;
            // You can adjust the swing motion here. This example uses Quaternion.Lerp.
            weaponTransforms[currentWeapon].localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 90, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Detect enemies in range of the sword
        RaycastHit2D[] hits = Physics2D.RaycastAll(weaponTransforms[currentWeapon].position, weaponTransforms[currentWeapon].right, swordDistance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) // Make sure to set the correct tag for your enemies
            {
                // Apply damage to the enemy here
                EnemyScript enemy = hit.collider.GetComponent<EnemyScript>(); // Use 'hit' instead of 'collision'
                if (enemy != null)
                {
                    enemy.TakeDamage(); // Make sure your EnemyScript has a TakeDamage method
                }
            }
        }

        // Reset the weapon's rotation after the action is complete.
        weaponTransforms[currentWeapon].localRotation = Quaternion.identity;
        isPerformingAction = false;
    }

    private IEnumerator KickbackWeapon()
    {
        isPerformingAction = true;

        float kickbackDuration = 0.15f; // Adjust the duration of the kickback
        float elapsedTime = 0.1f;

        while (elapsedTime < kickbackDuration)
        {
            float t = elapsedTime / kickbackDuration;
            // You can adjust the kickback motion here. This example uses a local position change.
            weaponTransforms[currentWeapon].localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0.01f, 0, 0), t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the weapon's position after the action is complete.
        weaponTransforms[currentWeapon].localPosition = Vector3.zero;
        isPerformingAction = false;
    }
}

