using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{
    [SerializeField] float localScaleModifier;
    [SerializeField] float animationSpeed;
    [SerializeField] GameObject netCover;
    [SerializeField] Transform animationStartPoint, animationEndPoint;
    [SerializeField] UnityEvent OnFinish;

    public bool finished;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (finished)
            return;

        if (collision.transform.TryGetComponent(out Ball ball))
        {
            if (ball.finished)
                return;


            finished = true;

            StartCoroutine(AnimateFinish(ball));
        }
    }
    IEnumerator AnimateFinish(Ball ball)
    {
        OnFinish?.Invoke();

        //��������� ��� � ����� ��������
        ball.transform.rotation = Quaternion.identity;
        ball.ResetRotation();
        Vector2 startPos = ball.transform.position;
        Vector2 endPos = animationStartPoint.position;
        float t = 0;
        do
        {
            t += Time.deltaTime * animationSpeed;
            ball.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        } while (t < 1);
        netCover.SetActive(true);

        //��������� ��������
        startPos = animationStartPoint.position;
        endPos = animationEndPoint.position;
        t = 0;

        Vector3 initialScale = ball.transform.localScale;

        Vector3 targetScale = ball.transform.localScale.WhereX(initialScale.x * localScaleModifier);
        do
        {
            t += Time.deltaTime * animationSpeed;
            ball.transform.position = Vector3.Lerp(startPos, endPos, t);
            if (t < 0.3f)
                ball.transform.localScale = Vector3.Lerp(initialScale, targetScale, t * (10f / 3f));
            else if (t > 0.6f)
                ball.transform.localScale = Vector3.Lerp(targetScale, initialScale, t * (10f / 3f) - 2);

            yield return null;
        } while (t < 1);

        //ball.transform.localScale = initialScale;
        netCover.SetActive(false);

        //������� ��� ���������
        int bounces = ball.OnFinish();

        LevelData levelData = LevelManager.Instance.CurrentLevelData;
        int stars = levelData.ConvertToStars(bounces);

        LevelManager.Instance.SaveProgress(stars);

        GameManager.Instance.UpdateLevelSelector();
        GameManager.Instance.ShowLevelCompleteScreen(stars);
    }
}

