using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSpriteSelector : MonoBehaviour {
	
	public Sprite 	spU, spD, spR, spL,
			spUD, spRL, spUR, spUL, spDR, spDL,
			spULD, spRUL, spDRU, spLDR, spUDRL;
	public bool up, down, left, right;
	public int type; // 0: normal, 1: enter
	public Color normalColor, enterColor;
	Color mainColor;
	Image img;
	void Start () {
		img = GetComponent<Image>();
		mainColor = normalColor;
		PickSprite();
		PickColor();
	}
	void PickSprite(){ //picks correct sprite based on the four door bools
		if (up){
			if (down){
				if (right){
					if (left){
						img.sprite = spUDRL;
					}else{
						img.sprite = spDRU;
					}
				}else if (left){
					img.sprite = spULD;
				}else{
					img.sprite = spUD;
				}
			}else{
				if (right){
					if (left){
						img.sprite = spRUL;
					}else{
						img.sprite = spUR;
					}
				}else if (left){
					img.sprite = spUL;
				}else{
					img.sprite = spU;
				}
			}
			return;
		}
		if (down){
			if (right){
				if(left){
					img.sprite = spLDR;
				}else{
					img.sprite = spDR;
				}
			}else if (left){
				img.sprite = spDL;
			}else{
				img.sprite = spD;
			}
			return;
		}
		if (right){
			if (left){
				img.sprite = spRL;
			}else{
				img.sprite = spR;
			}
		}else{
			img.sprite = spL;
		}
	}

	void PickColor(){ //changes color based on what type the room is
		if (type == 0){
			mainColor = normalColor;
		}else if (type == 1){
			mainColor = enterColor;
		}
		img.color = mainColor;
	}
}