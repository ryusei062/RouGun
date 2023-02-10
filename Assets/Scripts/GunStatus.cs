using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Gun", menuName = "Gun/CreateGun")]
public class GunStatus : ScriptableObject
{
    [SerializeField]
    private Sprite gunIcon;
    [SerializeField]
    private string gunName;
    [SerializeField]
    private string gunInfomation; 
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private float knockback;
    [SerializeField]
    private float shotCooldown;
    [SerializeField]
    private bool bulletBound;
    [SerializeField]
    private bool fullAuto;
    [SerializeField]
    private int gunID;

    public Sprite GetGunIcon()
    {
        return gunIcon;
    }

    public string GetGunName()
    {
        return gunName;
    }

    public string GetGunInfomation()
    {
        return gunInfomation;
    }

    public int  GetMaxAmmo()
    {
        return maxAmmo;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetReloadTime()
    {
        return reloadTime;
    }

    public float GetKnockback()
    {
        return knockback;
    }

    public float GetShotCooldown()
    {
        return shotCooldown;
    }

    public bool GetBulletBound()
    {
        return bulletBound;
    }

    public bool GetFullAuto()
    {
        return fullAuto;
    }

    public int GetGunID()
    {
        return gunID;
    }
}
