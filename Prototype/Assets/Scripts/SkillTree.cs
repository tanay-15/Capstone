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
    public SkillTreeCursor cursor;
    public SkillTreeNode[] nodes;
    public Image black;
    public Text nodeDescription;
    public Text skillPointsCounter;
    public TextAsset neighborMatrixFile;
    public TextAsset nodeDescriptionsFile;
    public TextAsset requiredNodesFile;
    public ConfirmWindow window;

    int nodeIndex;
    int[,] neighborMatrix;
    SkillNodes[] requiredNodes;
    string[] nodeDescriptions;
    int[] skillCosts;

    //Neighbor Matrix
    // Row: Source
    // Column: Destination
    //     L U R D
    //    --------
    // 0 | 1 1 4 0
    // 1 | 2 1 0 0
    // 2 | 2 3 1 2
    // 3 | 3 3 3 2
    // 4 | 0 5 5 4
    // 5 | 4 5 6 4
    // 6 | 5 7 6 6
    // 7 | 7 7 7 6

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
        LoadRequiredNodes();

        MoveCursor();
        window.gameObject.SetActive(false);
    }

    //Initialize nodes when entering the menu with shared nodesActivated
    void InitializeNodes()
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            //Activate nodes for skills already active
            if ((info.nodesActivated & nodes[i].node) == nodes[i].node)
            {
                //nodes[i].SetActive(true);
                nodes[i].SetState(SkillTreeNodeState.active);
            }
        }
    }

    void UpdateSkillPointCounter()
    {
        skillPointsCounter.text = "Skill points: " + info.skillPoints.ToString();
    }

    void LoadRequiredNodes()
    {
        string[] lines = requiredNodesFile.ToString().Split(',');
        requiredNodes = new SkillNodes[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            int line = int.Parse(lines[i]);
            requiredNodes[i] = (SkillNodes)((line == -1) ? 0 : 1 << line);
        }
    }

    //Load information for which nodes must be active before moving to another node
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

    //Is it possible to do with a single enum?
    bool canMove(int index)
    {
        return ((requiredNodes[index] == SkillNodes.None) || (info.nodesActivated & requiredNodes[index]) == requiredNodes[index]);
    }

    void CheckForArrowKeys()
    {
        if (state == MenuState.SkillTree)
        {
            //TODO: Copy code from the Pause Menu and make an input manager
            //Left: 0, Up: 1, Right: 2, Down: 3
            if (Input.GetKeyDown(KeyCode.LeftArrow) && nodeIndex != neighborMatrix[nodeIndex, 0] && canMove(neighborMatrix[nodeIndex, 0]))
            {
                nodeIndex = neighborMatrix[nodeIndex, 0];
                MoveCursor();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && nodeIndex != neighborMatrix[nodeIndex, 1] && canMove(neighborMatrix[nodeIndex, 1]))
            {
                nodeIndex = neighborMatrix[nodeIndex, 1];
                MoveCursor();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && nodeIndex != neighborMatrix[nodeIndex, 2] && canMove(neighborMatrix[nodeIndex, 2]))
            {
                nodeIndex = neighborMatrix[nodeIndex, 2];
                MoveCursor();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && nodeIndex != neighborMatrix[nodeIndex, 3] && canMove(neighborMatrix[nodeIndex, 3]))
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
                    //nodes[nodeIndex].SetActive(true);
                    nodes[nodeIndex].SetState(SkillTreeNodeState.active);
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
        cursor.gameObject.transform.position = nodes[nodeIndex].gameObject.transform.position;
        SetDescription(nodeDescriptions[nodeIndex], skillCosts[nodeIndex]);
        cursor.Reset();
    }

    void ReturnToPauseMenu()
    {
        PauseMenu.sharedInstance.ReturnToPauseMenu();
    }
}
