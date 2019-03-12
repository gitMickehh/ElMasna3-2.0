using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkerCustomizationSerializable
{
    public int HeadItem;
    public int FaceItem;
    public int BodyItem;

    public static WorkerCustomizationSerializable Empty()
    {
        return new WorkerCustomizationSerializable();
    }

    public WorkerCustomizationSerializable()
    {
        HeadItem = -1;
        FaceItem = -1;
        BodyItem = -1;
    }

    public WorkerCustomizationSerializable(CustomizationItem H, CustomizationItem F, CustomizationItem B)
    {
        HeadItem = H.id;
        FaceItem = F.id;
        BodyItem = B.id;
    }
}
