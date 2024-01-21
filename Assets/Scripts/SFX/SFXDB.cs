using UnityEngine;

namespace RPG.SFX
{
    [CreateAssetMenu(fileName = "SFX", menuName = "SFX/Make new SFX database")]
    public class SFXDB : ScriptableObject
    {
        [Header("Human Hurt")]
        [SerializeField] private AudioClip[] hurtAudioClips;
        [SerializeField] [Range(0, 1)] private float hurtVolume = 0.25f;

        [Header("Human Dead")]
        [SerializeField] private AudioClip[] deadAudioClips;
        [SerializeField] [Range(0, 1)] private float deadVolume = 0.25f;

        [Header("Alien Hurt")]
        [SerializeField] private AudioClip[] alienHurtAudioClips;
        [SerializeField] [Range(0, 1)] private float alienHurtVolume = 0.25f;

        [Header("Alien Dead")]
        [SerializeField] private AudioClip[] alienDeadAudioClips;
        [SerializeField] [Range(0, 1)] private float alineDeadVolume = 0.25f;

        public AudioClip[] HurtAudioClips => hurtAudioClips;
        public AudioClip[] DeadAudioClips => deadAudioClips;
        public AudioClip[] AlienHurtAudioClips => alienHurtAudioClips;
        public AudioClip[] AlienDeadAudioClips => alienDeadAudioClips;
    }
}

