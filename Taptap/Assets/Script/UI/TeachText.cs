using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine;

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
    public int talkCount = 0; //用来存储对话次数？

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
        switch (talkCount)
        {
            case 0:
                StartDialogue(new List<string>
                {    
                    "欢迎来到光怪梦离的世界！在本关中，你将会学到本游戏的基础操作。(按Down继续)",
                    "在光怪梦离的世界里，灯塔是所有建筑的立足之本，用于抵御怪物的防御塔只能建造在有光照亮的地块。(按Down继续)",
                    "在屏幕左下角的灯塔建筑栏里选择红色并选择煤油灯，在地图的中心位置花费初始的光之精华建造你的第一个灯塔！ (按Down继续)",
                    "趁热打灯，在屏幕右下角的防御塔建筑栏里选择水晶防御塔，在煤油灯旁建造你的第一个水晶防御塔吧！(按Down继续)",
                    "在红色灯塔内的防御塔会造成红色伤害，有效打击红色的敌人。(按Down继续)",
                    "你可以点击屏幕右下方的迎敌按钮放出敌人了，敌人死亡时，会掉落相应的光之精华。注意，敌人来袭时无法建造任何建筑！(按Down继续)",
                    "第二个波次的怪物不同于刚刚我们遇到的怪物，它具有绿色特征的身体，因此它必须要用绿色灯塔照亮的防御塔来抵御。(按Down继续)",
                    "待怪物消失后，点击右下角的拆除按钮，拆除防御塔与灯塔，将原本的红色煤油灯替换为绿色煤油灯，并再次建造水晶防御塔。(按Down继续)",
                    "做好防御工事后就点击迎敌按钮吧！",
                    "怪物进入终点会造成扣血！血量将会随着收集到怪物死亡掉落的色块慢慢恢复，(按Down继续)",
                    "但当终点的角色显示黑白图案时，说明你的角色岌岌可危，无法再承受更多伤害了。(按Down继续)",
                    "请试着自己迎接第三个波次的敌人。(按Down继续)",
                    "你完成本关的教程了！"
                });
                break;
            //
            // case 1:
            //     StartDialogue(new List<string>
            //     {
            //       
            //     });
            //     break;
            //
            // case 2:
            //     StartDialogue(new List<string>
            //     {
            //         
            //     });
            //     break;
            //
            // case 3:
            //     StartDialogue(new List<string>
            //     {
            //     });
            //     break;
            //
            // case 4:
            //     StartDialogue(new List<string>
            //     {
            //        
            //     });
            //     break;
            //
            // case 5:
            //     StartDialogue(new List<string>
            //     {
            //         
            //     });
            //     break;
            
            case 6:
                StartDialogue(new List<string>
                {
                    "灯塔与灯塔之间会产生奇妙的反应，不同三原色的灯塔叠加时，颜色重叠的部分会变为新的颜色！(按Down继续)",
                    "在地图的高墙上建造灯塔，使地图中出现区别于红、蓝、绿的其他颜色。(按Down继续)",
                    "在叠色地块上放置防御塔时，防御塔会根据颜色不同产生不同的效果。(按Down继续)",
                    "紫色地块上防御塔的攻击会使敌人减速，黄色地块上防御塔的攻击会使敌人死亡时掉落额外的光之精华，(按Down继续)",
                    "青色地块上防御塔的攻击会使敌人持续失去生命，白色地块则会使敌人受到大量额外伤害。(按Down继续)",
                    "同时，叠色色块上的防御塔攻击也会对敌人造成合成其叠色的原色伤害。(按Down继续)",
                    "如红色与绿色合成的黄色格上的防御塔，攻击时将会造成红色与绿色伤害。(按Down继续)",
                    "红、蓝、绿色合成的白色地块上的防御塔，则会对敌人造成三种颜色的伤害。(按Down继续)",
                    "观察下一波怪物的颜色，选择合适的对策吧！(按Down继续)",
                    "有些叠色地块被浪费了？不用担心，当怪物走到叠色地块上时，也会受到同样的特殊效果，如走到紫色地块上的怪物同样会被减速。(按Down继续)",
                    "教程到这里就结束了，尽情享受游戏吧！"
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isTyping)
        {
            DisplayNextSentence();
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
        
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
         //dialogueText.text = "";
        //nextButton.gameObject.SetActive(false);
        //gameObject.SetActive(false);

        if (talkCount == 0)
        {
            //UIManager.Instance.isTeaching0 = true;//建造红色煤油
        }

        if (talkCount == 1)
        {   
            Debug.Log("isTeaching1");
            //UIManager.Instance.isTeaching1 = true;//建造水晶
        }

        if (talkCount == 2)
        {
            //UIManager.Instance.isTeaching2 = true;//出怪
        }
        
        if (talkCount == 3)
        {
            //UIManager.Instance.isTeaching3 = true;//拆除加第二轮出怪
        }
        
        if (talkCount == 4)
        {
            //UIManager.Instance.isTeaching4 = true;//拆除加第二轮出怪
        }
        
        if (talkCount == 6)
        {
           // UIManager.Instance.isTeaching5 = true;//第二关文本显示不可出怪
        }
        
    }
}
