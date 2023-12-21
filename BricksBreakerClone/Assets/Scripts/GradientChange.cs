using UnityEngine;

namespace BrickBreaker
{
    public class GradientChange : MonoBehaviour
    {
        public Sprite GradientWhite;
        private Sprite MainSprite;
        private SpriteRenderer sr;

        private void Start()
        {
            this.sr = gameObject.GetComponent<SpriteRenderer>();
            this.MainSprite = this.sr.sprite;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Ball") {
                this.sr.sprite = this.GradientWhite;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.tag == "Ball") {
                this.sr.sprite = this.MainSprite;
            }
        }
    }
}