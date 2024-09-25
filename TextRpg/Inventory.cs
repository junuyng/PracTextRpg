﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{



    internal class Inventory
    {
        // 인벤토리의 아이템 목록을 저장하는 필드
        // List<Item>으로 여러 아이템 객체를 저장하고 관리함
        List<Item> items = new List<Item>();
        public List<Item> Item
        {
            get
            {
                return items;
            }

            private set
            {
                items = value;
            }

        }

        //플레이어 데이터를 저장
        Player _player;

        private bool isEquippingItem = false;
        private bool isArmorEquipped = false;
        private bool isWeaponEquipped = false;

        public void Init(Player player)
        {
            _player = player;
        }

        // 아이템을 인벤토리에 등록하기 위한 메서드.
        public void AddItem(Item item)
        {
            items.Add(item);
        }

        //아이템 장착 관리를 위한 메서드
        public void EquipItem()
        {
            while (isEquippingItem)
            {
                int itemNum = 1;

                Console.Clear();
                Console.WriteLine("[아이템 목록]");
                foreach (Item item in items)
                {
                    item.WriteItemInfo(itemNum++, 1);
                    Console.WriteLine();
                }
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
               

                bool isValid = int.TryParse(Console.ReadLine(), out int num);
                if (isValid && num == 0)
                {
                    isEquippingItem = false;
                    GameManager.Instance.SetCurrentState(2);
                }

                else if (isValid && 1 <= num && num < items.Count + 1)
                {
                    Item item = items[num - 1];

                   Console.Clear();

                    if (item.IsEquipped)
                    {
                        UnequipItem(item);
                    }

                    else
                    {
                        if (isWeaponEquipped || isArmorEquipped)
                            UnequipItem(item);


                        if (item.Type == ItemType.Weapon)
                            isWeaponEquipped = true;

                        else if (item.Type == ItemType.Armor)
                            isArmorEquipped = true;
                       
                        item.IsEquipped = true;
                        _player.SetItemStat(item);
                    }
                 }


                else
                {
                    Console.WriteLine("잘못된입력입니다.");
                    Thread.Sleep(500);
                }
            }

        }

        // ItemType별 착용 돼있는 장비를 해제하는 메서드 
        private void UnequipItem(Item item)
        {

            if (item.Type == ItemType.Weapon)
            {
                foreach (var i in Item)
                {
                    if (i.IsEquipped == true && i.Type == item.Type)
                    {
                        isWeaponEquipped = false;
                        i.IsEquipped = false;
                        _player.SetItemStat(i);
                    }
                }
            }

            else if (item.Type == ItemType.Armor)
            {
                foreach (var i in Item)
                {
                    if (i.IsEquipped == true && i.Type == item.Type)
                    {
                        isArmorEquipped = false;
                        i.IsEquipped = false;
                        _player.SetItemStat(i);
                    }
                }
            }
            
        }

        //인벤토리를 콘솔창에서 보여주기 위한 메서드.
        public void DisplayInventory()
        {
            while (GameManager.Instance.CurrentState == State.Inventory && !isEquippingItem)
            {
                int itemNum = 1;

                Console.Clear();
                GameManager.Instance.TitleBox("인벤토리");
                Console.WriteLine("[아이템 목록]");


                foreach (Item item in items)
                {
                    item.WriteItemInfo(itemNum++);
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("1. 장착관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int num = int.Parse(Console.ReadLine());


                if (num == 0)
                    GameManager.Instance.SetCurrentState();

                else if (num == 1)
                {
                    isEquippingItem = true;
                    EquipItem();
                }
            }
        }

        //아이템을 인벤토리에서 삭제하기 위한 메서드
        public void DeleteItem(int num)
        {

            // 기존에 이미 장착된 아이템일 경우 장착 해제 
            if (items[num].IsEquipped == true)
            {
                UnequipItem(items[num]);
            }


            items.RemoveAt(num);
        }
    }


}

