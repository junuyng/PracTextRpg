using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    //현재 게임 상태 
    public enum State
    {
        SelectChoice,  // 선택지 고르기
        ViewStatus,    // 상태 보기
        Inventory,     // 인벤토리
        Shop,          // 상점
        Dungeon,       // 던전
        Rest           // 휴식
    }

    [System.Serializable]
    public class GameManager
    {

        public State CurrentState { get { return currentState; } private set { currentState = value; } }

        private State currentState;
        private Player _player;
        private  Shop shop = new Shop();
        private  Rest rest = new Rest();
        private Dungeon dungeon = new Dungeon();
        private bool isPlay = false;
        private  bool isPlayFirstTime = true;
        private static GameManager instance;
       public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }

        }


        public void GameStart()
        {
           Console.Clear(); 
      

            isPlay = true;
            Console.WriteLine("스파르타 던전에 오신 여러분 환영 합니다.");


            if (!File.Exists("playerData.json"))
            {
                Console.WriteLine("당신의 이름을 입력해주세요.");
                Console.Write(">>");
                string name = Console.ReadLine();

                Console.Clear();
                Console.WriteLine("당신의 이름은 {0} 이군요.", name);
                Thread.Sleep(500);

                Console.Clear();
                Console.WriteLine("당신의 직업을 선택해주세요.");
                Console.WriteLine("1. 전사 , 2.궁수 , 3.마법사, 4.도적");
                Console.Write(">>");
                string str = Console.ReadLine();

                switch (str)
                {
                    case "1":
                        _player = new Chad();
                        break;
                    case "2":
                        _player = new Archer();
                        break;
                    case "3":
                        _player = new Mage();
                        break;
                    case "4":
                        _player = new Archer();
                        break;
                    default:
                        _player = new Player(PlayerType.None);
                        break;
                }

                _player.SetPlayerName(name);
                isPlayFirstTime = false;
                SetUpGame();
                SaveGameData();
            }


            else
            {
                LoadData();
                SetUpGame();
            }


            while (isPlay)
            {
                if (currentState == State.SelectChoice)
                    SelectBehavior();
            }

        }
        public void GameOver()
        {
            ResetGameData();
            Console.Clear();
            TitleBox("플레이어가 사망했습니다.");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        public void SetUpGame()
        {
            shop.Init(_player);
            rest.Init(_player);
            dungeon.Init(_player);
        }

        // 플레이어의 행동을 선택하고 그에 따라 게임 상태를 변경하는 메서드
        private  void SelectBehavior()
        {
            Console.Clear();

            Console.WriteLine("1.상태 보기");
            Console.WriteLine("2.인벤토리");
            Console.WriteLine("3.상점");
            Console.WriteLine("4.던전입장");
            Console.WriteLine("5.휴식하기");
            Console.WriteLine("0.종료\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "0":
                    Console.Clear();
                    Console.WriteLine("게임을 종료합니다.");
                    Thread.Sleep(500);
                    isPlay = false;
                    break;
                case "1":
                    SetCurrentState(1);
                    _player.DisplayPlayerStatus();
                    break;
                case "2":
                    SetCurrentState(2);
                    _player.Inventory.DisplayInventory();
                    break;
                case "3":
                    SetCurrentState(3);
                    shop.DisplayGoods();
                    break;
                case "4":
                    SetCurrentState(4);
                    dungeon.DisplayEnterDungeon();
                    break;
                case "5":
                    SetCurrentState(5);
                    rest.DisplayRest();
                    break;
            }
        }


        public  void SetCurrentState(int stateNum = 0)
        {
            switch (stateNum)
            {
                case 1:
                    currentState = State.ViewStatus;
                    break;
                case 2:
                    currentState = State.Inventory;
                    break;
                case 3:
                    currentState = State.Shop;
                    break;
                case 4:
                    currentState = State.Dungeon;
                    break;
                case 5:
                    currentState = State.Rest;
                    break;
                default:
                    currentState = State.SelectChoice;
                    break;
            }

            SaveGameData();
        }

       
        public void SaveGameData()
        {
            string playerJson = JsonConvert.SerializeObject(_player, Formatting.Indented);
            File.WriteAllText("playerData.json", playerJson);
        }

        public void LoadData()
        {
            if (File.Exists("playerData.json"))
            {
                string playerJson = File.ReadAllText("playerData.json");
                _player = JsonConvert.DeserializeObject<Player>(playerJson);
            }
     
        }

        public void ResetGameData()
        {
            if (File.Exists("playerData.json"))
            {
                File.Delete("playerData.json");
            }
        }


        public void TitleBox(String title)
        {
            for (int i = 0; i < title.Length; i++)
            {
                Console.Write("ㅡ");
            }
            Console.WriteLine();
            Console.WriteLine("{0}", title);
            for (int i = 0; i < title.Length; i++)
            {
                Console.Write("ㅡ");
            }
            Console.Write("\n\n");



        }

    }




}
