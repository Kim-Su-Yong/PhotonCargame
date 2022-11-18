using UnityEngine;
using System.Collections;

public class CarColor : MonoBehaviour {
//----------------------------------------------------------------
//
//  This script allows the user to change the main color of each 
//  instance of all cars without adding new materials.
//
//----------------------------------------------------------------
    public Color car_color;
    public Material car_main_color;
    //----------------------------
    Renderer car_renderer;
    int mat_index;
    int randomIdx; //>>카 색 랜덤으로태어나게 

//----------------------------------------------------------------
void Start () 
{
    car_renderer = gameObject.GetComponent<Renderer>();           

    string user_mat_name = car_main_color + " ";
    for(int i=0; i<car_renderer.materials.Length; i++)
    {
        string obj_mat_name = car_renderer.transform.GetComponent<Renderer>().materials[i] + " ";
        bool match = true;
        for(int j=0;j<23;j++)
        {
            if(user_mat_name[j] != obj_mat_name[j])        
                match = false;
        }
        if(match == true)
            mat_index = i;
    }
        randomIdx = Random.Range(1, 5);//>>카 색 랜덤으로태어나게 
        if (randomIdx == 1)
            car_color = Color.red;
        else if(randomIdx==2)
            car_color = Color.white;
        else if(randomIdx==3)
            car_color = Color.yellow;
        else if (randomIdx == 4)
            car_color = Color.black;
        else if (randomIdx == 5)
            car_color = Color.blue;
        else if (randomIdx == 6)
            car_color = Color.green;










    }
//----------------------------------------------------------------
void Update ()
{
    car_renderer.transform.GetComponent<Renderer>().materials[mat_index].color = car_color;
}
//----------------------------------------------------------------
}
