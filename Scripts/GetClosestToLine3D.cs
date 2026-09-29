using Godot;
using System;
using Godot.NativeInterop;

public partial class GetClosestToLine3D : Camera3D
{
    [Export] public float GrabRadiusPixels = 40f;
    
    [Export] private Path3D path;
    [Export] private PathFollow3D follower;

    private Vector2 rayToClosestPointStart;
    private Vector2 rayToClosestPointEnd;
    
    
    private bool _dragging;
    
    ShowLines showLines;

    private DebugRayTool _debugRayTool;
    private DebugPlaneTool _debugPlaneTool;
    
    public override void _Ready()
    {
        base._Ready();
        
        var debugLayer = new CanvasLayer();
        showLines = new ShowLines();
        AddChild(debugLayer);
        debugLayer.AddChild(showLines);
        showLines.Visible = true;



        showLines.follower = UnprojectPosition(follower.GlobalPosition);
        
        _debugRayTool = new DebugRayTool();
        _debugPlaneTool = new DebugPlaneTool();
        
        AddChild(_debugRayTool);
        AddChild(_debugPlaneTool);
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
        {
            if (mouseButton.Pressed)
            {
                Vector2 zipperOnScreen = UnprojectPosition(follower.GlobalPosition);
                
                //Shows where the zipperIsOnScreen in debug
                showLines.follower = zipperOnScreen;
                
                _dragging = zipperOnScreen.DistanceTo(mouseButton.Position) <= GrabRadiusPixels;
                if (_dragging)
                    GD.Print("Is Dragging");
            }
            else
            {
                _dragging = false;
            }
        }

        else if (@event is InputEventMouseMotion mouseMotion && _dragging)
        {
            DragTo(mouseMotion.Position);
        }

        if (showLines != null)
        {
            showLines.QueueRedraw();
            
        }
    }

    private void DragTo(Vector2 mouseMotionPosition)
    {
        // Ray from the camera through the mouse cursor
        Vector3 rayOrigin = ProjectRayOrigin(mouseMotionPosition);
        Vector3 rayDirection = ProjectRayNormal(mouseMotionPosition);

        //Debug line is one unit under origin, because the camera cannot see the lien if exactly on top of origin.
        _debugRayTool.DrawRayMesh(rayOrigin - new Vector3(0, 1, 0), rayOrigin + rayDirection * 50f);
        
        var plane = new Plane(-GlobalBasis.Z, follower.GlobalPosition);
        
        Vector3? hit = plane.IntersectsRay(rayOrigin, rayDirection);
        if (hit == null)
            return;
        
        Vector3 localHit = path.ToLocal(hit.Value);
        follower.Progress = path.Curve.GetClosestOffset(localHit);


        _debugPlaneTool.DrawPlaneMesh(plane, follower.GlobalPosition, GlobalTransform);
        
        Debug2D(hit, localHit);
    }

    private void Debug2D(Vector3? hitOnPlane, Vector3 localHit)
    {
        if (hitOnPlane == null)
            return;
        
        //Shows line to closest point to the path.
        Vector3 closestOnCurve = path.ToGlobal(path.Curve.GetClosestPoint(localHit));
        if(!IsPositionBehind(hitOnPlane.Value))
            rayToClosestPointStart = UnprojectPosition(hitOnPlane.Value);
        if(!IsPositionBehind(closestOnCurve))
            rayToClosestPointEnd = UnprojectPosition(closestOnCurve);
        
        
        showLines.rayToClosestPointStart = rayToClosestPointStart;
        showLines.rayToClosestPointEnd = rayToClosestPointEnd;
    }
}
