using UnityEngine;

public class CorrectEffect : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Play(Vector3 position)
    {
        transform.position = position;
        ps.Play();
    }
}