using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;


public class CraftManager : MonoBehaviour
{
    [SerializeField] private TextAsset craftingFile;

    public Sprite[] sprites;

    public Image[] imagesProv;

    public int idItemActive = -1;

    public Transform activeButton;

    public Image image;

    public int[] CraftItem = new int[9];

    public List<string[]> crafts = new List<string[]>{};

    public List<string[]> craftsItemGame = new List<string[]>{};

    public int idCraftItem = -1;

    public string[] BestCraft;

    public GameObject ButtonNext;

    public GameObject ButtonClear;

    public bool Active = true;
    
    public int attempt = 1;

    public int Rank = 0;


    public TextMeshProUGUI text;

    public GameObject WinMenu;
    public GameObject LossMenu;
    public GameObject StartMenu;
    public GameObject GameMenu;

    public int MnX = 1;


    public Image LossImage;
    public TextMeshProUGUI LossText;

    bool Prov = false;


    public Image[] imagesItem;
    [SerializeField] TextMeshProUGUI textAttemp;

    public static CraftManager Instance;

    void Awake () 
    {
	    if (Instance == null) 
        { 
	        Instance = this;
            DontDestroyOnLoad(gameObject);
            for(int i = 0; i < 9; i++)
            {
                CraftItem[i] = -1;
            }
	    } 
        else
        {
	        Destroy(gameObject);
	    }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Rank", Rank);
        PlayerPrefs.Save();
    }
    void Start()
    {


        Rank = PlayerPrefs.GetInt("Rank", 0);
        string[] lines = craftingFile.text.Split('\n');
            
        foreach (string line in lines)
        {
            string[] mas = line.Split(new[] { ", " }, System.StringSplitOptions.None);
            crafts.Add(mas);
        }

        GameMenu.SetActive(false);
        StartMenu.SetActive(true);
    }


    public void StartGame()
    {
        //Реклама
        for(int i = 0; i < imagesItem.Count(); i++)
        {
            imagesItem[i].color = Color.white;
        }
        attempt = 1;
        textAttemp.text = "1/10";
        idCraftItem = Random.Range(0, 141) % 141;
        bool ProvId = false;
        GameMenu.SetActive(true);
        StartMenu.SetActive(false);
        Active = true;
        for(int i = 0; i < 9; i++)
        {
            imagesProv[i].color = Color.black;
            CraftItem[i] = -1;
        }

        image.sprite = null;

        for(int i = 0; i < crafts.Count; i++)
        {
            if(idCraftItem == int.Parse((crafts[i])[1]))
            {
                ProvId = true;
                break;
            }
        }
        if (!ProvId)
        {
            idCraftItem += 20;
            idCraftItem = idCraftItem % 141;
        }

        craftsItemGame = new List<string[]>{};

        for(int i = 0; i < crafts.Count; i++)
        {
            if(idCraftItem == int.Parse((crafts[i])[1]))
            {
                craftsItemGame.Add(crafts[i]);
                
            }
        }

        BestCraft = null;
    }


    public void Proverka()
    {
        Prov = true;
        for(int i = 0; i < crafts.Count; i++)
        {
            Prov = true;
            string[] masCraft = crafts[i];
            for(int j = 2; j < 11; j++)
            {
                if(int.Parse(masCraft[j]) != CraftItem[j - 2])
                {
                    Prov = false;
                    break;
                } 
            }
            if (Prov)
            {
                image.sprite = sprites[int.Parse(masCraft[1])];
                break;
            }
        }
        if (!Prov)
        {
            image.sprite = null;
        }

        for(int j = 0; j < 9; j++)
        {
            imagesProv[j].color = Color.black;
        }

        ButtonNext.SetActive(false);
        ButtonClear.SetActive(true);
        Active = true;
    }


