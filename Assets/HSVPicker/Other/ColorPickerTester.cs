using UnityEngine;

public class ColorPickerTester : MonoBehaviour 
{
    public new Renderer renderer;
    public ColorPicker picker;

    public Color Color;

	void Start () 
    {
        if(renderer !=null)
        {
            Color = renderer.material.color;
            renderer.material.color = picker.CurrentColor;

            picker.onValueChanged.AddListener(color =>
            {
                renderer.material.color = color;
                Color = color;
            });


            picker.CurrentColor = Color;
        }
        
    }
	
}
