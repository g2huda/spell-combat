using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody3D
{
	public const float JumpVelocity = 4.5f;

	[Export] protected InputHandler InputHandler;
	[Export] protected float MoveSpeed = 6f;
	[Export] protected float RotationSpeed = 2.5f;

	private float _moveInput;
	private float _turnInput;

	public  override void _Ready()
	{
		InputHandler.Connect(InputHandler.Move, Callable.From<float>(OnMoveInput));
		InputHandler.Connect(InputHandler.Turn, Callable.From<float>(OnTurnInput));
		base._Ready();
	}

	
	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;

		// Rotation
		RotateY(-_turnInput * RotationSpeed * deltaF);

		// Forward movement
		Vector3 forward = -Transform.Basis.Z;
		Velocity = forward * _moveInput * MoveSpeed;

		MoveAndSlide();
	}

	public void OnMoveInput(float value)
	{
		Debug.Print($"Move input: {value}");
		_moveInput = value;
	}
	private void OnTurnInput(float value)
	{
		_turnInput = value;
	}
}
