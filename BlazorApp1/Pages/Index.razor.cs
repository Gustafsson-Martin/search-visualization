namespace BlazorApp1.Pages {

    public enum AlgorithmType {
        BreadthFirstSearch,
        DepthFirstSearch,
        DirectFind,
    }

    public partial class Index {
        public const int SIZE = 16;
        public static (int, int) RIGHT = (0, 1);
        public static (int, int) LEFT = (0, -1);
        public static (int, int) UP = (-1, 0);
        public static (int, int) DOWN = (1, 0);
        public static (int, int)[] NEIGHBOUR_VECTORS = new (int, int)[4] { UP, LEFT, DOWN, RIGHT };
        
        private readonly Random random = new();
        private readonly Cell[,] Grid;
        private AlgorithmType Algorithm { get; set; } = AlgorithmType.BreadthFirstSearch;
        private (int, int) Goal;
        private (int, int) Start;

        public Index() {
            Grid = new Cell[SIZE, SIZE];
            ResetGrid();
        }

        private void ResetGrid() {
            for (var i = 0; i < SIZE; i++) {
                for (var j = 0; j < SIZE; j++) {
                    Grid[i, j] = new Cell();
                }
            }
            Start = (random.Next(0, SIZE), random.Next(0, SIZE));
            Goal = (random.Next(0, SIZE), random.Next(0, SIZE));
            Grid[Start.Item1, Start.Item2].Type = CellType.START;
            Grid[Goal.Item1, Goal.Item2].Type = CellType.GOAL;
        }

        public void start() {
            switch (Algorithm) {
                case AlgorithmType.BreadthFirstSearch:
                    BreadthFirstSearch();
                    break;
                case AlgorithmType.DepthFirstSearch:
                    DepthFirstSearch();
                    break;
                case AlgorithmType.DirectFind:
                    DirectFind();
                    break;
            }
        }

        public void click(int v) {
            Console.WriteLine(v / SIZE + "," + v % SIZE);
            Console.WriteLine(Algorithm);
            Grid[v / SIZE, v % SIZE].Type = CellType.WALL;
        }

        async public void DirectFind() {
            (int row, int column) = Start;
            while ((row, column) != Goal) {
                if (row < Goal.Item1) row++;
                else if (row > Goal.Item1) row--;
                else if (column < Goal.Item2) column++;
                else if (column > Goal.Item2) column--;
                Grid[row, column].State = CellState.EXPLORED;
                await Task.Delay(20);
                StateHasChanged();
            }
        }

        async public void BreadthFirstSearch() {
            Queue<(int, int)> queue = new Queue<(int, int)>();
            (int row, int column) = Start;
            Grid[row, column].State = CellState.VISITED;
            queue.Enqueue((row, column));
            while (queue.Count != 0) {
                (row, column) = queue.Dequeue();
                if (Grid[row, column].Type == CellType.GOAL) return;
                await Task.Delay(20);
                Grid[row, column].State = CellState.EXPLORED;
                StateHasChanged();
                foreach ((int dx, int dy) in NEIGHBOUR_VECTORS) {
                    (int nextRow, int nextColumn) = (row + dy, column + dx);
                    if (nextRow == SIZE || nextColumn == SIZE || nextRow == -1 || nextColumn == -1) continue;
                    if (Grid[nextRow, nextColumn].Type == CellType.WALL) continue;
                    if (Grid[nextRow, nextColumn].State != CellState.NONE) continue;
                    await Task.Delay(10);
                    Grid[nextRow, nextColumn].State = CellState.VISITED;
                    queue.Enqueue((nextRow, nextColumn));
                    StateHasChanged();
                }
            }
            StateHasChanged();
        }

        async public void DepthFirstSearch() {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            (int row, int column) = Start;
            Grid[row, column].State = CellState.VISITED;
            stack.Push((row, column));
            while (stack.Count != 0) {
                (row, column) = stack.Pop();
                if (Grid[row, column].Type == CellType.GOAL) return;
                await Task.Delay(20);
                Grid[row, column].State = CellState.EXPLORED;
                StateHasChanged();
                foreach ((int drow, int dcolumn) in NEIGHBOUR_VECTORS) {
                    (int nextRow, int nextColumn) = (row + drow, column + dcolumn);
                    if (nextRow == SIZE || nextColumn == SIZE || nextRow == -1 || nextColumn == -1) continue;
                    if (Grid[nextRow, nextColumn].Type == CellType.WALL) continue;
                    if (Grid[nextRow, nextColumn].State != CellState.NONE) continue;
                    await Task.Delay(10);
                    Grid[nextRow, nextColumn].State = CellState.VISITED;
                    stack.Push((nextRow, nextColumn));
                    StateHasChanged();
                }
            }
        }
    }
}
