using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private TextMesh score;

    [SerializeField]
    private Transform healthbar;

    [SerializeField]
    private SpriteInfo playerInfo;

    private float maxHealth;

    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = playerInfo.Health;

        originalScale = healthbar.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.localScale = new Vector3(originalScale.x * (playerInfo.Health / maxHealth), originalScale.y, originalScale.z);

        score.text = "Score: " + CollisionManager.Instance.Score;
    }


}
