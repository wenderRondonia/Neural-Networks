using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Neuronio  {

    //numero de inputs do neuronio
   public int numeroDeInputs;

    //cada input tem um peso
   public List<double> listaDePesos=new List<double>();

    //constructor
    public Neuronio(int NumeroDeInputs)
    {
        numeroDeInputs = NumeroDeInputs+1;
      //  Debug.Log("criando neuronio com "+numeroDeInputs.ToString()+"");
        //soma mais um para o peso -1 do bias
          for (int i = 0; i < NumeroDeInputs + 1; ++i)
          {
            //inicializa os pesos em valores randomicos -1 a 1
            listaDePesos.Add( Random.Range(-1.0f,1.0f) );
           // Debug.Log("peso adicionado="+listaDePesos[i].ToString());
            
          }
    }
}
