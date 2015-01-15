using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamadaDeNeuronios {

    //numero de neuronios nessa camada
    public int numeroDeNeuronios; 

    //a camada de neuronios
     public List<Neuronio> listaDeNeuronios= new List<Neuronio>();

  public CamadaDeNeuronios(int NumeroDeNeuronios, int NumeroDeInputsPorNeuronio)
  {
      numeroDeNeuronios = NumeroDeNeuronios;
    //  Debug.Log("camada sendo criado com " +numeroDeNeuronios.ToString()+" neuronios");
    //  Debug.Log("camada sendo criada numero de inputs por neuronio="+NumeroDeInputsPorNeuronio.ToString());
      for (int i = 0; i < NumeroDeNeuronios; ++i)
      {
          listaDeNeuronios.Add(new Neuronio(NumeroDeInputsPorNeuronio));
         // Debug.Log("neuronio adicionado com qt de pesos=" + listaDeNeuronios[i].numeroDeInputs.ToString());
      }

  }
}
