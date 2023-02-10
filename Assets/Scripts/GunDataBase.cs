using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunDataBase", menuName = "Gun/CreateGunDataBase")]
public class GunDataBase : ScriptableObject
{

    [SerializeField]
    private List<GunStatus> gunLists = new List<GunStatus>();

    //　アイテムリストを返す
    public List<GunStatus> GetGunLists()
    {
        return gunLists;
    }
}
