using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using DG.Tweening;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay.Obstacles
{
    public class SecurityBot : Obstacle
    {
        private Sequence currentSequence;
    
        public override void Update()
        {
            // DO nothing
        }

        protected override void OnDisable()
        {
            if (firstInstantiation)
            {
                base.OnDisable();
                return;
            }

            if (currentSequence != null)
            {
                currentSequence.Kill(false);
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform bot = transform.GetChild(i);

                if (bot != null)
                {
                    SpawnDestroyedVersion(bot);
                    bot.localPosition = Vector3.zero;
                }
            }
        
        }

    
        public void InitSecurityBots()
        {
            InitBots();
            BotAnimation();
        }

        private void InitBots()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform securityBot = transform.GetChild(i);
                securityBot.GetChild(0).transform.localScale = Vector3.zero;
                securityBot.GetChild(1).transform.localScale = Vector3.zero;

                bool right = Random.Range(0, 2) * 2 - 1 > 0;

                float xPos = 0;
            
                if (right)
                {
                    xPos = SpawnLocationManager.instance.GetHorizontalPosition(HorizontalPosition.Right);
                    securityBot.eulerAngles = new Vector3(securityBot.eulerAngles.x, 180f, securityBot.eulerAngles.z);
                }
                else
                {
                    xPos = SpawnLocationManager.instance.GetHorizontalPosition(HorizontalPosition.Left);
                    securityBot.eulerAngles = new Vector3(securityBot.eulerAngles.x, 0f, securityBot.eulerAngles.z);
                }


                securityBot.localPosition = new Vector3(xPos, securityBot.position.y, 0f);
            }
        }

        private void BotAnimation()
        {
            currentSequence = DOTween.Sequence();

            int botIndex = 0;
            currentSequence.Append(GetBotSequence(transform.GetChild(0), ref botIndex));


            currentSequence.OnComplete(() => 
            {
                base.ReturnToPool();
            });
        }

        private Sequence GetBotSequence(Transform bot, ref int botIndex)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(bot.DOMoveY(1f, 1f));
            sequence.AppendCallback(() => { bot.GetComponent<Animation>().Play(); });
            sequence.AppendInterval(1.5f);
            sequence.Append(bot.DOMoveY(YToReturnToPool, 1f));

            botIndex++;

            if (botIndex < transform.childCount)
            {
                Transform nextBot = transform.GetChild(botIndex);
                sequence.Join(GetBotSequence(nextBot, ref botIndex));
            }

        
            return sequence;
        }
    }
}
