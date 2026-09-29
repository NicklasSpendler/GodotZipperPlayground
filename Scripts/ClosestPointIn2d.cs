using Godot;
using System;

public partial class ClosestPointIn2d : Node2D
{
    [Export] private Node2D point1;
    [Export] private Node2D point2;

    private float distance;
    private Vector2 closestPoint;

    private Vector2 mousePosition;
    
    public override void _Ready()
    {
        
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouseMotion mouseMotion)
        {
            mousePosition = mouseMotion.Position;
        }
        
    }

    public override void _PhysicsProcess(double delta)
    {
        distance = GetClosestPoint(mousePosition, point1.Position, point2.Position, out Vector2 closestPoint);
        this.closestPoint = closestPoint;
        QueueRedraw();
    }

    public override void _Draw()
    {
        base._Draw();
        
        DrawLine(point1.Position, point2.Position, Colors.Blue, 5f, true);
        
        DrawCircle(closestPoint, 10f, Colors.Red, true);
        
        GD.Print("distance: " + distance);
    }

    private float GetClosestPoint(Vector2 mousePos, Vector2 a, Vector2 b, out Vector2 closestPoint)
    {
        Vector2 ab = b - a;
        Vector2 ap = mousePos - a;

        float projection = ap.Dot(ab);
        float abLenSq = ab.LengthSquared();
        float normalisedProjection = projection / abLenSq;

        if (normalisedProjection <= 0)
        {
            closestPoint = a;
        }
        else if (normalisedProjection >= 1)
        {
            closestPoint = b;
        }
        else
        {
            closestPoint = a + ab * normalisedProjection;
        }

        return mousePos.DistanceTo(closestPoint);
    }
}
