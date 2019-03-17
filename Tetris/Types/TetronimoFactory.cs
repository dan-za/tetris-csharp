using System;
using System.Collections.Generic;
using Tetris.Types;

namespace Tetris.Types {
    public static class TetronimoFactory {
        
        private static readonly Dictionary<int, Tile[,]> Tetronimos;
        
        static TetronimoFactory() {
            Tetronimos = new Dictionary<int, Tile[,]> {
                {0, new[,] {
                    {Tile.Empty, Tile.Blue},
                    {Tile.Empty, Tile.Blue},
                    {Tile.Empty, Tile.Blue},
                    {Tile.Empty, Tile.Blue}
                }},
                {1, new[,] {
                    {Tile.Red, Tile.Empty},
                    {Tile.Red, Tile.Empty},
                    {Tile.Red, Tile.Red}
                }},
                {2, new[,] {
                    {Tile.Empty, Tile.Yellow, Tile.Empty},
                    {Tile.Yellow, Tile.Yellow, Tile.Yellow}
                }},
                {3, new[,] {
                    {Tile.Green, Tile.Green},
                    {Tile.Green, Tile.Green}
                }},
                {4, new[,] {
                    {Tile.Orange, Tile.Orange, Tile.Empty},
                    {Tile.Empty, Tile.Orange, Tile.Orange}
                }},
                {5, new[,] {
                    {Tile.Empty, Tile.Pink, Tile.Pink},
                    {Tile.Pink, Tile.Pink, Tile.Empty}
                }},
                {6, new[,] {
                    {Tile.Empty, Tile.Violet},
                    {Tile.Empty, Tile.Violet},
                    {Tile.Violet, Tile.Violet}
                }}
            };
        }
    
        public static Tetronimo GetNext(int x, int y) {
            return new Tetronimo(Tetronimos[new Random().Next(0, 7)], x, y);
        }
    }
}