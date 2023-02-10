using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Item/CreateItem")]
public class Item : ScriptableObject
{

    public enum KindOfItem
    {
        UseItem
    }

    //　アイテムの種類
    [SerializeField]
    private KindOfItem kindOfItem;
    //　アイテムのアイコン
    [SerializeField]
    private Sprite icon;
    //　アイテムの名前
    [SerializeField]
    private string itemName;
    //　アイテムの情報
    [SerializeField]
    private string information;
    //　アイテムの回復量
    [SerializeField]
    private int recoveryHP;
    [SerializeField]
    private int recoveryST;
    [SerializeField]
    private int recoveryLT;
    [SerializeField]
    private int exMag;
    [SerializeField]
    private int exHealth;




    public KindOfItem GetKindOfItem()
    {
        return kindOfItem;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public string GetInformation()
    {
        return information;
    }

    public int GetRecoveryHP()
    {
        return recoveryHP;
    }

    public int GetRecoveryST()
    {
        return recoveryST;
    }

    public int GetRecoveryLT()
    {
        return recoveryLT;
    }

    public int GetExMag()
    {
        return exMag;
    }

    public int GetExHealth()
    {
        return exHealth;
    }
}