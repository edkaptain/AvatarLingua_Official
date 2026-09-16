using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public Animator animator;
    public Text canvasText;
    public bool status { get; private set; }
    [ContextMenu("Door/Open")]
    public void OpenDoor()
    {
        animator.SetBool("Check", true);
        status = true;
    }

    [ContextMenu("Door/Close")]
    public void CloseDoor()
    {
        animator.SetBool("Check", false);
        status = false;
    }

    [ContextMenu("Update Score")]
    public void UpdateDoorCanvas()
    {

        if(canvasText == null)
        {
            Debug.LogWarning("No text component was detected");
            return;
        }

        int actualScore = ItemsManager.Instance.GetActualScore();

        Regex regex = new Regex(@"\d+");

        canvasText.text = regex.Replace(canvasText.text, actualScore.ToString(), 1);


    }
}
