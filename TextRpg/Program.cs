
namespace TextRpg

{
    internal class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("####################################");
            Console.WriteLine("#                                  #");
            Console.WriteLine("#         SPARTA DUNGEON           #");
            Console.WriteLine("#                                  #");
            Console.WriteLine("####################################");

            Thread.Sleep(1000);
            Console.Clear();

            while (true)
            {
                Console.WriteLine("####################################");
                Console.WriteLine("#                                  #");
                Console.WriteLine("#         SPARTA DUNGEON           #");
                Console.WriteLine("#                                  #");
                Console.WriteLine("####################################");

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                if (File.Exists("playerData.json"))
                {
                    Console.WriteLine(" 1. 이어하기 , 2. 새 게임 , 3. 종료 ");
                    Console.Write(">>");
                    bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);

                    if (isValid && 1<= selectNum && selectNum <= 3)
                    {
                        switch (selectNum)
                        {
                            case 1:
                                GameManager.Instance.GameStart();
                                break;
                            case 2:
                                GameManager.Instance.ResetGameData();
                                GameManager.Instance.GameStart();
                                break;
                            case 3:
                                Environment.Exit(0);
                                break;
                        }


                        break;

                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(500);
                    }

                }

                else
                {
                    Console.WriteLine("1. 시작하기 2. 종료");
                    Console.Write(">>");
                    bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);

                    if (isValid && selectNum == 1)
                    {
                        GameManager.Instance.GameStart();
                        break;
                    }
                    else if (isValid && selectNum == 2)
                    {
                        Environment.Exit(0);
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(500);
                    }


                }
            }





        }




    }
}