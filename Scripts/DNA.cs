using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DNA {

	List<int> genes = new List<int>();
	int dnaLength = 0;
	int maxValues = 0;

	public DNA(int l, int v){
		dnaLength = l;
		maxValues = v;
		SetRandom();
		//Debug.Log("Genes = " +String.Join("", genes.ConvertAll(i => i.ToString()).ToArray()));
	}

	public void SetRandom(){
		genes.Clear();
		for(int i = 0; i < dnaLength; i++){
			//int maxValue = maxValues + 1;
			genes.Add(UnityEngine.Random.Range(0, maxValues + 1));
		}

		
	}

	public void SetGenes(String dnaString){
		//genes = dnaString.Split("").Select(Int32.Parse).ToList();
		genes.Clear();
		foreach(char c in dnaString){
			genes.Add((int)Char.GetNumericValue(c));
		}

	}

	public List<int> getGenes(){
		return genes;
	}

	public void SetInt(int pos, int value){
		genes[pos] = value;
	}

	public void Combine(DNA d1, DNA d2){
		for(int i = 0; i < dnaLength; i++){
			if(i < dnaLength/2.0f){
				int c = d1.genes[i];
				genes[i] = c;
			} 
			else {
				int c = d2.genes[i];
				genes[i] = c;
			}
		}
	}

	int nrMutatedGenes = 10;
	public void Mutate(){
		
		for(int i = 0; i < nrMutatedGenes; i++){
			//int maxValue = maxValues + 1;
			genes[UnityEngine.Random.Range(0, dnaLength)] = UnityEngine.Random.Range(0, maxValues + 1);
		}
		
		//SetRandom();
	}

	public int GetAction(int sNorth, int sSouth, int sEast, int sWest, int sCur){
		double actionIndex = (Math.Pow(3, 4) * sNorth) + (Math.Pow(3, 3) * sSouth) + (Math.Pow(3, 2) * sEast) + (Math.Pow(3, 1) * sWest) + (Math.Pow(3, 0) * sCur);
		return genes[Convert.ToInt32(actionIndex)];
	}

}
