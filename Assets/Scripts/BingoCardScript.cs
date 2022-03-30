using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoCardScript : MonoBehaviour{
    #region Public Variables
    [Header("Bingo Button Set Up")]
    [Tooltip("This is for all of the buttons in the damn Bingo Card")]
    public Button[] BingoButtons = new Button[25];
    
    #endregion

    #region Private Variables
    //Bingo Numbers holds the bingo card's numbers
    int[,] BingoNumbers = new int[5,5];
    
    //Bingo Marked holds bools to tell if a player marked a spot
    bool[,] BingoMarkedSpots = new bool[5,5];
    #endregion

    // Start is called before the first frame update
    void Start(){
        GenerateBingoCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Set Up Functions
    /**
        This method generates a bingo card
        Just so I don't have to look for it later
        Each letter goes by 15 intervals when I looked at the bingo set
        So that's what I'm going with.
        0 B 1-15
        1 I 16-30
        2 N 31-45
        3 G 46-60
        4 O 61-75
    **/
    private async void GenerateBingoCard(){
        //Button Number is the number to traverse the button array
        int ButtonNumber = 0;
        //Traverses the X Axis (Columns the ones that go BINGO)
        for(int i = 0; i < BingoNumbers.GetLength(1); i++){
            //Traverses the Y Axis (Rows, the ones that don't have letters)
            for(int j = 0; j < BingoNumbers.GetLength(0); j++){
                Button GrabButton = BingoButtons[ButtonNumber];
                //Checking for a specific case here,
                //Center of the BingoCard is a free space, marked as 0
                if(i == 2 && j == 2){
                    //Replace later with a set function or something
                    BingoNumbers[i,j] = 0;
                    GrabButton.GetComponentInChildren<Text>().text = "Free Space!";
                    ButtonNumber++;
                    continue;
                }


                //Creates the letter modifer, this is so you can
                //Offset shit by 15 depending on the letter
                int LetterModifer = i * 15;
                //Creates a new number 1 [inclusive] to 15 [also inclusive]
                //Then adds the letter modifer
                int NewNumber = Random.Range(1,15) + LetterModifer;
                for(int n = 0; n < BingoNumbers.GetLength(0); n++){
                    if(n == j)
                        continue;

                    if(NewNumber == BingoNumbers[i, n])
                        NewNumber = Random.Range(1,15) + LetterModifer;
                }
                BingoNumbers[i,j] = NewNumber;
                

                GrabButton.GetComponentInChildren<Text>().text = NewNumber.ToString();
                
                ButtonNumber++;
                Debug.Log("Number of [" + i + ", " + j + "] is " + NewNumber);
            }
        }
    }

    private void ButtonAssignments(){
        
    }

    #endregion

    #region Button Interactions

    /**
        MarkSpotOnCard has 2 inputs, Row and Column
        This is to access the BingoMarkedSpots 2D Array
        It'll flip the values inside from true <=> false
        One of them. idk
    **/
    public void MarkSpotOnCard(int Row, int Column){
        //Checks if the values are in range, if not, goodbye out leave
        if(Row < 0 && Row > 4 && Column < 0 && Column > 4 )
            return;

        //Flips the spot from false <=> true, one of them.
        BingoMarkedSpots[Row,Column] = !BingoMarkedSpots[Row,Column];
        Debug.Log("Bingo Spot: " + Row + " " + Column + " is now marked: " + BingoMarkedSpots[Row,Column]);
    }

    /**
        Check Spot on Card just returns if that spot is trueee or false
    **/
    public bool CheckSpotOnCard(int Row, int Column){
        return BingoMarkedSpots[Row,Column];
    }

    #endregion

}