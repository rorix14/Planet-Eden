using UnityEngine.Playables;

namespace RPG.Cinamatics
{
    public class SubtitleBehaviour : PlayableBehaviour
    {
        public string subtitleText;

        //public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        //{
        //    TextMeshProUGUI text = (TextMeshProUGUI)playerData;
        //    text.text = subtitleText;
        //    text.color = new Color(1, 1, 1, info.weight);
        //}
    }
}

