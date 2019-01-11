using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tank  {
    NeuralNetwork neuralNetwork=new NeuralNetwork(); 
	public Vector2 position;
	public Vector2 direction;
	public float rotation;
	public float speed;
	public float dFitness;

	float rotateLeft;
    float rotateRight;
    int indexNearestObjetive;
  
    public	Tank(){
        rotation=Random.value*Mathf.PI*2;
        rotateLeft=0.16f;
        rotateRight=0.16f;
        dFitness=0;
        position=new Vector2((float)(Random.value*Settings.WidthWindow),(float)(Random.value*Settings.HeightWindow));
        speed = Settings.MaxSpeed;
    } 
	
   
	public bool	Update(ref List<Vector2> objectives){
        var inputs=new List<float>();
        var nearestObjectiveDirection=GetNearestObjectiveDirection(objectives);

        inputs.Add(nearestObjectiveDirection.x);
        inputs.Add(nearestObjectiveDirection.y);
        inputs.Add(direction.x);
        inputs.Add(direction.y);

        //neural network input: nearestObjective and direction
        //output: rotateLeft rotateRight         
        var output = neuralNetwork.Update(inputs);

        if(output.Count < Settings.outputCount) //check
            return false;
        
        float rotRight = output[1];
        float rotLeft = output[0];
       
        float rotationDiff = rotLeft-rotRight;  
        rotationDiff= Mathf.Clamp(rotationDiff,-Settings.RotationRange,Settings.RotationRange);
        
        rotation += rotationDiff;
        speed = rotLeft + rotRight;  

        speed = Mathf.Clamp(rotRight + rotLeft,Settings.MaxSpeed/5,Settings.MaxSpeed);

        direction.x =-Mathf.Sin(rotation);
        direction.y = Mathf.Cos(rotation);
       
        direction.Normalize();
  
        position += direction * speed;

        ClampPosition();

        return true; 
    }

    void ClampPosition(){
        if (position.x > Settings.WidthWindow) position.x = 0;
        if (position.x < 0) position.x = (float)Settings.WidthWindow;
        if (position.y > Settings.HeightWindow) position.y = 0;
        if (position.y <0)  position.y = (float)Settings.HeightWindow;
    }

	//retorna o vetor mais perto do Objetivo
    public Vector2 GetNearestObjectiveDirection(List<Vector2> objetives){
        float nearestDistance=99999;
        Vector2 nearestObjective=new Vector2(0,0);
        for(int i=0; i<objetives.Count;i++){
            float distance= Vector2.Distance(objetives[i],position);
            if(distance < nearestDistance ){
                nearestDistance=distance;
                nearestObjective=position-objetives[i];
                indexNearestObjetive=i;
            }
        }
        return nearestObjective.normalized;
    }

    public int CheckObjectives(List<Vector2> objectives){
        if(Vector2.Distance(position,objectives[indexNearestObjetive]) < Settings.ObjectiveArea )
            return indexNearestObjetive;
        return -1;
    }

    public void Reset(){
        position = new Vector2(Random.value*Settings.WidthWindow,Random.value*Settings.HeightWindow);
        dFitness = 0;
        rotation = Random.value*2*Mathf.PI;
    }

	public void	IncrementarFitness(){++dFitness;}
    public void ReplaceWeights(List<float> weights) { neuralNetwork.ReplaceWeights(weights);  }

    public int GetWeightCount(){return neuralNetwork.GetWeightCount();}
	
}
