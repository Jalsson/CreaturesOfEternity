using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaController : MonoBehaviour {

    private float startingStamina = 100;

    [SerializeField]
    private float currentStamina;

    [SerializeField]
    private Slider sliderStamina;
	// Use this for initialization
	void Start () {
        currentStamina = startingStamina;
	}
	
	// Update is called once per frame

        //reloads stamina and refresh slider. Wont go over 100 stamina.
	void Update () {
        currentStamina += 0.1f;
        if(sliderStamina != null)
        sliderStamina.value = currentStamina;
        
        if(currentStamina > 100)
        {
            currentStamina = 100;
        }


	}

    //takes the given amount of stamina out from current stamina and sets slider correctly
    public void useStamina(float amount)
    {
        currentStamina -= amount;
        //stamina wont go under 0
        if(currentStamina < 0)
        {
            currentStamina = 0;
        }

        if (sliderStamina != null)
            sliderStamina.value = currentStamina;

    }

    //returns current stamina
   public float getCurrenStamina()
    {
        return currentStamina;
    }
    

}
