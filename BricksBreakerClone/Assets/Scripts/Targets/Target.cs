using UnityEngine;
using TMPro;

namespace BrickBreaker
{
    public class Target : MonoBehaviour
    {
        public Color[] Colors;
        public int Life;
        private TextMeshPro txt;

        private int hitValuePoints = 1;
        private int destroyValuePoints = 100;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            this.spriteRenderer = gameObject.transform.GetComponent<SpriteRenderer>();
            this.txt = gameObject.transform.GetChild(0).GetComponent<TextMeshPro>();
        }

        private void Start()
        {
            this.txt.text = this.Life + "";
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag(Constants.Tags.Ball)) {
                return;
            }

            if (this.Life > 1) {
                this.Life--;
                this.txt.text = this.Life + "";
                ScorePoints(this.hitValuePoints);

                spriteRenderer.color = this.Colors[Random.Range(0, this.Colors.Length)];
            }
            else {
                transform.parent.GetComponent<ParticleSystemPlayer>().start = true;
                ScorePoints(destroyValuePoints); 
                Destroy(this.gameObject);
            }
        }

        private void ScorePoints(int points)
        {
            BottomWall.currentShotPoints += points;
            GameSessionPointsDisplay.Points += points;
        }
    }
}