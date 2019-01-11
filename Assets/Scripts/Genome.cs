
using System.Collections;
using System.Collections.Generic;

public class Genome  {
    public List<float> weights=new List<float>();
    public float dFitness;

    public Genome(){
        dFitness=0;
    }

    public Genome(ref List<float> pesos, float fitness){
        weights=pesos; 
        dFitness=fitness;
    }

    public static bool operator <(Genome lhs, Genome rhs){
        return ( lhs.dFitness < rhs.dFitness);
    }
    public static bool operator >(Genome lhs, Genome rhs){
        return (lhs.dFitness >rhs.dFitness);
    }

}
