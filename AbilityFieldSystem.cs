using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class AbilityFieldSystem : MonoBehaviour
{
    [Range(0.01f, 1f)]
    [SerializeField] float lineWidth = 0.1f;
    [Range(1f, 10f)]
    [SerializeField] float fieldRadius = 7f;
    [Range(3,50)]
    [SerializeField] int fieldNumSegments = 6;
    [SerializeField] Color32 flameColor = Color.red;
    [SerializeField] Color32 iceColor = Color.cyan;
    [SerializeField] Color32 electroColor = Color.yellow;

    bool flameOn, iceOn, electroOn;



    //TODO Put Particles 



    private void Start()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius = fieldRadius;
    }

    public void SetBuffsBools(bool flame, bool ice, bool electro)
    {
        flameOn = flame;
        iceOn = ice;
        electroOn = electro;

        if (flame) DoRenderer(flameColor);
        if (ice) DoRenderer(iceColor);
        if (electro) DoRenderer(electroColor);

    }

    public void DoRenderer(Color color)
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.positionCount = fieldNumSegments + 1;
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / fieldNumSegments;
        float theta = 0f;

        for (int i = 0; i <= fieldNumSegments; i++)
        {
            float x = fieldRadius * Mathf.Cos(theta);
            float z = fieldRadius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Health colHealth = other.GetComponent<Health>();

            if (flameOn) colHealth.SetBuffs(Buffs.Burned);
            if (iceOn) colHealth.SetBuffs(Buffs.Frozen);
            if (electroOn) return;//TODO Implement electro field mechanic
        }
    }
}
