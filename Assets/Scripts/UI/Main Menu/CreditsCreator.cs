using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Text.RegularExpressions;

public class CreditsCreator : MonoBehaviour
{
    [SerializeField] private string resourceFileName;

    [SerializeField] private GameObject creditSectionTitlePrefab;
    [SerializeField] private GameObject creditPrefab;
    [SerializeField] private RectTransform creditsParent;

    private List<GameObject> creditList = new();

    [ContextMenu("Test Credits")]
    public void Test()
    {
        UpdateCredits();
    }

    public void UpdateCredits()
    {
        TextAsset textAsset = Resources.Load(resourceFileName, typeof(TextAsset)) as TextAsset;
        
        if (textAsset)
        {
            IEnumerable<string> lines = Regex.Split(textAsset.text, "\n|\r|\r\n");

            foreach (var line in lines)
            {
                string[] splitLine = line.Split('(', ')', '[', ']');
                List<string> result = new();

                foreach (var item in splitLine)
                {
                    if (item.Trim() != string.Empty)
                    {
                        result.Add(item);
                    }
                }
                
                if(result.Count == 0) continue;

                switch (result.Count)
                {
                    // Title
                    case 1:
                        if(result[0][0] == '#')
                        {
                            TextMeshProUGUI sectionTitle = Instantiate(creditSectionTitlePrefab, Vector3.zero, Quaternion.identity, creditsParent).GetComponent<TextMeshProUGUI>();
                            sectionTitle.text = result[0].Substring(1, result[0].Length - 1);
                            creditList.Add(sectionTitle.gameObject);
                        }
                        break;

                    // Credit                        
                    default:
                        Credit credit = Instantiate(creditPrefab, Vector3.zero, Quaternion.identity, creditsParent).GetComponent<Credit>();
                        credit.SetTitle(result[0]);
                        credit.SetAuthor(result[1]);

                        if(result.Count == 3)
                        {
                            credit.SetLink(result[2]);
                        }
                        
                        creditList.Add(credit.gameObject);
                        break;
                }
            }
        }
        else
        {
            Debug.Log("Nope that's wrong chief");
        }
    }

    public void RemoveCredits()
    {
        foreach (GameObject item in creditList)
        {
            Destroy(item);
        }

        creditList.Clear();
        creditList.TrimExcess();
    }
}
