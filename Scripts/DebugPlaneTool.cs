using Godot;
using System;

public partial class DebugPlaneTool : MeshInstance3D
{
    [Export] private Vector2 Size = new Vector2(2f, 2f);
    

    
    public override void _Ready()
    {
        
        StandardMaterial3D material = new StandardMaterial3D
        {
            //Makes it so the material does not respond to light and shadows, I believe?
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            //Required to make the material transparent
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
            
            AlbedoColor = new Color(1.0f, 1.0f, 0.0f, 0.3f),
            
            //Makes it so the plane can be seen from both front and back. Nice to have to see the plane, in case the normal is not
            //Towards the camera.
            CullMode = BaseMaterial3D.CullModeEnum.Disabled
        };
        
        Mesh = new QuadMesh()
        {
            Size = Size,
            Material = material,
        };
    }

    public void DrawPlaneMesh(Plane plane, Vector3 position, Transform3D cameraTrans)
    {
        Vector3 planesCenter = plane.Project(position);
        GlobalTransform = new Transform3D(cameraTrans.Basis, planesCenter);
    }
}
