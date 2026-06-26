using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Snake
{
    /// <summary>
    /// Core game logic, completely decoupled from UI controls.
    /// Encapsulates snake movement, collision, scoring, level loading, and state management.
    /// </summary>
    public class GameEngine
    {
        #region Enums

        public enum Direction { None = 0, Up = 1, Right = 2, Down = 3, Left = 4 }

        public enum Difficulty { Easy = 1, Medium = 2, Hard = 3, Extreme = 4 }

        #endregion

        #region Public Events

        public event Action<int> OnScoreChanged;
        public event Action OnGameOver;
        public event Action OnLevelLoaded;

        #endregion

        #region Public Properties

        public bool IsRunning { get; private set; }
        public bool IsPaused { get; set; }
        public int Score { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public Direction NextDirection { get; private set; }
        public int ScoreMultiplier { get; private set; }
        public ColourScheme Scheme { get; private set; }
        public int GridSize => gridSize;

        #endregion

        #region Private Fields

        private const int MaxSnakeLength = 1100;
        private readonly int gridSize;
        private readonly int cellSize;
        private readonly Random random;

        private char[,] gridMap;
        private PictureBox[,] gridControls;

        private Tail[] snake;
        private int snakeLength;

        private Tail pickup;
        private bool pickupActive;

        private int gameSpeed;
        private int levelMultiplier;
        private bool needsEdgeScrolling;
        private bool edgeScrollingAllowed;

        private int spawnX;
        private int spawnY;

        #endregion

        #region Structs

        private struct Tail
        {
            public int X;
            public int Y;
        }

        #endregion

        #region Public API

        public GameEngine(ColourScheme scheme)
        {
            Scheme = scheme;
            gridSize = 32; // pixelDivider
            cellSize = 640 / gridSize;
            random = new Random();
            gridMap = new char[gridSize + 5, gridSize + 5];
            snake = new Tail[MaxSnakeLength];
        }

        public void SetGridControls(PictureBox[,] controls)
        {
            gridControls = controls;
        }

        public void Initialise()
        {
            IsRunning = false;
            IsPaused = false;
            Score = 0;
            CurrentDirection = Direction.None;
            NextDirection = Direction.None;
            snakeLength = 0;
            ClearGrid();
        }

        public void StartGame(Difficulty difficulty, int level, int levelMultiplier = 0, bool edgeScrollingEnabled = true)
        {
            Score = 0;
            snakeLength = 1;
            CurrentDirection = Direction.None;
            NextDirection = Direction.None;
            IsRunning = true;
            IsPaused = false;
            needsEdgeScrolling = false;
            edgeScrollingAllowed = edgeScrollingEnabled;
            this.levelMultiplier = levelMultiplier;

            // Difficulty settings
            switch (difficulty)
            {
                case Difficulty.Easy:
                    gameSpeed = 250;
                    ScoreMultiplier = 1;
                    break;
                case Difficulty.Medium:
                    gameSpeed = 150;
                    ScoreMultiplier = 2;
                    break;
                case Difficulty.Hard:
                    gameSpeed = 50;
                    ScoreMultiplier = 3;
                    break;
                case Difficulty.Extreme:
                    gameSpeed = 30;
                    ScoreMultiplier = 5;
                    break;
            }

            if (!edgeScrollingAllowed || needsEdgeScrolling)
                ScoreMultiplier *= 2;
            ScoreMultiplier += levelMultiplier;

            LoadLevel(level);
            PlaceInitialSnake();
            PlacePickup(true);
            Render();

            OnScoreChanged?.Invoke(Score);
        }

        public int GetGameSpeed() => gameSpeed;

        public void ChangeDirection(Direction dir)
        {
            if (!IsRunning || IsPaused) return;

            // Prevent reversing direction
            if (dir == Direction.Up && CurrentDirection == Direction.Down) return;
            if (dir == Direction.Down && CurrentDirection == Direction.Up) return;
            if (dir == Direction.Left && CurrentDirection == Direction.Right) return;
            if (dir == Direction.Right && CurrentDirection == Direction.Left) return;

            if (CurrentDirection == Direction.None)
            {
                // At start, allow initial direction to set the snake body
                switch (dir)
                {
                    case Direction.Right:
                        NextDirection = Direction.Right;
                        break;
                }
            }
            else
            {
                NextDirection = dir;
            }
        }

        public void Update()
        {
            if (!IsRunning || IsPaused || CurrentDirection == Direction.None) return;

            ApplyDirection();

            // Move head
            var head = snake[0];
            switch (CurrentDirection)
            {
                case Direction.Up:    head.X--; break;
                case Direction.Right: head.Y++; break;
                case Direction.Down:  head.X++; break;
                case Direction.Left:  head.Y--; break;
            }

            // Edge wrapping
            if (!edgeScrollingAllowed)
            {
                if (head.X < 1 || head.X > gridSize || head.Y < 1 || head.Y > gridSize)
                {
                    GameOver();
                    return;
                }
            }
            else
            {
                if (head.X < 1) head.X = gridSize;
                else if (head.X > gridSize) head.X = 1;
                else if (head.Y < 1) head.Y = gridSize;
                else if (head.Y > gridSize) head.Y = 1;
            }

            // Collision with obstacles
            if (gridMap[head.X, head.Y] == '#')
            {
                GameOver();
                return;
            }

            // Collision with self
            for (int i = 1; i < snakeLength; i++)
            {
                if (head.X == snake[i].X && head.Y == snake[i].Y)
                {
                    GameOver();
                    return;
                }
            }

            snake[0] = head;

            // Check pickup collision
            bool atePickup = false;
            if (pickupActive && head.X == pickup.X && head.Y == pickup.Y)
            {
                atePickup = true;
            }

            // Shift body
            for (int i = snakeLength; i > 0; i--)
            {
                snake[i] = snake[i - 1];
            }

            // Re-color snake
            for (int i = 0; i < snakeLength; i++)
            {
                if (snake[i].X >= 1 && snake[i].X <= gridSize &&
                    snake[i].Y >= 1 && snake[i].Y <= gridSize)
                {
                    gridControls[snake[i].X, snake[i].Y].BackColor = Scheme.Secondary;
                }
            }

            if (atePickup)
            {
                Score++;
                OnScoreChanged?.Invoke(Score);
                GrowSnake();
                PlacePickup(false);
            }

            Render();
        }

        public void Render()
        {
            // Draw obstacles
            for (int x = 1; x <= gridSize; x++)
            {
                for (int y = 1; y <= gridSize; y++)
                {
                    if (gridMap[x, y] == '#')
                    {
                        gridControls[x, y].BackColor = Scheme.Level;
                    }
                }
            }

            // Draw snake
            for (int i = 0; i < snakeLength; i++)
            {
                if (snake[i].X >= 1 && snake[i].X <= gridSize &&
                    snake[i].Y >= 1 && snake[i].Y <= gridSize)
                {
                    gridControls[snake[i].X, snake[i].Y].BackColor = Scheme.Secondary;
                }
            }

            // Draw pickup
            if (pickupActive)
            {
                gridControls[pickup.X, pickup.Y].BackColor = Scheme.Tertiary;
            }
        }

        public void ClearGrid()
        {
            for (int x = 1; x <= gridSize; x++)
            {
                for (int y = 1; y <= gridSize; y++)
                {
                    gridControls[x, y].BackColor = Scheme.Primary;
                }
            }
        }

        public void HideSnake()
        {
            for (int i = 0; i < snakeLength; i++)
            {
                if (snake[i].X >= 1 && snake[i].X <= gridSize &&
                    snake[i].Y >= 1 && snake[i].Y <= gridSize)
                {
                    gridControls[snake[i].X, snake[i].Y].BackColor = Scheme.Primary;
                }
            }
        }

        public void ShowPickup()
        {
            if (pickupActive)
            {
                gridControls[pickup.X, pickup.Y].BackColor = Scheme.Tertiary;
            }
        }

        public int GetSpawnX() => spawnX;
        public int GetSpawnY() => spawnY;

        public void SetDifficultyMultiplier(int diff) => ScoreMultiplier = diff;
        public void SetEdgeScrollingAllowed(bool allowed) => edgeScrollingAllowed = allowed;
        public void SetNeedsEdgeScrolling(bool needs) => needsEdgeScrolling = needs;
        public bool GetEdgeScrollingAllowed() => edgeScrollingAllowed;

        public void GameOver()
        {
            IsRunning = false;
            OnGameOver?.Invoke();
        }

        public int GetScoreMultiplier() => ScoreMultiplier;
        public int GetPickupX() => pickup.X;
        public int GetPickupY() => pickup.Y;
        public bool IsPickupActive() => pickupActive;

        #endregion

        #region Private Methods

        private void ApplyDirection()
        {
            // Prevent 180-degree turn on same tick
            if (CurrentDirection == Direction.Up && NextDirection == Direction.Down) return;
            if (CurrentDirection == Direction.Down && NextDirection == Direction.Up) return;
            if (CurrentDirection == Direction.Left && NextDirection == Direction.Right) return;
            if (CurrentDirection == Direction.Right && NextDirection == Direction.Left) return;

            CurrentDirection = NextDirection;
        }

        private void LoadLevel(int levelNo)
        {
            try
            {
                var filePath = $"levels/level{levelNo}.txt";
                using (var reader = new StreamReader(filePath))
                {
                    for (int x = 1; x <= gridSize; x++)
                    {
                        string line = reader.ReadLine();
                        for (int y = 1; y <= gridSize; y++)
                        {
                            gridMap[x, y] = line[y - 1];
                        }
                    }
                }
            }
            catch
            {
                // Fallback: clear map if file not found
                for (int x = 1; x <= gridSize; x++)
                    for (int y = 1; y <= gridSize; y++)
                        gridMap[x, y] = ' ';
            }

            GetSpawnLocation(levelNo);
            OnLevelLoaded?.Invoke();
        }

        private void PlaceInitialSnake()
        {
            snake[0].X = spawnX;
            snake[0].Y = spawnY;

            // Place initial 3-segment snake going right
            snakeLength = 3;
            snake[1].X = spawnX;
            snake[1].Y = spawnY + 1;
            snake[2].X = spawnX;
            snake[2].Y = spawnY + 2;
        }

        private void PlacePickup(bool isFirst)
        {
            int xMin = isFirst ? 5 : 1;
            int xMax = gridSize - 1;
            int yMin = isFirst ? 5 : 1;
            int yMax = gridSize - 1;

            bool placed = false;
            while (!placed)
            {
                int rx = random.Next(xMin, xMax + 1);
                int ry = random.Next(yMin, yMax + 1);

                // Check not on snake
                bool onSnake = false;
                for (int i = 0; i < snakeLength; i++)
                {
                    if (rx == snake[i].X && ry == snake[i].Y)
                    {
                        onSnake = true;
                        break;
                    }
                }

                // Check not on obstacle
                if (onSnake || gridMap[rx, ry] == '#')
                    continue;

                pickup.X = rx;
                pickup.Y = ry;
                pickupActive = true;
                placed = true;
            }
        }

        private void GrowSnake()
        {
            // Extend tail at the last position — the next Update() tick shifts it naturally
            if (snakeLength < MaxSnakeLength)
            {
                snake[snakeLength].X = snake[snakeLength - 1].X;
                snake[snakeLength].Y = snake[snakeLength - 1].Y;
                snakeLength++;
            }
        }

        private void GetSpawnLocation(int levelNo)
        {
            switch (levelNo)
            {
                case 1:
                    spawnX = random.Next(4, 28);
                    spawnY = random.Next(4, 28);
                    break;
                case 2:
                    spawnX = random.Next(5, 27);
                    spawnY = random.Next(5, 27);
                    break;
                case 3:
                case 4:
                    spawnX = 16;
                    spawnY = random.Next(1, 3) == 1 ? 8 : 24;
                    break;
                case 5:
                case 7:
                    int k = random.Next(1, 5);
                    switch (k)
                    {
                        case 1: spawnX = 8; spawnY = 8; break;
                        case 2: spawnX = 8; spawnY = 24; break;
                        case 3: spawnX = 24; spawnY = 8; break;
                        case 4: spawnX = 24; spawnY = 24; break;
                        default: spawnX = 16; spawnY = 15; break;
                    }
                    break;
                case 6:
                    spawnX = random.Next(3, 29);
                    while (spawnX % 2 != 0)
                        spawnX = random.Next(3, 29);
                    spawnY = random.Next(3, 28);
                    break;
                default:
                    spawnX = 16;
                    spawnY = 16;
                    break;
            }
        }

        #endregion
    }
}
