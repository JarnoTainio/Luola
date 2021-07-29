using UnityEngine;

public class AnimatedObject : MonoBehaviour
{
  public float duration;
  public Vector3 startingSize;
  public Vector3 targetSize;
  public bool playOnAwake;
  public float delay;

  private float timer;
  private bool playing = false;

  private void Start()
  {
    if (playOnAwake)
    {
      Restart();
    }
  }
  // Update is called once per frame
  void Update()
  {
    if (playing)
    {
      timer += Time.deltaTime;
      if (timer > 0f)
      {

        if (duration < timer)
        {
          timer = duration;
          playing = false;
        }
        transform.localScale = targetSize * (timer / duration);
      }
    }
  }

  public void Restart()
  {
    transform.localScale = startingSize;
    timer = -delay;
    playing = true;
  }
}
