using UnityEngine;
using UnityEngine.UI;

public class ChangeColorButton : MonoBehaviour
{
    public Material materialToChange;
    public Image myImage;

    public void ChangeColor()
    {
        materialToChange.color = myImage.color;
    }
}
