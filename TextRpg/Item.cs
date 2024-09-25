using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    enum ItemType
    {
        //무기 
        Weapon = 0,
        //방어구
        Armor = 1,
    }



    internal class Item
    {
        public ItemType Type { get; private set; }
        public string Name { get; private set; }
        public int Price { get; private set; }
        public int Stats { get; private set; }
        public string Description { get; private set; }
        public bool IsEquipped { get; set; }
        public bool IsSold { get; set; }
        public int sellPrice { get; private set; }

        // 아이템 객체 생성 시, 필수 속성들을 설정하는 생성자
        // 아이템 타입, 이름, 가격, 능력치, 설명, 장착 여부를 초기화
        public Item(ItemType type, string name, int price, int stats, string description, bool isEquipped = false , bool isSold = false)
        {
            Type = type;
            Name = name;
            Price = price;
            sellPrice = (int)(Price * 0.85);
            Stats = stats;
            Description = description;
            IsEquipped = isEquipped;
            IsSold = isSold;
        }
        
        //아이템 정보를 출력을 위한 메서드
        //item 출력 번호 , 정보 타입(shop과 inventory) 0=inventory , 1= equip , 2 =shop , 3 = shopping
        public void WriteItemInfo(int itemNum , int infoType = 0)
        {
            string attribute = Type == ItemType.Weapon ? "공격력" : "방어력";
            string equipment = IsEquipped ? "[E]" : "";
            string priceInfo = IsSold == true ? "구매완료" : $"{Price}G";
            string itemInfo = $"{equipment}{Name}   |  {attribute} +{Stats}  |  {Description}  |";  // "아이템명   |  아이템 스텟  | 아이템 설명  | "            

            switch (infoType)
            {
                case 0: //인벤토리
                    Console.Write(itemInfo);
                    break;
                case 1: //장착
                    Console.Write($"- {itemNum++}  {itemInfo}");
                    break;
                case 2: //상점 
                        Console.Write($"- {itemInfo}   {priceInfo}");
                    break;
                case 3: //아이템 구매 & 판매
                        Console.Write($"- {itemNum++}  {itemInfo}   {priceInfo}");
                    break;
                case 4: //아이템 판매
                    Console.Write($"- {itemNum++}  {itemInfo}  {sellPrice}G");
                    break;

                default:
                    Console.Write(itemInfo);
                    break;
            }
        }
    }
}
