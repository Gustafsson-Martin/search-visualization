namespace BlazorApp1.Pages {
    public enum CellType {
        EMPTY,
        WALL,
        GOAL,
        START,
    }

    public enum CellState {
        NONE,
        VISITED,
        EXPLORED,
    }

    public class Cell {
        public CellType Type { get; set; }
        public CellState State { get; set; }

        public Cell() {
            this.State = CellState.NONE;
            this.Type = CellType.EMPTY;
        }

        public string Color() {
            if (this.Type == CellType.START) return "green";
            if (this.Type == CellType.GOAL) return "orange";
            if (this.State == CellState.VISITED) return "yellow";
            if (this.State == CellState.EXPLORED) return "blue";
            if (this.Type == CellType.WALL) return "black";
            if (this.State == CellState.NONE) return "white";
            return "purple";
        }
    }
}
