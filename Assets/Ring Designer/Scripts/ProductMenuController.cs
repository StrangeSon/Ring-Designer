using System.Collections.Generic;
using UnityEngine;
using HyperSpark.HyperTween;
using System;
using TMPro;

namespace RingDesigner
{
    public class ProductMenuController : MonoBehaviour
    {
        public List<Product> Products;

        public TMP_Text Text;

        private int menuIndex;

        private FloatTween Tween;

        private GameObject showProduct;
        private GameObject hideProduct;

        private enum Direction
        {
            Forward,
            Backward
        }

        private Direction TransitionDirection;

        private void Awake()
        {
            foreach(Product product in Products)
            {
                product.gameObject.SetActive(false);
            }
            var mainProduct = Products[0];
            mainProduct.gameObject.SetActive(true);
            Text.text = mainProduct.Label;

            Tween = new(
                from: 0f,
                to: 1f,
                duration: 1f,
                onUpdate: TransitionProduct,
                curveFunc: Ease.OutQuad,
                onComplete: TransitionComplete);
        }

        private void TransitionProduct(float t)
        {
            if (TransitionDirection == Direction.Forward)
            {
                hideProduct.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(-20f, 0f, 0f), t);
                hideProduct.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
                showProduct.transform.position = Vector3.Lerp(new Vector3(20f, 0f, 0f), Vector3.zero, t);
                showProduct.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            }
            else
            {
                hideProduct.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(20f, 0f, 0f), t);
                hideProduct.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
                showProduct.transform.position = Vector3.Lerp(new Vector3(-20f, 0f, 0f), Vector3.zero, t);
                showProduct.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            }

        }

        private void TransitionComplete()
        {
            hideProduct.SetActive(false);
        }

        public void PrevProduct()
        {
            if (menuIndex <= 0)
                return;

            if (Tween.IsPlaying)
                Tween.Complete();

            TransitionDirection = Direction.Backward;

            var prevIndex = menuIndex;
            menuIndex--;
            var nextIndex = menuIndex;

            hideProduct = Products[prevIndex].gameObject;
            showProduct = Products[nextIndex].gameObject;
            showProduct.SetActive(true);
            Text.text = Products[nextIndex].Label;

            Tween.Play(fromStart: true);
        }

        public void NextProduct()
        {
            if (menuIndex + 1 >= Products.Count)
                return;

            if (Tween.IsPlaying)
                Tween.Complete();

            TransitionDirection = Direction.Forward;

            var prevIndex = menuIndex;
            menuIndex++;
            var nextIndex = menuIndex;

            hideProduct = Products[prevIndex].gameObject;
            showProduct = Products[nextIndex].gameObject;
            showProduct.SetActive(true);
            Text.text = Products[nextIndex].Label;

            Tween.Play(fromStart: true);
        }
    }
}