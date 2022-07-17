using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private int lettersPerSecond;
    private bool isTyping;
    private bool isToJumpText;
    private Dialog currentDialog;
    private int currentDialogLine;

    private static DialogManager _Instance;
    public static DialogManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<DialogManager>();
            }

            return _Instance;
        }
    }

    private void Update()
    {
        DoInteraction();
    }

    public void ShowDialog(Dialog dialog)
    {
        currentDialog = dialog;
        currentDialogLine = 0;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog());
    }

    public void HideDialog()
    {
        dialogBox.SetActive(false);
    }

    public void DoInteraction()
    {
        if (Input.GetButtonDown("Fire1") && dialogBox.activeSelf)
        {
            if (isTyping)
            {
                isToJumpText = true;
            }
            else if (currentDialog.Lines.Count > currentDialogLine)
            {
                StartCoroutine(TypeDialog());
            }
            else
            {
                HideDialog();
            }
        }
    }

    public IEnumerator TypeDialog()
    {
        isTyping = true;
        dialogText.text = "";
        var timingText = 1f / lettersPerSecond;
        foreach (var letter in currentDialog.Lines[currentDialogLine].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(isToJumpText ? 0f : timingText);
        }
        isTyping = false;
        isToJumpText = false;

        currentDialogLine++;
    }
}
