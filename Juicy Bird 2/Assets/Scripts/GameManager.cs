using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<GameObject> pipes = new List<GameObject>();
    GameObject player;

    public Text score_text;
    public int score;

  

   

    private void Start()
    {
        player = GameObject.Find("Player");
        score_text = GameObject.Find("Points_Text").GetComponent<Text>();
        
    }

    private void Update()
    {
        if(!player.GetComponent<PlayerController>().lost) // checks that player hasn't died, if so: won't give more points
        for(int i = 0; i < pipes.Count; i++) // gives +1 score for each pipe passed, destroys pipe after passed
        {
            GameObject pipe = pipes[i];
            if(pipe.transform.position.x < player.transform.position.x && pipe != null)
            {
                score += 1;
                score_text.text = "Score: " + score.ToString();
                pipes.Remove(pipe);
                Destroy(pipe, 3);
            }
        }       
    }

    public void DestroyPipes()
    {
        for(int i = 0; i < pipes.Count; i++)
        {
            GameObject pipe = pipes[i];
            if(pipe != null)
            {
                pipes.Remove(pipe);
                Destroy(pipe);
            }
        }
    }

    
 

    

    
}
