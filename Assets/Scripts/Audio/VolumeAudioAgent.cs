using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class VolumeAudioAgent : AudioAgent
{
    protected override void Update()
    {
        AgentSEVolume = Mathf.Clamp(GameManager.instance.CalculateVolumeModifier(transform.position), 0.0f, 1.0f);
        AgentBGVolume = AgentSEVolume;

        base.Update();
    }
}
