using System;

[Serializable]
public class ScoreData
{
    public string nickName;
    public int score;

    public ScoreData(string nickName, int score)
    {
        this.nickName = nickName;
        this.score = score;
    }
}
