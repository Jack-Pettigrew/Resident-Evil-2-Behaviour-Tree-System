using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class CreditsCreator : MonoBehaviour
{
    [SerializeField] private string relativeFilePath;
    public string FilePath { get { return Application.dataPath + relativeFilePath; } }

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
        if (File.Exists(FilePath))
        {
            IEnumerable<string> lines = File.ReadLines(FilePath);

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
                    case 3:
                        Credit credit = Instantiate(creditPrefab, Vector3.zero, Quaternion.identity, creditsParent).GetComponent<Credit>();
                        credit.SetTitle(result[0]);
                        credit.SetAuthor(result[1]);
                        credit.SetLink(result[2]);
                        
                        creditList.Add(credit.gameObject);
                        break;
                        
                    default:
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
