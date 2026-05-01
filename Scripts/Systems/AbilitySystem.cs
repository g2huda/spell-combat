using Godot;
using Godot.Collections;

public partial class AbilitySystem : Node
{
	[Signal] public delegate void OnAbilityHitEventHandler(AbilityHitData hitData);
	[Signal] public delegate void OnAbilityUsedEventHandler(AbilityHitData hitData);
	[Signal] public delegate void OnAbilitySwitchedEventHandler(StringName newAbilityName);

	[Export] protected Player Player;
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
		Player.OnSpellReady += OnAbilityRequested;
		Player.SwitchAbilityReady += OnSwitchAbilityRequested;
		base._Ready();
	}

	public override void _ExitTree()
	{
		Player.OnSpellReady -= OnAbilityRequested;
		Player.SwitchAbilityReady -= OnSwitchAbilityRequested;
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

	private void OnAbilityRequested(Vector3 abilityPoint)
	{
		_equippedAbility.FireAbility(Player, -Player.Transform.Basis.Z,
			abilityPoint);
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
		EmitSignal(SignalName.OnAbilitySwitched, newAbilityName);
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

	private void OnAbilityHitHandler(AbilityHitData hitData)
	{
		if(hitData.Target == Player || hitData.Target is BaseAbility) return;

		EmitSignal(SignalName.OnAbilityHit, hitData);
	}

	internal void OnAbilityUsedHandler(AbilityHitData hitData)
	{
		hitData.SourceAbility.EnableAbility(false);
		EmitSignal(SignalName.OnAbilityUsed, hitData);
	}
}
