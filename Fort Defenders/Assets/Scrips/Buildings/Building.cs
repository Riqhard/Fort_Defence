using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Building : ScriptableObject
{
    new public string name = "New Building";
    public BuildingType buildingType;
    public GameObject buildingPrefab;

    [Header("Costs")]
    public int costToBuild;
    public int costToUpgrade;
    public int maxLevel;



}
public enum BuildingType { Turret, Resource, UnitProduction };
