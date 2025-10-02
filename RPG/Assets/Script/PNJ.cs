using UnityEngine;
using UnityEngine.UI;

public class PNJ : MonoBehaviour
{
    [SerializeField]
    string[] sentences;
    [SerializeField] 
    string characterName;
    int index;
    bool isOnDial, canDial;

    HUDManager manager => HUDManager.instance;

    public QuestSO quest; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isOnDial)
        {
            if (quest !=  null && quest.statut == QuestSO.Statut.none)
            {
                StartDialogue(quest.sentences);
            } 
            else if (quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount < quest.amountToFind)
            {
                StartDialogue(quest.InProgressSentence);
            }
            else if (quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount == quest.amountToFind)
            {
                StartDialogue(quest.completeSentence);
                quest.statut = QuestSO.Statut.complete;
            }
            else if (quest != null && quest.statut == QuestSO.Statut.complete)
            {
                StartDialogue(quest.afterQuestSentence);
            }
            else if (quest != null)
            {
                StartDialogue(sentences);
            }
           

        }
    }

    public void StartDialogue(string[] sentence)
    {
        manager.dialogueHolder.SetActive(true);
        PlayerController.instance.canMove = false;
        PlayerController.instance.canAttack = false;
        isOnDial = true;
        TypingText(sentence);
        manager.continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        manager.continueButton.GetComponent<Button>().onClick.AddListener(delegate { NextLine(sentence); });
    }

    void TypingText(string[] sentence)
    {
        manager.nameDisplay.text = "";
        manager.textDisplay.text = "";

        manager.nameDisplay.text = characterName;
        manager.textDisplay.text = sentence[index];

        if (manager.textDisplay.text == sentence[index])
        {
            manager.continueButton.SetActive(true);
        }
    }

    public void NextLine(string[] sentence)
    {
        manager.continueButton.SetActive(false);

        if (isOnDial && index < sentence.Length - 1)
        {
            index++;
            manager.textDisplay.text = "";
            TypingText(sentence);
        }
        else if (isOnDial && index == sentence.Length - 1)
        {
            isOnDial = false;
            index = 0;
            manager.nameDisplay.text = "";
            manager.textDisplay.text = "";
            manager.dialogueHolder.SetActive(false);

            PlayerController.instance.canMove = true;
            PlayerController.instance.canAttack = true;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isOnDial = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isOnDial = false;
        }
    }

}
