using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    //istancia da classe que ira controlar a simulação
    Controller Controlador = null;
    public GameObject Tanque,Objetivo;
    
	void Start () {
        Controlador = new Controller(Tanque,Objetivo);
        
	}
    
    int velocidade = 1;
    float aux = 0;
	void Update () {

        if (velocidade <= 20 && velocidade >= 1)
        {
            if (Input.GetKeyDown("up"))
                velocidade++;
        }

         if (velocidade <= 21 && velocidade >= 2)
        {
            if (Input.GetKeyDown("down"))
                velocidade--;
        }
         aux += Time.deltaTime;
         if (velocidade == 1)
         {
             if (aux > 0.0166)
             {
                 aux = 0;
                 if (!Controlador.Update())
                     Application.Quit();
             }
         }
         else
         {
             for (int i = 0; i < velocidade; i++)
             {           
                     if (!Controlador.Update())
                         Application.Quit();               
             }
         }
            if (Input.GetKeyDown("r"))
                Controlador = new Controller(Tanque, Objetivo);
                
	}
    void OnGUI()
    {
        Controlador.Render();
        GUI.Box(new Rect(Screen.width - 10 -100, 110, 100, 40), "velocidade="+velocidade.ToString()+"x");
        if(GUI.Button(new Rect(Screen.width - 10 - 100, Screen.height - 50, 100, 40), "sair"))
            Application.Quit();       
    }
    
}
