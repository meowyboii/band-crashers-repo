using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public GameObject floatingTextPrefab;


    private List<float> allScores = new List<float>();

    [SerializeField] 
    private PitchVisualizer pitchVisualizer;




    public void ShowFloatingText(string score)
    {
        GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
        prefab.GetComponent<TextMesh>().text = score;
    }



    public float CalculateScore()
    {
        List<float> list1 = pitchVisualizer.GetOriginalFrequencies();
        List<float> list2 = pitchVisualizer.GetPlayerFrequencies();

        float sum = 0;
        int total = (list1.Count > list2.Count) ? list2.Count : list1.Count;
        Debug.Log("COUNT LIST 1: " + list1.Count);
        Debug.Log("COUNT LIST 2: " + list2.Count);

        string list = "";
        for (int i=0; i<list1.Count; i++){
            list += list1[i].ToString() + ", ";
        }
        list = "";
        Debug.Log("ORIGINAL: " + list);
        for (int i=0; i<list2.Count; i++){
            list += list2[i].ToString() + ", ";
        }
        Debug.Log("PLAYER: " + list);


        for (int i = 0; i < total; i++)
        {
            float penalty;
            if (list1[i] == 0)
            {
                penalty = 0;
            }
            else if (list1[i] != 0 && list2[i] == 0)
            {
                penalty = 100;
            }
            else
            {
                penalty = Mathf.Abs(list1[i] - list2[i]);
            }
            sum += penalty;
        }
        // Calculate the average of the absolute differences
        float average = sum / (float)total;
        if (average <= 5)
        {
            allScores.Add(100);
            return 100;
        }

        float score = 100 - (average - 5);

        if (score < 0)
        {
            score = 0;
        }

        //Add to the list of all scores
        allScores.Add(score);

        return Mathf.RoundToInt(score);

    }


    public float GetAverageAccuracy()
    {
        return Mathf.Round(allScores.Average() * 100f) / 100f;
    }
}
