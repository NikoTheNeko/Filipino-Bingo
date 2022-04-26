using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoCardScript : MonoBehaviour{
    #region Public Variables
    [Header("Bingo Button Set Up")]
    [Tooltip("This is for all of the buttons in the damn Bingo Card")]
    public Button[] BingoButtons = new Button[25];

    [Header("Bingo Roller")]
    [Tooltip("This is the Bingo Roller / Caller for the scene")]
    public BingoRoller BingoCallerRoller;

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
                //Debug.Log("Number of [" + i + ", " + j + "] is " + NewNumber);
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

    #region Card Verification

    /**
        This Veryifies the card before it checks.
    **/
    public void VerifyCard(){

        for(int Row = 0; Row < BingoMarkedSpots.GetLength(1); Row++){
            for(int Col = 0; Col < BingoMarkedSpots.GetLength(0); Col++){
                //If it's not marked move onto the next.
                if(BingoMarkedSpots[Row, Col] == false)
                    continue;

                if(!BingoCallerRoller.CheckIfBallWasCalled(BingoNumbers[Row, Col])){
                    BingoMarkedSpots[Row,Col] = false;
                    Debug.Log(BingoNumbers[Row,Col] + " in " + "Row: " + Row + " Col: " + Col + " was not called");
                }
            }
        }

    }

    public void CheckForBingo(){
        VerifyCard();
        if(CheckRows() || CheckCols() || CheckDiagonals()){
            Debug.Log("BINGO!!!");
            //return true;
        }
        Debug.Log("No Bingo");
        //return false;

    }

    private bool CheckRows(){
        //Check for rows
        //This checks if there's anything in the first column is marked
        //If there is, then look through the entire row
        for(int Row = 0; Row < BingoMarkedSpots.GetLength(1); Row++){
            //If that row's 1st spot (column 1 in normal people talk)
            //Enter a loop
            if(BingoMarkedSpots[Row, 0] == true){
                //If the goes through the rest of the rows
                for(int Col = 0; Col < BingoMarkedSpots.GetLength(0); Col++){
                    //If the next spot is marked false, break out of the loop.
                    if(BingoMarkedSpots[Row, Col] == false){
                        Debug.Log("Row: " + Row + " Col: " + Col + "It no good :(");
                        break;
                    }
                    
                    //If not, if it's the end of the loop, it is a bingo.
                    if(Col == 4){
                        return true;
                        Debug.Log("Row: " + Row + " Col: " + Col + "It good :)");
                    }

                    //If it's not just continue the loop
                    continue;
                }
            }
        }
        //If you've went through the entire thing, there probably isnt anything
        return false;
    }

    private bool CheckCols(){
        //Check for Cols
        //This checks if there's anything in the first column is marked
        //If there is, then look through the entire Col
        for(int Col = 0; Col < BingoMarkedSpots.GetLength(1); Col++){
            //If that Col's 1st spot (column 1 in normal people talk)
            //Enter a loop
            if(BingoMarkedSpots[0, Col] == true){
                //If the goes through the rest of the Cols
                for(int Row = 0; Row < BingoMarkedSpots.GetLength(0); Row++){
                    //If the next spot is marked false, break out of the loop.
                    if(BingoMarkedSpots[Row, Col] == false){
                        Debug.Log("Row: " + Row + " Col: " + Col + "It no good :(");
                        break;
                    }
                    
                    //If not, if it's the end of the loop, it is a bingo.
                    if(Row == 4){
                        Debug.Log("Row: " + Row + " Col: " + Col + "It no good :(");
                        return true;
                    }

                    //If it's not just continue the loop
                    continue;
                }
            }
        }
        //If you've went through the entire thing, there probably isnt anything
        return false;
    }

    private bool CheckDiagonals(){
        //Checks from top left to bottom right
        if(BingoMarkedSpots[0,0] == true && 
           BingoMarkedSpots[1,1] == true &&
           BingoMarkedSpots[2,2] == true &&
           BingoMarkedSpots[3,3] == true &&
           BingoMarkedSpots[4,4] == true)
           return true;

        //Checks from bottom left to top right
        if(BingoMarkedSpots[4,0] == true && 
           BingoMarkedSpots[3,1] == true &&
           BingoMarkedSpots[2,2] == true &&
           BingoMarkedSpots[1,3] == true &&
           BingoMarkedSpots[0,4] == true)
           return true;

        //Nothing is there.
        return false;
    }


    #endregion

}
