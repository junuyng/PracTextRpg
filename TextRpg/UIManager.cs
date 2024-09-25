using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    internal class UIManager
    {

        private static UIManager instance;
        public static UIManager Instance
        {

            get 
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }

            private set { instance = value; }   
        }




        // 콘솔 창에 타이틀을 출력하는 메서드
        public void TitleBox(String title)
        {
            for (int i = 0; i < title.Length; i++)
            {
                Console.Write("ㅡ");
            }
            Console.WriteLine();
            Console.WriteLine("{0}", title);
            for (int i = 0; i < title.Length; i++)
            {
                Console.Write("ㅡ");
            }
            Console.Write("\n\n");



        }

        public void AlignTextCenter(string text)
        {

            int CursorX = Console.WindowWidth / 2 - text.Length ;
            int CursorY = Console.WindowHeight / 2 -1;

            Console.SetCursorPosition(CursorX, CursorY);
            Console.WriteLine(text);
        }


        public void AlignTextCenter(string[] text)
        {
            int padding = 3;
            for (int i = 0; i < text.Length; i++)
            {
                int CursorX = (Console.WindowWidth / 2) - (text[i].Length / 2);
                int CursorY = (Console.WindowHeight / 2) - i - padding;
                Console.SetCursorPosition(CursorX, CursorY);
              
                Console.WriteLine(text[i]);
            }

        }


        public int DisplayInputUI()
        {
            int cursorPosY = (int)(Console.WindowHeight * 0.7);


            Console.SetCursorPosition(1, cursorPosY);
            for (int i = 0; i < Console.WindowWidth - 1; i++)
                {
                    Console.Write("-");
                }

                Console.Write(">>");

            bool isValid = int.TryParse(Console.ReadLine(), out int selectNum);


                return selectNum;
        }

        // 4개의 선택지 UI
        public int DisplaySelectionUI(string[] options)
        {

            int selectNum =0;
            int cursorPosY = (int)(Console.WindowHeight * 0.7);
            int selectCursorPosY = cursorPosY+1;
            int previousCursorPosY = selectCursorPosY; // 이전 커서 위치 저장
            bool isSelecting = true;

            while (isSelecting)
            {

                // 옵션 출력
                for (int i = 0; i <= options.Length; i++)
                {
                    Console.SetCursorPosition(1, cursorPosY + i);

                    if (i == 0)
                    {
                        // 상단 경계선 그리기
                        for (int j = 0; j < Console.WindowWidth - 1; j++)
                        {
                            Console.Write("-");
                        }
                    }
                    else
                    {
                        Console.Write(options[i - 1]);  // 옵션 출력
                    }
                }

                Console.CursorVisible = false; //콘솔창 커서 숨기기 


                // 이전 커서 위치의 '▶' 지우기
                Console.SetCursorPosition(0, previousCursorPosY);
                Console.Write(" ");  // 공백으로 커서를 지움


                //콘솔 좌표 설정
                if (selectCursorPosY < cursorPosY +1)
                    selectCursorPosY = cursorPosY + options.Length;
                else if(selectCursorPosY > cursorPosY + options.Length)
                    selectCursorPosY = cursorPosY + 1;

                // 새 위치에 '▶' 출력
                Console.SetCursorPosition(0, selectCursorPosY);
                Console.Write('▶');

                //사용자 입력처리 
                ConsoleKeyInfo input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.LeftArrow:
                        previousCursorPosY = selectCursorPosY;  
                        selectCursorPosY--;
                        break;
                    case ConsoleKey.RightArrow:                 
                    case ConsoleKey.DownArrow:
                        previousCursorPosY = selectCursorPosY;
                        selectCursorPosY++;
                        break;
                    case ConsoleKey.Enter:
                        selectNum = selectCursorPosY - cursorPosY;
                        isSelecting=false;
                        break;
                }

            }


            return selectNum;

        }


    }
}
