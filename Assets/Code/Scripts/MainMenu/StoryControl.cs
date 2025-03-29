using System.Collections.Generic;
using UnityEngine;

public class StoryControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> storyList;
    [SerializeField] private GameObject lockedText;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    private readonly string collectiblesCollectedString = "collectiblesCollected";
    private int collectiblesCollectedInt;

    private int currentStoryPage;

    void OnEnable() {
        collectiblesCollectedInt = PlayerPrefs.GetInt(collectiblesCollectedString);

        switch (collectiblesCollectedInt) {
            case 0:
                foreach (GameObject story in storyList) {
                    story.SetActive(false);
                }
                lockedText.SetActive(true);
                break;
            default:
                storyList[0].SetActive(true);
                for (int i = 1; i < storyList.Count; i++) {
                    storyList[i].SetActive(false);
                }
                lockedText.SetActive(false);
                break;
        }

        leftArrow.SetActive(false);
        rightArrow.SetActive(true);

        currentStoryPage = 0;
    }

    public void GoLeft() {
        storyList[currentStoryPage].SetActive(false);
        if (currentStoryPage - 1 < collectiblesCollectedInt) {
            storyList[currentStoryPage - 1].SetActive(true);
            lockedText.SetActive(false);
        }
        else {
            lockedText.SetActive(true);
        }
        currentStoryPage--;

        EnableArrows();        
    }

    public void GoRight() {
        storyList[currentStoryPage].SetActive(false);
        if (currentStoryPage + 1 < collectiblesCollectedInt) {
            storyList[currentStoryPage + 1].SetActive(true);
            lockedText.SetActive(false);
        }
        else {
            lockedText.SetActive(true);
        }
        currentStoryPage++;

        EnableArrows();
    }

    private void EnableArrows() {
        leftArrow.SetActive(currentStoryPage != 0);
        rightArrow.SetActive(currentStoryPage != storyList.Count - 1);
    }
}
