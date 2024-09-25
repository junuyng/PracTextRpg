using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    internal class Rest
    {
        private int restPrice = 500;
        private Player _player;
        private bool isResting =false;


        public void Init(Player player)
        {
            _player = player; 
        }




        // 휴식 관련 정보를 콘솔에 출력하는 메서드
        public void DisplayRest()
        {
            while (GameManager.Instance.CurrentState == State.Rest && !isResting)
            {
                Console.Clear();
                Console.WriteLine($"{restPrice}를 내면 체력을 회복할 수 있습니다. (보유 골드 : {_player.Gold}G)");
                Console.WriteLine();
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
              
                bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);
                if (isValid && selectNum == 0)
                {
                    GameManager.Instance.SetCurrentState();
                }
                else if (isValid && selectNum == 1)
                {
                    isResting = true;
                    TakeRest();
                }
            }
        }

        // 휴식을 취하고, 플레이어의 체력을 회복시키는 메서드
        private void TakeRest()
        {
            _player.SpendGold(restPrice); 
            _player.HealHP();

            for (int i = 1; i <= 3; i++)
            {
                Console.Clear();
                Console.WriteLine("휴식을 취하는 중 입니다! ...{0}", i);
                Thread.Sleep(1000);
            }

            isResting = false;
            DisplayRest();
        }

    }
}
