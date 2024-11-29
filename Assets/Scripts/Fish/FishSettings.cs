using UnityEngine;

[CreateAssetMenu(fileName = "NewFishSettings", menuName = "Fish/FishSettings")]
public class FishSettings : ScriptableObject
{
    [Header("Fish Properties")]
    public int points; // Points awarded when the fish is caught
    public int requiredClicks; // Clicks needed to kill the fish

    [Header("Particles")]
    public string clickParticle; // Name of particle effect on click
    public string catchParticle; // Name of particle effect on catch


    [Header("SoundEffects")]
    public SoundEffectType clickSound; // Name of sound effect on click
    public SoundEffectType catchSound; // Name of particle effect on catch

    [Header("Movement Settings")]
    public MovementSettings movementSettings;

    [System.Serializable]
    public struct MovementSettings
    {
        public float minFishSpeed;
        public float maxFishSpeed;
        public float bottomOffset;
        public float topOffset;
        public float floatAmount;
        public float floatDuration;
    }
}
