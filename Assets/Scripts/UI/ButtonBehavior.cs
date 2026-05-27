using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    bool isOn = false;
    #region UI Buttons
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void Options()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Retry()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex);

        if (currentSceneIndex == 5)
        {
            SceneManager.LoadSceneAsync(2);
        }

        GameManager.instance.RestartGameState();
    }

    public void StartMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
    #region Controls Menu
    [SerializeField] private GameObject ControlsMenu;

    public void ControlMenuOn()
    {
        ControlsMenu.SetActive(true);
    }

    public void ControlMenuOff()
    {
        ControlsMenu.SetActive(false);
    }

    public void PausGame(bool staus)
    {
        ControlsMenu.SetActive(staus);
    }
    #endregion
    #region Slot Machine
    [SerializeField] private GameObject slotMachineMenu;

    SlotMachine slotMachine;

    public void SlotMachineMenuOn()
    {
        SlotMachineMenu(true);
    }

    public void SlotMachineMenuOff()
    {
        SlotMachineMenu(false);
    }

    public void SlotMachineMenu(bool status)
    {
        slotMachineMenu.SetActive(status);

        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SlotMachineOn()
    {
        if (isOn == false)
        {
            StartCoroutine(slotMachine.SpinRoutine());
            isOn = true;
        }
    }
    #endregion
}