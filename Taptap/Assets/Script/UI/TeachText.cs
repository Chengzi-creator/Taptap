using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeachText : MonoBehaviour
{
    public static TeachText instance;

    public static TeachText Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<TeachText>();
            }

            return instance;
        }
    }

    [SerializeField] private TextMeshProUGUI dialogueText;
    //[SerializeField] private Button nextButton;

    [Header("对话更新")]
    public int talkConut = 0; //用来存储对话次数？

    [Header("对话显示")] private Queue<string> dialogueQueue=new Queue<string>(); //存储对话
    private bool isTyping = false; //判断是否正在显示
    private Coroutine typingCoroutine;

    public int Count = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //dialogueQueue = new Queue<string>();
        //nextButton.onClick.AddListener(DisplayNextSentence); //显示下一句
        //nextButton.gameObject.SetActive(false);
    }

    public void LoadDialogue()
    {
        switch (talkConut)
        {
            case 0:
                StartDialogue(new List<string>
                {      
                    "Press 'Enter' to continue",
                    "Hello Hello",
                    "Introduce",
                    "Learn to play"
                });
                break;

            case 1:
                StartDialogue(new List<string>
                {
                   
                });
                break;

            default:
                //Debug.Log("没有更多的对话。");
                break;
        }
    }

    //初始化,存储对话内容
    public void StartDialogue(List<string> dialogue)
    {
        dialogueQueue.Clear();

        foreach (string sentence in dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTyping)
        {
            DisplayNextSentence();
        }

        if (Count == 2)
        {
            talkConut = 1;
        }
    }


    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueQueue.Dequeue();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueText.text = "";
        //nextButton.gameObject.SetActive(false);
        gameObject.SetActive(false);

        if (talkConut == 0)
        {
           
        }

        if (talkConut == 1)
        {
           
            gameObject.SetActive(false);
        }
    }
}
