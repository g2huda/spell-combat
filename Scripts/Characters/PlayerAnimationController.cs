using Godot;

public partial class PlayerAnimationController : Node
{
	[Signal] public delegate void OnSpellReadyEventHandler();
	[Signal] public delegate void OnSpellCastCompleteEventHandler();

	[Export] private AnimationTree _animationTree;
	[Export] private StringName _attackOneShotName = "Attack";
	[Export] private StringName _moveBlendName = "Movement";

	private OneShotData _attackOneShotData => 
		new OneShotData { NodeName = _attackOneShotName, Request = AnimationNodeOneShot.OneShotRequest.Fire };
	private BlendSpace1DData _moveBlendData => 
		new BlendSpace1DData { NodeName = _moveBlendName };

	private bool _isAttacking = false;
	public void TriggerAttack()
	{
		if (_isAttacking) return; // Prevent overlapping attacks
		_animationTree.Set(_attackOneShotData.FullPath, (int)_attackOneShotData.Request);
		_isAttacking = true;
	}

	public void TriggerSpellReady()
	{
		EmitSignal(SignalName.OnSpellReady);
	}

	public void TriggerSpellCastComplete()
	{
		EmitSignal(SignalName.OnSpellCastComplete);
		_isAttacking = false;
	}

	public void SetMoveSpeed(float speed)
	{
		_animationTree.Set(_moveBlendData.FullPath, speed);
	}
}
