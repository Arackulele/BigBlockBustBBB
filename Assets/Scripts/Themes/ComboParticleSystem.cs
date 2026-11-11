using System;
using UnityEngine;

public class ComboParticleSystem : MonoBehaviour
{
    private ParticleSystem particles;

    void Start()
    {
        particles = this.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        //ToDo: Maybe the color doesnt need to update every frame but the expensive of that is marginal so it shouldn't matter
        particles.startColor = ThemerScript.Instance.CurrentTheme.TotalScoreTextColor;

        if (ScoreManagement.Instance.Combo == 1)
        {
            particles.Stop();
            Debug.Log("Stopped Particles");
        }
        else 
        {
            if (!particles.isPlaying) particles.Play();
            var em = particles.emission;
            em.rateOverTime = ScoreManagement.Instance.Combo / 2 ;
            var ma = particles.main;
            ma.startSpeed = new ParticleSystem.MinMaxCurve(1f + (ScoreManagement.Instance.Combo/10f), 2f + (ScoreManagement.Instance.Combo/6f));
        }
    }
}
