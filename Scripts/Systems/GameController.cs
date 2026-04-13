using Godot;

public partial class GameController : Node
{

	[Export] protected PackedScene WinPopup;
	[Export] protected GameUi GameUi;
	[Export] protected EnemySpawner EnemySpawner;
	[Export] protected AbilitySystem AbilitySystem;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EnemySpawner.OnEnemiesCountChanged += OnEnemiesCountChangedHandler;
		AbilitySystem.OnAbilitySwitched += OnAbilitySwitchedHandler;
		base._Ready();
	}
	public override void _ExitTree()
	{
		EnemySpawner.OnEnemiesCountChanged -= OnEnemiesCountChangedHandler;
		AbilitySystem.OnAbilitySwitched -= OnAbilitySwitchedHandler;

		base._ExitTree();
	}

	private void OnAbilitySwitchedHandler(StringName newAbilityName)
	{
		GameUi.UpdateSpellEquipped(newAbilityName);
	}

	private void OnEnemiesCountChangedHandler(int remainingEnemies)
	{
		GameUi.UpdateEnemiesRemaining(remainingEnemies);
		if(remainingEnemies <= 0)
		{
			GD.Print("All enemies defeated! You win!");
			Node winNode = WinPopup.Instantiate();
			AddChild(winNode);
		}
	}
}
