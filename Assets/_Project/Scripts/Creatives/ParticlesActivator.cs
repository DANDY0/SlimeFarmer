using UnityEngine;

public class ParticlesActivator : MonoBehaviour
{
 [SerializeField] private ParticleSystem m_DangerParticles;
 [SerializeField] private ParticleSystem m_AngryParticles;
 [SerializeField] private ParticleSystem m_HeartParticles;
 [SerializeField] private ParticleSystem m_СoinParticles;
 [SerializeField] private ParticleSystem m_MoneyParticles;
 [SerializeField] private Vector3[] heartPoses;
 private int timesClicked;

 private void Update()
 {
  if(Input.GetKeyDown(KeyCode.G))
    m_СoinParticles.Play();
  if(Input.GetKeyDown(KeyCode.B))
   m_MoneyParticles.Play();
  
   //m_AngryParticles.Play();
   //m_DangerParticles.Play();

   
  // if (Input.GetKeyDown(KeyCode.B))
  // {
  //  m_HeartParticles.transform.position = heartPoses[timesClicked];
  //  m_HeartParticles.Play();
  //  timesClicked++;
  // }
 }
}
