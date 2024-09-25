using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    internal class Dungeon
    {
        private Player _player;
        private int easyRequiredStats;
        private int normalRequiredStats;
        private int hardRequiredStats;
        private int previousPlayerHP;
        private int previousPlayerGold;
        private int _easyReward;
        private int _normalReward;
        private int _hardReward;
        private bool isInDungeon = false;


        // 던전 초기화 메서드: 플레이어 정보와 난이도별 요구 스탯 및 보상 설정
        public void Init(Player player, int easyStats = 5, int normalStats = 11, int hardStats = 17 ,int easyReward= 1000 , int normalReward = 1700 , int hardReward = 2500)
        {
            _player = player;
            easyRequiredStats = easyStats;
            normalRequiredStats = normalStats;
            hardRequiredStats = hardStats;
            _easyReward = easyReward;
            _normalReward = normalReward;   
            _hardReward = hardReward;   

        }


        // 던전 입장 화면을 출력하는 메서드
        public void DisplayEnterDungeon()
        {
            while (GameManager.Instance.CurrentState == State.Dungeon && isInDungeon == false)
            {
                Console.Clear();
                GameManager.Instance.TitleBox("던전입장");

                Console.WriteLine("1. 쉬운 던전     | 방어력 {0} 이상 권장", easyRequiredStats);
                Console.WriteLine("2. 일반 던전     | 방어력 {0}  이상 권장", normalRequiredStats);
                Console.WriteLine("3. 어려운 던전    | 방어력 {0}  이상 권장", hardRequiredStats);
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);
                if (isValid && selectNum == 0)
                {
                    GameManager.Instance.SetCurrentState();
                }
                
                else if (isValid && 0 < selectNum && selectNum < 4)
                {
                    DisplayInDungeon(selectNum);
                }
               
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(500);
                }

            }

        }

        // 던전 결과를 츨력 및 처리하는 메서드
        private void DisplayInDungeon(int dungeonNum)
        {
            bool isClear = CheckDungeonClear(dungeonNum);


            if (isClear)
            {
                isInDungeon = true;

                string dungeonName;
                switch (dungeonNum)
                {
                    case 1:
                        dungeonName = "쉬운 던전";
                        break;
                    case 2:
                        dungeonName = "일반 던전";
                        break;
                    case 3:
                        dungeonName = "어려운 던전";
                        break;
                    default:
                        dungeonName = "쉬운 던전";
                        break;
                }

                while (isInDungeon)
                {
                    OnDungeonClear();

                    Console.Clear();
                    GameManager.Instance.TitleBox("던전 클리어");
                    Console.WriteLine("축하합니다!");
                    Console.WriteLine($"{dungeonName}을 클리어 하였습니다.");
                    Console.WriteLine();
                    Console.WriteLine("[탐험 결과]");
                    Console.WriteLine($"체력 {previousPlayerHP} -> {_player.HP}");
                    Console.WriteLine($"Gold {previousPlayerGold} -> {_player.Gold}");
                    Console.WriteLine();
                    Console.WriteLine("0.나가기");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write(">>");
                   
                    bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);
                    if (isValid && selectNum == 0)
                    {
                        isInDungeon = false;
                        DisplayEnterDungeon();
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(500);
                    }

                }
            }

            else
            {
                Console.Clear();
                Console.WriteLine("던전 클리어에 실패하셨습니다.");
                _player.Damage(_player.HP / 2);
                Thread.Sleep(500);
            }


        }

        // 던전 클리어 여부를 반환 및 클리어 시 로직 수행
        private bool CheckDungeonClear(int dungeonNum)
        {
            int requiredStats;
            int reward;
            int playerDefenseStat = _player.GetDungeonDefenseStat();
            int playerAttackStat = _player.GetDungeonAttackStat();
            
            switch (dungeonNum)
            {
                case 1:
                    requiredStats = easyRequiredStats;
                    reward = _easyReward;
                    break;
                case 2:
                    requiredStats = normalRequiredStats;
                    reward = _normalReward;
                    break;
                case 3:
                    requiredStats = hardRequiredStats;
                    reward = _hardReward;
                    break;
                default:
                    requiredStats = 5;
                    reward = 1000;
                    break;
            }

            // 플레이어 방어 스탯이 요구 스탯보다 부족할 경우 실패 확률 존재
            if (playerDefenseStat < requiredStats)
            {
                int rand = new Random().Next(1, 11);
                if (0 < rand && rand <= 4)  // 실패 확률 40%
                {
                    return false;
                }
            }

            // 던전 클리어 시 플레이어 정보 저장
            previousPlayerHP = (int)_player.HP;
            previousPlayerGold = _player.Gold;

            // 플레이어가 던전에서 입을 데미지 계산
            int damage = new Random().Next(20, 36) - (requiredStats - playerDefenseStat);
            _player.Damage(damage);

            // 추가 보상 계산 (공격력  ~ 공격력 * 2 의 %  )
            double randomValue = 0.1 + (new Random().NextDouble() * 0.1);  // 0.1 ~ 0.2 범위의 난수 생성
            double extraRewardPercent = Math.Round(playerAttackStat * randomValue,2);  //(공격력  ~ 공격력 * 2 의 %  )
            int totalReward = (int)(reward + (reward * extraRewardPercent));  // 최종 보상 계산
            _player.GetGold(totalReward);    
          
            return true;
        }


        //던전 클리어시 호출 되는 메서드, 플레이어의 던전클리어수를 늘리고 LevelUp 함수 호출 
        private void OnDungeonClear()
        {
            _player.DungeonClearCount++;
            _player.LevelUp();
        }


    } 
}
