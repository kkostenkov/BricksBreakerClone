using UnityEngine;
using TMPro;

namespace BrickBreaker
{
    public class Target : MonoBehaviour
    {
        public Color[] Colors;
        public int Life;
        private TextMeshPro txt;

        private void Start()
        {
            this.txt = gameObject.transform.GetChild(0).GetComponent<TextMeshPro>();
            this.txt.text = this.Life + "";
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag(Constants.Tags.Ball)) {
                if (this.Life > 1) {
                    this.Life--;
                    this.txt.text = this.Life + "";

                    gameObject.transform.GetComponent<SpriteRenderer>().color = this.Colors[Random.Range(0, this.Colors.Length)];
                }
                else {
                    transform.parent.GetComponent<ParticleSystemPlayer>().start = true;
                    Destroy(this.gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            Wall.currentPoints += 10;
            UIManager.Points += Wall.currentPoints;
        }
    }
}