using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Toast : MonoBehaviour
{
    public static Toast instance;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (instance != null)
        {
            Debug.Log("�˾� ����");
        }
        else
        {
            instance = this;
            Debug.Log("�˾� �ν��Ͻ� ����");
        }

    }

    // Start is called before the first frame update
    public void CloaseToast()
    {
        
        //StartCoroutine(CloseThis());
        Invoke("CloseThis", 2f);
    }

    private void CloseThis()
    {
        Debug.Log("Toast��");
        animator.SetTrigger("close");
        gameObject.SetActive(false);
        animator.ResetTrigger("close");
    }

}
