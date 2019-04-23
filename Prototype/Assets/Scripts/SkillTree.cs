using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Flags]
public enum SkillNodes
{
    None = 0,       //00000000
    H_1 = 1,        //00000001
    H_2 = 1 << 1,   //00000010
    H_3 = 1 << 2,   //00000100
    H_4 = 1 << 3,   //00001000
    D_1 = 1 << 4,   //00010000
    D_2 = 1 << 5,   //00100000
    D_3 = 1 << 6,   //01000000
    D_4 = 1 << 7,   //10000000
    HUMAN = 15,     //00001111
    DEMON = 240     //11110000
}

enum MenuState
{
    SkillTree = 0,
    ConfirmWindow
}

public struct SkillTreeInfo
{
    public int skillPoints;
    public SkillNodes nodesActivated;
}

public class SkillTree : MonoBehaviour
{
    public static SkillTreeInfo info;
    static int nodeIndex;

    MenuState state;
    const int NUM_DIRECTIONS = 4;
    const int NUM_NODES = 8;
    public GameObject canvas;
    public GameObject skillTreeEdgePrefab;
    public SkillTreeCursor cursor;
    public SkillTreeNode[] nodes;
    public Image black;
    public Text nodeDescription;
    public Text skillPointsCounter;
    public TextAsset neighborMatrixFile;
    public TextAsset nodeDescriptionsFile;
    public TextAsset requiredNodesFile;
    public ConfirmWindow window;
    public Color humanNodeEdgeColor;
    public Color demonNodeEdgeColor;

    float minAxis = 0.5f;
    int vAxisDirectionPressed;
    int vAxisDirection;
    int hAxisDirectionPressed;
    int hAxisDirection;

    SkillTreeNodeEdge[] edges;
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
        info.skillPoints = 0;   //Max: 8
        nodeIndex = 0;
    }

    void Start()
    {
        state = MenuState.SkillTree;
        StartCoroutine(Transition(true));

        UpdateSkillPointCounter();
        LoadNeighborMatrix();
        LoadDescriptions();
        LoadRequiredNodes();
        UpdateNodes();

        MoveCursor();
        window.gameObject.SetActive(false);
    }

    void PlaceEdge(int index, int requiredIndex)
    {
        if (requiredIndex != -1)
        {
            GameObject edge = Instantiate(skillTreeEdgePrefab, gameObject.transform);
            Vector3 distance = nodes[requiredIndex].gameObject.transform.position - nodes[index].gameObject.transform.position;
            float angle = Mathf.Atan2(distance.y, distance.x);
            edge.transform.SetAsFirstSibling();
            edge.transform.localRotation = Quaternion.Euler(0f, 0f, angle * 180 / Mathf.PI);
            edge.transform.position = nodes[index].gameObject.transform.position;
            edge.transform.SetAsFirstSibling();

            edges[index] = edge.GetComponent<SkillTreeNodeEdge>();
            //Assign a human or a demon color for the edge
            if ((SkillNodes.HUMAN & nodes[index].node) == nodes[index].node)
                edges[index].baseColor = humanNodeEdgeColor;
            else if ((SkillNodes.DEMON & nodes[index].node) == nodes[index].node)
                edges[index].baseColor = demonNodeEdgeColor;
            else
                Debug.Log("Error.");
        }
    }

    //Initialize nodes when entering the menu with shared nodesActivated
    void UpdateNodes()
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            //Activate nodes for skills already active
            if ((info.nodesActivated & nodes[i].node) == nodes[i].node)
            {
                //nodes[i].SetActive(true);
                nodes[i].SetState(SkillTreeNodeState.active);
                if (edges[i] != null)
                    edges[i].SetActive(true);
            }
            //If a node is accessible, but not activated
            else if ((info.nodesActivated & requiredNodes[i]) == requiredNodes[i])
            {
                nodes[i].SetState(SkillTreeNodeState.inactive);
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
        edges = new SkillTreeNodeEdge[nodes.Length];
        requiredNodes = new SkillNodes[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            int line = int.Parse(lines[i]);
            PlaceEdge(i, line);
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
            nodeDescriptions[i] = line[0].Replace("\\n","\n");
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
        else
        {
            col.a = 0f;
            black.color = col;
        }
    }

    void CheckAxis()
    {
        if (Input.GetAxis("Vertical") >= minAxis && vAxisDirection != 1)
        {
            vAxisDirection = 1;
            vAxisDirectionPressed = 1;
        }
        else if (Input.GetAxis("Vertical") <= -minAxis && vAxisDirection != -1)
        {
            vAxisDirection = -1;
            vAxisDirectionPressed = -1;
        }
        else if (Input.GetAxis("Vertical") == 0 && vAxisDirection != 0)
        {
            vAxisDirection = 0;
            vAxisDirectionPressed = 0;
        }

        if (Input.GetAxis("Horizontal") >= minAxis && hAxisDirection != 1)
        {
            hAxisDirection = 1;
            hAxisDirectionPressed = 1;
        }
        else if (Input.GetAxis("Horizontal") <= -minAxis && hAxisDirection != -1)
        {
            hAxisDirection = -1;
            hAxisDirectionPressed = -1;
        }
        else if (Input.GetAxis("Horizontal") == 0 && hAxisDirection != 0)
        {
            hAxisDirection = 0;
            hAxisDirectionPressed = 0;
        }
    }

    void ResetCheckAxis()
    {
        vAxisDirectionPressed = 0;
        hAxisDirectionPressed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAxis();
        CheckForArrowKeys();
        CheckForConfirm();
        CheckForBack();
        ResetCheckAxis();
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
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || hAxisDirectionPressed == -1) && nodeIndex != neighborMatrix[nodeIndex, 0] && canMove(neighborMatrix[nodeIndex, 0]))
            {
                nodeIndex = neighborMatrix[nodeIndex, 0];
                MoveCursor();
            }
            if ((Input.GetKeyDown(KeyCode.UpArrow) || vAxisDirectionPressed == 1) && nodeIndex != neighborMatrix[nodeIndex, 1] && canMove(neighborMatrix[nodeIndex, 1]))
            {
                nodeIndex = neighborMatrix[nodeIndex, 1];
                MoveCursor();
            }
            if ((Input.GetKeyDown(KeyCode.RightArrow) || hAxisDirectionPressed == 1) && nodeIndex != neighborMatrix[nodeIndex, 2] && canMove(neighborMatrix[nodeIndex, 2]))
            {
                nodeIndex = neighborMatrix[nodeIndex, 2];
                MoveCursor();
            }
            if ((Input.GetKeyDown(KeyCode.DownArrow) ||  vAxisDirectionPressed == -1) && nodeIndex != neighborMatrix[nodeIndex, 3] && canMove(neighborMatrix[nodeIndex, 3]))
            {
                nodeIndex = neighborMatrix[nodeIndex, 3];
                MoveCursor();
            }
        }
        else if (state == MenuState.ConfirmWindow)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || hAxisDirectionPressed != 0)
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump"))
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

                    //Activating Human Node 1
                    if ((nodes[nodeIndex].node & SkillNodes.H_1) == SkillNodes.H_1)
                    {
                        PlayerLife.sharedInstance.SetSkillMaxLife();
                    }
                    //nodes[nodeIndex].SetState(SkillTreeNodeState.active);
                    UpdateNodes();
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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("PS4CIRCLE"))
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
