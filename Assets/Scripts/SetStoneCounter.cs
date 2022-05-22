using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI; 

public class SetStoneCounter : MonoBehaviour
{
    public GameObject StoneCounter;

    public Text StoneText;

    MovePlayer movePlayer;

    private int score = 0;

    private void Start()
    {
        movePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayer>();


    }

    void Update()
    {
        SetCounterValues();
    }

    void SetCounterValues()
    {
        this.score = movePlayer.GetStoneNum();
        StoneText.text = this.score.ToString();
    }
}
