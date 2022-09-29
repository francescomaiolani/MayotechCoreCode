using System;
using UnityEngine;

[Serializable]
public class ResolutionMatch
{
    [SerializeField] private Vector2 resolution;
    [SerializeField, Range(0F, 1F)] private float matching;

    public Vector2 Resolution => resolution;
    public float Matching => matching;
    public float AspectRatio => resolution.x / resolution.y;

    public ResolutionMatch(Vector2 resolution, float matching)
    {
        this.resolution = resolution;
        this.matching = matching;
    }
}