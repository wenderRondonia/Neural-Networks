using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Controller{
    public List<GameObject> tanksObjects=new List<GameObject>();
    public List<GameObject> objetivesObjects = new List<GameObject>();
    List<Genome> Apopulacao = new List<Genome>();
    List<Tank> tanks = new List<Tank>();
    List<Vector2> objetives=new List<Vector2>();
    GeneticAlgorithm geneticAlgorithm;
    int totalObjectiveFound;
    float time;
    int tankCount;
    int objectiveCount;
    int weightCountNeuralNetwork;

    int cyclesPerGeneration;
    int generationCount;
    int windowX, windowY;
    
    public Controller(GameObject tank, GameObject objective){
        geneticAlgorithm = null;
        cyclesPerGeneration = 0;
        tankCount = Settings.TankCount;
        objectiveCount = Settings.ObjetiveCount;
        generationCount = 0;
        windowX = Settings.WidthWindow;
        windowY = Settings.HeightWindow;
       
        for (int i = 0; i < tankCount;i++){
            tanks.Add(new Tank());
            tanksObjects.Add(GameObject.Instantiate(tank, new Vector3(Settings.MapScale * tanks[i].position.x, 0.3f, Settings.MapScale * tanks[i].position.y), Quaternion.Euler(0, (float)tanks[i].rotation, 0)) as GameObject);
        }
       
        weightCountNeuralNetwork = tanks[0].GetWeightCount();
        geneticAlgorithm = new GeneticAlgorithm(tankCount, Settings.MutationRate, Settings.CrossoverRate, weightCountNeuralNetwork );

        Apopulacao = geneticAlgorithm.genomes;

        for (int i = 0; i < tankCount;i++)
            tanks[i].ReplaceWeights(Apopulacao[i].weights);
        
        for (int i = 0; i < objectiveCount; ++i){
            Vector2 v = new Vector2(Random.value * windowX,Random.value * windowY);
            objetives.Add(v);
            objetivesObjects.Add(GameObject.Instantiate(objective, new Vector3(v.x, 0.3f, v.y), Quaternion.identity) as GameObject);
        }
        
    }

    public bool Update(){
        time += Time.deltaTime;
    
        //run tanks through the generations with cycles
        // neural networks will be updated with info and give inputs
        //if he find the objetive fitness increaed
        if (cyclesPerGeneration++ < Settings.CycleCount) { //test neural networks and calculte fitness
            for (int i=0; i<tankCount; ++i){
                if (!tanks[i].Update(ref objetives)){
                    Debug.Log("ERRO: Wrong amount of NN inputs!");
                    return false;
                }
                
                int objetiveIndex = tanks[i].CheckObjectives(objetives);
                if (objetiveIndex >= 0){                           
                    totalObjectiveFound++;
                    tanks[i].IncrementarFitness();
                    //reset it in another place
                    objetives[objetiveIndex] = new Vector2(Random.value * windowX, Random.value * windowY);
                }
                Apopulacao[i].dFitness = tanks[i].dFitness;       
            }
        }else{ //generation completed, time to learn
            tanksObjects[geneticAlgorithm.bestGenome].GetComponent<Renderer>().material.color = Color.white;
            generationCount++;
            cyclesPerGeneration = 0;
            //run genetic algorithm and teach neural networks
            Apopulacao = geneticAlgorithm.Epoch(Apopulacao);
            for (int i=0;i<tankCount;i++){
                tanks[i].ReplaceWeights(Apopulacao[i].weights);
                tanks[i].Reset();
            }
        }


        for (int i = 0; i < tankCount; ++i){
            tanksObjects[i].transform.position = new Vector3(Settings.MapScale * tanks[i].position.x, 0.3f, -Settings.MapScale * tanks[i].position.y);
            if(Vector2.Dot(Vector2.up, tanks[i].direction)<0)
                tanksObjects[i].transform.rotation = Quaternion.Euler(0, -Vector2.Angle(Vector2.right, tanks[i].direction), 0);
            else
                tanksObjects[i].transform.rotation = Quaternion.Euler(0, Vector2.Angle(Vector2.right, tanks[i].direction) , 0);
        }
        for (int i = 0; i < objectiveCount; ++i)
            objetivesObjects[i].transform.position = new Vector3((Settings.MapScale * objetives[i].x), 0.3f,-( Settings.MapScale * objetives[i].y));

        return true;
    }

    public void Render(){ 
        GUI.Box(new Rect(10, 10, 100, 30), " generation= " + generationCount);
        GUI.Box(new Rect(10, 60, 200, 30), " best="+ geneticAlgorithm.bestFitness);
        GUI.Box(new Rect(10, 95, 200, 30), " median=" + geneticAlgorithm.medianFitness);
        GUI.Box(new Rect(10, 130, 200, 30), " worst=" + geneticAlgorithm.worstFitness);

        GUI.Box(new Rect(10, Screen.height-50, 300, 40), " Objectives/sec=" +totalObjectiveFound/time);

        GUI.Box(new Rect(Screen.width-10 - 100, 10, 100, 40), "r-restart ");
        GUI.Box(new Rect(Screen.width - 10 - 300, 50, 300, 40), "up-fast / down-slow");
        
        tanksObjects[geneticAlgorithm.bestGenome].GetComponent<Renderer>().material.color = Color.red;
    }
}
