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

        public event Action<Target> Destroying;

        private int hitValuePoints = 1;
        private int destroyValuePoints = 100;

        private SessionPoints pointsHolder;

        public void Inject(SessionPoints pointsHolder)
        {
            this.pointsHolder = pointsHolder;
        }

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
                transform.parent.GetComponent<ParticleSystemPlayer>().Play();
                ScorePoints(destroyValuePoints);
                this.Destroying?.Invoke(this);
                Destroy(this.gameObject);
            }
        }

        private void ScorePoints(int points)
        {
            BottomWall.currentShotPoints += points;
            pointsHolder.Add(points);
        }
    }
}