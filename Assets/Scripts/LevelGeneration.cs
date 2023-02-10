using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {
	Vector2 worldSize = new Vector2(5,5);
	Room[,] rooms;
	List<Vector2> takenPositions = new List<Vector2>();
	int gridSizeX, gridSizeY, numberOfRooms = 20;
	public GameObject roomWhiteObj;
	public Transform mapRoot;
	void Start () {
		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2)){ 
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}
		gridSizeX = Mathf.RoundToInt(worldSize.x); 
		gridSizeY = Mathf.RoundToInt(worldSize.y);
		CreateRooms(); //レイアウト作成
		SetRoomDoors(); //ドアの設定
		DrawMap(); //部屋をインスタンス化してマップ生成
		GetComponent<SheetAssigner>().Assign(rooms); //部屋情報を渡す
	}
	void CreateRooms(){
		//設定
		rooms = new Room[gridSizeX * 2,gridSizeY * 2];
		rooms[gridSizeX,gridSizeY] = new Room(Vector2.zero, 1);
		takenPositions.Insert(0,Vector2.zero);
		Vector2 checkPos = Vector2.zero;
		//定数
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
		//部屋の追加
		for (int i =0; i < numberOfRooms -1; i++){
			float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
			//新しいポジション
			checkPos = NewPosition();
			//新しいポジションを試す
			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare){
				int iterations = 0;
				do{
					checkPos = SelectiveNewPosition();
					iterations++;
				}while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
				if (iterations >= 50)
					print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
			}
			//ポジションの確定
			rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new Room(checkPos, 0);
			takenPositions.Insert(0,checkPos);
		}	
	}
	Vector2 NewPosition(){
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
			x = (int) takenPositions[index].x;//x,y座標を取得
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);//横軸を見るか縦軸を見るかランダムに選択
			bool positive = (Random.value < 0.5f);//正負の判定
			if (UpDown){
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
		return checkingPos;
	}
	Vector2 SelectiveNewPosition(){ 
		int index = 0, inc = 0;
		int x =0, y =0;
		Vector2 checkingPos = Vector2.zero;
		do{
			inc = 0;
			do{ 
				//隣に1部屋しかない部屋から開始
                //分岐した部屋の可能性が上がる
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;
			}while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);
			if (UpDown){
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		if (inc >= 100){ //時間がかかると中止
			print("Error: could not find position with only one neighbor");
		}
		return checkingPos;
	}
	int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions){
		int ret = 0;
		if (usedPositions.Contains(checkingPos + Vector2.right)){ //using Vector.[direction] as short hands, for simplicity
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.left)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.up)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.down)){
			ret++;
		}
		return ret;
	}
	void DrawMap(){
		foreach (Room room in rooms){
			if (room == null){
				continue; //開いてないところは飛ばす
			}
            Vector2 mapParentPos = GameObject.Find("MapSystem").transform.position;
			Vector2 drawPos = room.gridPos;
			drawPos.x *= 16;//部屋のアスペクト比
			drawPos.y *= 8;
			//マップオブジェクトを作成したあと割り当てる
			MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, new Vector2(mapParentPos.x + drawPos.x , mapParentPos.y + drawPos.y), Quaternion.identity).GetComponent<MapSpriteSelector>();
			mapper.type = room.type;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
			mapper.gameObject.transform.parent = mapRoot;
		}
	}
	void SetRoomDoors(){
		for (int x = 0; x < ((gridSizeX * 2)); x++){
			for (int y = 0; y < ((gridSizeY * 2)); y++){
				if (rooms[x,y] == null){
					continue;
				}
				Vector2 gridPosition = new Vector2(x,y);
				if (y - 1 < 0){ //上チェック
					rooms[x,y].doorBot = false;
				}else{
					rooms[x,y].doorBot = (rooms[x,y-1] != null);
				}
				if (y + 1 >= gridSizeY * 2){ //下チェック
					rooms[x,y].doorTop = false;
				}else{
					rooms[x,y].doorTop = (rooms[x,y+1] != null);
				}
				if (x - 1 < 0){ //左チェック
					rooms[x,y].doorLeft = false;
				}else{
					rooms[x,y].doorLeft = (rooms[x - 1,y] != null);
				}
				if (x + 1 >= gridSizeX * 2){ //右チェック
					rooms[x,y].doorRight = false;
				}else{
					rooms[x,y].doorRight = (rooms[x+1,y] != null);
				}
			}
		}
	}
}
