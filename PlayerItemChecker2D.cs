using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemChecker2D : MonoBehaviour
{

    public GameObject invenSlotOne;
    public GameObject invenSlotTwo;
    public GameObject invenSlotThr;

    public GameObject importantInven;

    [HideInInspector]
    public bool isHaveInSlotOne = false;
    [HideInInspector]
    public bool isHaveInSlotTwo = false;
    [HideInInspector]
    public bool isHaveInSlotThr = false;

    [HideInInspector]
    public string keyOne;
    [HideInInspector]
    public string keyTwo;
    [HideInInspector]
    public string keyThr;

    string keyName;
    string needKeyName;

    [HideInInspector]
    public GameObject _TextObj;
    TextMesh _textMesh;

    public Text keyText;

    
    public List<Sprite> KeyImages = new List<Sprite>();
    Sprite invenKeyImg;

    bool isOnKey = false;
    bool isOnChecker = false;

    bool isOnRecipe = false;
    bool isGetRecipe = false;

    GameObject GateWhatOned;
    GameObject KeyWhatOnde;
    GameObject RecipeWhatOned;




    public static PlayerItemChecker2D instance;

    private void Awake()
    {
        instance = this;

        //_TextObj = this.transform.Find("TextMesh").gameObject;
        //_textMesh = _TextObj.GetComponent<TextMesh>();

    }

    // Start is called before the first frame update
    void Start()
    {
        //_TextObj.SetActive(false);

        keyText.enabled = false;

        importantInven.transform.GetChild(0).gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(isOnKey && Input.GetKeyDown(KeyCode.E))
        {
            GetTheKey();
        }
        else if (isOnChecker && Input.GetKeyDown(KeyCode.Q))
        {
            UseTheKey();
        }
        else if(isOnRecipe && Input.GetKeyDown(KeyCode.E))
        {
            GetTheRecipe();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "KeyChecker")
        {
            GateWhatOned = collision.transform.parent.gameObject;

            isOnChecker = true;
            //_textMesh.text = "Push Q";
            //_TextObj.SetActive(true);

            keyText.text = "Push Q";
            keyText.enabled = true;

            needKeyName = collision.gameObject.GetComponent<CheckArea2D>().needKey;
        }
        else if (collision.tag == "Key")
        {
            KeyWhatOnde = collision.gameObject;

            isOnKey = true;
            //_textMesh.text = "Push E";
            //_TextObj.SetActive(true);

            keyText.text = "Push E";
            keyText.enabled = true;

            keyName = collision.gameObject.name.ToString();

        }
        else if (collision.tag == "Recipe")
        {
            RecipeWhatOned = collision.gameObject;

            isOnRecipe = true;
            //_textMesh.text = "Push E";
            //_TextObj.SetActive(true);

            keyText.text = "Push E";
            keyText.enabled = true;
        }
        else if(collision.tag == "HidePlace")
        {
            //_textMesh.text = "Push Z";
            //_TextObj.SetActive(true);

            keyText.text = "Push Z";
            keyText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        keyText.enabled = false;

        if (collision.tag == "KeyChecker")
        {
            isOnChecker = false;
            //_TextObj.SetActive(false);

            needKeyName = null;

            GateWhatOned = null;
        }
        else if (collision.tag == "Key")
        {
            isOnKey = false;
            //_TextObj.SetActive(false);

            keyName = null;

            KeyWhatOnde = null;
        }
        else if(collision.tag == "Recipe")
        {
            isOnRecipe = false;
            //_TextObj.SetActive(false);
        }
        else if(collision.tag == "HidePlace")
        {
            //_TextObj.SetActive(false);
        }
    }

    public bool FullInventory()
    {
        if (isHaveInSlotOne && isHaveInSlotTwo && isHaveInSlotThr) return true;
        else return false;
    }

    void GetTheKey()
    {
        if (FullInventory())
        {
            Debug.Log("인벤토리가 꽉 찼습니다.");
        }
        else if(!FullInventory())
        {
            if (!isHaveInSlotOne)
            {

                if (keyName == "Key01")
                {
                    invenKeyImg = KeyImages[0];

                    invenSlotOne.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotOne.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }
                else if (keyName == "Key02")
                {
                    invenKeyImg = KeyImages[1];

                    invenSlotOne.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotOne.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }
                else if (keyName == "Key03")
                {
                    invenKeyImg = KeyImages[2];

                    invenSlotOne.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotOne.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }

                keyOne = keyName;

                isHaveInSlotOne = true;
            }
            else if (!isHaveInSlotTwo)
            {
                if (keyName == "Key01")
                {
                    invenKeyImg = KeyImages[0];

                    invenSlotTwo.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotTwo.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }
                else if (keyName == "Key02")
                {
                    invenKeyImg = KeyImages[1];

                    invenSlotTwo.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotTwo.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }
                else if (keyName == "Key03")
                {
                    invenKeyImg = KeyImages[2];

                    invenSlotTwo.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotTwo.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }

                keyTwo = keyName;

                isHaveInSlotTwo = true;
            }
            else if (!isHaveInSlotThr)
            {
                if (keyName == "Key01")
                {
                    invenKeyImg = KeyImages[0];

                    invenSlotThr.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotThr.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }
                else if (keyName == "Key02")
                {
                    invenKeyImg = KeyImages[1];

                    invenSlotThr.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotThr.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }
                else if (keyName == "Key03")
                {
                    invenKeyImg = KeyImages[2];

                    invenSlotThr.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invenKeyImg;
                    invenSlotThr.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                }

                keyThr = keyName;

                isHaveInSlotThr = true;
            }

            Destroy(KeyWhatOnde);
        }
        
        
    }

    void UseTheKey()
    {
        if (needKeyName == keyOne)
        {
            invenSlotOne.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            isHaveInSlotOne = false;
            keyOne = null;

            GateWhatOned.SendMessage("Gatework");
        }
        else if (needKeyName == keyTwo)
        {
            invenSlotTwo.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            isHaveInSlotTwo = false;
            keyTwo = null;

            GateWhatOned.SendMessage("Gatework");

        }
        else if (needKeyName == keyThr)
        {
            invenSlotThr.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            isHaveInSlotThr = false;
            keyThr = null;

            GateWhatOned.SendMessage("Gatework");

        }
        else Debug.Log(needKeyName + "가 필요합니다.");
    }

    void GetTheRecipe()
    {
        Debug.Log("탈출구가 열렸다!");

        isGetRecipe = true;
        RecipeWhatOned.SetActive(false);

        importantInven.transform.GetChild(0).gameObject.SetActive(true);

        RecipeWhatOned.transform.parent.gameObject.SendMessage("Gatework");
    }
    

}
