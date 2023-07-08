using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour {
	public static TickManager Instance;
	public bool active = true;
	[SerializeField] private int fixedTicksPerGameTick = 20;
	private int ticksSinceLastGameTick = 0;
	private List<BaseGuard> tickableGaurds = new List<BaseGuard>();
	private List<BaseIntruder> tickableIntruders = new List<BaseIntruder>();
	private List<BaseGuard> guardsToRemove = new List<BaseGuard>();
	private List<BaseIntruder> intrudersToRemove = new List<BaseIntruder>();

    void Start() {
        Instance = this;
    }

	void FixedUpdate() {
		executeRemovals();
		if (active && ++ticksSinceLastGameTick >= fixedTicksPerGameTick) {
			ticksSinceLastGameTick = 0;
			TickUnits();
		}
	}

	private void TickUnits() {
		foreach (var guard in tickableGaurds) {
			guard.TickUp();
		}

		foreach (var intruder in tickableIntruders) {
			intruder.TickUp();
		}
	}

	public void addGuard(BaseGuard guard) {
		tickableGaurds.Add(guard);
	}

	public void addIntruder(BaseIntruder intruder) {
		tickableIntruders.Add(intruder);
	}

	public void removeGuard(BaseGuard guard) {
		guardsToRemove.Add(guard);
	}

	public void removeIntruder(BaseIntruder intruder) {
		intrudersToRemove.Add(intruder);
	}

	private void executeRemovals() {
		foreach (var guard in guardsToRemove) {
			tickableGaurds.Remove(guard);
		}

		foreach (var intruder in intrudersToRemove) {
			tickableIntruders.Remove(intruder);
		}
	}
}
