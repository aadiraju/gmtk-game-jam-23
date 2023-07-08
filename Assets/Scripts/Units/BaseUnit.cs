using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour, Tickable
{
    public Tile OccupiedTile;
    protected Vector2 lookDirection = Vector2.down;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Rotate(Vector2 direction);
	
	public abstract void TickUp();
}
