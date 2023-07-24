using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CreditsCreator : MonoBehaviour
{
    [SerializeField] private string relativeFilePath;
    public string FilePath { get { return Application.dataPath + relativeFilePath; } }

    [SerializeField] private GameObject creditPrefab;
    [SerializeField] private RectTransform creditsParent;

    private List<Credit> creditList = new();

    [ContextMenu("Test Credits")]
    public void Test()
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

                if (result.Count == 3)
                {
                    Credit credit = Instantiate(creditPrefab, Vector3.zero, Quaternion.identity, creditsParent).GetComponent<Credit>();
                    credit.SetTitle(result[0]);
                    credit.SetAuthor(result[1]);

                    if (credit.TryGetComponent<ClickableUI>(out ClickableUI clickableUI))
                    {
                        clickableUI.OnClick.AddListener(() => URLOpener.OpenURL(result[2]));
                    }

                    creditList.Add(credit);
                }
            }
        }
        else
        {
            Debug.Log("Nope that's wrong chief");
        }
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

                if (result.Count == 3)
                {
                    Credit credit = Instantiate(creditPrefab, Vector3.zero, Quaternion.identity, creditsParent).GetComponent<Credit>();
                    credit.SetTitle(result[0]);
                    credit.SetAuthor(result[1]);

                    if (credit.TryGetComponent<ClickableUI>(out ClickableUI clickableUI))
                    {
                        clickableUI.OnClick.AddListener(() => URLOpener.OpenURL(result[2]));
                    }

                    creditList.Add(credit);
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
        foreach (Credit item in creditList)
        {
            Destroy(item.gameObject);
        }

        creditList.Clear();
        creditList.TrimExcess();
    }
}
