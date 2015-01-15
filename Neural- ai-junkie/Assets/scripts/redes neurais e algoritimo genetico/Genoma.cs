
using System.Collections;
using System.Collections.Generic;

public class Genoma  {

       public List <double>  listaDePesos=new List<double>();

       public double  dFitness;

       public Genoma()
       {
           dFitness=0;
       }

       public Genoma(ref List<double> pesos, double fitness)
       {
           listaDePesos=pesos; 
           dFitness=fitness;
       }

       //overload '<' para poder classificar os genomas
       public static bool operator <(Genoma lhs, Genoma rhs) 
         {
          return ( lhs.dFitness < rhs.dFitness);
        }
       public static bool operator >(Genoma lhs, Genoma rhs)
       {
           return (lhs.dFitness >rhs.dFitness);
       }

}
