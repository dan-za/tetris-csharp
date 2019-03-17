using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Tetris.Types;

namespace Tetris {
    public class TetrisGame {
        
        private readonly object _renderLock = new object();
        
        private enum Command : byte { Left, Right, Down, Rotate, FinishGame }
        
        private static readonly int Width = 10;
        private static readonly int Height = 24;
        
        private readonly Board _board;

        private readonly Timer _timer;

        private Tetronimo _currentTetronimo;
        private Tetronimo _nextTetronimo;
        
        private int _score;
        private int _completedLines;

        private bool _gameFinished;

        private readonly Dictionary<int, int> _points;

        private readonly Dictionary<Command, Action> _commands;
        private readonly Dictionary<Command, Action> _undoCommands;

        private readonly Dictionary<Tile, char> _tiles;

        public TetrisGame() {
            _score = 0;
            _completedLines = 0;
            _points = new Dictionary<int, int> {{1, 40}, {2, 100}, {3, 300}, {4, 1200}};

            InitCommands(out _commands, out _undoCommands);

            _tiles = new Dictionary<Tile, char> {
                {Tile.Empty, ' '},
                {Tile.Wall, '#'},
                {Tile.Blue, 'B'},
                {Tile.Red, 'R'},
                {Tile.Yellow, 'Y'},
                {Tile.Green, 'G'},
                {Tile.Orange, 'O'},
                {Tile.Pink, 'P'},
                {Tile.Violet, 'V'}
            };

            _board = new Board(Width, Height);
            _nextTetronimo = CreateTetronimo();
            GetNextTetronimo();
            
            _gameFinished = false;
            
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = false;
        }

        private void InitCommands(out Dictionary<Command, Action> commands, out Dictionary<Command, Action> undoCommands) {
            
            void MoveRight() { _currentTetronimo.MoveRight(); }
            void MoveLeft() { _currentTetronimo.MoveLeft(); }
            void RotateClockwise() { _currentTetronimo.RotateClockwise(); }
            void RotateCounterClockwise() { _currentTetronimo.RotateCounterClockwise(); }
            void MoveDown() { _currentTetronimo.MoveDown(); }
            void MoveUp() { _currentTetronimo.MoveUp(); }
            void FinishGame() { _gameFinished = true; }
            void DontFinishGame() { _gameFinished = false; }

            commands = new Dictionary<Command, Action> {
                {Command.Right, MoveRight},
                {Command.Left, MoveLeft},
                {Command.Rotate, RotateClockwise},
                {Command.Down, MoveDown},
                {Command.FinishGame, FinishGame}
            };
            
            undoCommands= new Dictionary<Command, Action> {
                {Command.Right, MoveLeft},
                {Command.Left, MoveRight},
                {Command.Rotate, RotateCounterClockwise},
                {Command.Down, MoveUp},
                {Command.FinishGame, DontFinishGame}
            };
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e) {

            if (_gameFinished) {
                _timer.Enabled = false;
                return;
            }
            
            Update(Command.Down);

            Render();
        }

        public void StartGame() {
           _timer.Enabled = true;
            

            Render();
            
            while (!_gameFinished) {
                
                var userCommand = GetPlayerInput();
                
                if (!userCommand.HasValue) return;
                
                Update(userCommand.Value);
                
                Render();
            }

            _timer.Enabled = false;
            
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
        }

        private void Update(Command command) {

            Execute(command);

            if (!IsValidPosition(_currentTetronimo)) {

                Undo(command);
                
                if (command != Command.Down) return;
                
                _board.Place(_currentTetronimo);

                var completedLines = _board.ClearCompletedLines();
                _points.TryGetValue(completedLines, out var toAdd);
                _score += toAdd;
                _completedLines += completedLines;
                
                GetNextTetronimo();
            }
            
            if (!IsValidPosition(_currentTetronimo)) {
                _gameFinished = true;
            }
        }

        private void Execute(Command command) {
            _commands[command]();
        }
        
        private void Undo(Command command) {
            _undoCommands[command]();
        }

        private bool IsValidPosition(Tetronimo tetronimo) {
            return _board.Fits(tetronimo);
        }
        
        private static Command? GetPlayerInput() {
            
            switch (Console.ReadKey(true).Key) {
                case ConsoleKey.RightArrow: return Command.Right;
                case ConsoleKey.LeftArrow: return Command.Left;
                case ConsoleKey.UpArrow: return Command.Rotate;
                case ConsoleKey.DownArrow: return Command.Down;
                case ConsoleKey.Escape: return Command.FinishGame;
                default: return null;
            }
        }

        private void GetNextTetronimo() {
            _currentTetronimo = _nextTetronimo;
            _nextTetronimo = CreateTetronimo();
        }

        private Tetronimo CreateTetronimo() {
            return TetronimoFactory.GetNext(0, _board.GetWidth() / 2);
        }

        private void Render() {

            lock (_renderLock) {
                Console.SetCursorPosition(0,0);
            
                var displayBuffer = GetDisplayBuffer();

                displayBuffer.ForEach(Console.WriteLine);    
            }
        }

        private List<string> GetDisplayBuffer() {

            var offset = new String(' ', 6);
            
            var displayBuffer = new List<string> {
                string.Empty,
                $"{offset}Score: {_score.ToString()}",
                $"{offset}Completed lines: {_completedLines.ToString()}", 
                string.Empty
            };

            var nextTetronimoBuffer = GetNextTetronimoBuffer();

            for (var x = 0; x < Height; x++) {

                var lineBuilder = new StringBuilder(offset);
                
                for (var y = 0; y < Width; y++) {
                    var tile = _currentTetronimo.GetRelativeTo(x, y);

                    if (tile == Tile.Empty) tile = _board.Get(x, y);

                    lineBuilder.Append(_tiles[tile]);
                }

                var nextTetronimoLine = nextTetronimoBuffer.ElementAtOrDefault(x);

                if (nextTetronimoLine != null) {
                    lineBuilder.AppendFormat("{0}{1}", offset, nextTetronimoLine);
                }

                displayBuffer.Add(lineBuilder.ToString());
            }
            
            if (_gameFinished) {    
                displayBuffer.Add(string.Empty);
                displayBuffer.Add($"{offset}GAME OVER");
                displayBuffer.Add($"{offset}Thanks for playing!");
            }

            return displayBuffer;
        }

        private List<string> GetNextTetronimoBuffer() {
            
            var nextTetronimo = new List<string>();

            const int width = 4;
            const int height = 4;
            
            var boarder = new string('#', width + 4);

            nextTetronimo.Add("Next:");
            nextTetronimo.Add(boarder);
            
            for (var x = 0; x < height; x++) {

                var lineBuilder = new StringBuilder("# ");
                
                for (var y = 0; y < width; y++) {
                    var tile = _nextTetronimo.GetAbsolute(x,y);
                    lineBuilder.Append(_tiles[tile]);
                }
                lineBuilder.Append(" #");
                nextTetronimo.Add(lineBuilder.ToString());
            }
            
            nextTetronimo.Add(boarder);

            return nextTetronimo;
        }        
    }
}