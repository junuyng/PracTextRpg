using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Shop shop = new Shop();
        private Rest rest = new Rest();
        private Dungeon dungeon = new Dungeon();
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

        //게임 시작을 관리하는 함수
        public void GameStart()
        {
            Console.Clear();


            Console.WriteLine("스파르타 던전에 오신 여러분 환영 합니다.");
            Console.Clear();

            //세이브 파일이 없다면 
            if (!IsSaveFileExists())
            {
                Console.WriteLine("당신의 이름을 입력해주세요.");
                Console.Write(">>");
                string name = Console.ReadLine();

                Console.Clear();
                UIManager.Instance.AlignTextCenter($"당신의 이름은 {name} 이군요.");
                Thread.Sleep(2000);

                Console.Clear();
                Console.WriteLine("당신의 직업을 선택해주세요.");

                string[] options = { "전사", "궁수", "마법사", "도적" };
         
                int selectNum = UIManager.Instance.DisplaySelectionUI(options);
                switch (selectNum)
                {
                    case 1:
                        _player = new Chad();
                        break;
                    case 2:
                        _player = new Archer();
                        break;
                    case 3:
                        _player = new Mage();
                        break;
                    case 4:
                        _player = new Rogue();
                        break;
                 
                }

                Console.Clear();

                string[] text = { $"앞으로 당신은 {_player.Job}로서 새로운 여정을 시작하게 됩니다.", "당신의 앞길에 행운이 가득하길 진심으로 기원합니다." };
                UIManager.Instance.AlignTextCenter(text);
                Thread.Sleep(2000);

                _player.SetPlayerName(name);
                SetUpGame();
                SaveGameData();
            }

            //세이브 파일이 있다면
            else
            {
                LoadData();
                SetUpGame();
            }


            while (true)
            {
                if (currentState == State.SelectChoice)
                        SelectBehavior();
            }

        }

        // 세이브 파일 존재 여부를 확인하는 메서드
        private bool IsSaveFileExists()
        {
            return File.Exists("playerData.json");
        }

        // 게임 오버 처리 메서드, 게임 데이터를 초기화하고 프로그램 종료
        public void GameOver()
        {
            ResetGameData();
            Console.Clear();
            UIManager.Instance.TitleBox("플레이어가 사망했습니다.");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        // 게임 초기값 설정 메서드
        public void SetUpGame()
        {
            shop.Init(_player);
            rest.Init(_player);
            dungeon.Init(_player);
        }

        // 플레이어의 행동을 선택하고 게임 상태를 변경하는 메서드
        private void SelectBehavior()
        {
            Console.Clear();
            UIManager.Instance.TitleBox("메인화면");
            string[] selectArray = { "상태 보기", "인벤토리", "상점", "던전입장", "휴식하기", "종료" };
            int choice = UIManager.Instance.DisplaySelectionUI(selectArray);

            switch (choice)
            {
            
                case 1:
                    SetCurrentState(1);
                    _player.DisplayPlayerStatus();
                    break;
                case 2:
                    SetCurrentState(2);
                    _player.Inventory.DisplayInventory();
                    break;
                case 3:
                    SetCurrentState(3);
                    shop.DisplayGoods();
                    break;
                case 4:
                    SetCurrentState(4);
                    dungeon.DisplayEnterDungeon();
                    break;
                case 5:
                    SetCurrentState(5);
                    rest.DisplayRest();
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("게임을 종료합니다.");
                    Thread.Sleep(500);
                    Environment.Exit(0);
                    break;
            }

        }

        // 현재 게임 상태를 변경하고 게임 진행 상황을 저장하는 메서드
        public void SetCurrentState(int stateNum = 0)
        {
            Console.Clear();

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

        // 플레이어 데이터를 저장하는 메서드
        public void SaveGameData()
        {
            string playerJson = JsonConvert.SerializeObject(_player, Formatting.Indented);
            File.WriteAllText("playerData.json", playerJson);
            
        }

        // 기존 데이터를 불러오는 메서드
        public void LoadData()
        {
            if (File.Exists("playerData.json"))
            {
                string playerJson = File.ReadAllText("playerData.json");
                _player = JsonConvert.DeserializeObject<Player>(playerJson);
                _player.Init();
            }

        }

        // 세이브 파일을 삭제하는 메서드
        public void ResetGameData()
        {
            if (File.Exists("playerData.json"))
            {
                File.Delete("playerData.json");
            }
        }

        




    }




}
