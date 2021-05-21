using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Ch2_Quest5Manager : MonoBehaviour
{
    //Dialog Objects
    public GameObject Quest, DialogBox;
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image Portrait, Character;
    public GameObject ChoicesPack;
    public TextMeshProUGUI[] choices = new TextMeshProUGUI[5];
    public Sprite portraitImage;
    public Sprite characterPortrait;

    private int answerNumber, dialogtotalcnt;
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
        Portrait.sprite = portraitImage;
        Character.sprite = characterPortrait;
        //�̹��� ������ ����
        RectTransform rt = (RectTransform)Portrait.transform;
        rt.sizeDelta = new Vector2(0, 1243);
        Quest.SetActive(true);
        Portrait.gameObject.SetActive(false); //�ʱ⿣ �ڵ� �̹��� NOT show
        QuestInfo.Clear();

        foreach (QuestBase.Info info in db.QuestInfo)
        {
            QuestInfo.Enqueue(info);
        }
        dialogtotalcnt = QuestInfo.Count;
        answerNumber = Random.Range(0, 4); //����-�Ź� ���� ���� / ���� ��ȣ �ο�
        Debug.Log("������ "+answerNumber);

        setChoiceText();
        DequeueQuest();
    }

    private bool flag = true; //�⺻���� true

    public void DequeueQuest()
    {
        if (QuestInfo.Count.Equals(dialogtotalcnt))
        {
            Character.gameObject.SetActive(true);
        }
        else if(QuestInfo.Count.Equals(dialogtotalcnt-4))
        {
            Portrait.gameObject.SetActive(true);
        }
        else if (QuestInfo.Count.Equals(2)) //������
        {
            Character.gameObject.SetActive(false);
            DialogBox.SetActive(false);
            ChoicesPack.SetActive(true);
            return;
        }
        else if (QuestInfo.Count.Equals(0)) //Quest ���̾�α� ������
        {
            Character.gameObject.SetActive(false);
            QuestManager.instance.spinStar();
            Invoke("EndofQuest", 4.5f);
            return;
        }


        QuestBase.Info info = QuestInfo.Dequeue();
        dialogueName.text = info.myName;
        dialogueText.text = info.myText;
    }

    private string[] examples = new string[4]
        {"scores.item", "scores.item()", "scores.key()", "scores.keys()"};
    private string answer = "scores.items()";

    private void setChoiceText()
    {
        int j = 0;
        for (int i = 0; i < 5; i++)
        {
            if (i.Equals(answerNumber)) choices[i].text = answer;
            else choices[i].text = examples[j++]; //j<4
        }
    }

    public void chooseAnswer(int choiceNumber) //Trigger choice one
    {
        QuestManager.instance.startLoading(choiceNumber.Equals(answerNumber));

        //������ �ִϸ��̼�
        if (choiceNumber.Equals(answerNumber)) //���� ���� ���
        {
            Portrait.gameObject.SetActive(false);
            Character.gameObject.SetActive(true);
            QuestBase.Info info = QuestInfo.Dequeue();
            DialogBox.SetActive(true);
            dialogueName.text = info.myName;
            dialogueText.text = info.myText;
            ChoicesPack.gameObject.SetActive(false);
        }
        else
        {
            ChoicesPack.gameObject.SetActive(false);
            DialogBox.SetActive(true);
            dialogueName.text = "�����";
            dialogueText.text = "�߸��� �����ΰͰ���!";
            flag = false;
        }
    }

    private void EndofQuest()
    {
        Quest.SetActive(false);
        DialogueManager2.instance.Qcompleted[4] = true;
        (DialogueManager2.instance.DialogueBox).SetActive(true);
        DialogueManager2.instance.DequeueDialogue();
    }
}
