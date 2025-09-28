using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Sprites de vie")]
    [SerializeField] private Sprite emptyPoint;
    [SerializeField] private Sprite fullPoint;

    [Header("UI - Images de vie")]
    [SerializeField] private Image life1;
    [SerializeField] private Image life2;
    [SerializeField] private Image life3;

    void Update()
    {
        switch (PlayerController.instance.currentHealth)
        {
            case 0:
                life1.sprite = emptyPoint;
                life2.sprite = emptyPoint;
                life3.sprite = emptyPoint;
                break;
            case 1:
                life1.sprite = fullPoint;
                life2.sprite = emptyPoint;
                life3.sprite = emptyPoint;
                break;
            case 2:
                life1.sprite = fullPoint;
                life2.sprite = fullPoint;
                life3.sprite = emptyPoint;
                break;
            case 3:
                life1.sprite = fullPoint;
                life2.sprite = fullPoint;
                life3.sprite = fullPoint;
                break;
        }
    }
}

