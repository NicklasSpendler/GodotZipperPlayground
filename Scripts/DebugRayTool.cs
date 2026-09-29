using Godot;
using System;

public partial class DebugRayTool : MeshInstance3D
{
    private ImmediateMesh mesh = new();
    private StandardMaterial3D material;

    public override void _Ready()
    {
        Mesh = mesh;
        
        //All of this is preventable by adding this to the root of the scene.
        //Use world space instead of offsetting the Cameras transform
        TopLevel = true;
        //Resets the transform for the mesh back to the world origin.
        GlobalTransform = Transform3D.Identity;

        material = new StandardMaterial3D
        {
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            AlbedoColor = Colors.Aqua,
            NoDepthTest = true,
        };
    }

    public void DrawRayMesh(Vector3 from, Vector3 to)
    {
        mesh.ClearSurfaces();
        mesh.SurfaceBegin(Mesh.PrimitiveType.Lines, material);
        mesh.SurfaceAddVertex(from);
        mesh.SurfaceAddVertex(to);
        mesh.SurfaceEnd();
    }
}
