using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InitialPopulationGenerator : MonoBehaviour
{
    [Range(0, 500)] public int maleSheepStartingCount = 10;
    [Range(0, 500)] public int femaleSheepStartingCount = 10;
    [Range(0, 500)] public int maleWolfStartingCount = 10;
    [Range(0, 500)] public int femaleWolfStartingCount = 10;
    [Range(0, 500)] public int treesStartingCount = 30;
    [Range(0, 500)] public int dirtStartingCount = 30;
    [Range(0, 500)] public int hayStartingCount = 30;

    public List<GameObject> agentPrefabs;

    const float AgentDensity = 1f;

    public Transform sheepField;
    public Transform wolvesField;

    Vector3 position;

    private void Start()
    {
        for (int i = 0; i < maleSheepStartingCount; i++)
        {
            position = GeneratePosition(maleSheepStartingCount, AgentDensity, sheepField);
            GameObject newAgent = Instantiate(agentPrefabs[0], position, Quaternion.identity);
            newAgent.name = "Sheep_Male_" + i;
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
        Vector3 pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) * count * density + parent.position;
        NavMeshHit navHit;
        while(!NavMesh.SamplePosition(pos, out navHit, 1, 1))
        {
            pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) * count * density + parent.position;
        }
        return pos;
    }

}