using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public abstract class Obstacle // Creating an abstract obstacle class
{
    public int X { get; set; }
    public int Y { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Obstacle"/> class.
    /// </summary>
    /// <param name="x">The X-coordinate of the obstacle.</param>
    /// <param name="y">The Y-coordinate of the obstacle.</param>
    public Obstacle(int x, int y)
    {
        X = x;
        Y = y;
    }
    /// <summary>
    /// Checks if the given (x,y) is blocked by an obstacle
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns> True if the position is blocked, otherwise False </returns>
    public abstract bool IsPositionBlocked(int x, int y);
}

/// <summary>
/// Represents a Guard obstacle 
/// </summary>
public class Guard : Obstacle 
{
    public Guard(int x, int y) : base(x, y) { }

    public override bool IsPositionBlocked(int x, int y)
    {
        return this.X == x && this.Y == y;
    }
}

/// <summary>
/// Represents a Fence obstacle 
/// </summary>
public class Fence : Obstacle
{
    public int EndX { get; set; }
    public int EndY { get; set; }

    public Fence(int startX, int startY, int endX, int endY) : base(startX, startY)
    {
        EndX = endX;
        EndY = endY;
    }

    public override bool IsPositionBlocked(int x, int y)
    {
        // We first check if the fences go horizontally or vertically
        // Then the logic checks if x,y lies on the line segment between start and end points of the fence
        return (X == EndX && x == X && y >= Math.Min(Y, EndY) && y <= Math.Max(Y, EndY)) || (Y == EndY && y == Y && x >= Math.Min(X, EndX) && x <= Math.Max(X, EndX));
    }
}

/// <summary>
/// Represents a sensor obstacle, which asks for a range
/// </summary>

public class Sensor : Obstacle
{
    public float Range { get; set; }

    public Sensor(int x, int y, float range) :base(x, y)
    {
        Range = range;
    }

    public override bool IsPositionBlocked(int x, int y)
    {
        double distance = Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));  // Here we use the distance formula to find the distance between the 2 (x,y) points
        return distance <= Range; // Then we do a check if that distance is lesser than the range of the sensor
    }
}

/// <summary>
/// Represents a sawblade obstacle, has extra property called Bladelength
/// </summary>
public class Sawblade : Obstacle
{
    public float Bladelength { get; set; }

    public Sawblade(int x, int y, float length) : base(x, y)
    {
        Bladelength = length;
    }

    public override bool IsPositionBlocked(int x, int y)
    {
        double distance = Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2)); // calculating the distance between the person and the sawblades
        return distance <= Bladelength; // Then we do a check if that distance is less than the blade length
    }
}

/// <summary>
/// Represents the Lasergrid obstacle, has 2 extra properties Width and Heigth
/// </summary>

public class LaserGrid : Obstacle
{
    public int Width { get; set; }
    public int Height { get; set; }

    public LaserGrid(int x, int y, int width, int height) : base(x, y)
    {
        Width = width;
        Height = height;
    }

    public override bool IsPositionBlocked(int x, int y)
    {
        return x >= X && x <= X + Width && y >= Y && y <= Y + Height; // Evaluates to true if the given position (x, y) is inside the rectangular area of grid.
    }
}

/// <summary>
/// Represents the camera obstacle, has an extra property called Direction
/// </summary>

public class Camera : Obstacle
 {

    public char Direction { get; set; }

    public Camera(int x, int y, char direction) : base(x, y)
    {
        Direction = direction;
    }

    public override bool IsPositionBlocked(int x, int y)
    {
        
        if (this.X == x && this.Y == y)
            return true;

        int deltaX = x - X; // gives a value on how far x,y is from the camera
        int deltaY = y - Y;

        switch (Direction)
        {
            case 'n':
                if (deltaX == 0 && deltaY < 0) return true; // Directly north
                if (deltaY != 0 && Math.Abs(deltaX / (double)deltaY) <= 1 && deltaY < 0) return true; // 45 degrees angle
                break;
            case 's':
                if (deltaX == 0 && deltaY > 0) return true; // Directly south
                if (deltaY != 0 && Math.Abs(deltaX / (double)deltaY) <= 1 && deltaY> 0) return true; 
                break;
            case 'e':
                if (deltaY == 0 && deltaX > 0) return true; // Directly east
                if (deltaX != 0 && Math.Abs(deltaY / (double)deltaX) <= 1 && deltaX > 0) return true; 
                break;
            case 'w':
                if (deltaY == 0 && deltaX < 0) return true; // Directly west
                if (deltaX !=0 && Math.Abs(deltaY / (double)deltaX) <= 1 && deltaX < 0) return true; 
                break;
        }

        return false;
    }

}

