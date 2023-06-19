using System.Collections;

namespace DD.UI
{   
    public interface IFadable
    {
        IEnumerator Fade(float startValue, float endValue, float fadeLength);
    }
}
