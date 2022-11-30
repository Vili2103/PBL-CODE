using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkMapGen : AbstractDungeonGen
{
  //  [SerializeField] // We do this so we can save these parameters in a scriptable object and we will also be able to edit them from the inspector in Unity :)
  //  protected Vector2Int startPos = Vector2Int.zero;
    [SerializeField]
    private int loops = 10; //Iterations of the RandomWalkAlg 
    [SerializeField]
    public int stepsToTake = 10; 
    [SerializeField]
    public bool startRandomlyEachLoop = true;
 //    [SerializeField]
    //private TileMaker tileMaker;
    protected override void RunPPG()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tileMaker.ClearTiles();
        tileMaker.PlaceFloorTiles(floorPositions);
        WallMaker.MakeWalls(floorPositions, tileMaker);
    }
    protected HashSet<Vector2Int> RunRandomWalk() // protected means that this can only be acessed by classes that derive from RandomWalkMapGen
    {
        var pos = startPos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // Hashset of positions where we need to draw tiles
        for(int i=0;i < loops; i++)
        {
            var path = RandomWalk.RandomWalkAlg(pos, stepsToTake); // We make a union between the taken path and the tile positions so there aren't gaps
            floorPositions.UnionWith(path); // UnionWith is a method from the HashSet
            //We do this to not go over tiles multiple times :)
            if (startRandomlyEachLoop)
            {
                pos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count)); 
            }
        }
        return floorPositions;
    }

   
}
