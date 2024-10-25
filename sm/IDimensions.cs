namespace sm
{
    struct Dimensions(int x, int y)
    {
        public int Width { get; set; } = x;
        public int Height { get; set; } = y;
    }

    internal interface IDimensions
    {
        Dimensions Dim { get; set; } 
    }
}
