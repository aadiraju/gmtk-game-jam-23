using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour {
	public static TickManager Instance;
	public bool active = false;
	[SerializeField] private int fixedTicksPerGameTick;
	private int ticksSinceLastGameTick = 0;
	private List<BaseGuard> tickableGuards = new List<BaseGuard>();
	private BaseIntruder intruder;
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
		if (intruder != null) {
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
		this.intruder = intruder;
	}

	public void removeGuard(BaseGuard guard) {
		guardsToRemove.Add(guard);
	}

	public void removeIntruder() {
		intruder.OccupiedTile.OccupyingUnit = null;
		intruder = null;
	}

	private void executeRemovals() {
		foreach (var guard in guardsToRemove) {
			tickableGuards.Remove(guard);
		}
	}
}
