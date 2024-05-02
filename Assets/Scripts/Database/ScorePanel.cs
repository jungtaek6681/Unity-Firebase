using Firebase.Database;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] TMP_Text scoreBoardText;

    [SerializeField] TMP_InputField nickNameInputField;
    [SerializeField] TMP_InputField scoreInputField;

    [SerializeField] Button addButton;
    [SerializeField] Button resetButton;

    public const string ScoreBoardRef = "ScoreBoard";

    private void Awake()
    {
        addButton.onClick.AddListener(AddScore);
        resetButton.onClick.AddListener(ResetScore);
    }

    private void Start()
    {
        FirebaseManager.DB
            .GetReference(ScoreBoardRef)
            .OrderByChild("score")
            .LimitToLast(5)
            .ValueChanged += HighScoreValueChanged;
    }

    private void HighScoreValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        // Do something with the data in args.Snapshot
        StringBuilder sb = new StringBuilder();
        DataSnapshot snapshot = args.Snapshot;
        List<DataSnapshot> children = new List<DataSnapshot>(snapshot.Children);
        children.Reverse();
        for (int i = 0; i < children.Count; i++)
        {
            string json = children[i].GetRawJsonValue();
            ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json);
            sb.AppendLine($"{i + 1}. {scoreData.nickName} - {scoreData.score}");
        }

        scoreBoardText.text = sb.ToString();
    }

    private string nickName;
    private int score;
    private void AddScore()
    {
        nickName = nickNameInputField.text;
        score = int.Parse(scoreInputField.text);

        FirebaseManager.DB
            .GetReference(ScoreBoardRef)
            .RunTransaction(AddScoreTransaction);
    }

    TransactionResult AddScoreTransaction(MutableData mutableData)
    {
        List<object> scoreboard = mutableData.Value as List<object>;
        if (scoreboard == null)
        {
            scoreboard = new List<object>();
        }
        else if (mutableData.ChildrenCount >= 5)
        {
            // If the current list of scores is greater or equal to our maximum allowed number,
            // we see if the new score should be added and remove the lowest existing score.
            long minScore = long.MaxValue;
            object minVal = null;
            foreach (var child in scoreboard)
            {
                if (!(child is Dictionary<string, object>))
                    continue;
                long childScore = (long)((Dictionary<string, object>)child)["score"];
                if (childScore < minScore)
                {
                    minScore = childScore;
                    minVal = child;
                }
            }
            // If the new score is lower than the current minimum, we abort.
            if (minScore > score)
            {
                return TransactionResult.Abort();
            }
            // Otherwise, we remove the current lowest to be replaced with the new score.
            scoreboard.Remove(minVal);
        }

        // Now we add the new score as a new entry that contains the email address and score.
        Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
        newScoreMap["score"] = score;
        newScoreMap["nickName"] = nickName;
        scoreboard.Add(newScoreMap);

        // You must set the Value to indicate data at that location has changed.
        mutableData.Value = scoreboard;
        return TransactionResult.Success(mutableData);
    }

    private void ResetScore()
    {
        FirebaseManager.DB
            .GetReference(ScoreBoardRef)
            .RemoveValueAsync();
    }
}

