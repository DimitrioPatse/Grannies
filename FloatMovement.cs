using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMovement : MonoBehaviour
{
    [SerializeField] float maxDistanceFromPlayer = 8f;
    [SerializeField] float checkEverySeconds = 2f;
    [SerializeField] AnimationCurve followCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    [SerializeField] float curveDuration = 0.5f;

    [SerializeField] bool animPos = true;
    [SerializeField] Vector3 posAmplitude = Vector3.one;
    [SerializeField] Vector3 posSpeed = Vector3.one;

    [SerializeField] bool animRot = true;
    [SerializeField] Vector3 rotAmplitude = Vector3.one * 20;
    [SerializeField] Vector3 rotSpeed = Vector3.one;

    [SerializeField] bool animScale = false;
    [SerializeField] Vector3 scaleAmplitude = Vector3.one * 0.1f;
    [SerializeField] Vector3 scaleSpeed = Vector3.one;

    Vector3 pos;
    Vector3 rot;
    Vector3 origScale;
    Vector3 dir;

    GameObject player;
    float startAnimOffset = 0;
    float timeSinceLastCheck = Mathf.Infinity;
    float distFromPlayer;

    void Awake()
    {
        origScale = transform.localScale;
        startAnimOffset = Random.Range(0f, 540f);        // so that the xyz anims are already offset from each other since the start
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        SinePos();
        SineRot();
        SineScale();

        timeSinceLastCheck += Time.deltaTime;

        if(timeSinceLastCheck > checkEverySeconds)
        {
            distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
            if(distFromPlayer > maxDistanceFromPlayer)
            {
                StartCoroutine(FollowPlayer());
            }
            timeSinceLastCheck = 0f;
        }

        Move(pos);
    }

    IEnumerator FollowPlayer()
    {
        while (Vector3.Distance(player.transform.position, transform.position) > maxDistanceFromPlayer/4)
        {
            dir = new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z);
            Move(dir);
            yield return null;
        }
    }
    public void Move(Vector3 direction)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rot), 1f);
        transform.position = Vector3.Lerp(transform.position, transform.position + direction, followCurve.Evaluate(Time.deltaTime / curveDuration));
    }
    void SinePos()
    {
        if (animPos)
        {
            pos.x = posAmplitude.x * Mathf.Sin(posSpeed.x * Time.time + startAnimOffset);
            pos.y = posAmplitude.y * Mathf.Sin(posSpeed.y * Time.time + startAnimOffset);
            pos.z = posAmplitude.z * Mathf.Sin(posSpeed.z * Time.time + startAnimOffset);
        }
    }
    void SineRot()
    {
        if (animRot)
        {
            rot.x = rotAmplitude.x * Mathf.Sin(rotSpeed.x * Time.time + startAnimOffset);
            rot.y = rotAmplitude.y * Mathf.Sin(rotSpeed.y * Time.time + startAnimOffset);
            rot.z = rotAmplitude.z * Mathf.Sin(rotSpeed.z * Time.time + startAnimOffset);
        }
    }
    void SineScale()
    {
        if (animScale)
        {
            Vector3 scale;
            scale.x = origScale.x * (1 + scaleAmplitude.x * Mathf.Sin(scaleSpeed.x * Time.time + startAnimOffset));
            scale.y = origScale.y * (1 + scaleAmplitude.y * Mathf.Sin(scaleSpeed.y * Time.time + startAnimOffset));
            scale.z = origScale.z * (1 + scaleAmplitude.z * Mathf.Sin(scaleSpeed.z * Time.time + startAnimOffset));
            transform.localScale = scale;
        }
    }
}