using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField]
    private float maxHealthPoints = 100;

    [SerializeField]
    private float healthBarStepsLength = 10;

    [SerializeField]
    private float damagesDecreaseRate = 10;

    //    private float regenarationIncreaseRate = 10;

    private Image image;

    private float currentHealthPoints;

    private RectTransform imageRectTransform;

    private float damages;

    private float regenaration;

    public float Health {
        get { return currentHealthPoints; }
        set {
            currentHealthPoints = Mathf.Clamp(value, 0, MaxHealthPoints);
            image.material.SetFloat("_Percent", currentHealthPoints / MaxHealthPoints);

            if (currentHealthPoints < Mathf.Epsilon)
                Damages = 0;

        }
    }

    public float Damages {
        get { return damages; }
        set {
            damages = Mathf.Clamp(value, 0, MaxHealthPoints);
            //image.material.SetFloat("_IsRecover", 0);

            image.material.SetFloat("_DamagesPercent", damages / MaxHealthPoints);
        }
    }

    public float Regenaration {
        get { return regenaration; }
        set {
            regenaration = Mathf.Clamp(value, 0, MaxHealthPoints);
            //image.material.SetFloat("_IsRecover", 1);

            image.material.SetFloat("_Percent", regenaration / MaxHealthPoints);
        }
    }

    public float MaxHealthPoints {
        get { return maxHealthPoints; }
        set {
            maxHealthPoints = value;
            image.material.SetFloat("_Steps", MaxHealthPoints / healthBarStepsLength);
            this.Hurt(0);
        }
    }

    protected void Awake() {
        image = GetComponent<Image>();

        imageRectTransform = image.GetComponent<RectTransform>();
        image.material = Instantiate(image.material); // Clone material

        image.material.SetVector("_ImageSize", new Vector4(imageRectTransform.rect.size.x, imageRectTransform.rect.size.y, 0, 0));

        MaxHealthPoints = MaxHealthPoints; // Force the call to the setter in order to update the material
        currentHealthPoints = MaxHealthPoints; // Force the call to the setter in order to update the material
    }

    protected void Update() {

        if (Damages > 0) {
            Damages -= damagesDecreaseRate * Time.deltaTime;
            GetComponentInParent<Canvas>().gameObject.SetActive(true);
        }

    }

    public void Hurt(float damagesPoints) {
        Damages = damagesPoints;
        Health -= Damages;
    }

    public void Recover(float healthPoints) {
        Regenaration = healthPoints;
        Health += Regenaration;
    }
}