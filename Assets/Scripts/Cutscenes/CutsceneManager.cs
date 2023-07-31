using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DD.Cutscene
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CutsceneManager : MonoBehaviour
    {
        private static PlayableDirector director;
        
        private void Awake() {
            director = GetComponent<PlayableDirector>();
        }
        
        public static void ReadyCutsceneDependencies(Cutscene cutscene)
        {
            foreach (CutsceneDependency dependency in cutscene.CutsceneDependencies)
            {
                dependency.Setup();
            }
        }

        public static void PlayCutscene(Cutscene cutscene)
        {
            ReadyCutsceneDependencies(cutscene);
            director.Play(cutscene.CutsceneAsset);
        }
    }

    [Serializable]
    public abstract class CutsceneDependency
    {
        public abstract bool Setup();
    }


    [Serializable]
    public class ActorPositionDependency : CutsceneDependency
    {
        public GameObject actor;
        public Transform positionTransform;
        
        public ActorPositionDependency() {}
        
        public ActorPositionDependency(GameObject actor, Transform positionTransform)
        {
            this.actor = actor;
            this.positionTransform = positionTransform;
        }
        
        public override bool Setup()
        {
            actor.transform.position = positionTransform.position;
            actor.transform.rotation = positionTransform.rotation;
            return true;
        }
    }
}
