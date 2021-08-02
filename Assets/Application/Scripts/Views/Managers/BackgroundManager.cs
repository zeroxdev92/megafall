using System.Collections.Generic;
using System.Linq;
using Application.Scripts.Model;
using Application.Scripts.Views.Utils;
using UnityEngine;

namespace Application.Scripts.Views.Managers
{
    public class BackgroundManager : MonoBehaviour
    {

        public static BackgroundManager instance;

        public float offSet = 40f;
        public BackgroundData[] bgDataList;

        private List<GameObject> backgrounds;
        private ManagerGame gm;

        private bool changeBG = false;
        private bool fondoChanged = false;
        private BackgroundType nextBGType = BackgroundType.Forest;
        private BackgroundType currentBGType = BackgroundType.Forest;
        public BackgroundType CurrentBG
        {
            get
            {
                return currentBGType;
            }
        }

        private string bgEntraceKey, bgExitKey, bgLoopKey;

        public Texture nightTexture;
        public MeshRenderer bgRenderer;
        private Texture mainTexture;
        private int caveCount = 0;
        private int cascadeCount = 0;

        void Awake()
        {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {

            gm = ManagerGame.instancia;
            backgrounds = new List<GameObject>();
            mainTexture = bgRenderer.material.mainTexture;

            SetBackground();

        }

        // Update is called once per frame
        void Update()
        {

            int bgCount = backgrounds.Count; // Cache for performance

            if (backgrounds == null || bgCount == 0)
                return;


            if (CurrentBG == BackgroundType.Cave && backgrounds[2].transform.position.y >= 0) //Cambio el fondo cuando se que un bloque del medio ya es cueva
            {
                if (!fondoChanged)
                {
                    if (bgRenderer.material.mainTexture == mainTexture)
                    {
                        bgRenderer.material.mainTexture = nightTexture;
                    }
                    else
                    {
                        bgRenderer.material.mainTexture = mainTexture;
                    }

                    fondoChanged = true;
                }

            }

            if (nextBGType == BackgroundType.Forest && !changeBG)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                float pct = Random.value;

                if (pct <= 0.7f && caveCount <= 2) //70%
                {
                    caveCount++;
                    cascadeCount = 0;
                    ChangeBackground(BackgroundType.Cave);
                }
                else
                {
                    if (cascadeCount > 1)
                    {
                        caveCount++;
                        cascadeCount = 0;
                        ChangeBackground(BackgroundType.Cave);
                    }
                    else
                    {
                        caveCount = 0;
                        cascadeCount++;
                        ChangeBackground(BackgroundType.Cascade);
                    }
                }
            }
        
            if (backgrounds[bgCount - 1].transform.position.y >= 0)
            {
                if (changeBG)
                {
                    changeBG = false;
                    SetBackground();
                }

                RestartBGPositions();

                if (nextBGType != BackgroundType.Forest)
                {
                    ChangeBackground(BackgroundType.Forest);
                }

                //gm.CanSpawnObstacle = true;
            }

        }

        public void ChangeBackground(BackgroundType newBG)
        {
            changeBG = true;
            nextBGType = newBG;
        }


        private void SetBackground()
        {

            GameObject[] oldBGList = backgrounds.ToArray();

            backgrounds.Clear();

            SetBGKeys();
            GameObject lastBG = null;
            int backgroundAmount = GetBGAmount();
            for (int i = 0; i < backgroundAmount; i++)
            {
                GameObject bg;

                if (i == 0) //first
                {
                    bg = ObjectPooler.instance.GetPooledObject(bgEntraceKey, true);
                    bg.transform.position = Vector3.zero;

                }
                else if (i == backgroundAmount - 1) // last
                {
                    bg = ObjectPooler.instance.GetPooledObject(bgExitKey, true);
                    bg.transform.position = new Vector3(0f, lastBG.transform.position.y - offSet, 0f);
                }
                else
                {
                    bg = ObjectPooler.instance.GetPooledObject(bgLoopKey, true);
                    bg.transform.position = new Vector3(0f, lastBG.transform.position.y - offSet, 0f);
                }

                backgrounds.Add(bg);
                lastBG = bg;
            }

            if (nextBGType != BackgroundType.Forest)
            {
                //Adding offset to start and end so BG doens't jump.
                backgrounds.Insert(0, ObjectPooler.instance.GetPooledObject(Constants.PooledObjects.BG_FOREST, true));
                backgrounds.Insert(backgrounds.Count, ObjectPooler.instance.GetPooledObject(Constants.PooledObjects.BG_FOREST, true));
            }

            for (int i = 0; i < oldBGList.Length; i++)
            {
                oldBGList[i].SetActive(false);
            }

            currentBGType = nextBGType;
            fondoChanged = false;
        }


        private void SetBGKeys()
        {
            switch (nextBGType)
            {
                case BackgroundType.Cascade:
                    bgEntraceKey = Constants.PooledObjects.BG_CASCADE_ENTRACE;
                    bgLoopKey = Constants.PooledObjects.BG_CASCADE;
                    bgExitKey = Constants.PooledObjects.BG_CASCADE_EXIT;
                    break;
                case BackgroundType.Cave:
                    bgEntraceKey = Constants.PooledObjects.BG_CAVE_ENTRACE;
                    bgLoopKey = Constants.PooledObjects.BG_CAVE;
                    bgExitKey = Constants.PooledObjects.BG_CAVE_EXIT;
                    break;
                default:
                    bgEntraceKey = Constants.PooledObjects.BG_FOREST;
                    bgLoopKey = Constants.PooledObjects.BG_FOREST;
                    bgExitKey = Constants.PooledObjects.BG_FOREST;
                    break;
            }
        }

        private int GetBGAmount()
        {
            BackgroundData bgData = bgDataList.First(x => x.bgType == nextBGType);

            return Random.Range(bgData.cantMin, bgData.cantMax + 1);
        }

        private void RestartBGPositions()
        {
            for (int i = 0, length = backgrounds.Count; i < length; i++)
            {
                backgrounds[i].transform.position = new Vector3(0f, -offSet * i, 0f);
            }
        }
    }
}
