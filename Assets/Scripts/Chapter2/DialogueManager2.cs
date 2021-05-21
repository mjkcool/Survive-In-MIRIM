using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class DialogueManager2 : MonoBehaviour
{
    public static DialogueManager2 instance;
    private void Awake()
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

    public static string UserName = "User";
    public AudioClip crowdshoutSound; //�������
    public AudioClip outcrowdSound;
    public AudioClip wheeSound;
    public AudioClip twothreeSound;
    public AudioClip jumpropeSound;
    public AudioClip duguduguSound;

   public Sprite[] bg = new Sprite[13]; //����̹���

    public GameObject DialogueBox;
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image dialoguePortrait;
    public Image backgroundPortrait;
    public float delay = 2f;
    public QuestStarter2 questStarter;
    public DialogueButton2 DialogBtn;

    public int department;

    private string[] major = { "����Ʈ", "����", "������"};
    private string[] sub = { "", "���� ���� ���֢�", "" };
    private string[] F1 = { "�þ���", "������", "������" };
    private string[] F2 = { "������", "���츲", "����" };
    private string[] F3 = { "�����", "���ڹ�", "�����" };
    private string[] F4 = { "�����", "������", "�����" };
    private string[] T2 = { "������ ������", "������ ������", "���Ͼ� ������" };
    private string[] T2_1 = { "�������� ������ ��ٷ�. �� �������ǰž�.", "�������� ������ ��ٷ�. �� �������ǰž�.", "��, ���� �� �𸣰ڳ�. ���弱������ ��� ��� ���̳���." };
    private string[] T2_2 = { "���� �� �� �԰��! ���� ������� ���� �� ���� ���ٿ�. �̵� ����ߵǴµ�.", "���� �� �� �԰��! ���� ������� ���� �� ���� ���ٿ�. �̵� ����ߵǴµ�.", "��ħ�� �� �Ծ���? ���ð��� ���� �� �Ծ����~" };
    private string[] text1 = { "�;ƾƾ�!! ��~�̰� �ι̰� �ְ� �ι̰�~!", "�;ƾ�!!!! ����!!", "�����! ��־־ֹ�!!" };
    private string[] text2 = { "�װ��� ����� ���� ���߱���������. �������� ��", "����! ����! �ַ�!! ���� ����!", "�;ƾ�!!!! 1�� �Դ� �������� è�Ǿ�!!!" };
    private string[] text3 = { "���� �Ф�", "����! ������ ������ ����~ �츮�� ������ ��ӵȴ�!", "�̾�~ ���� ��ɸǵ�! ����� ���ϸ� ��ϳ�~!" };
    private string[] text4 = { "�� ������ȭ�Ф� �����Ҿ� �ְ� �ι̰�~!", "���� �׾�� �� ������ �����ֳפ���", "���� �� ������ �ִ�!" };
    private string[] text5 = { "�������� �����̳�, �̺� ��!", "������ �̺����� �޷�! ������ �԰� ����!!", "��ü�� ȸ�� ����~~~!!!" };

    public Sprite[] F1_1, F1_2, F1_3, F1_4, F1_5;
    public Sprite[] F2_1, F2_2, F2_3;
    public Sprite[] F3_1, F3_2, F4_1, F4_2;
    public Sprite[] T1;
    public Sprite empty;

    public bool isCurrentlyTyping;
    private string completeText, name;
    public int thisId;
    private bool isDelayturn;
    public GameObject MedalAnimation;
    public GameObject endingAnimation;
    public GameObject MedalGround;

    public Queue<DialogueBase.Info> dialogueInfo;
    public Queue<PrologueBase.Info> prologueInfo;

    public bool[] Qcompleted = new bool[5];
    private AudioSource audio; //����� ����� �ҽ� ������Ʈ

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void EnqueueDialogue(DialogueBase db)
    {
        dialogueInfo = new Queue<DialogueBase.Info>();  //���̾�α� �ʱ�ȭ
        DialogueBox.SetActive(true); //ȭ�鿡 ���
        dialogueInfo.Clear();

        foreach (DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }
        isDelayturn = false;
        DequeueDialogue();
    }

    //���̺�� thisId�����Ͱ� ����Ʈ�κ��϶�
    public void QuestDialogue(DialogueBase db)
    {
        dialogueInfo = new Queue<DialogueBase.Info>();
        foreach (DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }
        for (int i = 0; i < thisId; i++)
        {
            dialogueInfo.Dequeue(); //thisId���� ���� ���� thisId ����
        }
        DequeueDialogue();
        DialogueBox.SetActive(false);
    }

    //���̺�� thisId������ �ε�
    public void LoadDialogue(DialogueBase db)
    {
        dialogueInfo = new Queue<DialogueBase.Info>();
        DialogueBox.SetActive(true); //ȭ�鿡 ���
        foreach (DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }
        for (int i = 0; i < thisId; i++)
        {
            dialogueInfo.Dequeue(); //thisId���� ���� ���� thisId ����
        }
        DequeueDialogue();
    }

    public void DequeueDialogue()
    {
        if (!isCurrentlyTyping) //�� ��� Ÿ������ ������ ����
        {
            if (dialogueInfo.Count.Equals(0)) //é�� 2 ����
            {
                DialogueBox.SetActive(false);
                EndofDialogue();

            }
            else
            { //���̾�α� ����
                if (isDelayturn)
                {
                    delayDialog(); return;
                }

                DialogueBox.SetActive(true);

                lock (dialogueInfo)
                {
                    if ((thisId.Equals(13)) && (!Qcompleted[0])) //����Ʈ 1 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 1;
                        questStarter.questnum = 1;
                        questStarter.start();
                    }
                    else if ((thisId.Equals(29)) && (!Qcompleted[1])) //����Ʈ 2 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 2;
                        questStarter.questnum = 2;
                        questStarter.start();
                    }
                    else if ((thisId.Equals(45)) && (!Qcompleted[2])) //����Ʈ 3 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 3;
                        questStarter.questnum = 3;
                        questStarter.start();
                    }
                    else if ((thisId.Equals(87)) && (!Qcompleted[3])) //����Ʈ 4 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 4;
                        questStarter.questnum = 4;
                        questStarter.start();
                    }
                    else if ((thisId.Equals(102)) && (!Qcompleted[4])) //����Ʈ 5 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 5;
                        questStarter.questnum = 5;
                        questStarter.start();
                    }

                    dialogueText.text = "";

                    DialogueBase.Info info = dialogueInfo.Dequeue();
                    thisId = info.id;
                    completeText = info.myText;
                    name = info.myName;
                    completeText = completeText.Replace("[User]", UserName);
                    name = name.Replace("[User]", UserName);
                    int department = PlayerPrefs.GetInt("Department");

                    //���� ���� �̸��� ����
                    completeText = completeText.Replace("[F1]", F1[department]);
                    name = name.Replace("[F1]", F1[department]);
                    completeText = completeText.Replace("[F2]", F2[department]);
                    name = name.Replace("[F2]", F2[department]);
                    completeText = completeText.Replace("[F3]", F3[department]);
                    name = name.Replace("[F3]", F3[department]);
                    completeText = completeText.Replace("[F4]", F4[department]);
                    name = name.Replace("[F4]", F4[department]);
                    name = name.Replace("[T2]", T2[department]);
                    completeText = completeText.Replace("[T2_1]", T2_1[department]);
                    completeText = completeText.Replace("[T2_2]", T2_2[department]);
                    completeText = completeText.Replace("[text1]", text1[department]);
                    completeText = completeText.Replace("[text2]", text2[department]);
                    completeText = completeText.Replace("[text3]", text3[department]);
                    completeText = completeText.Replace("[text4]", text4[department]);
                    completeText = completeText.Replace("[text5]", text5[department]);
                    completeText = completeText.Replace("[major]", major[department]);
                    completeText = completeText.Replace("[sub]", sub[department]);

                    dialogueName.text = name;

                    Sprite p = empty;
                    int other = 0;
                    if (department >= 3) other = department - 1;
                    else other = department + 1;

                    //Array.Exists(language, element => element == "Ruby")
                    //�ι� portrait
                    if ((new int[] { 0, 1, 106 }).Contains(thisId)) p = F1_2[department];
                    else if ((new int[] { 2, 3, 4, 16, 79 }).Contains(thisId)) p = F1_4[department];
                    else if ((new int[] { 21, 23, 92 }).Contains(thisId)) p = F4_1[department];
                    else if ((new int[] { 35, 48, 48, 49, 50, 38, 43, 45, 99 }).Contains(thisId)) p = F3_1[department];
                    //else if ((new int[] { }))
                    else if ((new int[] { 58, 80, 85, 87 }).Contains(thisId)) p = F2_2[department];
                    else if ((new int[] { 62, 66, 75 }).Contains(thisId)) p = F1_1[department];
                    else if ((new int[] { 63, 77, 91, 114 }).Contains(thisId)) p = F2_1[department];
                    else if ((new int[] { 7, 9 }).Contains(thisId)) p = T1[department];
                    else if ((new int[] { 94, 109, 113 }).Contains(thisId)) p = F4_2[department];
                    else if ((new int[] { 99, 115, 117 }).Contains(thisId)) p = F1_3[department];
                    else if ((new int[] { 107 }).Contains(thisId)) p = F3_2[department];
                    else if ((new int[] { 110 }).Contains(thisId)) p = F2_3[department];
                    else p = info.portrait;

                    dialoguePortrait.sprite = p;

                    // ��� ����
                    Sprite thisBg = backgroundPortrait.sprite;
                    switch (thisId) //����
                    {
                        case 0:
                            thisBg = bg[0]; break;
                        case 10:
                            thisBg = bg[1]; break;
                        case 15:
                            thisBg = bg[2]; break;
                        case 17: 
                            thisBg = bg[3]; break;
                        case 32:
                        case 46:
                            thisBg = bg[4]; break;
                        case 36:
                            thisBg = bg[5]; break;
                        case 51:
                            thisBg = bg[10]; break;
                        case 56:
                            thisBg = bg[6]; break;
                        case 61:
                            thisBg = bg[7]; break;
                        case 68:
                        case 89:
                            thisBg = bg[12]; break;
                        case 74:
                            thisBg = bg[8]; break;
                        case 92: 
                            thisBg = bg[11]; break;
                        case 97: 
                            thisBg = bg[9]; break;
                        case 105: 
                            thisBg = bg[10]; break;
                        default: break;
                    }
                    backgroundPortrait.sprite = thisBg;
                    


                    //����� ����
                    if (thisId.Equals(1))
                    {
                        GetComponent<AudioSource>().clip = outcrowdSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 13) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(56))
                    {
                        GetComponent<AudioSource>().clip = wheeSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 56) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(59))
                    {
                        GetComponent<AudioSource>().clip = jumpropeSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 59) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(62))
                    {
                        GetComponent<AudioSource>().clip = duguduguSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 62) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(70))
                    {
                        GetComponent<AudioSource>().clip = crowdshoutSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 73) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(74))
                    {
                        GetComponent<AudioSource>().clip = wheeSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 74) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(75))
                    {
                        GetComponent<AudioSource>().clip = crowdshoutSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 77) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(80))
                    {
                        GetComponent<AudioSource>().clip = twothreeSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 80) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(102))
                    {
                        GetComponent<AudioSource>().clip = duguduguSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 102) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(104))
                    {
                        GetComponent<AudioSource>().clip = duguduguSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 104) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(106))
                    {
                        GetComponent<AudioSource>().clip = crowdshoutSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 111) { GetComponent<AudioSource>().Stop(); }

                    StartCoroutine(TypeText(completeText));

                    switch (thisId)
                    {
                        case 10:
                        case 16:
                        case 25:
                        case 38:
                        case 39:
                        case 50:
                        case 60:
                        case 67:
                        case 78:
                        case 92:
                        case 102:
                        case 104:
                        case 111:
                            isDelayturn = true; break;
                        default: break;
                    }
                }//end of lock
            }
        }
        else
        {
            StopAllCoroutines();
            isCurrentlyTyping = false;
            dialogueText.text = completeText;
            return;
        }


    }

    IEnumerator TypeText(string completeText)
    {
        isCurrentlyTyping = true;
        string text = completeText;
        foreach (char c in text.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
        }
        isCurrentlyTyping = false;
    }

    private void EndofDialogue()
    {
        MedalGround.SetActive(true);
        MedalAnimation.SetActive(true);
        endingAnimation.SetActive(true);
        Invoke("GoEnd", 5f);
    }

    private void GoEnd()
    {
        MedalGround.SetActive(false);
        MedalAnimation.SetActive(false);
        endingAnimation.SetActive(true);
        FadeOutScript.instance.Fade();
    }

    //��� 2�� �ڵ� ����̱� �Լ�
    private void delayDialog()
    {
        DialogueBox.SetActive(false);
        isDelayturn = false;
        Invoke("DequeueDialogue", 2f);
    }

}