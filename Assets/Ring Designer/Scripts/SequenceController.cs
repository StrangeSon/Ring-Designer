using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

namespace RingDesigner
{
    public class SequenceController : MonoBehaviour
    {
        public PlayableDirector OpeningPlayable;
        public PlayableDirector CinematicPlayable;
        public CameraController OrbitCameraController;

        public void Awake()
        {
            OpeningPlayable.Play();
            OpeningPlayable.played += OpeningPlayable_played;
            CinematicPlayable.played += CinematicPlayable_played;
            CinematicPlayable.stopped += CinematicPlayable_stopped;
        }

        public void OnDestroy()
        {
            OpeningPlayable.played -= OpeningPlayable_played;
            CinematicPlayable.played -= CinematicPlayable_played;
            CinematicPlayable.stopped -= CinematicPlayable_stopped;
        }

        private void OpeningPlayable_played(PlayableDirector obj)
        {
            OrbitCameraController.Reinitialize();
        }

        private void CinematicPlayable_stopped(PlayableDirector obj)
        {
            OpeningPlayable.Play();
        }

        private void CinematicPlayable_played(PlayableDirector obj)
        {
            
        }

        public void Play()
        {
            if (CinematicPlayable.state != PlayState.Playing && OpeningPlayable.state != PlayState.Playing)
                CinematicPlayable.Play();
        }
    }
}