using UnityEngine;

public class RandomActivator : MonoBehaviour
{
    public GameObject[] Yellow; 
    public GameObject[] Blue;  
    public GameObject[] Cyan; 

    void Start()
    {
        int randomNumber = Random.Range(1, 4);
        Debug.Log("Random Number: " + randomNumber);

        ActivateList(randomNumber);
    }

    void ActivateList(int number)
    {
        switch (number)
        {
            case 1:
                SetActiveItems(Yellow);
                break;
            case 2:
                SetActiveItems(Blue);
                break;
            case 3:
                SetActiveItems(Cyan);
                break;
        }
    }

    void SetActiveItems(GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            item.SetActive(true);
        }
    }
}
