using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public enum PlayerType
    {
        None = 0,
        Chad = 1,
        Archer = 2,
        Mage = 3,
        Rogue = 4,
    }

    [System.Serializable]
    class Player
    {

        [JsonProperty] private PlayerType _type;
        [JsonProperty] private int _weaponStat = 0;
        [JsonProperty] private int _armorStat = 0;
        [JsonProperty] private float _attackPower;
        [JsonProperty] private float _defensePower;

        private string job;
        private int _level = 1;
        private int maxHp = 100;
        private float _hp;
         private int _gold = 0;
        private string name;
        private Inventory _inventory = new Inventory();


       [JsonProperty]
        public Inventory Inventory
        {
            get { return _inventory; }
            private set { _inventory = value; }
        }
        [JsonProperty]
        public string Job
        {
            get { return job; }
            protected set { job = value; }
        }
        [JsonProperty]
        public string Name
        {
            get { return name; }
            private set { name = value; }
        }
        [JsonProperty]
        public int Gold
        {
            get { return _gold; }
            private set { _gold = value; }
        }
        [JsonProperty]
        public int Level
        {
            get { return _level; }
            private set { _level = value; }
        }
        [JsonProperty]
        public float HP
        {
            get { return _hp; }
            private set { _hp = value; }
        }
        [JsonProperty]
        public int DungeonClearCount { get; set; }





        public Player(PlayerType type)
        {
            _type = type;
            HP = maxHp;
            Inventory.Init(this);
        }




        //플레이어 스탯을 설정하기 위한 함수
        protected void SetStatus(int attackPower, int defensePower)
        {
            _attackPower = attackPower;
            _defensePower = defensePower;
        }


        public void SetPlayerName(string name)
        {
            Name = name;
        }

        //플레이어의 스테이터스를 확인하기 위한 함수.
        public void DisplayPlayerStatus()
        {
            string weaponStatString = _weaponStat > 0 ? $"({_weaponStat})" : "";
            string armorStatString = _armorStat > 0 ? $"({_armorStat})" : "";

            while (GameManager.Instance.CurrentState == State.ViewStatus)
            {
                Console.Clear();
                GameManager.Instance.TitleBox("플레이어 상태보기");
                Console.WriteLine("Lv.{0}", _level);
                Console.WriteLine("{0}", job);
                Console.WriteLine("공격력: {0}{1}", _attackPower, weaponStatString);
                Console.WriteLine("방어력: {0}{1}", _defensePower, armorStatString);
                Console.WriteLine("체력: {0} / {1}", HP, maxHp);
                Console.WriteLine("Gold: {0} G", _gold);
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int num = int.Parse(Console.ReadLine());

                if (num == 0)
                    GameManager.Instance.SetCurrentState();
            }

        }


        //장착 중인 아이템 능력치를 플레이어에게 적용해주는 메서드 
        public void SetItemStat(Item item)
        {

            if (item.Type == ItemType.Weapon)
            {
                if (item.IsEquipped)
                    _weaponStat = item.Stats;
                else
                    _weaponStat = 0;

            }

            else if (item.Type == ItemType.Armor)
            {
                if (item.IsEquipped)
                    _armorStat = item.Stats;
                else
                    _armorStat = 0;
            }
        }



        public void GetGold(int price)
        {
            Gold += price;
        }


        public void SpendGold(int price)
        {
            Gold -= price;
        }




        public int GetDungeonDefenseStat()
        {
            return (int)(_defensePower + _armorStat);
        }


        public int GetDungeonAttackStat()
        {
            return (int)(_attackPower + _weaponStat);
        }


        public void Damage(float damageAmount)
        {
            HP -= damageAmount;

            if (HP <= 0)
                GameManager.Instance.GameOver();
        }

        public void HealHP(float healAmount = 30)
        {
            if (HP <= maxHp - healAmount)
                HP += healAmount;

            else
                HP = maxHp;
        }

        public void LevelUp()
        {

            if (Level == DungeonClearCount)
            {
                DungeonClearCount = 0;
                _level++;
                _attackPower += 0.5f;
                _attackPower += 1f;
            }

        }





    }


    class Chad : Player
    {
        public Chad() : base(PlayerType.Chad)
        {
            SetStatus(3, 9);
            Job = "Chad ( 전사 )";
        }
    }

    class Archer : Player
    {
        public Archer() : base(PlayerType.Archer)
        {
            SetStatus(7, 5);
            Job = "Archer ( 궁수 )";
        }
    }

    class Mage : Player
    {
        public Mage() : base(PlayerType.Mage)
        {
            SetStatus(6, 6);
            Job = "Mage ( 마법사 )";
        }
    }

    class Rogue : Player
    {
        public Rogue() : base(PlayerType.Rogue)
        {
            SetStatus(9, 3);
            Job = "Rogue ( 도적 )";
        }
    }


}
