using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoRoller : MonoBehaviour{
    
    #region Public Variables
    public Text BingoCallText;


    #endregion

    #region Private Varirables
    //Both Bingo Ball sheets, Uncalled are the 
    private int[] UncalledBingoBalls = new int[76];
    private int[] CalledBingoBalls = new int[76];

    //Timer Variable to use for timers
    private float Timer = 0f;

    //Current Ball
    private int CurrentBall = 0;

    //The amount of balls that were called
    private int BallsCalled = 0;

    #endregion

    // Start is called before the first frame update
    void Start(){
        ResetBingoRoller();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetButtonDown("Use")){
            CursedCallOutEverythingInTheUncalledBallsFunctionLMAOLookAtHowLongThisFunctionNameIs();
        }

        RunGame();

    //Debug.Log("Time left in timer: " + Mathf.Ceil(Timer) );
        

    }

    #region Bingo Set Up Functions

    /**
        Reset BingoRoller empties out the Called Bingo Balls list & Uncalled Bingo Balls list
        It then fills the Uncalled Bingo Ball list with values 1-75
    **/
    public void ResetBingoRoller(){
        //Clears out both sheets
        ClearBingoBallSheet(UncalledBingoBalls);
        ClearBingoBallSheet(CalledBingoBalls);
        
        //Fills the Uncalled Balls lmao
        //This comment is so fucking funny
        for(int i = 0; i <= 75; i++){
            UncalledBingoBalls[i] = i;
        }

    }

    /**
        Gets a BingoBallSheet Array and then sets all the values to 0
    **/
    private void ClearBingoBallSheet(int[] BingoBallSheet){
        //Goes through the entire list and sets all values to 0
        for(int i = 0; i < BingoBallSheet.Length; i++){
            BingoBallSheet[i] = 0;
        }
    }

    #endregion

    #region Ball Calling Functions

    public void RunGame(){
        if(Timer > 0){
            Timer -= Time.deltaTime;
            BingoCallText.text = GetBingoLetter(CurrentBall) + CurrentBall.ToString();
        } else {
            int NewTime = Random.Range(3, 7);
            Timer = NewTime;
            CurrentBall = CallBall();
        }

    }

    /**
        Calls a bingo ball by finding a ball that hasn't been called
        Then moving the value to the called bingoball sheet

        Returns the ball that has been called
    **/
    private int CallBall(){
        //Rolls a Ball
        int BallNumber = Random.Range(1,75);

        if(BallsCalled != 75){
            //If the ball has been called (Value of 0) then keep finding one
            if(UncalledBingoBalls[BallNumber] == 0){
                while(UncalledBingoBalls[BallNumber] == 0){
                    BallNumber = Random.Range(1,75);
                }
            }
        }

        //Once the ball has been picked, move the ball into the Called Bingo Balls Sheet
        //Mark the ball used in uncalled bingo balls
        CalledBingoBalls[BallNumber] = BallNumber;
        UncalledBingoBalls[BallNumber] = 0;
        //Increments BallsCalled by 1, so to not get stuck in the infinite loop
        BallsCalled++;

        return BallNumber;
    }

    /**
        Get Bingo Letter returns the char of the bingo number
        B 1-15
        I 16-30
        N 31-45
        G 46-60
        O 61-75
    **/
    private char GetBingoLetter(int BallNum){
        //Finds the letter and returns
        if(BallNum >= 61){
            return 'O';
        } else if (BallNum >= 46){
            return 'G';
        } else if (BallNum >= 31){
            return 'N';
        } else if (BallNum >= 16){
            return 'I';
        } else{
            return 'B';
        }

    }

    #endregion

    #region Card Checking

    public bool CheckIfBallWasCalled(int BallToCheck){
        //If it's 0, it's the free space.
        if(BallToCheck == 0)
            return true;

        //Looks for the called bingo balls, if it's in there then return true
        //If not return false
        if(CalledBingoBalls[BallToCheck] == BallToCheck){
            return true;
        } else {
            return false;
        }

        //Safety Precaution, if nothing hits for some reason, return false.
        return false;
    }

    #endregion

    public void CursedCallOutEverythingInTheUncalledBallsFunctionLMAOLookAtHowLongThisFunctionNameIs(){

        for(int i = 0; i < UncalledBingoBalls.Length; i++){
            Debug.Log(UncalledBingoBalls[i]);
        }

    }

}
