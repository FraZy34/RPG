using UnityEngine;

[CreateAssetMenu (fileName = "Quest", menuName = "ScriptableObject/Quest", order = 0)]

public class QuestSO : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public string[] sentences, InProgressSentence, completeSentence, afterQuestSentence;
    public string objectTofind;
    public int actualAmount, amountToFind;
    public int goldToGive;

    [System.Serializable]

    public enum Statut
    {
        none,
        accepter,
        complete
    }

    public Statut statut;
}
