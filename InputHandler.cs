using Godot;
using System.Diagnostics;

public partial class InputHandler : Node
{

	[Signal] public delegate void MoveInputEventHandler(float value);
	[Signal] public delegate void TurnInputEventHandler(float value);

	[Export] protected StringName MoveForwardActionName = "move_forward";
	[Export] protected StringName MoveBackwardActionName = "move_backward";
	[Export] protected StringName TurnRightActionName = "turn_right";
	[Export] protected StringName TurnLeftActionName = "turn_left";

	public readonly StringName Move = SignalName.MoveInput;
	public readonly StringName Turn = SignalName.TurnInput;


	public override void _Input(InputEvent @event)
	{
		if(@event.IsAction(MoveForwardActionName)||@event.IsAction(MoveBackwardActionName))
		{
			float move = @event.GetActionStrength(MoveForwardActionName) - @event.GetActionStrength(MoveBackwardActionName);
			EmitSignal(Move, move);
		}

		if(@event.IsAction(TurnRightActionName) || @event.IsAction(TurnLeftActionName))
		{
			float turn = @event.GetActionStrength(TurnRightActionName) - @event.GetActionStrength(TurnLeftActionName);
			EmitSignal(Turn, turn);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
