using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellBoard {

	private int dimX, dimY;
	public Cell [,] board;

	public CellBoard(int dimX, int dimY){
		this.dimX = dimX;
		this.dimY = dimY;
		board = new Cell[dimX,dimY];
		generateBoard();
	}

	public object Clone()
    {
        return this.MemberwiseClone();
    }

	public int GetDimX(){
		return dimX;
	}

	public int GetDimY(){
		return dimY;
	}

	public Cell[,] getBoard(){
		return board;
	}

	public void generateBoard(){
		for(int x = 0; x < dimX; x++){
			for(int y = 0; y < dimY; y++){
				//Wall states
				if(x==0 || x == dimX -1 || y == 0 || y == dimY -1) board[x,y] = new Cell(2,x,y);
				//Empty states
				else if(Random.Range(0,100) >= 50) board[x,y] = new Cell(0,x,y);
				//Soda states
				else board[x,y] = new Cell(1,x,y);

			}
		}
	}

	public void printArray(){
		int rowLength = board.GetLength(0);
        int colLength = board.GetLength(1);

        for (int i = 0; i < rowLength; i++){
			string linha = "";
            for (int j = 0; j < colLength; j++){
				linha += board[i,j].getState() + ", ";
            }
			Debug.Log(linha);
        }

		Debug.Log("-------------------------");
		
	}



}
