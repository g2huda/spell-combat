using Godot;
using System;

public partial class CameraRig : Node3D
{
	[Export] protected Node3D Target;
	[Export] protected float FollowSpeed = 5f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Target == null)
			return;

		Vector3 desiredPosition = Target.GlobalPosition;
		desiredPosition.Y = 0f; // Adjust the height of the camera

		GlobalPosition = GlobalPosition.Lerp(desiredPosition, (float)delta * FollowSpeed);
	}
}
