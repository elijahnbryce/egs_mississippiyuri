using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Attributes;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;

    public DialogueRunner dialogueRunner;
    public YarnProject myProject;

    [YarnNode(nameof(myProject))] public string testNode;
    public List<string> yarnNodes;

    private void Awake()
    {
        if (null == _Instance) _Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (dialogueRunner.YarnProject != null)
        {
            yarnNodes = dialogueRunner.YarnProject.NodeNames.ToList();
        }
    }

    public void PlayDialogue(int i) => StartCoroutine(SendStartDialgoue(i));

    private IEnumerator SendStartDialgoue(int i)
    {
        yield return dialogueRunner.StartDialogue(yarnNodes[i]);
    }
}
