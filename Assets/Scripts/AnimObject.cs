using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [Tooltip("Velocidad de rotación en grados por segundo")]
    public float velocidadRotacion = 50f;

    [Header("Configuración de Flotación")]
    [Tooltip("Altura del movimiento de flotación")]
    public float alturaFlotacion = 0.5f;

    [Tooltip("Velocidad del movimiento de flotación")]
    public float velocidadFlotacion = 1f;

    [Header("Curva de Animación")]
    [Tooltip("Curva para controlar el ease in/out de la flotación")]
    public AnimationCurve curvaFlotacion = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Opciones de Easing Predefinidas")]
    [Tooltip("Tipo de easing para la flotación")]
    public TipoEasing tipoEasing = TipoEasing.Senoidal;

    [Tooltip("Usar curva personalizada en lugar del tipo de easing")]
    public bool usarCurvaPersonalizada = false;

    private Vector3 posicionInicial;
    private float tiempo = 0f;

    public enum TipoEasing
    {
        Senoidal,
        EaseInOut,
        EaseIn,
        EaseOut,
        Elastico,
        Rebote
    }

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Rotación continua
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime);

        // Actualizar tiempo
        tiempo += Time.deltaTime * velocidadFlotacion;

        // Normalizar tiempo entre 0 y 1
        float t = Mathf.PingPong(tiempo, 1f);

        // Calcular el valor de easing
        float valorEasing;

        if (usarCurvaPersonalizada)
        {
            valorEasing = curvaFlotacion.Evaluate(t);
        }
        else
        {
            valorEasing = ObtenerValorEasing(t, tipoEasing);
        }

        // Aplicar flotación con easing
        float nuevaY = posicionInicial.y + (valorEasing * 2f - 1f) * alturaFlotacion;
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }

    float ObtenerValorEasing(float t, TipoEasing tipo)
    {
        switch (tipo)
        {
            case TipoEasing.Senoidal:
                return (Mathf.Sin(t * Mathf.PI * 2f) + 1f) * 0.5f;

            case TipoEasing.EaseInOut:
                return t < 0.5f
                    ? 2f * t * t
                    : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;

            case TipoEasing.EaseIn:
                return t * t;

            case TipoEasing.EaseOut:
                return 1f - (1f - t) * (1f - t);

            case TipoEasing.Elastico:
                float c4 = (2f * Mathf.PI) / 3f;
                return t == 0f ? 0f
                    : t == 1f ? 1f
                    : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;

            case TipoEasing.Rebote:
                return ReboteEaseOut(t);

            default:
                return t;
        }
    }

    float ReboteEaseOut(float t)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (t < 1f / d1)
        {
            return n1 * t * t;
        }
        else if (t < 2f / d1)
        {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        }
        else if (t < 2.5f / d1)
        {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        }
        else
        {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }
}