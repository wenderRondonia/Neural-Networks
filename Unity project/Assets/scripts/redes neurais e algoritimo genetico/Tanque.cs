using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tanque  {

   
    //a rede neural do tanque
    RedeNeural cerebro=new RedeNeural(); 
    
	//suas coordenadas
	public Vector2	posicao=new Vector2(0,0);

	//direcao que ele esta olhando
	public Vector2	visao;

	//sua rotacao (surprise surprise)
	public double rotacao=0;

    //velocidade.. aha!
	public double velocidade=0;

	//o output da rede neural dele
	double	virarParaEsquerda=0;
      double   virarParaDireita=0;

	//sua pontuacao de Fitness
	double	dFitness=0;

	//sua escala 
	//double			dEscala=0;

    //seu indice do objetivo mais proximo
    int         iObjetivoMaisPerto=0;
  

    public	Tanque()
    {
            rotacao=Random.value*Mathf.PI*2;
            virarParaEsquerda=0.16f;
            virarParaDireita=0.16f;
            dFitness=0;
           // dEscala=Parametros.EscalaDoTanque;
            iObjetivoMaisPerto=0;
            posicao=new Vector2((float)(Random.value*Parametros.LarguraDaJanela),(float)(Random.value*Parametros.AlturaDaJanela));
            velocidade = Parametros.MaxSpeed;
    } 
	
    //atualiza as informações rede neural artificial das acoes do tanque
    //primeirp pegamos sensores de leitura and alimentamos no cerebor(rede neural)
    //os inputs são:
    //
    //o vetor mais proximo do objetivo ( x, y )
    //o vetor de visao ( x , y )
    //
    //recebe-se 2 outputs da rede neural: virar pra esquerda e virar pra direita
    //então, dada uma forca que é calculada dada esses 2 outputs
    //fazemos uma aceleracao and aplicamos no vetor velocidade 
	public bool	Update(ref List<Vector2> objetivos)
    {
        //guarda todos os inputs para a rede neural
        List<double> inputs=new List<double>();

        //pega o vetor para proximo do objetivo
        Vector2 vetorMaisPertoDoObjetivo=VetorMaisPertoDoObjetivo(ref objetivos);

        vetorMaisPertoDoObjetivo.Normalize();
        //adiciona o vetor para o mais perto do objetivo
        inputs.Add(vetorMaisPertoDoObjetivo.x);
        inputs.Add(vetorMaisPertoDoObjetivo.y);

        //adiciona no tanque a visao
        inputs.Add(visao.x);
        inputs.Add(visao.y);

        //atualiza o cerebro e pega um feedback (pensaa ahaha ele está vivo muahahaha)
       
                 
         List<double> output= cerebro.Update(ref inputs);
      //   Debug.Log(output[0]+ "   " +output[1]);
      
        //tenha certeza que não tenha erro ao calcular o output
        if(output.Count < Parametros.NumeroDeOutputs)
        {
            return false;
        }
        //coloca os outputs para os tanques virarem pra esquerda ou direita
        float virarParaDireita = (float)output[1];
        float virarParaEsquerda = (float)output[0];
       
        //controla a rotação
        float rotacaoaux = virarParaEsquerda-virarParaDireita;  
       rotacaoaux= Mathf.Clamp((float)rotacaoaux,-(float)Parametros.TaxaDeRotacaoMaxima,(float)Parametros.TaxaDeRotacaoMaxima);
        
        rotacao += rotacaoaux;
        velocidade = virarParaEsquerda + virarParaDireita;  
       // Debug.Log(rotacao);
        float Velocidade = (float)(virarParaDireita + virarParaEsquerda);
        velocidade = Mathf.Clamp(Velocidade, (float)Parametros.MaxSpeed/5, (float)Parametros.MaxSpeed);
        
        //atualiza a visão
      //  Debug.Log(rotacao);
        visao.x=-Mathf.Sin((float)rotacao);
        visao.y = Mathf.Cos((float)rotacao );
       
        visao.Normalize();
        //atualiza posição
        
        posicao += visao * (float)velocidade;
       // Debug.Log(Vector2.Angle(Vector2.right, visao));
        
        //envelopar em volta da janela
       if (posicao.x > Parametros.AlturaDaJanela) posicao.x = 0;
       if (posicao.x < 0) posicao.x = (float)Parametros.LarguraDaJanela;
       if (posicao.y > Parametros.AlturaDaJanela) posicao.y = 0;
       if (posicao.y <0)  posicao.y = (float)Parametros.AlturaDaJanela;

        return true; //enfimm :D
    }

	//retorna o vetor mais perto do Objetivo
    public Vector2	VetorMaisPertoDoObjetivo(ref List<Vector2> Objetivos)
    {
         double MaisPertoAteAgora=99999;
         Vector2 vetorMaisPertoDoObjetivo=new Vector2(0,0);
         //circulo onde tem que encontrar os Objetivos mais proximos

         //para todos os objetivos
         for(int i=0; i<Objetivos.Count;i++)
         {
            double distanciaDoObjetivo=(Objetivos[i]-posicao).magnitude;
            if(distanciaDoObjetivo < MaisPertoAteAgora )
            {
                MaisPertoAteAgora=distanciaDoObjetivo;
                vetorMaisPertoDoObjetivo=posicao-Objetivos[i];

                iObjetivoMaisPerto=i;
            }
         }
         return vetorMaisPertoDoObjetivo;

    }

      //checks to see if the minesweeper has 'collected' a mine
   public int ChecarObjetivos(ref List<Vector2> objetivos)
      {
        Vector2 distanciaParaObjeto= posicao - objetivos[iObjetivoMaisPerto];

        if(distanciaParaObjeto.magnitude < (Parametros.AreaDoObjetivo ))
        {
            return iObjetivoMaisPerto;
        }
        return -1;
      }

   public void Reset()
      {
            //reseta as posicoes
	        posicao = new Vector2((float)(Random.value * Parametros.LarguraDaJanela), (float)(Random.value * Parametros.AlturaDaJanela));	
	        // e os fitness
	        dFitness = 0;
            // e rotocoes
            rotacao = Random.value*2*Mathf.PI;
	        return;
      }

	//-------------------accessor functions
	public Vector2	Position(){return posicao;}

	public void	IncrementarFitness(){++dFitness;}

	public double Fitness(){return dFitness;}

    public void ColocarPesos(ref List<double> peso) { cerebro.ColocarPesos(ref peso); return; }

    public int NumeroDePesos(){ return cerebro.NumeroDePesos();}
	
}
