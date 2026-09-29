using Godot;

/// Attach to Zipper (child of PathFollow3D, which is a child of Path3D).
/// Click near the zipper and drag with the left mouse button; it slides along the path.
public partial class ZipperDrag : Node3D
{
    [Export] public float GrabRadiusPixels = 40f; // how close (on screen) the click must be

    private PathFollow3D _follow;
    private Path3D _path;
    private bool _dragging;

    public override void _Ready()
    {
        _follow = GetParent<PathFollow3D>();
        _path = _follow.GetParent<Path3D>();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        Camera3D cam = GetViewport().GetCamera3D();
        if (cam == null)
            return;

        if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left)
        {
            if (mb.Pressed)
            {
                // Grab if the click lands near the zipper's position on screen (no collider needed)
                if (cam.IsPositionBehind(GlobalPosition))
                    return;
                Vector2 zipperOnScreen = cam.UnprojectPosition(GlobalPosition);
                _dragging = zipperOnScreen.DistanceTo(mb.Position) <= GrabRadiusPixels;
            }
            else
            {
                _dragging = false;
            }
        }
        else if (@event is InputEventMouseMotion mm && _dragging)
        {
            DragTo(cam, mm.Position);
        }
    }

    private void DragTo(Camera3D cam, Vector2 mousePos)
    {
        // Ray from the camera through the mouse cursor
        Vector3 rayOrigin = cam.ProjectRayOrigin(mousePos);
        Vector3 rayDir = cam.ProjectRayNormal(mousePos);

        // Plane facing the camera, passing through the zipper
        var plane = new Plane(-cam.GlobalBasis.Z, GlobalPosition);
        Vector3? hit = plane.IntersectsRay(rayOrigin, rayDir);
        if (hit == null)
            return;

        // Closest point on the curve to where the mouse is -> distance along the path
        Vector3 localHit = _path.ToLocal(hit.Value);
        _follow.Progress = _path.Curve.GetClosestOffset(localHit);
    }
}
