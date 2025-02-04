using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonBGMChanger : MonoBehaviour
{
    
    public void PlaySeasonBGM(SeasonEnum season){
        string bgmTitle = "";
        switch(season){
            case SeasonEnum.Spring : 
                bgmTitle = "Spring";
                break;
            case SeasonEnum.Summer :
                bgmTitle = "Summer";
                break;
            case SeasonEnum.Autumn :
                bgmTitle = "Autumn";
                break;
            case SeasonEnum.Winter :
                bgmTitle = "Winter";
                break;
        }

        SoundManager.I.PlayBGM(bgmTitle);
    }

    
}
