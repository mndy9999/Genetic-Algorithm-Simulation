using System.Collections.Generic;
using UnityEngine;

public class InitialPopulationGenerator : MonoBehaviour
{
    [Range(0, 500)] public int maleSheepStartingCount = 10;
    [Range(0, 500)] public int femaleSheepStartingCount = 10;
    [Range(0, 500)] public int maleWolfStartingCount = 10;
    [Range(0, 500)] public int femaleWolfStartingCount = 10;

    public List<GameObject> agentPrefabs;

    const float AgentDensity = 1f;

    public Transform sheepField;
    public Transform wolvesField;

    private void Start()
    {
        for (int i = 0; i < maleSheepStartingCount; i++)
        {
            GameObject newAgent = Instantiate(agentPrefabs[0], (new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) * maleSheepStartingCount * AgentDensity) + sheepField.position, Quaternion.identity);
            newAgent.name = "Sheep_Male_" + i;
        }
        for (int i = 0; i < femaleSheepStartingCount; i++)
        {
            GameObject newAgent = Instantiate(agentPrefabs[1], (new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) * femaleSheepStartingCount * AgentDensity) + sheepField.position, Quaternion.identity);
            newAgent.name = "Sheep_Female_" + i;
        }
        for (int i = 0; i < maleWolfStartingCount; i++)
        {
            GameObject newAgent = Instantiate(agentPrefabs[2], (new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) * maleWolfStartingCount * AgentDensity) + wolvesField.position, Quaternion.identity);
            newAgent.name = "Wolf_Male_" + i;
        }
        for (int i = 0; i < femaleWolfStartingCount; i++)
        {
            GameObject newAgent = Instantiate(agentPrefabs[3], (new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) * femaleWolfStartingCount * AgentDensity) + wolvesField.position, Quaternion.identity);
            newAgent.name = "Wolf_Female_" + i;
        }
    }

}