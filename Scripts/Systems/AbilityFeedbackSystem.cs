using Godot;
using System.Collections.Generic;

public partial class AbilityFeedbackSystem : Node
{
	[Export] protected AbilitySystem AbilitySystem;
	[Export] protected int VFXCount = 6;
	[Export] protected int SFXCount = 6;

	private Dictionary<string, List<EnemyHitVfx>> _abilityVfxMap = new Dictionary<string, List<EnemyHitVfx>>();
	private Dictionary<string, List<EnemyHitSfx>> _abilitySfxMap = new Dictionary<string, List<EnemyHitSfx>>();
	private EffectData<EnemyHitVfx> _vfxData = new EffectData<EnemyHitVfx>();
	private EffectData<EnemyHitSfx> _sfxData = new EffectData<EnemyHitSfx>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AbilitySystem.OnAbilityUsed += GiveHitFeedback;
		base._Ready();
	}

	public override void _ExitTree()
	{
		AbilitySystem.OnAbilityUsed -= GiveHitFeedback;
		base._ExitTree();
	}
   
	private void GiveHitFeedback(AbilityHitData hitData)
	{
		if(_abilityVfxMap.ContainsKey(hitData.AbilityID))
		{
			_vfxData = new EffectData<EnemyHitVfx>
			{
				AbilityID = hitData.AbilityID,
				CurrentEffect = _abilityVfxMap[hitData.AbilityID][_vfxData.CurrentIndex],
				CurrentIndex = _vfxData.CurrentIndex
			};
		}
		else
		{
			List<EnemyHitVfx> vfxPool = InstantiatePool<EnemyHitVfx>(hitData.HitVfxScene, VFXCount);
			_abilityVfxMap[hitData.AbilityID] = vfxPool;
			_vfxData.CurrentEffect = _abilityVfxMap[hitData.AbilityID][_vfxData.CurrentIndex];
			_vfxData = new EffectData<EnemyHitVfx>
			{
				AbilityID = hitData.AbilityID,
				CurrentEffect = _abilityVfxMap[hitData.AbilityID][0],
				CurrentIndex = 0
			};
		}

		if(_abilitySfxMap.ContainsKey(hitData.AbilityID))
		{
			_sfxData = new EffectData<EnemyHitSfx>
			{
				AbilityID = hitData.AbilityID,
				CurrentEffect = _abilitySfxMap[hitData.AbilityID][_sfxData.CurrentIndex],
				CurrentIndex = _sfxData.CurrentIndex
			};
		}
		else
		{
			List<EnemyHitSfx> sfxPool = InstantiatePool<EnemyHitSfx>(hitData.HitSfxScene, SFXCount);
			_abilitySfxMap[hitData.AbilityID] = sfxPool;
			_sfxData = new EffectData<EnemyHitSfx>
			{
				AbilityID = hitData.AbilityID,
				CurrentEffect = _abilitySfxMap[hitData.AbilityID][0],
				CurrentIndex = 0
			};
		}

		_vfxData.CurrentEffect.Play(hitData.SourceAbility.GlobalPosition, hitData.HitNormal);
		_sfxData.CurrentEffect.Play(hitData.SourceAbility.GlobalPosition);

		_sfxData.CurrentIndex = (_sfxData.CurrentIndex + 1) % SFXCount;
		_vfxData.CurrentIndex = (_vfxData.CurrentIndex + 1) % VFXCount;
	   
	}

	private List<T> InstantiatePool<T>(PackedScene scene, int count) where T : Node3D
	{
		List<T> pool = new List<T>();
		for(int i = 0; i < count; i++)
		{
			T instance = scene.Instantiate<T>();
			AddChild(instance);
			pool.Add(instance);
		}
		return pool;
	}
}

public partial class EffectData<T> : RefCounted where T : Node3D
{
	public string AbilityID { get; set; }
	public T CurrentEffect { get; set; }
	public int CurrentIndex { get; set; }
}
