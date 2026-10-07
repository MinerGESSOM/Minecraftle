using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using YG;


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

    public bool EveryDayQuest = false;

    public TextMeshProUGUI text;

    public GameObject WinMenu;
    public GameObject LossMenu;
    public GameObject StartMenu;
    public GameObject GameMenu;
    public GameObject TopMenu;
    public GameObject MenuQuest;
    public GameObject ButtonEveryDayQuest;

    public int MnX = 1;

    public bool ActiveEveryDayQuest = true;

    public Image LossImage;
    public TextMeshProUGUI LossText;

    public LeaderboardYG Top;

    bool Prov = false;

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


    private void OnEnable()
    {
        YG2.MultiplayerSessions.onSessionsLoaded += OnSessionsLoaded;
    }

    private void OnDisable()
    {
        YG2.MultiplayerSessions.onSessionsLoaded -= OnSessionsLoaded;
    }


    public void CheckIfQuestDoneByOthers()
    {
        // Создаем конфиг для поиска сессий с нужным днем в meta1
        var config = new InitConfig();
        var meta = new MetaFilter();
        
        // Ищем сессии, где meta1 равен текущему дню (Day)
        // Если хотите найти диапазон, используйте new YG.Range(min, max)
        int Day = int.Parse(YG2.GetFlag("Day"));
        meta.meta1 = new YG.Range(Day - 1, Day + 1); 
        config.meta = meta;
        config.count = 10; // Максимум 10 записей (лимит Яндекс)

        YG2.MultiplayerSessions.Init(config);
    }

    // Обработчик загруженных сессий
    private void OnSessionsLoaded(List<Session> sessions)
    {
        if (sessions == null || sessions.Count == 0)
        {
            OnQuestCheckCompleted(false);
            return;
        }

        int playersCount = sessions.Count;


        if (playersCount > 2)
        {
            OnQuestCheckCompleted(true);
        }
        else
        {
            OnQuestCheckCompleted(false);
        }
    }

    // Событие для передачи результата проверки
    private void OnQuestCheckCompleted(bool isDoneByOthers)
    {
        if(isDoneByOthers)
        {
            ActiveEveryDayQuest = false;
        }
        else
        {
            YG2.InterstitialAdvShow();
            string value = YG2.GetFlag("EveryDayQuest");
            EveryDayQuest = true;
            attempt = 1;
            GameMenu.SetActive(true);
            StartMenu.SetActive(false);
            Active = true;
            for(int i = 0; i < 9; i++)
            {
                imagesProv[i].color = Color.black;
                CraftItem[i] = -1;
            }

            image.sprite = null;

            craftsItemGame = new List<string[]>{};

            for(int i = 0; i < crafts.Count; i++)
            {
                if(value == crafts[i][1])
                {
                    craftsItemGame.Add(crafts[i]);
                }
            }

            BestCraft = null;


            SavePlayerQuestCompletion();
            ActiveEveryDayQuest = false;
        }
        Debug.Log($"Результат проверки: {isDoneByOthers}");
    }

    // Функция сохранения, что этот человек сделал задание
    public void SavePlayerQuestCompletion()
    {
        var meta = new Meta();

        int Day = int.Parse(YG2.GetFlag("Day"));
        meta.meta1 = Day;


        YG2.MultiplayerSessions.Commit(new Payload());
        YG2.MultiplayerSessions.Push(meta);

        Debug.Log($"Сохранено выполнение задания для дня {Day}");
    }


    void Start()
    {


        Rank = YG2.saves.Rank;


        string[] lines = craftingFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            
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
        YG2.InterstitialAdvShow();
        EveryDayQuest = false;
        attempt = 1;
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
                RankWin();
                if(YG2.lang == "ru")
                {
                    text.text = "Ваш рейтинг\n" + Rank.ToString();
                }
                else
                {
                    text.text = "Your rank\n" + Rank.ToString();
                }
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
        }
        
        ButtonNext.SetActive(true);
        ButtonClear.SetActive(false);
        Active = false;

        if(attempt > 10)
        {
            Active = false;
            LossImage.sprite = sprites[int.Parse(BestCraft[1])];
            if(YG2.lang == "ru")
            {
                LossText.text = BestCraft[0];
            }
            else
            {
                LossText.text = BestCraft[11];
            }
            LossMenu.SetActive(true);
            GameMenu.SetActive(false);
        }
    }

    public void StartMenuButton()
    {
        YG2.InterstitialAdvShow();
        LossMenu.SetActive(false);
        WinMenu.SetActive(false);
        TopMenu.SetActive(false);
        StartMenu.SetActive(true);
        MenuQuest.SetActive(false);
    }


    public void EveryDayQuestStart()
    {
        ButtonEveryDayQuest.SetActive(false);
        if (!ActiveEveryDayQuest)
        {
            return;
        }
        else
        {
            CheckIfQuestDoneByOthers();
        }
    }


    void RankWin()
    {
        if(EveryDayQuest == false)
        {
            Rank += MnX;
        }
        else
        {
            Rank += 10 * MnX;
        }
        YG2.saves.Rank = Rank;
        YG2.SetLeaderboard("RankTop", Rank);
        Top.SetLeaderboard(Rank); 
        YG2.SaveProgress();
    }

    public void ButtonRank()
    {
        TopMenu.SetActive(true);
        StartMenu.SetActive(false);
        Top.UpdateLB();
    }

    public void OnMenuEveryDayQuest()
    {
        MenuQuest.SetActive(true);
    }
}
