using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DD.Cutscene
{
    public class Cutscene : MonoBehaviour
    {
        [field: SerializeField] public string CutsceneName { private set; get; }
        [field: SerializeField] public PlayableAsset CutsceneAsset { private set; get; }
        [field: SerializeField, SerializeReference] public List<CutsceneDependency> CutsceneDependencies { get; private set; } = new();

        [ContextMenu("Add Position Dependency")]
        private void AddPositionDependency()
        {
            CutsceneDependencies.Add(new ActorPositionDependency());
        }
    }
}
