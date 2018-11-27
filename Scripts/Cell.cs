using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell{

	private int state;
	private int posX, posY;

	public Cell(int state, int posX, int posY){
		this.state = state;
		this.posX = posX;
		this.posY = posY;
	}

	public int getState(){
		return state;
	}

	public void setState(int newState){
		this.state = newState;
	}
}
