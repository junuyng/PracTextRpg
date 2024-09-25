
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

            while (true)
            {
              

                if (File.Exists("playerData.json"))
                {

                    string[] options = { "이어하기", "새게임", "종료" };
                   int selectNum= UIManager.Instance.DisplaySelectionUI(options);

                   
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


                     

                }

                else
                {
                    string[] options = { "시작하기", "종료" };

                    int selectNum = UIManager.Instance.DisplaySelectionUI(options);

                    if (selectNum == 1)
                    {
                        GameManager.Instance.GameStart();
                        break;
                    }
                    else if (selectNum == 2)
                    {
                        Environment.Exit(0);
                    }

                }
            }





        }




    }
}