using System;

namespace Boron
{
    // Shape interface that all shape classes will implement
    public interface IShape
    {
        double CalculateArea();
    }

    // Abstract base class that implements IShape
    public abstract class Shape : IShape
    {
        public abstract double CalculateArea();

        // Common functionality for all shapes could be added here
        public string GetShapeDescription()
        {
            return $"This is a {GetType().Name} with an area of {CalculateArea():F2} square units.";
        }
    }

    // Square implementation with primary constructor
    // This uses C# 12's primary constructor syntax
    public class Square(double side) : Shape
    {
        public double Side { get; set; } = side;

        public override double CalculateArea()
        {
            return Side * Side;
        }
    }

    // Rectangle implementation with primary constructor
    // This uses C# 12's primary constructor syntax
    public class Rectangle(double length, double width) : Shape
    {
        public double Length { get; set; } = length;
        public double Width { get; set; } = width;

        public override double CalculateArea()
        {
            return Length * Width;
        }
    }

    // Circle implementation with primary constructor
    // This uses C# 12's primary constructor syntax
    public class Circle(double radius) : Shape
    {
        public double Radius { get; set; } = radius;

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
    }

    // Trapezium (Trapezoid) implementation with primary constructor
    // This uses C# 12's primary constructor syntax
    public class Trapezium(double topSide, double bottomSide, double height) : Shape
    {
        public double TopSide { get; set; } = topSide;
        public double BottomSide { get; set; } = bottomSide;
        public double Height { get; set; } = height;

        public override double CalculateArea()
        {
            return 0.5 * (TopSide + BottomSide) * Height;
        }
    }

    // Star implementation (regular five-pointed star) with primary constructor
    // This uses C# 12's primary constructor syntax
    public class Star(double outerRadius, double innerRadius) : Shape
    {
        public double OuterRadius { get; set; } = outerRadius;
        public double InnerRadius { get; set; } = innerRadius;

        public override double CalculateArea()
        {
            // Area of a regular five-pointed star
            // This is an approximation based on the outer and inner radius
            double areaOfPentagon = 5 * 0.5 * OuterRadius * OuterRadius * Math.Sin(2 * Math.PI / 5);
            double areaOfTriangles =
                5 * 0.5 * InnerRadius * InnerRadius * Math.Sin(2 * Math.PI / 5);

            return areaOfPentagon - areaOfTriangles;
        }
    }
}
