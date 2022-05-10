using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompPlate2D : MonoBehaviour
{
    GameObject parents;
    public enum PLATECOLOR
    {
        RED = 0,
        BLUE,
        YELLOW,
        GREEN
    }

    string plateColorName;

    SpriteRenderer _PlateColor;

    Color RedPlate;
    Color GreenPlate;


    [HideInInspector]
    public bool isStomped = false;

    public PLATECOLOR plateColor = PLATECOLOR.RED;

    private void Awake()
    {
        parents = this.transform.parent.gameObject;

        switch (plateColor)
        {
            case PLATECOLOR.RED:
                plateColorName = "RED";
                break;
            case PLATECOLOR.BLUE:
                plateColorName = "BLUE";
                break;
            case PLATECOLOR.YELLOW:
                plateColorName = "YELLOW";
                break;
            case PLATECOLOR.GREEN:
                plateColorName = "GREEN";
                break;
        }

        _PlateColor = GetComponent<SpriteRenderer>();

        RedPlate = Color.red;
        GreenPlate = Color.green;

    }

    // Start is called before the first frame update
    void Start()
    {
        if (plateColor == PLATECOLOR.RED)
        {
            _PlateColor.color = RedPlate;
        }
        else if (plateColor == PLATECOLOR.GREEN)
        {
            _PlateColor.color = GreenPlate;
        }


        Debug.Log(_PlateColor.color);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.gameObject.GetComponent<MoverColor2D>().moverColorName == plateColorName)
        {

            if (!isStomped)
            {
                isStomped = true;
                Debug.Log("토글 켜짐");
                _PlateColor.color = Color.gray;
            }
            else if (isStomped)
            {
                isStomped = false;
                Debug.Log("토글 꺼짐");
                _PlateColor.color = Color.red;
            }
        }
    }
}
