using Newtonsoft.Json;
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
            //상점 아이템 중복 방지
            goods.Clear();

            //상점 아이템 리스트 요소 추가
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

            //플레이어 정보 받아오기
            _player = player;

            
            foreach (var item in goods)
            {
                foreach (var i in _player.Inventory.Item)
                { 
                    if(item.Name == i.Name)
                        item.IsSold = true;
                }

            }
        }



        // 플레이어의 돈과 아이템의 가격을 비교하여, 구매 가능 여부를 판단하고
        // 구매가 가능할 경우 아이템을 구매하는 메서드
        private void CanBuyItem(int itemNum)
        {
            Console.Clear();

            Item item = goods[itemNum - 1];
            int itemPrice = goods[itemNum - 1].Price;

            if (item.IsSold == true)
            {
                Console.WriteLine("이미 판매된 상품 입니다.");
                Thread.Sleep(500);
            }

            else if (_player.Gold < itemPrice)
            {
                Console.WriteLine("Gold 가 부족합니다.");
            }
            
            else
            {
                Console.WriteLine("구매를 완료했습니다.");
                Thread.Sleep(500);

                item.IsSold = true;
                _player.SpendGold(itemPrice);
                _player.Inventory.AddItem(item);
            }

        }


        // 상점에서 플레이어가 아이템을 팔 수 있게 하는 메서드.
        private void SellItem()
        {
            while (isShopping)
            {
                Console.Clear();
                GameManager.Instance.TitleBox("상점 - 아이템 판매");
                Console.ResetColor();
                Console.WriteLine("[보유 골드] [{0}]", _player.Gold);

                // 플레이어가 보유중인 아이템 목록을 출력
                List<Item> inventoryItems = _player.Inventory.Item;
                int itemNum = 1;
                foreach (Item item in inventoryItems)
                {
                    item.WriteItemInfo(itemNum++, 4);
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);
                if (isValid && selectNum == 0)
                {
                    isShopping = false;
                    GameManager.Instance.SetCurrentState(3); 
                }

                else if (isValid && 0 < selectNum && selectNum < inventoryItems.Count + 1)
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


        // 번호가 달린 아이템 목록을 출력 후 플레이어가 아이템을 선택하고 구매할 수 있게 하기 위한 메서드
        private void BuyItem()
        {

            while (isShopping)
            {
                Console.Clear();
                GameManager.Instance.TitleBox("상점 - 아이템 구매");
                Console.WriteLine("[보유 골드] [{0}]", _player.Gold);

                int itemNum = 1;
                foreach (Item goods in goods)
                {
                    goods.WriteItemInfo(itemNum++, 3);
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);
                if (isValid && selectNum == 0)
                {
                    isShopping = false;
                    GameManager.Instance.SetCurrentState(3);
                }
                else if (isValid && 0 < selectNum && selectNum < goods.Count + 1)
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


        //상점에서 판매하는 상품들을 콘솔창에서 보여주기 위한 메서드.
        public void DisplayGoods()
        {
            while (GameManager.Instance.CurrentState == State.Shop && !isShopping)
            {
                Console.Clear();
                GameManager.Instance.TitleBox("상점 - 아이템 목록");
                Console.WriteLine("[보유 골드] [{0}]", _player.Gold);

                int itemNum = 1;
                foreach (Item goods in goods)
                {
                    goods.WriteItemInfo(itemNum++, 2);
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);


                if (isValid && selectNum == 0)
                    GameManager.Instance.SetCurrentState();

                else if (isValid && 0 < selectNum && selectNum < 3)
                {
                    isShopping = true;

                    if (selectNum == 1)
                        BuyItem();

                    else if (selectNum == 2)
                        SellItem();
                }

                else
                {
                    Console.Write("잘못된 입력입니다");
                    Thread.Sleep(500);
                }

            }
        }

    }
}
