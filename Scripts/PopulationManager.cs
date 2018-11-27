using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PopulationManager : MonoBehaviour {

	//Board
	private int dimX = 12;
	private int dimY = 12;
	private CellBoard board;

	//List<Brain> population = new List<Brain>(); //Where brains are added after breeding
	List<float> bestRewardPerEpoch = new List<float>(); //Placeholder, to have an idea of the reward of the best gen per epoch
	//List <object[]> bestBrains = new List<object[]>(); //Brains ordered by reward
	List <object[]> population = new List<object[]>();
	public int nSimulations = 100;
	public int populationSize = 200;
	public int maxEpochs = 1000;
	public int moves = 200;
	private int curEpoch = 1;

	//csv
	private SaveData save;
	private float elapsedTime;
	private string bestDNA;
	private float rewardOfBestDNA;
	private float avgRewardCurEpoch;
	private float cansPickedUp;

	public void Start(){
		
		string[] headerRow = new string[]{"ElapsedTime", "Epoch",
										  "BestDNA", "RewardOfBestDNA", "CansPickedUpByBest",
										  "AvgRewardCurEpoch"};

		save = new SaveData(headerRow, headerRow.Length, "RobbyWorld");

		elapsedTime = 0;
		bestDNA = "-1";
		rewardOfBestDNA = -1;
		avgRewardCurEpoch = -1;


		//Inicializar population
		for(int i = 0; i < populationSize; i++) population.Add(new object[2]{new Brain(null), 0.0f});

		List<int> bestAgentDNA = ((Brain)population[0][0]).dna.getGenes();
		List<string> stringBestAgentDNA = bestAgentDNA.ConvertAll<string>(delegate(int i){return i.ToString();});
		string dnaList = string.Join("", stringBestAgentDNA.ToArray());
		Debug.Log("DNA so para debug? " + dnaList);
		
		//bestBrains = findingBestBrains();

	}

	//Simulate one epoch
	public void simulate(){

		for(int nSims = 0; nSims < nSimulations; nSims++){

			board = new CellBoard(dimX, dimY);
			//Debug.Log("nSim = " + nSims);
			for (int nPop = 0; nPop < populationSize; nPop++){

				Brain curBrain = (Brain) population[nPop][0];
				curBrain.resetBrain();

				CellBoard boardCopy = board;

				//boardCopy.printArray();
				//Debug.Log("Antes (nSim,nPop,Reward) = (" + nSims + ", " + nPop + ", " + curBrain.GetReward() + ")");

				for(int nMoves = 0; nMoves < moves; nMoves++) boardCopy = curBrain.move(boardCopy);

				//boardCopy.printArray();

				//Debug.Log("Depois (nSim,nPop,Reward) = (" + nSims + ", " + nPop + ", " + curBrain.GetReward() + ")");
				
				//Update reward of current brain
				population[nPop] = new object[2]{curBrain, ((float)population[nPop][1] + curBrain.GetReward())};

				//if(nPop==0){
					//List<int> bestAgentDNA = curBrain.dna.getGenes();
					//List<string> stringBestAgentDNA = bestAgentDNA.ConvertAll<string>(delegate(int i){return i.ToString();});
					//string dnaList = string.Join("", stringBestAgentDNA.ToArray());
					//Debug.Log("DNA? " + dnaList);
					//Debug.Log("Cans? " + curBrain.cans);
				//}
				
			}
			
		}

		avgRewardCurEpoch = 0;
		for(int nPop = 0; nPop < populationSize; nPop++){
			float rewardMedia = ((float)population[nPop][1]/nSimulations);
			avgRewardCurEpoch += rewardMedia;
			population[nPop] = new object[2]{ (Brain)population[nPop][0] , rewardMedia };
		}
		avgRewardCurEpoch /= populationSize;


	}

	// Update is called once per frame
	void Update () {

		if(curEpoch > maxEpochs){
			save.exportCSV();
        	//Stop playing the scene
        	UnityEditor.EditorApplication.isPlaying = false;	
		}

		Debug.Log("Ep = " + curEpoch);

		elapsedTime += Time.deltaTime;
		/* 
		Debug.Log("time: " + elapsedTime);
		Debug.Log("curE: " + curEpoch);
		Debug.Log("besD: " + bestDNA);
		Debug.Log("rwBD: " + rewardOfBestDNA);
		Debug.Log("avgR: " + avgRewardCurEpoch);
		*/
		save.addRow(new string[]{elapsedTime.ToString(), curEpoch.ToString(),
							bestDNA.ToString(), rewardOfBestDNA.ToString(), cansPickedUp.ToString(),
							avgRewardCurEpoch.ToString()});


		//Runs simulation
		simulate();
		//Breeds new population based on simulation
		BreedNewPopulation();
		
		

	}

	void LateUpdate(){
		if (Input.GetKey("g")){
			save.exportCSV();
		}
	}

	Brain Breed(Brain parent1, Brain parent2){
		
		//Mutate 1 in 100
		//int intervalo = Random.Range(0,100);
		
		Brain b = new Brain(null);
		//if(intervalo >= 0 && intervalo < 2){
		if(Random.Range(0,50) == 1){
			//Debug.Log("mutated");
		
			b.dna.Mutate();	
		} 
		else {
			b.dna.Combine(parent1.dna, parent2.dna);
		}
		
		return b;
	}

	
	void BreedNewPopulation(){
		//Order by reward, which is in population<Object[1]>
		List<object[]> sortedList = population.OrderByDescending(brain => brain[1]).ToList();

		//Clear everything after it was used
		population.Clear();

		//Convert list<int> into int for saving purposes
		List<int> bestAgentDNA = ((Brain)sortedList[0][0]).dna.getGenes();
		List<string> stringBestAgentDNA = bestAgentDNA.ConvertAll<string>(delegate(int i){return i.ToString();});
		string dnaList = string.Join("", stringBestAgentDNA.ToArray());
		
		//Debug.Log("Best(Epoch,reward) : (" + curEpoch +", "+ (float)sortedList[0][1] + ")");
		bestDNA = dnaList;
		rewardOfBestDNA = (float)sortedList[0][1];
		cansPickedUp = ((Brain)sortedList[0][0]).cans;

		//Debug.Log("board do melhor agente nesta epoca");
		//((Brain)sortedList[0][0]).cellBoard.printArray();

		for(int i = 0; i < (int)(sortedList.Count / 2.0f); i++){

			object[] child1 = new object[2]{ Breed(((Brain)sortedList[i][0]), ((Brain)sortedList[i+1][0])) , 0.0f};
			population.Add(child1);
			object[] child2 = new object[2]{ Breed(((Brain)sortedList[i+1][0]), ((Brain)sortedList[i][0])) , 0.0f};
			population.Add(child2);

			//population.Add( Breed((Brain)sortedList[i+1][0], (Brain)sortedList[i][0]) );
		}	

		curEpoch++;
	}
	
}
