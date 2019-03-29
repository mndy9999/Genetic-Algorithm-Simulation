using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InitialPopulationGenerator : MonoBehaviour
{
    //number of individual from each species that will be generated at initialization
    [Range(0, 500)] public int maleSheepStartingCount = 10;
    [Range(0, 500)] public int femaleSheepStartingCount = 10;
    [Range(0, 500)] public int maleWolfStartingCount = 10;
    [Range(0, 500)] public int femaleWolfStartingCount = 10;
    [Range(0, 500)] public int treesStartingCount = 30;
    [Range(0, 500)] public int dirtStartingCount = 30;
    [Range(0, 500)] public int hayStartingCount = 30;

    public List<GameObject> agentPrefabs;   //holds prefabs of every critter type 

    const float AgentDensity = 1f;      //makes sure they don't spawn on top of each other

    //default positions of the groups
    public Transform sheepField;        
    public Transform wolvesField;

    Vector3 position;

    private void Start()
    {
        for (int i = 0; i < maleSheepStartingCount; i++)
        {
            //generate random position on the navmesh taking into account the number of individuals, agent density and the starting position of the species group
            position = GeneratePosition(maleSheepStartingCount, AgentDensity, sheepField);
            GameObject newAgent = Instantiate(agentPrefabs[0], position, Quaternion.identity);      //create object at that position
            newAgent.name = "Sheep_Male_" + i;      //give it a name and number to keep count
        }
        for (int i = 0; i < femaleSheepStartingCount; i++)
        {
            position = GeneratePosition(femaleSheepStartingCount, AgentDensity, sheepField);
            GameObject newAgent = Instantiate(agentPrefabs[1], position, Quaternion.identity);
            newAgent.name = "Sheep_Female_" + i;
        }
        for (int i = 0; i < maleWolfStartingCount; i++)
        {
            position = GeneratePosition(maleWolfStartingCount, AgentDensity, wolvesField);
            GameObject newAgent = Instantiate(agentPrefabs[2], position, Quaternion.identity);
            newAgent.name = "Wolf_Male_" + i;
        }
        for (int i = 0; i < femaleWolfStartingCount; i++)
        {
            position = GeneratePosition(femaleWolfStartingCount, AgentDensity, wolvesField);
            GameObject newAgent = Instantiate(agentPrefabs[3], position, Quaternion.identity);
            newAgent.name = "Wolf_Female_" + i;
        }
        for (int i = 0; i < treesStartingCount; i++)
        {
            position = GeneratePosition(treesStartingCount, 5, transform);
            GameObject newAgent = Instantiate(agentPrefabs[4], position, Quaternion.identity);
            newAgent.name = "Tree_" + i;
        }
        for (int i = 0; i < dirtStartingCount; i++)
        {
            position = GeneratePosition(dirtStartingCount, 5, transform);
            GameObject newAgent = Instantiate(agentPrefabs[5], position, Quaternion.identity);
            newAgent.name = "Dirt_" + i;
        }
        for (int i = 0; i < hayStartingCount; i++)
        {
            position = GeneratePosition(hayStartingCount, 5, transform);
            GameObject newAgent = Instantiate(agentPrefabs[6], position, Quaternion.identity);
            newAgent.name = "Hay_" + i;
        }
    }


    Vector3 GeneratePosition(float count, float density, Transform parent)
    {
        //generate random position using the given details
        //the count and density variables make sure the creatures don't spawn too close to each other
        Vector3 pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) * count * density + parent.position;
        NavMeshHit navHit;
        //check if the generated position is on the navmesh
        while(!NavMesh.SamplePosition(pos, out navHit, 0.1f, 1))
        {
            //if not, keep generating new ones
            pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) * count * density + parent.position;
        }
        return pos;
    }

}