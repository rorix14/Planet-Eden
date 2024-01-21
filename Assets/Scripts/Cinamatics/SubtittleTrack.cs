using UnityEngine;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.Playables;

namespace RPG.Cinamatics
{
    [TrackBindingType(typeof(TextMeshProUGUI))]
    [TrackClipType(typeof(SubtitleClip))]
    public class SubtittleTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SubtitleTrackMixer>.Create(graph, inputCount);
        }
    }
}
