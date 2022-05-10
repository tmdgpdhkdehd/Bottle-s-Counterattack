using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverColor2D : MonoBehaviour
{

    public enum MOVERCOLOR
    {
        RED = 0,
        BLUE,
        YELLOW,
        GREEN
    }

    [HideInInspector]
    public string moverColorName;

    public MOVERCOLOR moverColor = MOVERCOLOR.RED;


    private void Start()
    {
        switch (moverColor)
        {
            case MOVERCOLOR.RED:
                moverColorName = "RED";
                break;
            case MOVERCOLOR.BLUE:
                moverColorName = "BLUE";
                break;
            case MOVERCOLOR.YELLOW:
                moverColorName = "YELLOW";
                break;
            case MOVERCOLOR.GREEN:
                moverColorName = "GREEN";
                break;
        }
    }

}
