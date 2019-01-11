using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    public GameObject tank,objetive;
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
                
	}
    void OnGUI(){
        controller.Render();
        GUI.Box(new Rect(Screen.width - 10 -100, 110, 100, 40), "speed="+speed+"x");
          
    }
    
}
