using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour {
	public static TickManager Instance;
	public bool active = false;
	[SerializeField] private int fixedTicksPerGameTick = 20;
	private int ticksSinceLastGameTick = 0;
	private List<BaseGuard> tickableGuards = new List<BaseGuard>();
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
		foreach (var intruder in tickableIntruders) {
			intruder.TickUp();
		}

		foreach (var guard in tickableGuards) {
			guard.TickUp();
		}

		foreach (var guard in tickableGuards) {
			if (guard.currentSuspicion >= guard.maxSuspicion) {
				GameManager.Instance.GuardsAlerted();
				break;
			}
		}
	}

	public void addGuard(BaseGuard guard) {
		tickableGuards.Add(guard);
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
			tickableGuards.Remove(guard);
		}

		foreach (var intruder in intrudersToRemove) {
			tickableIntruders.Remove(intruder);
		}
	}
}
