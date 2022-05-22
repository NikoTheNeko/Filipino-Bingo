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
        for(int Col = 0; Col < BingoNumbers.GetLength(1); Col++){
            //Traverses the Y Axis (Rows, the ones that don't have letters)
            for(int Row = 0; Row < BingoNumbers.GetLength(0); Row++){
                Button GrabButton = BingoButtons[ButtonNumber];
                //Checking for a specific case here,
                //Center of the BingoCard is a free space, marked as 0
                if(Col == 2 && Row == 2){
                    //Replace later with a set function or something
                    BingoNumbers[Col, Row] = 0;
                    GrabButton.GetComponentInChildren<Text>().text = "Free Space!";
                    ButtonNumber++;
                    continue;
                }


                //Creates the letter modifer, this is so you can
                //Offset shit by 15 depending on the letter
                int LetterModifer = Col * 15;
                //Creates a new number 1 [inclusive] to 15 [also inclusive]
                //Then adds the letter modifer
                int NewNumber = Random.Range(1,15) + LetterModifer;
                //Checks if the number exists in the bingo card
                for(int n = 0; n < BingoNumbers.GetLength(0); n++){
                    //If it doesn't continue the loop to check
                    if(n == Row)
                        continue;

                    //If it does generate a new number
                    while(NewNumber == BingoNumbers[Col, n])
                        NewNumber = Random.Range(1,15) + LetterModifer;
                }

                //Sets the number to the bingo spot
                BingoNumbers[Col, Row] = NewNumber;
                
                //Sets the text to the number
                GrabButton.GetComponentInChildren<Text>().text = NewNumber.ToString();
                
                //Increments the button number to move onto the next
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
        BingoMarkedSpots[Column, Row] = !BingoMarkedSpots[Column, Row];
        Debug.Log("Bingo Spot: " + Row + " " + Column + " is now marked: " + BingoMarkedSpots[Row,Column]);
    }

    /**
        Check Spot on Card just returns if that spot is trueee or false
    **/
    public bool CheckSpotOnCard(int Row, int Column){
        return BingoMarkedSpots[Column, Row];
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
                if(BingoMarkedSpots[Col, Row] == false)
                    continue;

                if(!BingoCallerRoller.CheckIfBallWasCalled(BingoNumbers[Col, Row])){
                    BingoMarkedSpots[Col, Row] = false;
                    Debug.Log(BingoNumbers[Col, Row] + " in " + " Row: " + Row + " Col: " + Col + " was not called");
                }
            }
        }

    }

    public void CheckForBingo(){
        VerifyCard();
        if(CheckAllRows() || CheckAllCols() || CheckDiagonals()){
            Debug.Log("BINGO!!!");
            //return true;
        }
        Debug.Log("No Bingo");
        //return false;

    }

    /**
        This goes through the entire bingo sheet to see if any row is true
    **/
    private bool CheckAllRows(){
        //Goes through all the rows, if any row is marked at all, then mark as true
        for(int i = 0; i < BingoMarkedSpots.GetLength(1); i++){
            if(CheckRow(i))
                return true;
        }

        //if none are true, then just return false.
        return false;
    }

    //CheckRows Start Function
    private bool CheckRow(int Row){
        Debug.Log("Checking Column: " + 0 + " and Row: " + Row + " For the Check Row Function");
        //If the row is marked as true, then go and check the rest
        if(BingoMarkedSpots[0, Row] == true)
            CheckRow(1, Row); 

        return false;
    }

    /**
        Checks rows helper function
        This goes in and checks each row and will go through to see if things are good.
    **/
    private bool CheckRow(int Row, int Col){
        Debug.Log("Checking Column: " + Col + " and Row: " + Row + " For the Check Row Function");
        //If the is the last COLUMN in a ROW is true, then return true.
        if(BingoMarkedSpots[Col, Row] == true && Col == 4)
            return true;

        //If not, if the current row is true, access recursively
        if(BingoMarkedSpots[Col, Row] == true)
            //Checks for the next column in the row
            CheckRow(Row, Col++);

        //If they're not true, just return false.
        return false;
    }

    /**
        This goes through the entire bingo sheet to see if any row is true
    **/
    private bool CheckAllCols(){
        //Goes through all the Cols, if any Col is marked at all, then mark as true
        for(int i = 0; i < BingoMarkedSpots.GetLength(1); i++){
            if(CheckCol(i))
                return true;
        }

        //if none are true, then just return false.
        return false;
    }

    //CheckCols Start Function
    private bool CheckCol(int Col){
        Debug.Log("Checking Column: " + Col + " and Row: " + 0 + " For the Check Column Function");
        //If the Col is marked as true, then go and check the rest
        if(BingoMarkedSpots[Col, 0] == true)
            CheckCol(Col, 1); 

        return false;
    }

    /**
        Checks Cols helper function
        This goes in and checks each Col and will go through to see if things are good.
    **/
    private bool CheckCol(int Col, int Row){
        Debug.Log("Checking Column: " + Col + " and Row: " + Row + " For the Check Column Function");
        //If the is the last COLUMN in a Col is true, then return true.
        if(BingoMarkedSpots[Col, Row] == true && Row == 4)
            return true;

        //If not, if the current Col is true, access recursively
        if(BingoMarkedSpots[Col, Row] == true)
            //Checks for the next column in the Col
            CheckCol(Row++, Col);

        //If they're not true, just return false.
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
