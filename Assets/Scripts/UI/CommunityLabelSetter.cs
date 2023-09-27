using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CommunityLabelSetter : MonoBehaviour
{
    [SerializeField] private string resourceFileName;

    private void Awake()
    {
        SetDeskLabels();
    }

    private void SetDeskLabels()
    {
        CommunityLabel[] labels = FindObjectsOfType<CommunityLabel>(true);

        if(labels.Length == 0) return;
        
        TextAsset textAsset = Resources.Load(resourceFileName, typeof(TextAsset)) as TextAsset;

        if(textAsset)
        {
            string[] lines = Regex.Split(textAsset.text, "\n");

            for (int labelIndex = 0; labelIndex < labels.Length; labelIndex++)
            {
                if (labelIndex > lines.Length - 1) break;

                labels[labelIndex].SetLabel(lines[labelIndex]);
            }
        }
    }
}