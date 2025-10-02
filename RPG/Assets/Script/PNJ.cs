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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isOnDial)
        {
            StartDialogue();
            manager.continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
            manager.continueButton.GetComponent<Button>().onClick.AddListener(delegate { NextLine();  });
        }
    }

    public void StartDialogue()
    {
        manager.dialogueHolder.SetActive(true);
        PlayerController.instance.canMove = false;
        PlayerController.instance.canAttack = false;
        isOnDial = true;
        TypingText(sentences);
    }

    void TypingText(string[] sentences)
    {
        manager.nameDisplay.text = "";
        manager.textDisplay.text = "";

        manager.nameDisplay.text = characterName;
        manager.textDisplay.text = sentences[index];

        if (manager.textDisplay.text == sentences[index])
        {
            manager.continueButton.SetActive(true);
        }
    }

    public void NextLine()
    {
        manager.continueButton.SetActive(false);

        if (isOnDial && index < sentences.Length - 1)
        {
            index++;
            manager.textDisplay.text = "";
            TypingText(sentences);
        }
        else if (isOnDial && index == sentences.Length - 1)
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
