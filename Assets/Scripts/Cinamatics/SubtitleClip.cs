using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinamatics
{
    public class SubtitleClip : PlayableAsset
    {
        [TextArea(1, 3)]
        public string subtitleText;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<SubtitleBehaviour> playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);

            SubtitleBehaviour subtitleBehaviour = playable.GetBehaviour();
            subtitleBehaviour.subtitleText = subtitleText;

            return playable;
        }
    }
}

