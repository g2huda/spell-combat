using Godot;

public partial class InputHandler : Node
{

	[Signal] public delegate void MoveInputEventHandler(float value);
	[Signal] public delegate void TurnInputEventHandler(float value);
	[Signal] public delegate void AbilityRequestedEventHandler();
	[Signal] public delegate void SwitchAbilityEventHandler();

	[Export] protected StringName MoveForwardActionName = "move_forward";
	[Export] protected StringName MoveBackwardActionName = "move_backward";
	[Export] protected StringName TurnRightActionName = "turn_right";
	[Export] protected StringName TurnLeftActionName = "turn_left";
	[Export] protected StringName AbilityActionName = "ability";
	[Export] protected StringName SwitchAbilityActionName = "switch_ability";

	public override void _Input(InputEvent @event)
	{
		if(@event.IsAction(MoveForwardActionName)||@event.IsAction(MoveBackwardActionName))
		{
			float move = @event.GetActionStrength(MoveForwardActionName) - @event.GetActionStrength(MoveBackwardActionName);
			EmitSignal(SignalName.MoveInput, move);
		}

		if(@event.IsAction(TurnRightActionName) || @event.IsAction(TurnLeftActionName))
		{
			float turn = @event.GetActionStrength(TurnRightActionName) - @event.GetActionStrength(TurnLeftActionName);
			EmitSignal(SignalName.TurnInput, turn);
		}

		if(@event.IsAction(AbilityActionName) && @event.IsPressed())
		{
			EmitSignal(SignalName.AbilityRequested);
		}

		if(@event.IsAction(SwitchAbilityActionName) && @event.IsPressed())
		{
			EmitSignal(SignalName.SwitchAbility);
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
