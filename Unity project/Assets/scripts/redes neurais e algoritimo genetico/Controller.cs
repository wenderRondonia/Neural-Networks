using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Controller  {

    int TotalDeObjetivosEncontrados = 0;
    float tempo = 0;
    public GameObject Tanque;
    public GameObject Objetivo;
    public List<GameObject> tanques=new List<GameObject>();
    public List<GameObject> objetivos = new List<GameObject>();
        //guarda para populacao de genomas
         List<Genoma> Apopulacao = new List<Genoma>();
        //guarda os tanques
         List<Tanque> listaDeTanques = new List<Tanque>();

         //e os objetivos
         List<Vector2> listaDeObjetivos=new List<Vector2>();

         //referencia para o GenomaAlgoritimo
         GenomaAlgoritimo genomaAlgoritimo;

         int NumeroDeTanques;
         int NumeroDeObjetivos;
         int NumeroDePesosNaRedeNeural;

         //vertice buffer da forma do tanque
        // List<Vector2> tanqueVB=new List<Vector2>();
         
         //vertice bufer para a forma do objetivo
        // List<Vector2> objetivosVB= new List<Vector2>();
        
        //guarda o melhor fitness por geração
         List<double> listaMedioFitness= new List<double>();
        
         //guarda o melhor fitness por geração
         List<double> listaMelhorFitness= new List<double>();

	     //fator que controla a velocidade da simulação
        public bool RapidoRenderizador;
        //ciclos por regeneração
         int CiclosPorGeracao;
        //contador de gerações
         int numeroDeGeracoes;
        //Dimensoes da janela
         int DimensaoXJanela, DimensaoYJanela;
        
         public Controller(GameObject tanque, GameObject objetivo)
         {
                genomaAlgoritimo = null;
                RapidoRenderizador = false;
                CiclosPorGeracao = 0;
                NumeroDeTanques = Parametros.NumeroDeTanques;
                NumeroDeObjetivos = Parametros.NumeroDeObjetivos;
                numeroDeGeracoes = 0;
                DimensaoXJanela = Parametros.LarguraDaJanela;
                DimensaoYJanela = Parametros.AlturaDaJanela;
                //cria todos os tanques
                for (int i = 0; i < NumeroDeTanques; ++i)
                {
                    listaDeTanques.Add(new Tanque());

                    tanques.Add(GameObject.Instantiate(tanque, new Vector3(Parametros.escalaDoMapa * listaDeTanques[i].posicao.x, 0.3f, Parametros.escalaDoMapa * listaDeTanques[i].posicao.y), Quaternion.Euler(0, (float)listaDeTanques[i].rotacao, 0)) as GameObject);
                }
                //pega o total dos pesos usados nos tanques
                NumeroDePesosNaRedeNeural = listaDeTanques[0].NumeroDePesos();
                //inicializa a classe de algoritimo genetico
                genomaAlgoritimo = new GenomaAlgoritimo(NumeroDeTanques, Parametros.TaxaDeMutacao, Parametros.TaxaDeCrossOver, NumeroDePesosNaRedeNeural );
                //pegas os pesos e coloca na populacao
                Apopulacao = genomaAlgoritimo.GetChromos();
                //coloca os pesos
                for (int i = 0; i < NumeroDeTanques; ++i)
                {
                    listaDeTanques[i].ColocarPesos(ref Apopulacao[i].listaDePesos);
                }
                 //inicializa os objetivos em posicoes aleatorias
                for (int i = 0; i < NumeroDeObjetivos; ++i)
                {
                    Vector2 vetor = new Vector2((float)(Random.value * DimensaoXJanela), (float)(Random.value * DimensaoYJanela));
                    listaDeObjetivos.Add(vetor);
                    objetivos.Add(GameObject.Instantiate(objetivo, new Vector3(vetor.x, 0.3f, vetor.y), Quaternion.identity) as GameObject);
                }
               
         }

         public   bool Update()
        {
            tempo += Time.deltaTime;
          
               //roda os 'tanques' através do numero de gerações para quantidade de ciclos. 
               //Durante esse loop cada rede neural de cada "tanque"  é constantemente atualizada com
               // as informações apropriadas do seu redor. O output das redes neurais é obtido e os tanques se movem.
               //se ele encontrar o "objetivo" seu fitness é atualizado apropriadamente ( ele é foda! )

             if (CiclosPorGeracao++ < Parametros.NumeroDeCiclos) //aqui roda uma geração(um loop de vários ciclos)
              {
                  
                    for (int i=0; i<NumeroDeTanques; ++i)
                    {
                         //atualiza a rede neural e a posicao
                          if (!listaDeTanques[i].Update(ref listaDeObjetivos))
                          {
                            //erro ao processar a rede neural
                            Debug.Log("ERRO: Wrong amount of NN inputs!");
                            return false;
                          }
                          //olha se achou o objetivo
                          int indiceDoObjetivo = listaDeTanques[i].ChecarObjetivos(ref listaDeObjetivos);
                          if (indiceDoObjetivo >= 0)
                          {                           
                              TotalDeObjetivosEncontrados++;
                              //tanque descobriu o objetivo!!!
                               listaDeTanques[i].IncrementarFitness();
                               //mina encontrada então adiciona uma em um lugar aleatório
                               //posição
                               listaDeObjetivos[indiceDoObjetivo] = new Vector2(Random.value * DimensaoXJanela, Random.value * DimensaoYJanela);
                          }
                         //atualiza os pontos de fitness
                         Apopulacao[i].dFitness = listaDeTanques[i].Fitness();
                         
                }
               //     System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/Resultados/Geracao"+numeroDeGeracoes.ToString()+".txt");
                //    file.WriteLine(Dados);
                //    file.Close();
              }
              //outra geração foi completada
              //hora de rodar o algoritimo genético  para os tanques renovarem suas redes neurais
              else
              {
                   tanques[genomaAlgoritimo.melhorGenoma].renderer.material.color = Color.white;
                    //incrementa o numero de geração
                    ++numeroDeGeracoes;
            
                    //resetar os ciclos
                    CiclosPorGeracao = 0;

                    //roda o algoritimo genético para uma nova populacao
                    Apopulacao = genomaAlgoritimo. Epoch(ref Apopulacao);
                    //insere a nova estrutura da rede neural de volta nos tanques
                    //e reseta status
                    for (int i=0; i<NumeroDeTanques; ++i)
                    {
                          listaDeTanques[i].ColocarPesos(ref Apopulacao[i].listaDePesos);
                          listaDeTanques[i].Reset();
                    }
              }

             //atualiza os objetos na cena
              for (int i = 0; i < NumeroDeTanques; ++i)
              {
                  
                  tanques[i].transform.position = new Vector3(Parametros.escalaDoMapa * listaDeTanques[i].posicao.x, 0.3f, -Parametros.escalaDoMapa * listaDeTanques[i].posicao.y);
                  if(Vector2.Dot(Vector2.up, listaDeTanques[i].visao)<0)
                      tanques[i].transform.rotation = Quaternion.Euler(0, -Vector2.Angle(Vector2.right, listaDeTanques[i].visao), 0);
                  else
                      tanques[i].transform.rotation = Quaternion.Euler(0, Vector2.Angle(Vector2.right, listaDeTanques[i].visao) , 0);
              }
              for (int i = 0; i < NumeroDeObjetivos; ++i)
              {
                  objetivos[i].transform.position = new Vector3((float)(Parametros.escalaDoMapa * listaDeObjetivos[i].x), 0.3f,-(float)( Parametros.escalaDoMapa * listaDeObjetivos[i].y));
              }

              return true;
        }

         public void Render()
         { 
             //mostra os status
             GUI.Box(new Rect(10, 10, 100, 30), "Geração= " + numeroDeGeracoes.ToString());
                 GUI.Box(new Rect(10, 60, 200, 30), " melhor fitness="+ genomaAlgoritimo.melhorfitnessDaPopulacao.ToString());
                 GUI.Box(new Rect(10, 95, 200, 30), " fitness medio=" + genomaAlgoritimo.fitnessMedioDaPopulacao.ToString());
                 GUI.Box(new Rect(10, 130, 200, 30), " fitness pior =" + genomaAlgoritimo.piorfitnessDaPopulacao.ToString());

                 GUI.Box(new Rect(10, Screen.height-50, 300, 40), " Objetivos encontrados por segundo=" +TotalDeObjetivosEncontrados/tempo);

                 GUI.Box(new Rect(Screen.width-10 - 100, 10, 100, 40), "r-restart ");
                 GUI.Box(new Rect(Screen.width - 10 - 300, 50, 300, 40), "seta cima/baixo-aumenta/diminui velocidade ");
                
                
                 tanques[genomaAlgoritimo.melhorGenoma].renderer.material.color = Color.red;
                 
            //colocar mais uns gráficos aqui
         }
}
