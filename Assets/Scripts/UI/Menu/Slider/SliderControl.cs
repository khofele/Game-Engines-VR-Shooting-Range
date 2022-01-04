using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour {

    private float clickValue = 0f;
    [SerializeField] private float increment;
    private Slider incrementorSlider;
    private float maxSliderValue;
    private float minSliderValue;
    private float limit;
 

    // Use this for initialization
    void Start () {

        //Get the components which the script is attached to & slider
        incrementorSlider = GetComponent<Slider>();
        maxSliderValue = incrementorSlider.maxValue;
        minSliderValue = incrementorSlider.minValue;

    }
	
	// Update is called once per frame
	void Update () {

        //Update the Slider if there is a change in the button's state
        incrementorSlider.value = clickValue;
  
    }

    //Increase the value of the float variable "clickValue" by one increment
    public void IncreaseByOne()
    {
        if (clickValue < maxSliderValue)
        {
            limit = maxSliderValue - increment;
            if (clickValue < limit)
            {
                clickValue = clickValue + increment;
            }
            else
            {
                clickValue = maxSliderValue;
            }
        }
    }

    //Decrease the value of the float variable "clickValue" by one increment
    public void DecreaseByOne()
    {
        if (clickValue > minSliderValue)
        {
            if (clickValue > increment)
            {
                clickValue = clickValue - increment;
            }
            else
            {
                clickValue = minSliderValue;
            }
        }
    }


}
