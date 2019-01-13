using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneticAlgorithm  {             
    public List <Genome> genomes=new List<Genome>(); 
    public float bestFitness;
    public float medianFitness;
    public float worstFitness;
    public int bestGenome;
    int populationSize;
    int weightsPerChromosomeCount;
    float totalFitness; 
    float mutationRate; 
    float crossoverRate; 

    void Crossover(ref List<float> mother, ref List<float> father, ref List<float> baby1, ref List<float> baby2){

        if ((Random.value > crossoverRate) || (mother == father)){
            baby1 = mother;
            baby2 = father;
            return;
        }
        
        int cutPos = Random.Range(0, weightsPerChromosomeCount - 1);

        for (int i = 0; i < cutPos; ++i){
            baby1.Add(mother[i]);
            baby2.Add(father[i]);
        }

        for (int i = cutPos; i < mother.Count; ++i){
            baby1.Add(mother[i]);
            baby2.Add(father[i]);
        }
    
    }

    void Mutate(ref List<float> chromosomes){ 
        for (int i=0; i<chromosomes.Count; ++i)
            if (Random.value < mutationRate)
                chromosomes[i] += (Random.Range(-1.0f,1.0f) * Settings.MutationRange);
    }
    Genome SelectGenomeToMate(){ 
        float cutPos = Random.value * totalFitness;
        var selected=new Genome();
        float fitnessSum = 0;
        for (int i = 0; i < populationSize; ++i){
            fitnessSum += genomes[i].dFitness;
            if (fitnessSum >= cutPos){
                selected = genomes[i];
                break;
            }
        }

        return selected;
    }  
    void NaturalSelection(int bestCount, int copyCount, List<Genome> population){ 
        while (bestCount-- > 0){
            if (bestCount < 0) Debug.Log("error negative bestCount");
            for (int i = 0; i < copyCount; ++i){
                if (populationSize - 1 - bestCount <0)
                    Debug.Log("index negative");
                else
                    population.Add(genomes[(populationSize-1)-bestCount]);
            }
        }
    }
    
    void UpdateStats(){
        totalFitness = 0;
        float best = 0;
        float lowest = 9999999;
        for (int i = 0; i < populationSize; ++i){
            if (genomes[i].dFitness > best){
                best = genomes[i].dFitness;
                bestGenome = i;
                bestFitness = best;
            }
            if (genomes[i].dFitness < lowest){
                lowest = genomes[i].dFitness;
                worstFitness = lowest;
            }
            totalFitness += genomes[i].dFitness;
        }
        medianFitness = totalFitness / populationSize;
    }

    void Reset(){
        totalFitness = 0;
        bestFitness = 0;
        worstFitness = 9999999;
        medianFitness = 0;
    }

    public GeneticAlgorithm(int popsize, float mutationrate, float crossoverrate, int weightPerchromosomeCount){
        
        populationSize = popsize;
        mutationRate = mutationrate;
        crossoverRate = crossoverrate;
        weightsPerChromosomeCount=weightPerchromosomeCount;
        totalFitness=0;
        
        bestFitness=0;
        bestGenome=0;   
        worstFitness=99999999;
        medianFitness = 0;
            
        for (int i = 0; i < populationSize; ++i){                
            genomes.Add(new Genome());
            for (int j = 0; j < weightsPerChromosomeCount; ++j)    
                genomes[i].weights.Add(Random.Range(-1.0f,1.0f));   
        }
    }

    public static int CompeteGenome(Genome lhs, Genome rhs){
        if (lhs.dFitness > rhs.dFitness)
            return 1;
        if (lhs.dFitness < rhs.dFitness)
            return -1;
        return 0;
    }
  
    public List<Genome> Epoch(List<Genome> oldPopulation){ 
        Reset();        
        genomes = oldPopulation;
        genomes.Sort(CompeteGenome);
        var newPopulation=new List<Genome>();
        UpdateStats();

        if ((Settings.EliteCopyCount * (Settings.EliteNumber % 2 ))!=0 ){
           Debug.Log("wrong settings elite");
        }
         
        NaturalSelection(Settings.EliteNumber,Settings.EliteCopyCount,newPopulation);

        while (newPopulation.Count < populationSize){

            var mother = SelectGenomeToMate();
            var father = SelectGenomeToMate();

            var baby1 = new List<float>();
            var baby2 = new List<float>();
            Crossover(ref mother.weights, ref father.weights,ref baby1,ref baby2);
            Mutate(ref baby1);
            Mutate(ref baby2);

            newPopulation.Add(new Genome(ref baby1, 0));
            newPopulation.Add(new Genome(ref baby2, 0));
        }
      
        genomes = newPopulation;

        return genomes;
    
    }



}
