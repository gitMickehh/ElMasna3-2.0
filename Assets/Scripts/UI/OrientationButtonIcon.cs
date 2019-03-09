using UnityEngine;
using UnityEngine.UI;

public class OrientationButtonIcon : MonoBehaviour
{
    public Sprite OrientationButton;
    public Sprite BackButton;

    [SerializeField]
    private Image myButton;

    /// <summary>
    /// true for Orientation icon
    /// false for Back Icon
    /// </summary>
    /// <param name="i"></param>
    public void SwitchButtonIcon(bool i)
    {
        if (!i)
            myButton.sprite = OrientationButton;
        else
            myButton.sprite = BackButton;
    }

}
