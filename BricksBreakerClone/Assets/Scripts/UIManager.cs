using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BrickBreaker
{
    public class UIManager : MonoBehaviour
    {
        public static int Points;
        public Slider Slider;
        public float TargetScore;
        public Image Star1;
        public Image Star2;
        public Image Star3;
        private float changeSpeed;
        public TextMeshProUGUI PointsText;

        private void Start()
        {
            Points = 0;
        }

        private void Update()
        {
            this.changeSpeed = Points - this.Slider.value;
            this.Slider.value = Mathf.MoveTowards(this.Slider.value, Points / this.TargetScore * 100, this.changeSpeed * Time.deltaTime);
            this.PointsText.text = Points + "";

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