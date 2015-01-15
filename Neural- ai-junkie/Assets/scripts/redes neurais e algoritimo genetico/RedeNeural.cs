using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RedeNeural  {

     int NumeroDeInputs, NumeroDeOutputs,NumeroDeHiddenLayers, NeuroniosPorHiddenLayer;
     //camadas == hidden Layers
     //guarda uma lista de camadas de neuronios
     List<CamadaDeNeuronios> listaDeCamadas=new List<CamadaDeNeuronios>();
     //Cria uma rede neural artificial com base na classe de Parametros
     public RedeNeural()
     {
        NumeroDeInputs=Parametros.NumeroDeInputs;
        NumeroDeOutputs=Parametros.NumeroDeOutputs;
        NumeroDeHiddenLayers=Parametros.NumeroDeHiddenLayers;
        NeuroniosPorHiddenLayer=Parametros.NeuroniosPorHiddenLayer;
        CriarRedeNeural();
       
     }

     public void CriarRedeNeural()
     {
        // Debug.Log("criando rede neural");
         //esse método constroi uma rede neural artificial. Os pesos são todos inicializados com valores randomicos entre -1 e 1
         //cria as camadas de neurônios
         if (NumeroDeHiddenLayers > 0)
         {
             //cria a primeira camada            
             listaDeCamadas.Add(new CamadaDeNeuronios(NeuroniosPorHiddenLayer, NumeroDeInputs));
             //agora as camadas escondidas( hiddes layers )
             for (int i = 0; i < NumeroDeHiddenLayers - 1; ++i)
             {               
                 listaDeCamadas.Add(new CamadaDeNeuronios(NeuroniosPorHiddenLayer, NeuroniosPorHiddenLayer));
             }
             //cria a ultima camada (output layer)
             
             listaDeCamadas.Add(new CamadaDeNeuronios(NumeroDeOutputs, NumeroDeHiddenLayers)); 
         }
         else
         {
             //cria a ultima camada (output layer)
             listaDeCamadas.Add(new CamadaDeNeuronios(NumeroDeOutputs,NumeroDeInputs)); 
         }

        
     }

     //pega todos pesos da rede neural
     public List<double> Pesos()
     {
         //essa lista vai guardar os pesos
         List<double> pesos=new List<double>();
         //para cada camada
         for (int i = 0; i < NumeroDeHiddenLayers + 1; ++i)
         {
             //para cada neuronio
             for (int j = 0; j < listaDeCamadas[i].numeroDeNeuronios; ++j)
             {
                 //para cada peso
                 for (int k = 0; k < listaDeCamadas[i].listaDeNeuronios[j].numeroDeInputs; ++k)
                 {                  
                     pesos.Add(listaDeCamadas[i].listaDeNeuronios[j].listaDePesos[k]);
                 }
             }
         }
         return pesos;
     }

     //retorna o numero total de pesos da rede neural
     public int NumeroDePesos()
     {
         int NumeroDePesos = 0;
         //para cada camada
         for (int i = 0; i < NumeroDeHiddenLayers + 1; ++i)
         {
             //para cada neuronio        
             for (int j = 0; j < listaDeCamadas[i].numeroDeNeuronios; ++j)
             {
                 //para cada peso
              //   Debug.Log(" numero de inputs=" + listaDeCamadas[i].listaDeNeuronios[j].numeroDeInputs.ToString());
                 for (int k = 0; k < listaDeCamadas[i].listaDeNeuronios[j].numeroDeInputs; ++k)
                 {
                     NumeroDePesos++;
                 }
             }
         }

         return NumeroDePesos;
     }

     //substitui os pesos por novo vetor
     public void ColocarPesos(ref List<double> Pesos)
     {
         int pesoAtual = 0;
         //para cada camada
         for (int i = 0; i < NumeroDeHiddenLayers + 1; ++i)
         {
             //para cada neuronio
             for (int j = 0; j < listaDeCamadas[i].numeroDeNeuronios; ++j)
             {
                 //para cada peso
                // Debug.Log("numero de inputs" + listaDeCamadas[i].listaDeNeuronios[j].numeroDeInputs.ToString());
                 for (int k = 0; k < listaDeCamadas[i].listaDeNeuronios[j].numeroDeInputs; ++k)
                 {
                     listaDeCamadas[i].listaDeNeuronios[j].listaDePesos[k]=Pesos[pesoAtual++];
                     //Debug.Log("peso adicionado=" + listaDeCamadas[i].listaDeNeuronios[j].listaDePesos[k].ToString());
                 }
             }
         }    
         return;
     }

     //calcula os outputs dado os inputs
     public  List<double> Update(ref List<double> Inputs)
     {
        
              //pra guardar os outputs de cada camada
              List<double> outputs=new List<double>();
              int PesoAtual = 0;
              //checagem para numero corretos de input
              if (Inputs.Count != NumeroDeInputs)
              {             
                return outputs;
              }
              //pra cada camada....
              for (int i=0; i<NumeroDeHiddenLayers + 1; i++)
              {
                  if (i > 0)
                  {
                      for (int r = 0; r < Inputs.Count; r++)
                          Inputs[r] = outputs[r];
                  }
                          outputs.Clear();
                        PesoAtual = 0;                       
                        //Pra cada neuronio, soma todos os inputs*pesos correspondentes depois joga o total na funcao sigmoid pra ter o output
                        for (int j=0; j<listaDeCamadas[i].numeroDeNeuronios; ++j)
                        {                                                                                                                                               
                              double InputDaRede = 0;
                              int NumeroDeInputsLocal = listaDeCamadas[i].listaDeNeuronios[j].numeroDeInputs;
                              //Pra cada Peso..
                            
                              for (int k=0; k<NumeroDeInputsLocal - 1; ++k)
                              { 
                                  //soma os pesos x inputs                                   
                                    InputDaRede += listaDeCamadas[i].listaDeNeuronios[j].listaDePesos[k] * Inputs[PesoAtual++];
                                                                      
                              }
                              //adiciona no bias
                            
                              if(NumeroDeInputsLocal-1==-1)
                                  InputDaRede += listaDeCamadas[i].listaDeNeuronios[j].listaDePesos[listaDeCamadas[i].listaDeNeuronios[j].listaDePesos.Count- 1] * Parametros.Bias;
                              else
                                InputDaRede += listaDeCamadas[i].listaDeNeuronios[j].listaDePesos[NumeroDeInputsLocal-1] * Parametros.Bias;
                              //podemos guardar os outputs de cada camada a maneira que geramos ela                            
                              //a combinação da activação é a primeira coisa filtrada da funcao sigmoid
                             
                               outputs.Add(Sigmoid( InputDaRede , Parametros.RespostaDeAtivacao));                             
                               PesoAtual = 0;
                         }                      
              }
             

              return  outputs;
     }

      //sigmoid response curve
     public double Sigmoid(double inputDaRede, double response)
     {
         return (1 / ((1 + Math.Exp(-inputDaRede / response))));
        
     }

}
