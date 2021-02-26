using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Ch2_Quest5Manager : MonoBehaviour
{
    //Dialog Objects
    public GameObject QuestDialogBox;
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image Portrait, Character;
    public Sprite[] portraitImages = new Sprite[2];
    public Sprite characterPortrait;

    private int dialogtotalcnt;
    public Queue<QuestBase.Info> QuestInfo;

    public static Ch2_Quest5Manager instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("fix this" + gameObject.name);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        QuestInfo = new Queue<QuestBase.Info>();  //�ʱ�ȭ
    }

    public void EnqueueQuest(QuestBase db)
    {
        Portrait.sprite = portraitImages[0];
        Character.sprite = characterPortrait;
        //�̹��� ������ ����
        RectTransform rt = (RectTransform)Portrait.transform;
        rt.sizeDelta = new Vector2(0, 1243);
        QuestDialogBox.SetActive(true);
        Portrait.gameObject.SetActive(false); //�ʱ⿣ �ڵ� �̹��� NOT show
        QuestInfo.Clear();

        foreach (QuestBase.Info info in db.QuestInfo)
        {
            QuestInfo.Enqueue(info);
        }
        dialogtotalcnt = QuestInfo.Count;

        DequeueQuest();
    }

    private bool flag = true; //�⺻���� true

    public void DequeueQuest()
    {
        if (QuestInfo.Count == dialogtotalcnt - 1)
        {
            Portrait.gameObject.SetActive(true);
        }

        QuestBase.Info info = QuestInfo.Dequeue();
        dialogueName.text = info.myName;
        dialogueText.text = info.myText;
    }
}
