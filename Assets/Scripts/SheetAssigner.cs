using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SheetAssigner : MonoBehaviour {
    private GameObject mapParent;
    [SerializeField]
	GameObject[] sheetsNormal;
	[SerializeField]
	GameObject RoomObj;
    private int rand,itemRand,xposRand,yposRand;
    public Vector2 roomDimensions = new Vector2(14 * 14, 14 * 14);
    public Vector2 gutterSize = new Vector2(14 * 14, 14 * 14);
    private int maptype = 0; //0:初期部屋,1:通常部屋,2:階段部屋
    private int stairRand, staorGenRand;
    private bool stairGen = true;
    private bool firstMap = true;

    private void Start()
    {
        stairRand = Random.Range(0, 2);
        staorGenRand = Random.Range(5, 7);
        mapParent = GameObject.FindGameObjectWithTag("MapParent");
    }

    public void Assign(Room[,] rooms)
    {
        int loopNum = 0;

        if (PlayerStatusTakeOver.floorLevel % 10 == 0)
        {
            if(PlayerStatusTakeOver.floorLevel == 10)
            {
                RoomInstance myRoom = Instantiate(RoomObj, transform.position, Quaternion.identity, mapParent.transform).GetComponent<RoomInstance>();
                Instantiate(myRoom.bossRoom, transform.position, Quaternion.identity, mapParent.transform);
                Vector3 bossspawnpos = transform.position;
                bossspawnpos.y += 23;
                Instantiate(myRoom.bossEnemy, bossspawnpos, Quaternion.identity);
            }
            else if (PlayerStatusTakeOver.floorLevel == 20)
            {
                RoomInstance myRoom = Instantiate(RoomObj, transform.position, Quaternion.identity, mapParent.transform).GetComponent<RoomInstance>();
                Instantiate(myRoom.bossRoom, transform.position, Quaternion.identity, mapParent.transform);
                Instantiate(myRoom.bossEnemy2, transform.position, Quaternion.identity);
            }
        }
        else
        { 
            foreach (Room room in rooms)
            {
                loopNum++;
                int enemyRand = Random.Range(0, 100);
                rand = Random.Range(0, 100);
                itemRand = Random.Range(0, 100);
                xposRand = Random.Range(0, 5);
                yposRand = Random.Range(0, 5);
                //スペースがなければ飛ばす
                if (room == null)
                {
                    loopNum--;
                    continue;
                }
                //配列のランダムな値を選択
                int index = Mathf.RoundToInt(Random.value * (sheetsNormal.Length - 1));
                //部屋の位置を決める
                Vector3 pos = new Vector3(room.gridPos.x * (roomDimensions.x + gutterSize.x) / 30, room.gridPos.y * (roomDimensions.y + gutterSize.y) / 15, 0);
                Vector3 objectPos = new Vector3(pos.x + xposRand, pos.y + yposRand, 0);
                RoomInstance myRoom = Instantiate(RoomObj, pos, Quaternion.identity, mapParent.transform).GetComponent<RoomInstance>();
                if (stairGen && firstMap && stairRand >= 1)
                {
                    Instantiate(myRoom.stair, pos, Quaternion.identity, myRoom.transform);
                    stairGen = false;
                }
                else if(loopNum == staorGenRand && stairGen)
                {
                    Instantiate(myRoom.stair, pos, Quaternion.identity, myRoom.transform);
                    stairGen = false;
                }
                if (rand <= 30)
                {
                    if(enemyRand <= 33)
                    {
                        Instantiate(myRoom.enemy, objectPos, Quaternion.identity, myRoom.transform);
                    }
                    else if(enemyRand >= 34 && enemyRand <= 66)
                    {
                        Instantiate(myRoom.enemy2, objectPos, Quaternion.identity, myRoom.transform);
                    }
                    else
                    {
                        Instantiate(myRoom.enemy3, objectPos, Quaternion.identity, myRoom.transform);
                    }
                }
                else if (rand >= 31 && rand <= 60)
                {
                    if (itemRand <= 33)
                    {
                        Instantiate(myRoom.item1, objectPos, Quaternion.identity, myRoom.transform);
                    }
                    else if (itemRand >= 34 && itemRand <= 66)
                    {
                        Instantiate(myRoom.item2, objectPos, Quaternion.identity, myRoom.transform);

                    }
                    else
                    {
                        Instantiate(myRoom.item3, objectPos, Quaternion.identity, myRoom.transform);
                    }

                }
                else if (rand >= 61 && rand <= 70)
                {
                    Instantiate(myRoom.Bonfire, objectPos, Quaternion.identity, myRoom.transform);
                }
                if (room != null)
                {
                    if (room.doorTop)
                    {
                        Instantiate(myRoom.doorUWall, pos, Quaternion.identity, myRoom.transform);
                    }
                    if (room.doorBot)
                    {
                        Instantiate(myRoom.doorDWall, pos, Quaternion.identity, myRoom.transform);
                    }
                    if (room.doorLeft)
                    {
                        Instantiate(myRoom.doorLWall, pos, Quaternion.identity, myRoom.transform);
                    }
                    if (room.doorRight)
                    {
                        Instantiate(myRoom.doorRWall, pos, Quaternion.identity, myRoom.transform);
                    }
                    if (!room.doorTop)
                    {
                        Instantiate(myRoom.uWall, pos, Quaternion.identity, myRoom.transform);
                    }
                    if (!room.doorBot)
                    {
                        Instantiate(myRoom.dWall, pos, Quaternion.identity, myRoom.transform);
                    }
                    if (!room.doorLeft)
                    {
                        Instantiate(myRoom.lWall, pos, Quaternion.identity, myRoom.transform);
                    }
                    if (!room.doorRight)
                    {
                        Instantiate(myRoom.rWall, pos, Quaternion.identity, myRoom.transform);
                    }

                }
                if (firstMap) { firstMap = false; }
                stairRand--;
            }
        }
    }
}
