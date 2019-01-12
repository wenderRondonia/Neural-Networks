using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Main : MonoBehaviour {
    public GameObject tank,objetive;
    public Text text;
    Controller controller = null;
    int speed = 1;
    float aux = 0;
	void Start () {
        controller = new Controller(tank,objetive);
	}
    
	void Update () {

        if (Input.GetKeyDown("up")) speed++;

        if (Input.GetKeyDown("down")) speed--;
        speed = Mathf.Clamp(speed,1,50);
        aux += Time.deltaTime;
        if (speed == 1){
            if (aux > 0.0166f){
                aux = 0;
                controller.Update();
            }
        }else{
            for (int i = 0; i < speed; i++){           
                controller.Update();              
            }
        }
        if (Input.GetKeyDown("r"))
            controller = new Controller(tank, objetive);
        UpdateUI();      
	}

 
    void UpdateUI(){
        
        text.text = " generation= " + controller.generationCount
        +"\n best="+ controller.geneticAlgorithm.bestFitness
        +"\n median=" + controller.geneticAlgorithm.medianFitness
        +"\n worst=" + controller.geneticAlgorithm.worstFitness
        +"\n objectives/sec=" + controller.totalObjectiveFound/controller.time
        +"\n speed="+speed+"x"
        +"\n Controls: r-restart up-fast down-slow";

        
        controller.tanksObjects[controller.geneticAlgorithm.bestGenome].GetComponent<Renderer>().material.color = Color.red;
    }

    
}
