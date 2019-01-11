using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NeuralNetwork{
    int inputCount,outputCount,hiddenLayerCount,neuronsPerHiddenLayerCount;
    List<NeuralLayer> layers=new List<NeuralLayer>();
    public NeuralNetwork(){
        inputCount=Settings.inputCount;
        outputCount=Settings.outputCount;
        hiddenLayerCount=Settings.hiddenLayerCount;
        neuronsPerHiddenLayerCount=Settings.neuronsPerHiddenLayerCount;
       
        if (hiddenLayerCount > 0){
            layers.Add(new NeuralLayer(neuronsPerHiddenLayerCount, inputCount));  // first layer
            for (int i = 0; i < hiddenLayerCount - 1; ++i)       
                layers.Add(new NeuralLayer(neuronsPerHiddenLayerCount, neuronsPerHiddenLayerCount));
            layers.Add(new NeuralLayer(outputCount, hiddenLayerCount)); //output layer
        }else{
            layers.Add(new NeuralLayer(outputCount,inputCount)); //single output/input layer
        }
    }

    public int GetWeightCount(){
        var weights=0;
        for (int i = 0; i < hiddenLayerCount + 1; ++i)
            for (int j = 0; j < layers[i].neuronCount; ++j)
                for (int k = 0; k < layers[i].neurons[j].inputCount; ++k)                
                    weights++;
        return weights;
    }
    public List<float> GetWeights(){
        var weights=new List<float>();
        for (int i = 0; i < hiddenLayerCount + 1; ++i)
            for (int j = 0; j < layers[i].neuronCount; ++j)
                for (int k = 0; k < layers[i].neurons[j].inputCount; ++k)                
                    weights.Add(layers[i].neurons[j].weights[k]);
        return weights;
    }

    public void ReplaceWeights( List<float> weights){
        int currentWeight = 0;
        for (int i = 0; i < hiddenLayerCount + 1; ++i)
            for (int j = 0; j < layers[i].neuronCount; ++j)
                for (int k = 0; k < layers[i].neurons[j].inputCount; ++k)
                    layers[i].neurons[j].weights[k]=weights[currentWeight++];
    }

     //calculate the right output given input
     public  List<float> Update(List<float> inputs){
        
        List<float> outputs=new List<float>();
        int currentWeight = 0;

        //check if wrong input output
        if (inputs.Count != inputCount) return outputs;
        
        for (int i=0; i<hiddenLayerCount + 1; i++){
            if (i > 0)
                for (int r = 0; r < inputs.Count; r++)
                    inputs[r] = outputs[r];
            outputs.Clear();
            currentWeight = 0; 
                                  
            // for each neuron, sum all input*weight and then use sigmoid math function
            for (int j=0; j<layers[i].neuronCount; ++j){                                                                                                                                               
                float networkInput = 0;
                int inputCountLocal = layers[i].neurons[j].inputCount;
                for (int k=0; k<inputCountLocal - 1; ++k)                    
                    networkInput += layers[i].neurons[j].weights[k] * inputs[currentWeight++];
                
                var sum = 0f;
                if(inputCountLocal-1==-1){
                   sum+=layers[i].neurons[j].weights[layers[i].neurons[j].weights.Count- 1]; 
                }else{
                    sum+=layers[i].neurons[j].weights[inputCountLocal-1];
                }

                networkInput +=  sum+ Settings.Bias;
                                           
                outputs.Add(Sigmoid(networkInput,Settings.ActivationResponse));                             
                currentWeight = 0;
            }                      
        }
        return  outputs;
     }

    //sigmoid response curve
    public float Sigmoid(float inputDaRede, float response){
        var i = (double)(-inputDaRede / response);
        return (1f / ((1f + (float)Math.Exp(i))));
    }

}


public class NeuralLayer {
    public int neuronCount; 
    public List<Neuron> neurons= new List<Neuron>();
    public NeuralLayer(int neuronCount, int inputPerLayer){
        this.neuronCount = neuronCount;
        for (int i = 0; i < neuronCount; ++i)
            neurons.Add(new Neuron(inputPerLayer));
    }
}


public class Neuron  {
  public int inputCount;
  public List<float> weights=new List<float>();
  public Neuron(int inputCount){
    this.inputCount = inputCount+1; //plus for the bias weight
    for (int i = 0; i < inputCount + 1; ++i)
      weights.Add( UnityEngine.Random.Range(-1.0f,1.0f) );
  }
}
