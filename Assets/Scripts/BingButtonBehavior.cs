using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingButtonBehavior : MonoBehaviour{
    #region Public Variables
    [Header("Bingo Card")]
    [Tooltip("This holds the bingo card so it can be used")]
    public BingoCardScript BingoCard;

    [Header("Button Attributes")]
    [Tooltip("This is the row it will access")]
    public int Row;
    [Tooltip("This is the column it will access")]
    public int Column;
    [Tooltip("This is the visual indicator that the bingo is marked")]
    public GameObject VisualMarking;
    #endregion

    #region Private Varirables
    #endregion

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    #region Button behaviors
    
    /**
        MarkSpots marks the spot in the Bingo Card, calls 
        MarkSpotOnCard from the BingoCard object
    **/
    public void MarkSpot(){
        BingoCard.MarkSpotOnCard(Row, Column);
        ToggleButtonIndicator();
    }

    /**
        ToggleButtonIndicator shows the visual indicator
        It'll call BingoCard's CheckSpotOnCard to see if it's true/false
        Then will act on it accordingly
    **/
    public void ToggleButtonIndicator(){
        if(BingoCard.CheckSpotOnCard(Row, Column)){
            VisualMarking.active = true;
        } else {
            VisualMarking.active = false;
        }
    }

    #endregion

}
