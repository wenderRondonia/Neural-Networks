using UnityEngine;
using System.Collections;

public static class Settings  {

    public static int inputCount = 4;
    public static int hiddenLayerCount = 1;
    public static int neuronsPerHiddenLayerCount = 6;
    public static int outputCount = 2;
    public static float ActivationResponse = 1; //sigmoid function
    public static float Bias = -1;
    public static float MutationRange = 0.4f;
    public static float RotationRange = 0.3f;
    public static float MaxSpeed = 0.4f;
    public static float ObjectiveArea = 1.5f;
    public static int ObjetiveCount = 40;
    public static int TankCount = 30;

    public static int CycleCount = 3000;

    public static float CrossoverRate = 0.7f;//recommended 0.7
    public static float MutationRate = 0.2f;//0.05 to 0.3
    public static int EliteNumber = 4; 
    public static int EliteCopyCount = 1;

    public static int MapScale = 1;
    public static int HeightWindow = 140;
    public static int WidthWindow = 140;
	
}
