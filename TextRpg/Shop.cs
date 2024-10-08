﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    [System.Serializable]
    internal class Shop
    {


        [JsonProperty] private List<Item> goods = new List<Item>();
        private Player _player;
        private bool isShopping = false;

        public void Init(Player player)
        {

            // 상점 아이템 목록 생성
            Item item1 = new Item(ItemType.Armor, "수련자 갑옷", 1000, 5, "수련에 도움을 주는 갑옷입니다.");
            goods.Add(item1);
            Item item2 = new Item(ItemType.Armor, "무쇠갑옷", 2000, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.");
            goods.Add(item2);
            Item item3 = new Item(ItemType.Armor, "스파르타의 갑옷", 3500, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.");
            goods.Add(item3);
            Item item4 = new Item(ItemType.Weapon, "낡은 검", 600, 2, "쉽게 볼 수 있는 낡은 검 입니다.");
            goods.Add(item4);
            Item item5 = new Item(ItemType.Weapon, "청동 도끼 ", 1500, 5, "어디선가 사용됐던거 같은 도끼입니다.");
            goods.Add(item5);
            Item item6 = new Item(ItemType.Weapon, "스파르타의 창", 2000, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.");
            goods.Add(item6);
            Item item7 = new Item(ItemType.Weapon, "날카로운 양날 도끼", 2500, 12, "매우 강력한 무기이지만 속도가 느린 도끼입니다.");
            goods.Add(item7);

            // 플레이어 정보 초기화
            _player = player;

            // 플레이어가 이미 소유한 아이템을 상점에서 판매할 수 없도록 처리
            foreach (var item in goods)
            {
                foreach (var i in _player.Inventory.Item)
                { 
                    if(item.Name == i.Name)
                        item.IsSold = true;
                }

            }
        }

        // 아이템을 구매할 수 있는지 확인하고, 구매가 가능할 경우 처리하는 메서드
        private void CanBuyItem(int itemNum)
        {
            Console.Clear();

            Item item = goods[itemNum - 1];
            int itemPrice = goods[itemNum - 1].Price;

            if (item.IsSold == true)
            {
                UIManager.Instance.AlignTextCenter("이미 판매된 상품 입니다.");
                Thread.Sleep(500);
            }

            else if (_player.Gold < itemPrice)
            {
                UIManager.Instance.AlignTextCenter("소지금이 부족합니다.");
            }
            
            else
            {
                UIManager.Instance.AlignTextCenter("구매를 완료했습니다.");
                Thread.Sleep(500);

                item.IsSold = true;
                _player.SpendGold(itemPrice);
                _player.Inventory.AddItem(item);
            }

        }

        // 상점에서 아이템을 판매하는 메서드
        private void SellItem()
        {
            while (isShopping)
            {
                Console.Clear();
                UIManager.Instance.TitleBox("상점 - 아이템 판매");
                Console.ResetColor();
                Console.WriteLine("[보유 골드:{0}$]", _player.Gold);

                // 플레이어가 보유중인 아이템 목록을 출력
                List<Item> inventoryItems = _player.Inventory.Item;
                int itemNum = 1;
                foreach (Item item in inventoryItems)
                {
                    item.WriteItemInfo(itemNum++, 4);
                    Console.WriteLine();
                }

                int CursorPosY = Console.WindowHeight - 10;
                Console.SetCursorPosition(0, CursorPosY);
                Console.WriteLine("0. 나가기");


                int selectNum = UIManager.Instance.DisplayInputUI();
             
                if (selectNum == 0)
                {
                    isShopping = false;
                    GameManager.Instance.SetCurrentState(3); 
                }

                else if ( 0 < selectNum && selectNum < inventoryItems.Count + 1)
                {
                    int deleteItemNum = selectNum - 1; // 선택된 아이템의 인덱스

                    // 상점의 판매 리스트에서 해당 아이템을 다시 판매 가능 상태로 변경
                    foreach (Item item in goods)
                    {
                        if (item.Name == inventoryItems[deleteItemNum].Name)
                            item.IsSold = false; 
                    }

                    // 플레이어 인벤토리에서 해당 아이템 삭제 및 판매 금액 추가
                    Inventory inventory = _player.Inventory;
                    _player.GetGold((int)(inventory.Item[deleteItemNum].sellPrice)); 
                    inventory.DeleteItem(deleteItemNum); 
                }
            }
        }

        // 상점에서 아이템을 구매하는 메서드
        private void BuyItem()
        {

            while (isShopping)
            {
                Console.Clear();
                UIManager.Instance.TitleBox("상점 - 아이템 구매");
                Console.WriteLine("[보유 골드:{0}$]", _player.Gold);

                int itemNum = 1;
                foreach (Item goods in goods)
                {
                    goods.WriteItemInfo(itemNum++, 3);
                    Console.WriteLine();
                }

                int CursorPosY = Console.WindowHeight - 10;
                Console.SetCursorPosition(0, CursorPosY);
                Console.WriteLine("0. 나가기");

                int selectNum =UIManager.Instance.DisplayInputUI();
                if (selectNum == 0)
                {
                    isShopping = false;
                    GameManager.Instance.SetCurrentState(3);
                }
                else if ( 0 < selectNum && selectNum < goods.Count + 1)
                {
                    Console.Clear();
                    CanBuyItem(selectNum);
                    Thread.Sleep(500);
                }

                else
                {
                    Console.Write("잘못된 입력입니다");
                    Thread.Sleep(500);
                }
            }

        }

        // 상점에서 판매 중인 아이템 목록을 콘솔창에 출력하는 메서드
        public void DisplayGoods()
        {
            while (GameManager.Instance.CurrentState == State.Shop && !isShopping)
            {
                Console.Clear();
                UIManager.Instance.TitleBox("상점 - 아이템 목록");
                Console.WriteLine("[보유 골드:{0}$]", _player.Gold);

                //아이템 목록 출력 
                int itemNum = 1;
                foreach (Item goods in goods)
                {
                    goods.WriteItemInfo(itemNum++, 2);
                    Console.WriteLine();
                }


                string title = $"상점 - 아이템 목록\n[보유 골드] [{_player.Gold}]";
                string[] options = { "아이템 구매", "아이템 판매", "나가기" };
                int selectNum = UIManager.Instance.DisplaySelectionUI(options);

                switch (selectNum)
                {
                    case 1:
                        isShopping = true;
                        BuyItem();
                        break;
                        case 2:
                        isShopping = true;
                        SellItem();
                        break;
                        case 3:
                        GameManager.Instance.SetCurrentState();
                        break;
                }

               

            }
        }

    }
}
