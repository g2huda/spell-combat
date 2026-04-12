using Godot;
using Godot.Collections;
using System;

public partial class AbilitySystem : Node
{
	[Signal] public delegate void OnAbilityHitEventHandler(Node3D target, float power, BaseAbility ability);

	[Export] protected InputHandler InputHandler;
	[Export] protected Node3D Player;
	[Export] protected Node3D AbilitiesLocalPosition;
	[Export] protected Array<PackedScene> Abilities { get; set; }
	[Export] protected float AbilityRange = 6f;


	private BaseAbility _equippedAbility;
	private Dictionary<StringName, Array<BaseAbility>> _abilitiesPool
		= new Dictionary<StringName, Array<BaseAbility>>();
	private Array<StringName> _abilityNames = new Array<StringName>();
	private int _currentAbilityIndex = 0;
	private int _currentAbilityInstanceIndex = 0;

	public override void _EnterTree()
	{
		InitializeAbilities();
		base._EnterTree();
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputHandler.AbilityRequested += OnAbilityRequested;
		InputHandler.SwitchAbility += OnSwitchAbilityRequested;
		base._Ready();
	}

	public override void _ExitTree()
	{
		InputHandler.AbilityRequested -= OnAbilityRequested;
		InputHandler.SwitchAbility -= OnSwitchAbilityRequested;
		foreach(Array<BaseAbility> abilityList in _abilitiesPool.Values)
		{
			foreach(BaseAbility ability in abilityList)
			{
				ability.OnAbilityHit -= OnAbilityHitHandler;
			}
		}
		base._ExitTree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private T InstantiateAbility<T>(PackedScene abilityScene) where T : BaseAbility
	{
		T abilityInstance = abilityScene.Instantiate<T>();
		AddChild(abilityInstance);
		abilityInstance.EnableAbility(false);
		return abilityInstance;
	}

	private void OnAbilityRequested()
	{
		_equippedAbility.FireAbility(Player, -Player.Transform.Basis.Z,
			AbilitiesLocalPosition.GlobalPosition);
		_currentAbilityInstanceIndex =
			(_currentAbilityInstanceIndex + 1)
			% _abilitiesPool[_abilityNames[_currentAbilityIndex]].Count;
		_equippedAbility =
			_abilitiesPool[_abilityNames[_currentAbilityIndex]][_currentAbilityInstanceIndex];
	}

	private void OnSwitchAbilityRequested()
	{
		_equippedAbility.EnableAbility(false);
		_currentAbilityIndex = (_currentAbilityIndex + 1) % _abilityNames.Count;
		StringName newAbilityName = _abilityNames[_currentAbilityIndex];
		_currentAbilityInstanceIndex = 0;
		_equippedAbility = _abilitiesPool[newAbilityName][_currentAbilityInstanceIndex];
	}

	private void InitializeAbilities()
	{
		foreach(PackedScene ability in Abilities)
		{
			BaseAbility abilityInstance = InstantiateAbility<BaseAbility>(ability);
			abilityInstance.OnAbilityHit += OnAbilityHitHandler;
			StringName abilityName = abilityInstance.Name;
			_abilitiesPool[abilityName] = new Array<BaseAbility> { abilityInstance };
			_abilityNames.Add(abilityName);

			for(int i = 0; i < AbilityRange - 1; i++)
			{
				BaseAbility additionalInstance = InstantiateAbility<BaseAbility>(ability);
				additionalInstance.OnAbilityHit += OnAbilityHitHandler;
				_abilitiesPool[abilityName].Add(additionalInstance);
			}
		}

		_equippedAbility =
			_abilitiesPool[_abilityNames[_currentAbilityIndex]][_currentAbilityInstanceIndex];
	}

	private void OnAbilityHitHandler(Node3D target, float power, BaseAbility ability)
	{
		if(target == Player || target is BaseAbility) return;

		EmitSignal(SignalName.OnAbilityHit, target, power, ability);
	}

	internal void OnAbilityUsed(BaseAbility ability)
	{
		ability.EnableAbility(false);
	}
}
