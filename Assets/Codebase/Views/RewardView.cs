using Codebase.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Codebase.Views
{
    public class RewardView : ViewBase
    {
        [SerializeField] private TMP_Text text;

        private void Start()
        {
            transform.DOMoveY(transform.position.y + 3, 2);
            text.DOFade(0, 2);
        }
    }
}