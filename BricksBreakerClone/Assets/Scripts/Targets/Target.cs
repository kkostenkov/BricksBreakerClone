using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

namespace BrickBreaker
{
    public class Target : MonoBehaviour
    {
        [SerializeField]
        private Color[] colors;
        [SerializeField]
        private int lifePoints;
        [SerializeField]
        private TextMeshPro txt;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public event Action Destroyed;

        private int hitValuePoints = 1;
        private int destroyValuePoints = 100;

        private void Start()
        {
            this.txt.text = this.lifePoints + "";
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag(Constants.Tags.Ball)) {
                return;
            }

            if (this.lifePoints > 1) {
                this.lifePoints--;
                this.txt.text = this.lifePoints + "";
                ScorePoints(this.hitValuePoints);

                spriteRenderer.color = this.colors[Random.Range(0, this.colors.Length)];
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