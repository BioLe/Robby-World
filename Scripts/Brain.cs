using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain {

	int DNALength = 243;
	int DNAMaxValue = 6;

	//public CellBoard cellBoard;
	public DNA dna;
	private int curPosX, curPosY;
	private int reward = 0;

	public int cans = 0;

	public Brain(DNA newDna){

		dna = (newDna == null) ? new DNA(DNALength, DNAMaxValue) : newDna;

		//Robot starting position
		curPosX = 1;
		curPosY = 1;
	}

	public void resetBrain(){
		SetPosX(1);
		SetPosY(1);	
		SetReward(0);
		cans = 0;
	}

	public int GetReward(){
		return reward;
	}

	public void SetReward(int rew){
		reward = rew;
	}

	public int GetPosX(){
		return curPosX;
	}

	public int GetPosY(){
		return curPosY;
	}

	public void SetPosX(int x){
		curPosX = x;
	}

	public void SetPosY(int y){
		curPosY = y;
	}

	//True if cellBoard pos is equals to state
	public bool checkState(CellBoard curBoard, int posCheckX, int posCheckY, int state){
		if(curBoard.board[posCheckX, posCheckY].getState() == state){
			return true;
		}
		return false;
	}

	public CellBoard performAction(int newAction, CellBoard curBoard){
		switch (newAction)
		{
			//Move North
			case 0: 
				if( !checkState(curBoard, curPosX - 1, curPosY, 2) ){
					//Debug.Log("Move north sucess");
					curPosX -= 1;
				}else{
					reward -= 5;
					//Debug.Log("Move north unsucess");
				}
				break;

			//Move South
			case 1:
				if( !checkState(curBoard, curPosX + 1, curPosY, 2) ){
					//Debug.Log("Move south sucess");
					curPosX += 1;
				}else{
					//Debug.Log("Move south unsucess");
					reward -= 5;
				}
				break;

			//Move East
			case 2:
				if( !checkState(curBoard, curPosX, curPosY + 1, 2) ){
					//Debug.Log("Move east sucess");
					curPosY += 1;
				}else{
					//Debug.Log("Move east unsucess");
					reward -= 5;
				}
				break;

			//Move West
			case 3:
				if( !checkState(curBoard, curPosX, curPosY - 1, 2) ){
					//Debug.Log("Move west sucess");
					curPosY -= 1;
				}else{
					//Debug.Log("Move west unsucess");
					reward -= 5;
				}
				break;

			//Stay put
			case 4:
				//Debug.Log("Do nothing");
				break;

				

			//Bend down to pick a can
			case 5:
				if( checkState(curBoard, curPosX, curPosY, 1)){
					//Debug.Log("Pick can");
					reward += 10;
					cans++;
					curBoard.board[curPosX, curPosY].setState(0);
				} else{
					//Debug.Log("No can");
					reward -= 1;
				}
				break;

			//Move Random
			case 6:
				//Debug.Log("Move random");
				performAction(Random.Range(0, 4), curBoard);
				break;
		}

		return curBoard;
	}


	public CellBoard MoveNorth(CellBoard curBoard){
		curPosX -= 1;
		return curBoard;
	}

	public CellBoard MoveSouth(CellBoard curBoard){
		curPosX += 1;
		return curBoard;
	}

	public CellBoard MoveEast(CellBoard curBoard){
		curPosY += 1;
		return curBoard;
	}

	public CellBoard MoveWest(CellBoard curBoard){
		curPosY -= 1;
		return curBoard;
	}

	public CellBoard PickCan(CellBoard curBoard){
		reward += 10;
		cans++;
		curBoard.board[curPosX, curPosY].setState(0);
		return curBoard;
	}





	public CellBoard move(CellBoard curBoard){

		int northState = curBoard.board[(curPosX - 1), curPosY].getState();
		int southState = curBoard.board[(curPosX + 1), curPosY].getState();

		int eastState = curBoard.board[curPosX, (curPosY + 1)].getState();
		int westState = curBoard.board[curPosX, (curPosY - 1)].getState();
		
		int curState = curBoard.board[curPosX, curPosY].getState();

		// Get next action from strategy table
		//(int sNorth, int sSouth, int sEast, int sWest, int sCur)
		int newAction = dna.GetAction(northState, southState, eastState, westState, curState);

		
		curBoard = performAction(newAction, curBoard);

		return curBoard;
	}


}
