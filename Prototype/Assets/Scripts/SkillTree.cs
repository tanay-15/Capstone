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

public class SkillTree : MonoBehaviour
{
    const int NUM_DIRECTIONS = 4;
    const int NUM_NODES = 8;
    public GameObject cursor;
    public SkillTreeNode[] nodes;
    public Image black;
    public TextAsset neighborMatrixFile;
    SkillNodes nodesEnum;

    int[,] neighborMatrix;

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

    void Start()
    {
        StartCoroutine(Transition(true));
        nodesEnum = SkillNodes.None;
        nodeIndex = 0;
        MoveCursor();

        LoadNeighborMatrix();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(Transition(false));
        }

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

    void MoveCursor()
    {
        cursor.transform.position = nodes[nodeIndex].gameObject.transform.position;
    }

    void ReturnToPauseMenu()
    {
        PauseMenu.sharedInstance.ReturnToPauseMenu();
    }
}
