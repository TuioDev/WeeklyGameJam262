using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private int lettersPerSecond = 30;

    private bool isTyping;
    private bool isFirstLetterWritten;
    private bool isToJumpText;
    private Dialog currentDialog;
    private int currentDialogLine;
    private IInteractable showDialogSource;

    private static DialogManager _Instance;
    public static DialogManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<DialogManager>();
            }

            return _Instance;
        }
    }

    void Update()
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

    public void ShowDialogAndNotifyWhenClosed(Dialog dialog, IInteractable notifyBack)
    {
        showDialogSource = notifyBack;
        ShowDialog(dialog);
    }

    public void HideDialog()
    {
        dialogBox.SetActive(false);
        StartCoroutine(NotifySource());
    }

    private IEnumerator NotifySource()
    {
        // The player can't interact with the Interactable in the same frame
        yield return new WaitForEndOfFrame();
        showDialogSource?.IsDoneInteracting();
        showDialogSource = null;
    }

    public void DoInteraction()
    {
        if (InputManager.Instance.IsPressingConfirmation() && dialogBox.activeSelf)
        {
            if (isTyping)
            {
                isToJumpText = true && isFirstLetterWritten;
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

            isFirstLetterWritten = true;
        }

        isTyping = false;
        isFirstLetterWritten = false;
        isToJumpText = false;

        currentDialogLine++;
    }
}
