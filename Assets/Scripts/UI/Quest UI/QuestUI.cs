using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace DD.UI
{
    public class QuestUI : MonoBehaviour
    {                
        [Header("Components")]
        [SerializeField] private RectTransform questUpdatePanel;
        [SerializeField] private TextMeshProUGUI questUIText;

        [Header("DoTween Values")]
        [SerializeField] private float questUpdateStartingPos = 0;
        [SerializeField] private float questUpdateTargetPosX = 0;
        [SerializeField] private float lengthToTargetPos = 1.0f;
        [SerializeField] private float objectiveUpdateDisplayTime = 3.0f;

        private Sequence questUpdateSequence;

        private Queue<Quest> questUpdateQueue = new Queue<Quest>();

        private void Start() {
            // Create DoTween sequence
            questUpdateSequence = DOTween.Sequence();
                                                
            questUpdateSequence
                .Append(questUpdatePanel.DOAnchorPosX(questUpdateTargetPosX, lengthToTargetPos).From(new Vector2(questUpdateStartingPos, questUpdatePanel.anchoredPosition.y)))
                .AppendInterval(objectiveUpdateDisplayTime)
                .Append(questUpdatePanel.DOAnchorPosX(questUpdateStartingPos, lengthToTargetPos).From(new Vector2(questUpdateTargetPosX, questUpdatePanel.anchoredPosition.y)))
                // Below append is a workaround for DOTween setting the start position as the last From() call (i'm definitely missing something here)
                .Append(questUpdatePanel.DOAnchorPos(new Vector2(questUpdateStartingPos, questUpdatePanel.anchoredPosition.y), 0).From(new Vector2(questUpdateStartingPos, questUpdatePanel.anchoredPosition.y)))
                .OnComplete(HandleQueue);
            
            Quest.Instance.OnQuestProgressed += HandleQuestUpdated;
        }

        private void HandleQuestUpdated(Quest progressedQuest)
        {
            Debug.Log(progressedQuest.CurrentObjective.ObjectiveTitle);
            
            questUpdateQueue.Enqueue(progressedQuest);

            if(!questUpdateSequence.IsPlaying() && questUpdateQueue.Count == 1)
            {
                HandleQueue();
            }
            
        }

        private void HandleQueue()
        {
            Debug.Log("Handling Queue");
            
            if(questUpdateQueue.TryDequeue(out Quest quest))
            {
                Debug.Log("Dequeued queue");
                
                questUIText.text = quest.CurrentObjective.ObjectiveTitle;
                questUpdateSequence.Restart();
            }
        }
    }
}
