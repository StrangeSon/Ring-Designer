using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

namespace RingDesigner
{
    public class SequenceController : MonoBehaviour
    {
        public PlayableDirector PlayableDirector;

        public void OnPlay()
        {
            PlayableDirector.Play();
        }
    }
}