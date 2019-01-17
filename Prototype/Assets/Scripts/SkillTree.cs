using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Flags]
public enum SkillNodes
{
    None = 0,
    H_1 = 1,
    H_2 = 1 << 1,
    H_3 = 1 << 2,
    H_4 = 1 << 3,
    D_1 = 1 << 4,
    D_2 = 1 << 5,
    D_3 = 1 << 6,
    D_4 = 1 << 7
}

enum MenuState
{
    SkillTree = 0,
    ConfirmWindow
}

struct SkillTreeInfo
{
    public int skillPoints;
    public SkillNodes nodesActivated;
}

public class SkillTree : MonoBehaviour
{
    static SkillTreeInfo info;

    MenuState state;
    const int NUM_DIRECTIONS = 4;
    const int NUM_NODES = 8;
    public GameObject cursor;
    public SkillTreeNode[] nodes;
    public Image black;
    public Text nodeDescription;
    public Text skillPointsCounter;
    public TextAsset neighborMatrixFile;
    public TextAsset nodeDescriptionsFile;
    public ConfirmWindow window;

    int[,] neighborMatrix;
    string[] nodeDescriptions;
    int[] skillCosts;

    int nodeIndex;
    // Row: Source
    // Column: Destination
    //     L U R D
    //    --------
    // 0 | 1 1 4 2
    // 1 | 3 1 0 2
    // 2 | 3 1 0 2
    // 3 | 3 1 1 2
    // 4 | 0 5 5 6
    // 5 | 4 5 7 6
    // 6 | 4 5 7 6
    // 7 | 5 5 7 6

    static SkillTree()
    {
        info.nodesActivated = SkillNodes.None;
        info.skillPoints = 16;
    }
    
    void Start()
    {
        state = MenuState.SkillTree;
        StartCoroutine(Transition(true));
        nodeIndex = 0;

        InitializeNodes();
        UpdateSkillPointCounter();
        LoadNeighborMatrix();
        LoadDescriptions();

        MoveCursor();
        window.gameObject.SetActive(false);
    }

    //Initialize nodes when entering the menu with shared nodesActivated
    void InitializeNodes()
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            if ((info.nodesActivated & nodes[i].node) == nodes[i].node)
            {
                nodes[i].SetActive(true);
            }
        }
    }

    void UpdateSkillPointCounter()
    {
        skillPointsCounter.text = "Skill points: " + info.skillPoints.ToString();
    }

    void LoadNeighborMatrix()
    {
        neighborMatrix = new int[NUM_NODES, NUM_DIRECTIONS];
        string neighborsString = neighborMatrixFile.ToString();
        string[] lines = neighborsString.Split('\n');
        for (int i = 0; i < NUM_NODES; i++)
        {
            for (int j = 0; j < NUM_DIRECTIONS; j++)
            {
                neighborMatrix[i, j] = (int)(lines[i][j] - 48);
            }
        }
    }

    void SetDescription(string description, int cost)
    {
        nodeDescription.text = description + "\nCost: " + cost.ToString() + " skill point" + ((cost != 1) ? "s." : ".");
    }

    void LoadDescriptions()
    {
        string[] lines = nodeDescriptionsFile.ToString().Split('\n');
        nodeDescriptions = new string[lines.Length];
        skillCosts = new int[lines.Length];
        for(int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            nodeDescriptions[i] = line[0];
            skillCosts[i] = int.Parse(line[1]);
        }

        SetDescription(nodeDescriptions[0], skillCosts[0]);
    }

    IEnumerator Transition(bool fadeIn)
    {
        Color col = Color.black;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            col.a = (fadeIn) ? (1 - i) : i;
            black.color = col;
            yield return 0;
        }
        if (!fadeIn)
            ReturnToPauseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForArrowKeys();
        CheckForConfirm();
        CheckForBack();
    }

    void CheckForArrowKeys()
    {
        if (state == MenuState.SkillTree)
        {
            //TODO: Copy code from the Pause Menu and make an input manager
            //Left: 0, Up: 1, Right: 2, Down: 3
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                nodeIndex = neighborMatrix[nodeIndex, 0];
                MoveCursor();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                nodeIndex = neighborMatrix[nodeIndex, 1];
                MoveCursor();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                nodeIndex = neighborMatrix[nodeIndex, 2];
                MoveCursor();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                nodeIndex = neighborMatrix[nodeIndex, 3];
                MoveCursor();
            }
        }
        else if (state == MenuState.ConfirmWindow)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                window.ChangeSelection();
            }
        }
    }

    void OnChangeState(MenuState newState)
    {
        if (state == newState) return;
        switch (newState)
        {
            case MenuState.ConfirmWindow:
                window.gameObject.SetActive(true);
                break;

            case MenuState.SkillTree:
                window.gameObject.SetActive(false);
                break;
        }
        state = newState;
    }

    void CheckForConfirm()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (state == MenuState.SkillTree)
            {
                //TODO: Make error noise
                
                //Check cost of skill
                //Enough skill points
                if (info.skillPoints >= skillCosts[nodeIndex] && ((info.nodesActivated & nodes[nodeIndex].node) != nodes[nodeIndex].node))
                {
                    OnChangeState(MenuState.ConfirmWindow);
                    window.SetSkillPointsIndicator(skillCosts[nodeIndex]);
                }
                //Not enough skill points
                else
                {

                }
            }
            else if (state == MenuState.ConfirmWindow)
            {
                //Activate a skill node
                if (window.selectedYes)
                {
                    info.skillPoints -= skillCosts[nodeIndex];
                    info.nodesActivated |= nodes[nodeIndex].node;
                    nodes[nodeIndex].SetActive(true);
                    UpdateSkillPointCounter();
                }
                else
                {

                }
                OnChangeState(MenuState.SkillTree);
            }
        }
    }

    void CheckForBack()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == MenuState.SkillTree)
                StartCoroutine(Transition(false));
            else if (state == MenuState.ConfirmWindow)
                OnChangeState(MenuState.SkillTree);
        }
    }

    void MoveCursor()
    {
        cursor.transform.position = nodes[nodeIndex].gameObject.transform.position;
        SetDescription(nodeDescriptions[nodeIndex], skillCosts[nodeIndex]);
    }

    void ReturnToPauseMenu()
    {
        PauseMenu.sharedInstance.ReturnToPauseMenu();
    }
}
