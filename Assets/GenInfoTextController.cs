using UnityEngine;
using UnityEngine.UI;

public class GenInfoTextController : MonoBehaviour
{

    public TimerController timer;

    GameObject g, sp, wp, saf, waf, shf, whf;

    
    float generation = 0;

    // Use this for initialization
    void Start()
    {
        g = GameObject.Find("G");
        sp = GameObject.Find("SP");
        wp = GameObject.Find("WP");
        saf = GameObject.Find("SAF");
        waf = GameObject.Find("WAF");
        shf = GameObject.Find("SHF");
        whf = GameObject.Find("WHF");
    }

    // Update is called once per frame
    void Update()
    {

        generation = (int)timer.timer / 10;


        g.GetComponent<Text>().text = generation.ToString();
        sp.GetComponent<Text>().text = Critter.crittersDict["Herbivore"].Count.ToString();
        wp.GetComponent<Text>().text = Critter.crittersDict["Carnivore"].Count.ToString();

        float averageFitness = 0;

        foreach (Critter c in Critter.crittersDict["Herbivore"])
            averageFitness += c.FitnessScore;
        averageFitness /= Critter.crittersDict["Herbivore"].Count;
        saf.GetComponent<Text>().text = averageFitness.ToString();

        averageFitness = 0;

        foreach (Critter c in Critter.crittersDict["Carnivore"])
            averageFitness += c.FitnessScore;
        averageFitness /= Critter.crittersDict["Carnivore"].Count;
        waf.GetComponent<Text>().text = averageFitness.ToString();

        shf.GetComponent<Text>().text = Critter.crittersDict["Herbivore"][0].FitnessScore.ToString();
        whf.GetComponent<Text>().text = Critter.crittersDict["Carnivore"][0].FitnessScore.ToString();

    }
}
