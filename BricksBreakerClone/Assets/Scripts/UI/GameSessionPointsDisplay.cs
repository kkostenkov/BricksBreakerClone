using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BrickBreaker
{
    public class GameSessionPointsDisplay : MonoBehaviour
    {
        public Slider Slider;
        public float TargetScore;
        public Image Star1;
        public Image Star2;
        public Image Star3;
        private float changeSpeed;
        public TextMeshProUGUI PointsText;

        private SessionPoints pointsHolder;

        public void Inject(SessionPoints pointsHolder)
        {
            this.pointsHolder = pointsHolder;
        }

        private void Update()
        {
            this.changeSpeed = pointsHolder.Points - this.Slider.value;
            this.Slider.value = Mathf.MoveTowards(
                this.Slider.value, pointsHolder.Points / this.TargetScore * 100, 
                this.changeSpeed * Time.deltaTime);
            this.PointsText.text = pointsHolder.Points + "";

            if (Mathf.Round(this.Slider.value) >= 100) {
                this.Star3.color = new Color(1, 1, 1);
            }
            else if (Mathf.Round(this.Slider.value) >= 70) {
                this.Star2.color = new Color(1, 1, 1);
            }
            else if (Mathf.Round(this.Slider.value) > 0) {
                this.Star1.color = new Color(1, 1, 1);
            }
        }
    }
}