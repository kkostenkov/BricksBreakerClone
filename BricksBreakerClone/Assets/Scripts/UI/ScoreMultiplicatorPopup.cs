using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ScoreMultiplicatorPopup : MonoBehaviour
{
    [SerializeField]
    private List<ScoreMultiplicationButton> multiplicatorButtons;

    public async Task<int> GetMultiplicatorAsync()
    {
        var ct = new CancellationToken();
        var tasks = multiplicatorButtons.Select((b) => WaitForPress(b, ct));
        var finishedTask = await Task.WhenAny(tasks);
        var pressedButton = finishedTask.Result;
        return pressedButton.Muliplicator;
    }

    private async Task<ScoreMultiplicationButton> WaitForPress(ScoreMultiplicationButton button, CancellationToken ct)
    {
        bool isPressed = false;
        button.MultiplicatorSelected += () => isPressed = true;
        while (!isPressed) {
            await Task.Yield();
            if (ct.IsCancellationRequested) {
                button.MultiplicatorSelected = null;
                return null;
            }
        }
        return button;
    }
}