    public void Craft()
    {
        if (!Prov)
        {
            return;
        }
        if (!Active)
        {
            return;
        }
        attempt++;
        textAttemp.text = attempt.ToString() + "/10";
        bool Sovpadenie = true;
        for(int i = 0; i < craftsItemGame.Count; i++)
        {
            Sovpadenie = true;
            for(int j = 0; j < 9; j++)
            {
                if(CraftItem[j] != int.Parse(craftsItemGame[i][j + 2]))
                {
                    Sovpadenie = false;
                    break;

                }
            }

            if (Sovpadenie)
            {
                for(int j = 0; j < 9; j++)
                {
                    imagesProv[j].color = Color.green;
                }
                Active = false;
                WinMenu.SetActive(true);
                GameMenu.SetActive(false);
                Rank += MnX;
                /*
                if(true)//Язык
                {
                    text.text = "Ваш рейтинг\n" + Rank.ToString();
                }
                else
                {
                    text.text = "Your rank\n" + Rank.ToString();
                }
                */
                text.text = "Ваш рейтинг\n" + Rank.ToString();
                break;
            }
        }

        if (!Sovpadenie)
        {
            int[] masProv;
            if(BestCraft == null)
            {
                masProv = new int[craftsItemGame.Count];
                for(int i = 0; i < craftsItemGame.Count; i++)
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if(CraftItem[j] == int.Parse(craftsItemGame[i][j + 2]))
                        {
                            masProv[i]++;
                        }
                    }
                }


                int maxProv = -1;
                int maxProvId = -1;
                for(int i = 0; i < masProv.Count(); i++)
                {
                    if(masProv[i] > maxProv)
                    {
                        maxProv = masProv[i];
                        maxProvId = i;
                    }
                }

                BestCraft = craftsItemGame[maxProvId];
            }


            masProv = new int[9];

            for(int i = 0; i < 9; i++)
            {
                if(CraftItem[i] != -1 && CraftItem[i] == int.Parse(BestCraft[i + 2]))
                {
                    masProv[i] = 2;
                }
            }

            int[] masId = new int[18];

            for(int i = 0; i < 9; i++)
            {
                if(masProv[i] != 2)
                {
                    if(int.Parse(BestCraft[i + 2]) != -1)
                    {
                        masId[int.Parse(BestCraft[i + 2])]++;
                    }
                }
            }

            for(int i = 0; i < 9; i++)
            {
                if(CraftItem[i] != int.Parse(BestCraft[i + 2]))
                {
                    if(CraftItem[i] != -1 && masId[CraftItem[i]] > 0)
                    {
                        masId[CraftItem[i]]--;
                        masProv[i] = 1;
                    }
                }
            }


            for(int i = 0; i < 9; i++)
            {
                if(masProv[i] == 2)
                {
                    imagesProv[i].color = Color.green;
                }
                else if(masProv[i] == 1)
                {
                    imagesProv[i].color = Color.yellow;
                }
                else
                {
                    imagesProv[i].color = Color.black;
                }

            }

            for(int i = 0; i < 9; i++)
            {
                int idItemCraft = CraftItem[i];
                if(idItemCraft != -1)
                {
                    if(masProv[i] == 2)
                    {
                        imagesItem[idItemCraft].color = Color.green;
                    }
                    else if(masProv[i] == 1)
                    {
                        if(imagesItem[idItemCraft].color != Color.green)
                        {
                            imagesItem[idItemCraft].color = Color.yellow;
                        }
                    }
                    else
                    {
                        if(imagesItem[idItemCraft].color != Color.green && imagesItem[idItemCraft].color != Color.yellow)
                        {
                            imagesItem[idItemCraft].color = Color.red;
                        }
                    }
                }
            }
        }
        
        ButtonNext.SetActive(true);
        ButtonClear.SetActive(false);
        Active = false;

        if(attempt > 10)
        {
            Active = false;
            LossImage.sprite = sprites[int.Parse(BestCraft[1])];
            /*
            if(true)//Язык
            {
                LossText.text = BestCraft[0];
            }
            else
            {
                LossText.text = BestCraft[11];
            }
            */
            LossText.text = BestCraft[0];
            LossMenu.SetActive(true);
            GameMenu.SetActive(false);
        }
    }

    public void StartMenuButton()
    {
        //Реклама
        LossMenu.SetActive(false);
        WinMenu.SetActive(false);
        StartMenu.SetActive(true);
    }
}
