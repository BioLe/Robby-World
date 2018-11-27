using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualDebugger: MonoBehaviour{

    private List<GameObject> auxBoard;

    public int moves = 200;

    public GameObject can;
    public GameObject empty;
    public GameObject wall;
    public GameObject robby;

    private Brain brain;
    private CellBoard cBoard;
    public int dimX = 12;
    public int dimY = 12;

    //Random 
    public string genes = null;
    

	public void Start(){

        auxBoard = new List<GameObject>();

        cBoard = new CellBoard(dimX,dimY);
        brain = new Brain(null);
        if(!genes.Equals("")) brain.dna.SetGenes(genes);

        List<int> bestAgentDNA = brain.dna.getGenes();
        List<string> stringBestAgentDNA = bestAgentDNA.ConvertAll<string>(delegate(int i){return i.ToString();});
        string dnaList = string.Join("", stringBestAgentDNA.ToArray());
        Debug.Log("DNA? " + dnaList);

        if(cBoard != null){
            instanciateBoard(cBoard);
            
        }

    }


    public void Update(){

        if (Input.GetKeyUp("w")){
            cBoard = brain.MoveNorth(cBoard);
		}
        if (Input.GetKeyUp("s")){
            cBoard = brain.MoveSouth(cBoard);
		}
        if (Input.GetKeyUp("d")){
            cBoard = brain.MoveEast(cBoard);
		}
        if (Input.GetKeyUp("a")){
            cBoard = brain.MoveWest(cBoard);
		}
        if (Input.GetKeyUp("f")){
            cBoard = brain.PickCan(cBoard);
		}


        ClearBoard();
        
        cBoard = brain.move(cBoard);
        auxBoard.Add(Instantiate(robby, new Vector3(brain.GetPosX() * 10.0F, 10, brain.GetPosY() * 10.0F), Quaternion.identity));
        Debug.Log("(x,y): (" + brain.GetPosX() + ", " + brain.GetPosY() + ")" );
        instanciateBoard(cBoard);


    }


    private void ClearBoard(){
        foreach(GameObject a in auxBoard){
            Destroy(a);
        }
    }


    void LateUpdate(){
		if (Input.GetKey("x")){
            
		}
	}

    public void instanciateBoard(CellBoard newBoard){

        for(int x = 0; x < newBoard.GetDimX(); x++){
            for(int y = 0; y < newBoard.GetDimY(); y++){

                int cellState = newBoard.board[x,y].getState();

                switch (cellState){
                    case 0:
                        auxBoard.Add(Instantiate(empty, new Vector3(x * 10.0F, 0, y * 10.0F), Quaternion.identity));
                        break;
                    case 1:
                        auxBoard.Add(Instantiate(can, new Vector3(x * 10.0F, 0, y * 10.0F), Quaternion.identity));
                        break;
                    case 2:
                        auxBoard.Add(Instantiate(wall, new Vector3(x * 10.0F, 0, y * 10.0F), Quaternion.identity));
                        break;
                }
            }
        }

    }


}