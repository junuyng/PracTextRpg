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
        private int healAmout = 30;

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

                string[] text = { $"{restPrice}골드를 지불해 {healAmout}만큼의 체력을 회복할 수 있습니다. (보유 골드 :{_player.Gold}G)", $"현재 플레이어 체력 {_player.HP}" };
                UIManager.Instance.AlignTextCenter(text);
                Console.WriteLine();


                string[] options = { "휴식하기","나가기" };
                int selectNum = UIManager.Instance.DisplaySelectionUI(options);


                switch (selectNum)
                {
                    case 1:
                        isResting = true;
                        TakeRest();
                        break;
                    case 2:
                        GameManager.Instance.SetCurrentState();
                        break;
                
                }

            }
        }

        // 휴식을 취하고, 비용을 지불한 플레이어의 체력을 회복시키는 메서드
        private void TakeRest()
        {
            isResting = false;

            if (_player.Gold < restPrice)
            {
                Console.Clear();
                UIManager.Instance.AlignTextCenter($"소지금이 부족합니다.");
                Thread.Sleep(1000);
            }

            else
            {
                _player.SpendGold(restPrice);
                _player.HealHP();

                for (int i = 1; i <= 3; i++)
                {
                    Console.Clear();
                    UIManager.Instance.AlignTextCenter($"휴식을 취하는 중 입니다!...{i}");
                    Thread.Sleep(1000);
                }

                Console.Clear();
                UIManager.Instance.AlignTextCenter($"회복 후 플레이어 체력: {_player.HP}");
                Thread.Sleep(1000);

            }

            DisplayRest();
        }

    }
}
