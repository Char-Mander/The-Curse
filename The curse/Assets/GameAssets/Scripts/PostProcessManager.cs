using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    [SerializeField]
    List<PostProcessProfile> profiles = new List<PostProcessProfile>();
    PostProcessVolume volume;
    ColorGrading cgLayerDV = null;

    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        SetGeneralProfile();
    }

    public void SetGeneralProfile()
    {
        if (profiles.Count > 0) volume.profile = profiles[0];
    }

    public void SetDeathProfile()
    {
        if (profiles.Count > 1)
        {
            volume.profile = profiles[1];
            StartCoroutine(DeathEffect());
        }
    }

    IEnumerator DeathEffect()
    {
        if (cgLayerDV == null)
        {
            volume.profile.TryGetSettings(out cgLayerDV);
            cgLayerDV.saturation.value = 0;
        }
        if (cgLayerDV.saturation > -100)
        {
            cgLayerDV.saturation.value -= 2.5f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(DeathEffect());
        }
        else cgLayerDV = null;
    }
}
