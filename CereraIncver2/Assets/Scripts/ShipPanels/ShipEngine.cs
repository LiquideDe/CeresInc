using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngine : MonoBehaviour
{
    [SerializeField] ParticleSystem engine1, engine2, HNE, HSE, HNW, HSW, SNE, SSE, SNW, SSW;
    private ParticleSystem.EmissionModule HNEE, HSEE, HNWE, HSWE, SNEE, SSEE, SNWE, SSWE;

    private void Start()
    {
        HNEE = HNE.emission;
        HSEE = HSE.emission;
        HNWE = HNW.emission;
        HSWE = HSW.emission;
        SNEE = SNE.emission;
        SSEE = SSE.emission;
        SNWE = SNW.emission;
        SSWE = SSW.emission;

        HNEE.enabled = true;
        HSEE.enabled = true;
        HNWE.enabled = true;
        HSWE.enabled = true;
        SNEE.enabled = true;
        SSEE.enabled = true;
        SNWE.enabled = true;
        SSWE.enabled = true;
    }
    public void FullForward()
    {
        if (engine1.isStopped)
        {
            engine1.Play();
            engine2.Play();
        }
        
    }

    public void StopEngine()
    {
        if (engine1.isPlaying)
        {
            engine1.Stop();
            engine2.Stop();
        }
        
    }

    public bool IsEnginesRunning()
    {
        if (engine1.isPlaying)
            return true;
        else
            return false;
    }

    public void OMSStop()
    {
        HSW.Stop();
        HSE.Stop();
        SNW.Stop();
        SNE.Stop();
        HNW.Stop();
        HNE.Stop();
        SSW.Stop();
        SSE.Stop();
    }

    public void OMSRotate(Vector3 direct)
    {
        if (Mathf.Abs(direct.x) > 0)
        {
            float tr = 100 * Mathf.Abs(direct.x);
            if (direct.x < 0)
            {
                OMSRotateLeft(tr);
                if (direct.z > 0.88 && direct.z < 0.99)
                {

                    float obtr = direct.z * 50;
                    OMSRotateRight(obtr);
                }
                else
                {
                    float obtr = 1 / direct.z * 5;
                    OMSRotateRight(obtr);
                }
            }
            if (direct.x > 0)
            {
                OMSRotateRight(tr);
                if (direct.z > 0.88 && direct.z < 0.99)
                {
                    float obtr = direct.z * 50;
                    OMSRotateLeft(obtr);
                }
                else
                {
                    float obtr = 1 / direct.z * 5;
                    OMSRotateLeft(obtr);
                }
            }
        }
        if (Mathf.Abs(direct.y) > 0)
        {
            float tr = 100 * Mathf.Abs(direct.y);
            if (direct.y > 0)
            {
                OMSRotateDown(tr);
                if (direct.z > 0.88 && direct.z < 0.99)
                {
                    float obtr = direct.z * 50;
                    OMSRotateUp(obtr);
                }
                else
                {
                    float obtr = 1 / direct.z * 5;
                    OMSRotateUp(obtr);
                }
            }
            if (direct.y < 0)
            {
                OMSRotateUp(tr);
                if (direct.z > 0.88 && direct.z < 0.99)
                {
                    float obtr = direct.z * 50;
                    OMSRotateDown(obtr);
                }
                else
                {
                    float obtr = 1 / direct.z * 5;
                    OMSRotateDown(obtr);
                }
            }
        }
    }

    public void OMSMove(Vector3 direct)
    {
        if (Mathf.Abs(direct.x) > 0)
        {
            float tr = 100 * Mathf.Abs(direct.x);
            if (direct.x > 0)
            {
                OMSMoveRight(tr);
                if (direct.x < 1.5)
                {
                    OMSMoveLeft(20);
                }
            }
            if (direct.x < 0)
            {
                OMSMoveLeft(tr);
                if (Mathf.Abs(direct.x) < 1.5)
                {
                    OMSMoveRight(tr * 20);
                }
            }
        }
        if (Mathf.Abs(direct.y) > 0)
        {
            float tr = 100 * Mathf.Abs(direct.y);
            if (direct.y > 0)
            {
                OMSMoveUp(tr);
                if (direct.y < 1.5)
                {
                    OMSMoveDown(tr * 20);
                }
            }
            if (direct.y < 0)
            {
                OMSMoveDown(tr);
                if (Mathf.Abs(direct.y) < 1.5)
                {
                    OMSMoveUp(tr * 20);
                }
            }
        }
        if (Mathf.Abs(direct.z) > 1)
        {
            float tr = 100 * Mathf.Abs(direct.z);
            OMSMoveForward(tr);
        }
    }

    private void OMSRotateLeft(float rate)
    {
        if(rate > 100) {
            rate = (rate/10);
        }
        if (HNE.isStopped || HSE.isStopped || SNW.isStopped || SSW.isStopped)
        {
            HNE.Play();
            HSE.Play();
            SNW.Play();
            SSW.Play();
        }

        HNEE.rateOverTime = rate;
        HSEE.rateOverTime = rate;
        SNWE.rateOverTime = rate;
        SSWE.rateOverTime = rate;
    }
    private void OMSRotateRight(float rate)
    {
        if (rate > 100)
        {
            rate = (rate / 10);
        }
        if (HNW.isStopped || HSW.isStopped || SNE.isStopped || SSE.isStopped)
        {
            HNW.Play();
            HSW.Play();
            SNE.Play();
            SSE.Play();
        }

        HNWE.rateOverTime = rate;
        HSWE.rateOverTime = rate;
        SNEE.rateOverTime = rate;
        SSEE.rateOverTime = rate;
    }
    private void OMSRotateDown(float rate)
    {
        if (rate > 100)
        {
            rate = (rate / 10);
        }
        if (HSW.isStopped || HSE.isStopped || SNW.isStopped || SNE.isStopped)
        {
            HSW.Play();
            HSE.Play();
            SNW.Play();
            SNE.Play();
        }

        HSWE.rateOverTime = rate;
        HSEE.rateOverTime = rate;
        SNWE.rateOverTime = rate;
        SNEE.rateOverTime = rate;
    }
    private void OMSRotateUp(float rate)
    {
        if (rate > 100)
        {
            rate = (rate / 10);
        }
        if (HNW.isStopped || HNE.isStopped || SSW.isStopped || SSE.isStopped)
        {
            HNW.Play();
            HNE.Play();
            SSW.Play();
            SSE.Play();
        }
        HNWE.rateOverTime = rate;
        HNEE.rateOverTime = rate;
        SSWE.rateOverTime = rate;
        SSEE.rateOverTime = rate;
    }
    private void OMSMoveForward(float rate)
    {
        if (rate > 100)
        {
            rate = (rate / 10);
        }
        if (HNW.isStopped || HNE.isStopped || HSW.isStopped || HSE.isStopped || SNW.isStopped || SNE.isStopped || SSW.isStopped || SSE.isStopped)
        {
            HNW.Play();
            HNE.Play();
            HSW.Play();
            HSE.Play();
            SNW.Play();
            SNE.Play();
            SSW.Play();
            SSE.Play();
        }
        HNWE.rateOverTime = rate;
        HNEE.rateOverTime = rate;
        HSWE.rateOverTime = rate;
        HSEE.rateOverTime = rate;
        SNWE.rateOverTime = rate;
        SNEE.rateOverTime = rate;
        SSWE.rateOverTime = rate;
        SSEE.rateOverTime = rate;
    }
    private void OMSMoveLeft(float rate)
    {
        if (rate > 100)
        {
            rate = (rate / 10);
        }
        if (HNE.isStopped || HSE.isStopped || SNE.isStopped || SSE.isStopped)
        {
            HNE.Play();
            HSE.Play();
            SNE.Play();
            SSE.Play();
        }
        HNEE.rateOverTime = rate;
        HSEE.rateOverTime = rate;
        SNEE.rateOverTime = rate;
        SSEE.rateOverTime = rate;
    }
    private void OMSMoveRight(float rate)
    {
        if (rate > 100)
        {
            rate = (rate / 10);
        }
        if (HNW.isStopped || HSW.isStopped || SNW.isStopped || SSW.isStopped)
        {
            HNW.Play();
            HSW.Play();
            SNW.Play();
            SSW.Play();
        }
        HNWE.rateOverTime = rate;
        HSWE.rateOverTime = rate;
        SNWE.rateOverTime = rate;
        SSWE.rateOverTime = rate;
    }
    private void OMSMoveUp(float rate)
    {
        if (rate > 100)
        {
            rate = (rate / 10);
        }
        if (HSW.isStopped || HSE.isStopped || SSE.isStopped || SSW.isStopped)
        {
            HSW.Play();
            HSE.Play();
            SSE.Play();
            SSW.Play();
        }
        HSWE.rateOverTime = rate;
        HSEE.rateOverTime = rate;
        SSEE.rateOverTime = rate;
        SSWE.rateOverTime = rate;
    }
    private void OMSMoveDown(float rate)
    {
        if (rate > 100)
        {
            rate = (rate / 10);
        }
        if (HNW.isStopped || HNE.isStopped || SNW.isStopped || SNE.isStopped)
        {
            HNW.Play();
            HNE.Play();
            SNW.Play();
            SNE.Play();
        }
        HNWE.rateOverTime = rate;
        HNEE.rateOverTime = rate;
        SNWE.rateOverTime = rate;
        SNEE.rateOverTime = rate;
    }
}
