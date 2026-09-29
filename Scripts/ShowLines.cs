using Godot;
using System;

public partial class ShowLines : Control
{
    public Vector2 follower ;
    public Vector2 rayToClosestPointStart = Vector2.Zero;
    public Vector2 rayToClosestPointEnd  = Vector2.Zero;
    
    public override void _Ready()
    {
        // Cover the whole screen, and never block mouse clicks meant for the game.
        SetAnchorsPreset(LayoutPreset.FullRect);
        MouseFilter = MouseFilterEnum.Ignore;
    }

    public override void _Draw()
    {
        DrawCircle(follower, 3f, Colors.Red);
        
        DrawLine(rayToClosestPointStart, rayToClosestPointEnd, Colors.Gray);
        
        base._Draw();
    }
}
