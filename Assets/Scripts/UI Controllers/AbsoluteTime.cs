using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteTime : MonoBehaviour {

	[RangeAttribute(0, 5)]
    public float TimeScale = 1.0f;

	// Update is called once per frame
	void Update () {
        Time.timeScale = TimeScale;
	}

    public void Pause() { TimeScale = 0.0f; }
    public void Resume() { TimeScale = 1.0f; }
    public void Forward() { TimeScale = 3.0f; }
    public void FastForward() { TimeScale = 5.0f; }

}
