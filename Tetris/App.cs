using System;

namespace Tetris {
    internal class App {
     
        public static void Main() {
            
            Console.CursorVisible = false;
            Console.SetWindowSize(Console.LargestWindowWidth / 2, Console.LargestWindowHeight / 2);
            
            
            new TetrisGame().StartGame();
        }
    }
}