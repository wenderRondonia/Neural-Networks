using UnityEngine;
using System.Collections;

public static class Parametros  {

    public static int NumeroDeInputs = 4;
    public static int NumeroDeHiddenLayers = 1;
    public static int NeuroniosPorHiddenLayer = 6;
    public static int NumeroDeOutputs = 2;
    //para ajustar a funcao sigmoid
    public static double RespostaDeAtivacao = 1;
    //valor do bias
    public static double Bias = -1;
    //máximo que o Genomaalgoritimo pode fazer mutação com os pesos
    public static double PertubacaoMaxima = 0.4;

    public static double TaxaDeRotacaoMaxima = 0.3;
    public static double MaxSpeed = 0.4f;
    public static double AreaDoObjetivo = 1.5f;
   // public static double EscalaDoTanque = 1;

    public static int NumeroDeObjetivos = 40;
    public static int NumeroDeTanques = 30;

    public static int NumeroDeCiclos = 3000;

    public static double TaxaDeCrossOver = 0.7;
    public static double TaxaDeMutacao = 0.2;

    
    public static int NumeroDeElite = 4; //Os N melhores genomas usados na Epoch 
    public static int NumeroDeCopiasDeElite = 1;// numero de genomas da elite que serão copiados

    
    //--------------------------------------parametros gerais
    public static int escalaDoMapa = 1;
    public static int AlturaDaJanela = 140;
    public static int LarguraDaJanela = 140;
    //-------------------------------------used for the neural network



	
}
