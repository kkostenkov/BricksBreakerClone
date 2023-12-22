using System.Collections;
using UnityEngine;
using TMPro;

namespace BrickBreaker
{
    public class ParticleSystemPlayer : MonoBehaviour
    {
        [SerializeField]
        public TextMeshPro particlesText;
        private ParticleSystem particles;

        private void Start()
        {
            this.particles = gameObject.GetComponent<ParticleSystem>();
        }

        private IEnumerator DestroyParticles()
        {
            this.particlesText.gameObject.SetActive(true);
            this.particlesText.text = BottomWall.currentShotPoints + "";
            this.particles.Play();
            yield return new WaitForSeconds(0.8f);
            Destroy(this.particles.gameObject);
            Destroy(this.particlesText.gameObject);
        }

        public void Play()
        {
            StartCoroutine(DestroyParticles());
        }
    }
}