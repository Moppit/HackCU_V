using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeFall : MonoBehaviour
{
    public string stringToEdit = "Score: ";
    public int total_score;
    public string endGame = "GAME OVER";
    private GameObject newTrafficBlock;
    bool stillPlaying;
    int count = 0;
    // Allow only 3 at once for now
    List<GameObject> currentlyFallingCubes = new List<GameObject>();
    List<int> scoreCounted = new List<int>();
    private int prob_something_falls;

    int initialHeight = 10;

    public void OnGUI()
    {
        string addToPrint = stringToEdit + total_score.ToString();
        GUI.TextField(new Rect(10, 10, 200, 20), addToPrint, 25);
        if (stillPlaying == false)
        {
            GUI.TextField(new Rect(650, 350, 100, 20), endGame, 40);
        }
    }

    public void printEndGame()
    {
        GUI.TextField(new Rect(200, 200, 200, 20), endGame, 25);
    }

    public static bool AboutEqual(double x, double y, double precision)
    {
        double epsilon = Math.Max(Math.Abs(x), Math.Abs(y)) * precision;
        return Math.Abs(x - y) <= epsilon;
    }

    public void SpawnTraffic(int type, bool malicious)
    {
        this.newTrafficBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer newBlockRenderer = this.newTrafficBlock.GetComponent<Renderer>();

        // Give it a position - need to generate an x, y, and z coordinate to fit in screen
        int x = UnityEngine.Random.Range(0, 2);
        int y = 10;
        // Range for z is about -6 to positive 1
        bool validZ = false;
        int z = UnityEngine.Random.Range(0, 12);

        this.newTrafficBlock.transform.localPosition = new Vector3(x, y ,z);

        // Change the color to indicate maliciousness
        if (malicious)
        {
            newBlockRenderer.material.color = Color.red;
        }
        else
        {
            newBlockRenderer.material.color = Color.green;
        }

        // Add new block to vector of cubes
        this.currentlyFallingCubes.Add(this.newTrafficBlock);
        this.scoreCounted.Add(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        total_score = 0;
        stillPlaying = true;
        System.Threading.Thread.Sleep(5000);
    }

    // Update is called once per frame
    void Update()
    {
        if (stillPlaying && (count != 0))
        {
            // See if you should make another random hecking block fall from the sky
            prob_something_falls = UnityEngine.Random.Range(0, 1000);
            if (prob_something_falls < 10)
            {
                // One in four chance the block will be malicious
                if (UnityEngine.Random.Range(0, 4) == 1)
                {
                    SpawnTraffic(1, true);
                }
                else
                {
                    SpawnTraffic(1, false);
                }
            }
            // Iterate through each of the falling blocks in the vector and move it

            for (int i = 0; i < this.currentlyFallingCubes.Count; i++)
            {
                this.currentlyFallingCubes[i].transform.position += Vector3.down * Time.deltaTime * 2;
                Renderer eachBlock = currentlyFallingCubes[i].GetComponent<Renderer>();
                // Check z coordinate
                if (AboutEqual(this.currentlyFallingCubes[i].transform.position.y, -4, 1E-2))
                {
                    // If malicious and it hit the floor, game over
                    if (eachBlock.material.color == Color.red)
                    {
                        stillPlaying = false;
                        // Destroy the game object to end the game
                        Destroy(currentlyFallingCubes[i]);
                    }
                    else if ((eachBlock.material.color == Color.green) && (scoreCounted[i] == 0))
                    {
                        total_score++;
                        scoreCounted[i] = 1;
                    }
                    // If it is not, add one to the score
                    this.currentlyFallingCubes[i].SetActive(false);
                    // Need to make sure it only increments by one instead of >=2
                }
            }
            // this.transform.position += Vector3.down * Time.deltaTime * 2;
        }
        else if (Input.GetKeyDown("space"))
        {
            count++;
        }
        return;
    }
}
