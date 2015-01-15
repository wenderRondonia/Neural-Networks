using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenomaAlgoritimo  {
                
        //guarda toda a população de cromossomos/ genomas (um individuo seria um genoma)
        public List <Genoma> listaDeGenomas=new List<Genoma>(); 

        //tamanho da população de cromossomos
        int tamanhoDaPopulacao=0;

        //quantidade de pesos por cromossomo
        int quantidadeDePesosPorCromossomo=0;

        // fitness total da populacao
        double fitnessTotalDaPopulacao=0; 

        //melhor fitness da população
       public double melhorfitnessDaPopulacao=0;

        //medio fitness
       public double fitnessMedioDaPopulacao=0;

        //pior
       public double piorfitnessDaPopulacao=0;

        //guarda o melhor genoma
       public int melhorGenoma=0;

        //probabilidade que os cromossomos vao sofrer mutacao (indicado seria entre 0.05 para 0.3)
        double taxaDeMutacao=0; 

        //probabilidade que os cromossomos sofram crossover (indicado seria 0.7) yeah yeah hue
        double taxaDeCrossOver=0; //pode haver varios tipos de crossover - só entre os bons-bons e ruins-caras dos meios- tem que experimentar

        //contador de gerações (nomes sugestivos né)
     //   int contadorDeGeracoes=0; 

        //melhor pesquisar  o que é crossover em genética pra ter uma idéia
        void Crossover(ref List<double> Mae, ref List<double> Pai, ref List<double> bebe1, ref List<double> bebe2)
        {
            //retornando os pais como filhos dependente da taxa de crossover ou se os parentes são o mesmo 
            if ((Random.value > taxaDeCrossOver) || (Mae == Pai))
            {
                bebe1 = Mae;
                bebe2 = Pai;
                return;
            }
            //determinando um ponto do crossover
            int corte = Random.Range(0, quantidadeDePesosPorCromossomo - 1);

            //cria os filhos
            for (int i = 0; i < corte; ++i)
            {
                bebe1.Add(Mae[i]);
                bebe2.Add(Pai[i]);
            }

            for (int i = corte; i < Mae.Count; ++i)
            {
                bebe1.Add(Mae[i]);
                bebe2.Add(Pai[i]);
            }
            return;
        
        }

        //mutação
        void FazerMutacao(ref List<double> cromossomo)
        { 
            //aqui a mutação acontece por portubação dos pesos em uma quantidade menor do que a definida Parametros.PertubacaoMaxima
            //atravessamos o cromossomo e fazemos a mutação dele com peso dependente da taxa de mutação
            for (int i=0; i<cromossomo.Count; ++i)
	        {
		        //devemos pertubar esse peso?
		        if (Random.value < taxaDeMutacao)
		        {
			        //mutação consiste de uma pequena pertubação(subtrai ou soma um bostésimo)
			        cromossomo[i] += (Random.Range(-1.0f,1.0f) * Parametros.PertubacaoMaxima);
		        }
	        }

        }
        //tipo gira-gira do silvio santos Má Óieê
        Genoma pegarCromossomoAleatorio()
        { 
            //gera um numero random entre 0 e o numero total de fitness 
            double Corte = (double)(Random.value * fitnessTotalDaPopulacao);

            //isso vai ser um conjunto dos cromossomos escolhidos
            Genoma Protagonista=new Genoma();

            // vá através dos cromossomos e adicionando fitness
            double FitnessAteAgora = 0;
            for (int i = 0; i < tamanhoDaPopulacao; ++i) //gira e gira girando hehe
            {
                FitnessAteAgora += listaDeGenomas[i].dFitness;

                //se fitnessAteAgora for maior que o numero randomico retorna o cromossomo até este ponto 
                if (FitnessAteAgora >= Corte)
                {
                    Protagonista = listaDeGenomas[i];
                    break;
                }
            }

            return Protagonista;
        }

        //essa seleção funcionana como uma forma avançada de eleitismo inserindo NumeroDeCopias
        //cópias dos N melhores (fittest mais alto) genomas dentro da lista populacao
        void PegaOsNMelhores(int NMelhores, int NumeroDeCopias, ref List<Genoma> populacao)//populacao -> é uma lista de genomas
        { 
            //adiciona a quantidade necessaria de cópias dos n melhores fittest para fornecer ao vetor
            while (NMelhores-- > 0)
            {
                 if (NMelhores < 0)
                     Debug.Log("erro: numero negativo");
                
                for (int i = 0; i < NumeroDeCopias; ++i)
                {
                    if (tamanhoDaPopulacao - 1 - NMelhores <0)
                    {
                        Debug.Log("index negativo");
                        
                    }
                    else
                    populacao.Add(listaDeGenomas[(tamanhoDaPopulacao-1)-NMelhores]);
                }
            
            }
        }
       
        //calcula os mais fraco genoma e a média/total dos fitness
        void CalcularMelhorPiorMedioETotal()
        {
            fitnessTotalDaPopulacao = 0;
            double MaisAltoAteAgora = 0;
            double MaisBaixoAteAgora = 9999999;
            for (int i = 0; i < tamanhoDaPopulacao; ++i)
            {
                //atualiza o melhor fitness se necessário
                if (listaDeGenomas[i].dFitness > MaisAltoAteAgora)
                {
                    MaisAltoAteAgora = listaDeGenomas[i].dFitness;
                    melhorGenoma = i;
                    
                    melhorfitnessDaPopulacao = MaisAltoAteAgora;
                }

                //atualiza o pior fitness se necessario
                if (listaDeGenomas[i].dFitness < MaisBaixoAteAgora)
                {
                    MaisBaixoAteAgora = listaDeGenomas[i].dFitness;
                    piorfitnessDaPopulacao = MaisBaixoAteAgora;
                }
                fitnessTotalDaPopulacao += listaDeGenomas[i].dFitness;
            }//proximo cromossomo
            fitnessMedioDaPopulacao = fitnessTotalDaPopulacao / tamanhoDaPopulacao;
        }

        void Reset()
        {
            fitnessTotalDaPopulacao = 0;
            melhorfitnessDaPopulacao = 0;
            piorfitnessDaPopulacao = 9999999;
            fitnessMedioDaPopulacao = 0;
        }

        public GenomaAlgoritimo(int populacaotamanho, double taxamutacao, double taxacrossover, int quantidadedepesos)
        {
            tamanhoDaPopulacao = populacaotamanho;
            taxaDeMutacao = taxamutacao;
            taxaDeCrossOver = taxacrossover;
            quantidadeDePesosPorCromossomo=quantidadedepesos;
            fitnessTotalDaPopulacao=0;
           
            melhorfitnessDaPopulacao=0;
            melhorGenoma=0;   
            piorfitnessDaPopulacao=99999999;
            fitnessMedioDaPopulacao = 0;
            //inicializa a populacao com os cromossomos com pesos randomicos e todos os fitness ajustados para zero!           
            for (int i = 0; i < tamanhoDaPopulacao; ++i)
            {                
                listaDeGenomas.Add(new Genoma());
              //  Debug.Log("quantidade de pesos por cromossomo="+quantidadeDePesosPorCromossomo.ToString());
                for (int j = 0; j < quantidadeDePesosPorCromossomo; ++j)
                {              
                    listaDeGenomas[i].listaDePesos.Add(Random.Range(-1.0f,1.0f));
                   // Debug.Log("peso adicionado=" + listaDeGenomas[i].listaDePesos[j].ToString());
                }
            }
        }

        public static int CompareGenoma(Genoma lhs, Genoma rhs)
        {
            if (lhs.dFitness > rhs.dFitness)
                return 1;
            if (lhs.dFitness < rhs.dFitness)
                return -1;
            return 0;
         }
        //isso roda o algoritimo de genoma para uma populacao
        public List<Genoma> Epoch(ref List<Genoma> populacaoAntiga)
        { 
             //atualiza a populacao
              listaDeGenomas = populacaoAntiga;

              //reseta as variaveis apropriadas
              Reset();

              //organiza a populacao por elitismo de fitness
                listaDeGenomas.Sort(CompareGenoma);
               
              //calcula melhor pior media e total..mas não é?!
	            CalcularMelhorPiorMedioETotal();
              //vetor temporário pra os novo cromossomos
	            List <Genoma> listaNovaPopulacao=new List<Genoma>();
               //agora, para adicionar um pouco de elitismo devemos adicionar algumas cópias dos melhores genomas
                //tenha certeza que nós adicinemos até um numero impar ou a funcao aleatória vai quebrar
	            if ((Parametros.NumeroDeCopiasDeElite * (Parametros.NumeroDeElite % 2 ))==0 ) //pra que der par
	            {
                    PegaOsNMelhores(Parametros.NumeroDeElite,Parametros.NumeroDeCopiasDeElite,ref listaNovaPopulacao);
	            }

	            //Agora vamos ao loop do GenomaAlgoritimo!	
	            //repete até uma nova popula seja gerada
                while (listaNovaPopulacao.Count < tamanhoDaPopulacao)
	            {
		            //pega dois cromossomos
		            Genoma Mae = pegarCromossomoAleatorio();
		            Genoma Pai = pegarCromossomoAleatorio();

		            //pega dois filhos via crossover
                    List<double> bebe1 = new List<double>();
                    List<double> bebe2 = new List<double>();
		            Crossover(ref Mae.listaDePesos, ref Pai.listaDePesos,ref bebe1,ref bebe2);

		            //agora usa uma mutação neles ..mais o elemento x
		            FazerMutacao(ref bebe1);
		            FazerMutacao(ref bebe2);

		            //agora insere eles na nova populacao
                    listaNovaPopulacao.Add(new Genoma(ref bebe1, 0));
                    listaNovaPopulacao.Add(new Genoma(ref bebe2, 0));
	            }
	            //acabouuu agora recoloca na lista da populacao
                listaDeGenomas = listaNovaPopulacao;

	return listaDeGenomas;
        
        }


        //-------------------métodos para acessar as variaveis
        public List<Genoma> GetChromos(){    return listaDeGenomas; }

        public double MedioFitness(){   return ( fitnessTotalDaPopulacao/ tamanhoDaPopulacao);}

        double BestFitness(){   return melhorfitnessDaPopulacao;}

}
