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
        AgentSEVolume = GameManager.instance.CalculateVolumeModifier(transform.position);
        AgentBGVolume = AgentSEVolume;

        base.Update();
    }
}
